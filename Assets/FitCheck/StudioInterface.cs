using System;
using System.Linq;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        readonly Color paper=new Color(.94f,.92f,.86f), muted=new Color(.61f,.58f,.66f),
            surface=new Color(.09f,.075f,.12f), violet=new Color(.66f,.48f,.95f), line=new Color(.23f,.20f,.28f);
        GUIStyle display, heading, copy, caption, action, tiny;
        Font uiFont;bool fontDirty=true;
        void FontRebuilt(Font font){if(font==uiFont)fontDirty=true;}
        void WarmFont()
        {
            const string glyphs="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789áàâãéêíóôõúüçÁÀÂÃÉÊÍÓÔÕÚÜÇ .,:;!?+-−/\\%()[]‘’“”•…←→↺✓–—";
            foreach(int size in new[]{13,17,21})uiFont.RequestCharactersInTexture(glyphs,size,FontStyle.Normal);
            foreach(int size in new[]{18,30})uiFont.RequestCharactersInTexture(glyphs,size,FontStyle.Bold);
            uiFont.RequestCharactersInTexture("0123456789 /FITCHECKOUAFim de expedienteUma pausaO visual foi pra ruaA semana virou história.",86,FontStyle.Bold);
            fontDirty=false;
        }
        bool dragging, moved; Vector2 dragStart;
        readonly System.Collections.Generic.Dictionary<string,Texture2D> thumbnails=new System.Collections.Generic.Dictionary<string,Texture2D>();
        readonly System.Collections.Generic.Dictionary<string,Texture2D> lockedThumbnails=new System.Collections.Generic.Dictionary<string,Texture2D>();
        void BuildThumbnails()
        {
            foreach(var item in Catalog.All){
                var image=studio.Thumbnail(item);thumbnails[item.id]=image;
                var gray=new Texture2D(128,128,TextureFormat.RGBA32,false){filterMode=FilterMode.Point};var pixels=image.GetPixels();
                for(int i=0;i<pixels.Length;i++){float value=pixels[i].grayscale*.55f;pixels[i]=new Color(value,value,value,pixels[i].a);}
                gray.SetPixels(pixels);gray.Apply();lockedThumbnails[item.id]=gray;
            }
            Apply();UpdateCamera();
        }
        DayProgress campaignBeforePreview;
        void PreviewSession()
        {
            studio.TallPreview=false;studio.PreviewProportions=true;studio.ExpressionOverride=-1;studio.HairOverride=-1;
            campaignBeforePreview=progress;progress=new DayProgress();
            storageOverride=System.IO.Path.Combine(Application.temporaryCachePath,"FitCheck-VisualPreview");
            System.IO.Directory.CreateDirectory(storageOverride);Begin();
            inventory=Catalog.All.Select(item=>item.id).ToArray();
            outfit=new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""};Apply();
        }
        void Home()
        {
            pendingDeleteSlot=-1;
            if(stage==Stage.City||(city!=null&&city.IsHome&&city.gameObject.activeSelf)||(shopService&&stage==Stage.Dialogue)||shopDressingInStore)ExitCity();
            shopService=false;shopSequence=ShopSequence.None;
            if(stage==Stage.CharacterCreator){creatorDraft=null;studio.PlayerLook=playerAppearance;}
            studio.TallPreview=false;studio.PreviewProportions=false;studio.ExpressionOverride=-1;studio.HairOverride=-1;
            if(campaignBeforePreview!=null){progress=campaignBeforePreview;campaignBeforePreview=null;storageOverride=null;message="";}
            replayIndex=-1;fromHome=false;stage=Stage.Welcome;ShowPlayer();
        }
        void InterfaceStyles()
        {
            if(display!=null){if(fontDirty)WarmFont();return;}
            uiFont=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");Font.textureRebuilt+=FontRebuilt;
            display=new GUIStyle(GUI.skin.label){font=uiFont,fontSize=86,fontStyle=FontStyle.Bold,wordWrap=false};
            heading=new GUIStyle(GUI.skin.label){font=uiFont,fontSize=30,fontStyle=FontStyle.Bold,wordWrap=true};
            copy=new GUIStyle(GUI.skin.label){font=uiFont,fontSize=21,wordWrap=true};
            caption=new GUIStyle(copy){fontSize=17};tiny=new GUIStyle(copy){fontSize=13};
            action=new GUIStyle(copy){fontSize=18,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleCenter};
            WarmFont();
        }
        void Fill(Rect r,Color c)
        {var old=GUI.color;GUI.color=c;GUI.DrawTexture(r,Texture2D.whiteTexture);GUI.color=old;}
        void Txt(Rect r,string text,GUIStyle style,Color? color=null)
        {style.normal.textColor=color??paper;GUI.Label(r,text,style);}
        void Rule(float x,float y,float w){Fill(new Rect(x,y,w,1),line);}
        bool Tap(Rect r,string label,bool primary=false,bool enabled=true)
        {
            bool hot=r.Contains(Event.current.mousePosition)&&enabled;
            Fill(r,primary?(hot?new Color(.77f,.62f,1):violet):(hot?new Color(.19f,.16f,.24f):surface));
            if(!primary){Fill(new Rect(r.x,r.y,r.width,1),line);Fill(new Rect(r.x,r.yMax-1,r.width,1),line);}
            Txt(r,label,action,enabled?(primary?surface:paper):muted);
            bool active=GUI.enabled;GUI.enabled=active&&enabled;
            bool clicked=GUI.Button(r,GUIContent.none,GUIStyle.none);GUI.enabled=active;if(clicked)Sound(0);return clicked;
        }
        void OnGUI()
        {
            var originalMatrix=GUI.matrix;var originalEnabled=GUI.enabled;
            var originalColor=GUI.color;var originalContent=GUI.contentColor;var originalBackground=GUI.backgroundColor;
            GUI.color=Color.white;GUI.contentColor=Color.white;GUI.backgroundColor=Color.white;
            GUI.matrix=Matrix4x4.identity;cam.GetComponent<RetroCamera>().DrawWorld();GUI.matrix=originalMatrix;
            Stage renderedStage=stage;
            InterfaceStyles();
            float scale=Mathf.Min(Screen.width/1440f,Screen.height/900f);
            var offset=new Vector3((Screen.width-1440*scale)/2,(Screen.height-900*scale)/2,0);
            GUI.matrix=Matrix4x4.TRS(offset,Quaternion.identity,Vector3.one*scale);
            var e=Event.current;
            if(e.type==EventType.KeyDown&&e.keyCode==KeyCode.Escape){if(pendingDeleteSlot>=0)CancelSaveDeletion();else if(renderedStage==Stage.CharacterCreator&&!paused)CancelCreator();else if(renderedStage==Stage.Confirm&&!paused)stage=Stage.Dressing;else paused=!paused;dragging=false;e.Use();}
            bool modal=paused || renderedStage==Stage.Confirm||pendingDeleteSlot>=0;
            GUI.enabled=!modal;
            if(renderedStage==Stage.Title)TitleUI();else{
            // The header is deliberately quieter than the outfit.
            Fill(new Rect(0,0,1440,88),new Color(.045f,.035f,.065f,.97f));
            Txt(new Rect(38,22,280,45),"FIT CHECK",heading);
            Txt(new Rect(244,33,180,28),"by DripLab",caption,muted);
            Txt(new Rect(965,28,300,30),stage==Stage.Welcome?"SEUS PERSONAGENS":stage==Stage.City?(city.IsHome?"EM CASA":city.IsInterior?"DRIPLAB  /  LOJA":"PRAÇA DO ENCONTRO"):campaignBeforePreview!=null?"PRÉVIA  /  CAMPANHA PRESERVADA":"DIA 0"+progress.day+"  /  DRIPLAB",tiny,muted);
            if(Tap(new Rect(1298,20,105,46),"Pausa"))paused=true;
            if(renderedStage==Stage.Welcome)WelcomeUI();
            if(renderedStage==Stage.Dialogue)DialogueUI();
            if(renderedStage==Stage.Dressing || renderedStage==Stage.Confirm)DressingUI();
            if(renderedStage==Stage.Leaving){
                Fill(new Rect(0,88,1440,812),surface);
                Txt(new Rect(210,315,1000,85),"O visual foi pra rua.",display);
                Txt(new Rect(215,425,850,60),Customer.name+" saiu para o compromisso. Daqui a pouco tem novidade no FitFeed.",copy,muted);
                Fill(new Rect(215,525,Mathf.Clamp01(transition/1.2f)*600,4),violet);
            }
            if(renderedStage==Stage.Post)PostUI();
            if(renderedStage==Stage.Summary)SummaryUI();
            if(renderedStage==Stage.Home)HomeUI();
            if(renderedStage==Stage.Retrospective)RetrospectiveUI();
            if(renderedStage==Stage.City)CityUI();
            if(renderedStage==Stage.CharacterCreator&&stage==Stage.CharacterCreator)CreatorUI();
            }
            GUI.enabled=true;
            if(pendingDeleteSlot>=0)SaveDeletionUI();
            if(renderedStage==Stage.Confirm&&!paused){
                Fill(new Rect(0,0,1440,900),new Color(.025f,.018f,.04f,.88f));
                Fill(new Rect(440,280,560,320),surface);Fill(new Rect(440,280,560,4),violet);
                Txt(new Rect(478,316,490,55),"Esse é o visual?",heading);
                Txt(new Rect(478,381,475,65),"Depois do envio, o cliente sai com as peças que você escolheu.",copy,muted);
                if(Tap(new Rect(478,485,222,60),"Voltar ao provador"))stage=Stage.Dressing;
                if(Tap(new Rect(718,485,242,60),"Enviar visual",true))Submit();
            }
            if(paused){
                Fill(new Rect(0,0,1440,900),new Color(.025f,.018f,.04f,.92f));
                Txt(new Rect(510,300,500,90),"Uma pausa.",display);
                Txt(new Rect(515,408,440,50),"Seu atendimento continua daqui.",copy,muted);
                if(Tap(new Rect(515,495,410,64),"Retomar",true))paused=false;
                if(Tap(new Rect(515,578,410,56),"Voltar ao início")){paused=false;ReturnToTitle();}
                if(Tap(new Rect(515,650,410,48),mutedAudio?"Sons: desligados":"Sons: ligados")){mutedAudio=!mutedAudio;PlayerPrefs.SetInt("FitCheck.Mute",mutedAudio?1:0);PlayerPrefs.Save();}
                if(Tap(new Rect(515,713,410,48),"Salvar novamente"))TrySave();
                if(Tap(new Rect(515,777,410,48),reducedMotion?"Movimento reduzido: sim":"Movimento reduzido: não")){reducedMotion=!reducedMotion;PlayerPrefs.SetInt("FitCheck.ReducedMotion",reducedMotion?1:0);PlayerPrefs.Save();}
                if(Tap(new Rect(515,837,410,44),retroFilter?"Pixelização: leve":"Pixelização: desligada")){retroFilter=!retroFilter;PlayerPrefs.SetInt("FitCheck.Retro",retroFilter?1:0);PlayerPrefs.Save();}
            }
            DrawAchievement();
            GUI.matrix=originalMatrix;GUI.enabled=originalEnabled;
            GUI.color=originalColor;GUI.contentColor=originalContent;GUI.backgroundColor=originalBackground;
        }
        void WelcomeUI()
        {
            SaveSelectionUI();
        }
        void CustomerBadge()
        {
            Txt(new Rect(38,117,640,28),"DIA 0"+(customer/3+1)+"   /   ATENDIMENTO 0"+(customer%3+1)+" DE 03"+(replayIndex>=0?"   /   REPETIÇÃO":""),tiny,violet);
            Txt(new Rect(38,148,650,50),Customer.name,heading);
        }
        void DialogueUI()
        {
            CustomerBadge();
            Fill(new Rect(1000,88,440,812),surface);
            Txt(new Rect(1032,123,370,38),"Qual é a ocasião?",heading);
            Txt(new Rect(1032,184,366,310),"“"+Request+"”",copy);
            Rule(1032,505,366);
            if(Timed)Txt(new Rect(50,715,850,70),"Malu está com pressa: você terá 120 segundos no provador. Pausa interrompe o relógio.",copy,violet);
            if(question<0){
                Txt(new Rect(1032,526,365,45),"Se quiser, pergunte um pouco mais.",caption,muted);
                for(int i=0;i<2;i++)if(Tap(new Rect(1032,581+i*89,366,76),questions[i]))question=i;
            }else{
                Txt(new Rect(1032,526,366,47),questions[question],caption,violet);
                Txt(new Rect(1032,590,366,130),"“"+answers[question]+"”",copy);
            }
            if(Tap(new Rect(1032,786,366,66),question<0?"Ir ao provador  →":"Vamos montar o visual  →",true)){ProceedToFitting();}
            Txt(new Rect(50,800,850,44),"Primeiro, entenda o pedido. Depois, experimente à vontade.",caption,muted);
        }
        void DressingUI()
        {
            CustomerBadge();
            if(Timed){var warn=remaining<=30?new Color(1,.47f,.36f):violet;Txt(new Rect(770,122,210,55),Mathf.CeilToInt(remaining)+" s",heading,warn);Fill(new Rect(770,180,190*Mathf.Clamp01(remaining/120),4),warn);}
            if(Tap(new Rect(38,222,190,46),showRequest?"Fechar pedido":"Reler pedido")){showRequest=!showRequest;showDrops=false;}
            if(Tap(new Rect(38,281,190,46),showDrops?"Fechar drops":"Ver drops")){showDrops=!showDrops;showRequest=false;}
            if(showRequest||showDrops){
                Fill(new Rect(38,343,315,375),new Color(.065f,.05f,.09f,.97f));
                detailsScroll=GUI.BeginScrollView(new Rect(54,361,282,335),detailsScroll,new Rect(0,0,255,680));
                Txt(new Rect(0,0,247,435),showRequest?"“"+Request+"”":"AFTER HOURS\nA cidade continua depois do trabalho. Luzes, encontros e caminhos.\n\nCOMMON GROUND\nPraça, quadra e pertencimento.\n\nSIDE B\nMúsica independente, colagem e criação autoral.",caption);
                if(showRequest&&question>=0)Txt(new Rect(0,450,247,220),answers[question],caption,violet);
                GUI.EndScrollView();
            }
            if(campaignBeforePreview!=null&&!showRequest&&!showDrops){
                Txt(new Rect(38,585,240,55),"Corpo compacto\nExpressão e cabelo ao seu jeito.",caption,muted);
                if(Tap(new Rect(38,711,250,48),"Expressão: "+studio.ExpressionName+"  ↔"))studio.CycleExpression();
                if(Tap(new Rect(38,772,250,48),"Cabelo: "+studio.HairName+"  ↔")){studio.CycleHair();Apply();}
            }
            Fill(new Rect(325,827,565,57),new Color(.045f,.035f,.065f,.9f));
            Txt(new Rect(340,840,535,30),"ARRASTE PARA GIRAR   /   ROLE PARA APROXIMAR",tiny,paper);
            Fill(new Rect(1000,88,440,812),surface);
            Txt(new Rect(1032,118,370,42),"Seu toque.",heading);
            Txt(new Rect(1032,164,370,30),"Experimente. As peças contam a história.",caption,muted);
            string[] tabs={"Cima","Baixo","Tênis","Cabeça"};
            for(int i=0;i<4;i++){
                if(Tap(new Rect(1032+i*94,219,87,46),tabs[i],category==i)){category=i;scroll=Vector2.zero;}
            }
            var items=Catalog.All.Where(x=>x.slot==category).ToArray();
            int count=items.Length+(category==3?1:0);int rows=(count+1)/2;
            scroll=GUI.BeginScrollView(new Rect(1026,287,392,425),scroll,new Rect(0,0,366,rows*164));
            for(int i=0;i<count;i++){
                var r=new Rect(6+(i%2)*184,(i/2)*164,174,153);
                if(i==items.Length){if(Tap(r,"Sem acessório",outfit[3]=="")){outfit[3]="";Apply();}continue;}
                var item=items[i];bool locked=!IsAvailable(item);bool selected=outfit[category]==item.id;
                Fill(r,selected?new Color(.22f,.16f,.3f):new Color(.12f,.10f,.16f));
                if(selected)Fill(new Rect(r.x,r.y,3,r.height),violet);
                ClothingIcon(new Rect(r.x+39,r.y+10,95,75),item,locked);
                Txt(new Rect(r.x+12,r.y+88,152,25),item.name,caption,locked?muted:paper);
                Txt(new Rect(r.x+12,r.y+119,154,25),locked?"BLOQUEADA  /  "+item.unlockAt:item.drop,tiny,locked?muted:violet);
                if(GUI.Button(r,GUIContent.none,GUIStyle.none)&&stage==Stage.Dressing){Sound(0);
                    if(locked)message=item.name+": alcance "+item.unlockAt+" de Reputação. Entrega no fechamento.";
                    else{outfit[category]=item.id;message="";Apply();}
                }
            }
            GUI.EndScrollView();
            Rule(1032,730,366);
            Txt(new Rect(1032,742,365,40),message.Length>0?message:"Você pode mudar de ideia até enviar o visual.",tiny,muted);
            if(Tap(new Rect(1032,796,366,64),"Finalizar visual  →",true))stage=Stage.Confirm;
            if(Tap(new Rect(910,836,66,38),"↺")){rotation=-12;distance=7.2f;UpdateCamera();}
            HandlePreview();
        }
        void ClothingIcon(Rect r,Garment g,bool locked)
        {
            if(thumbnails.ContainsKey(g.id)){GUI.DrawTexture(r,locked?lockedThumbnails[g.id]:thumbnails[g.id],ScaleMode.ScaleToFit,true);return;}
            Color c=locked?new Color(.3f,.28f,.34f):g.color;
            if(g.slot==0){Fill(new Rect(r.x+24,r.y+13,47,55),c);Fill(new Rect(r.x+9,r.y+13,20,24),c);Fill(new Rect(r.x+67,r.y+13,20,24),c);Fill(new Rect(r.x+39,r.y+10,19,8),surface);Fill(new Rect(r.x+53,r.y+29,8,5),locked?muted:violet);}
            if(g.slot==1){Fill(new Rect(r.x+24,r.y+6,47,19),c);Fill(new Rect(r.x+24,r.y+25,21,g.shorts?23:48),c);Fill(new Rect(r.x+50,r.y+25,21,g.shorts?23:48),c);}
            if(g.slot==2){Fill(new Rect(r.x+14,r.y+26,40,25),c);Fill(new Rect(r.x+14,r.y+47,67,14),c);Fill(new Rect(r.x+14,r.y+61,67,5),paper);}
            if(g.slot==3){Fill(new Rect(r.x+23,r.y+20,49,34),c);Fill(new Rect(r.x+16,r.y+45,g.cap?72:63,10),c);}
        }
        void HandlePreview()
        {
            ProcessPreview(Event.current);
        }
        void ProcessPreview(Event e)
        {
            if(paused||stage!=Stage.Dressing)return;
            var area=new Rect(showRequest||showDrops?405:38,285,showRequest||showDrops?575:942,490);
            if(e.type==EventType.MouseDown&&e.button==0&&area.Contains(e.mousePosition)){dragging=true;moved=false;dragStart=e.mousePosition;e.Use();}
            if(dragging&&e.type==EventType.MouseDrag){rotation-=e.delta.x*.6f;moved|=(e.mousePosition-dragStart).sqrMagnitude>25;UpdateCamera();e.Use();}
            if(dragging&&e.type==EventType.MouseUp){dragging=false;if(!moved){showRequest=!showRequest;showDrops=false;}e.Use();}
            if(e.type==EventType.ScrollWheel&&area.Contains(e.mousePosition)){distance=Mathf.Clamp(distance+e.delta.y*.15f,5.3f,8.2f);UpdateCamera();e.Use();}
        }
        void PostUI()
        {
            Fill(new Rect(0,88,1440,812),new Color(.055f,.043f,.077f));
            Txt(new Rect(52,119,700,52),"FitFeed",heading);Txt(new Rect(52,170,700,30),"O VISUAL GANHOU A CIDADE.",tiny,violet);
            if(campaignBeforePreview==null){
                if(Tap(new Rect(262,119,180,44),"← Anterior",false,customer>0)){customer--;record=progress.posts[customer];Continue();}
                if(Tap(new Rect(460,119,180,44),"Próxima →",false,customer+1<progress.posts.Count)){customer++;record=progress.posts[customer];Continue();}
            }
            if(photo!=null)GUI.DrawTexture(new Rect(52,219,596,596),photo,ScaleMode.ScaleToFit);
            Txt(new Rect(704,225,650,55),Customer.name,heading);Txt(new Rect(704,282,650,30),Customer.location,caption,muted);
            Txt(new Rect(704,337,650,92),Customer.caption,copy);
            Rule(704,454,650);Txt(new Rect(704,480,500,85),record.score+" / 10",display);
            var c=DayOne.Evaluate(customer,record.outfit);
            for(int i=0;i<3;i++){
                Fill(new Rect(708,597+i*44,8,8),c[i]?violet:muted);
                Txt(new Rect(731,586+i*44,615,37),Customer.criteria[i]+"  /  "+(c[i]?(i==0?4:3):0)+" pontos",caption,c[i]?paper:muted);
            }
            Txt(new Rect(708,734,630,34),campaignBeforePreview!=null?"PRÉVIA  /  SEM ALTERAR A REPUTAÇÃO":"Contribuição "+(record.reputation>=0?"+":"")+record.reputation+"   /   Reputação total "+progress.reputation,copy,violet);
            Txt(new Rect(52,827,596,54),message, tiny,muted);
            if(campaignBeforePreview!=null){
                if(Tap(new Rect(704,805,390,60),"Continuar experimentando",true))stage=Stage.Dressing;
                if(Tap(new Rect(1114,805,240,60),"Menu"))ReturnToTitle();
            }else if(fromHome){
                if(Tap(new Rect(704,805,390,60),"Refazer nas condições originais",true))Replay();
                if(Tap(new Rect(1114,805,240,60),"Voltar para casa"))GoHome();
            }else{
                if(Tap(new Rect(704,805,390,60),progress.DayComplete?"Fechar expediente  →":"Próximo cliente  →",true)){if(progress.DayComplete)stage=Stage.Summary;else if(shopService){shopService=false;EnterShop();}else Begin();}
                if(Tap(new Rect(1114,805,240,60),"Menu"))ReturnToTitle();
            }
        }
    }
}
