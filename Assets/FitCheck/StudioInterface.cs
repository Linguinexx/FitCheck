using System;
using System.Linq;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        readonly Color paper=new Color(.94f,.92f,.86f), muted=new Color(.65f,.63f,.57f),
            surface=KaosPanel, violet=new Color(227/255f,6/255f,19/255f), line=KaosTrack;
        GUIStyle display, heading, copy, caption, action, tiny,nameEntry;
        Font uiFont,uiStrong,titleFont;bool fontDirty=true;
        void FontRebuilt(Font font){if(font==uiFont||font==uiStrong||font==titleFont)fontDirty=true;}
        void WarmFont()
        {
            const string glyphs="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789áàâãéêíóôõúüçÁÀÂÃÉÊÍÓÔÕÚÜÇ .,:;!?+-−/\\%()[]‘’“”•…←→↺✓–—";
            foreach(int size in new[]{13,17,21})uiFont.RequestCharactersInTexture(glyphs,size,FontStyle.Normal);
            foreach(int size in new[]{14,15,17,18,21,30})uiStrong.RequestCharactersInTexture(glyphs,size,FontStyle.Normal);
            foreach(int size in new[]{30,86})titleFont.RequestCharactersInTexture(glyphs,size,FontStyle.Normal);
            uiFont.RequestCharactersInTexture("0123456789 /FITCHECKOUAFim de expedienteUma pausaO visual foi pra ruaA semana virou história.",86,FontStyle.Bold);
            fontDirty=false;
        }
        bool dragging, moved; Vector2 dragStart;
        readonly System.Collections.Generic.Dictionary<string,Texture2D> thumbnails=new System.Collections.Generic.Dictionary<string,Texture2D>();
        readonly System.Collections.Generic.Dictionary<string,Texture2D> lockedThumbnails=new System.Collections.Generic.Dictionary<string,Texture2D>();
        readonly System.Collections.Generic.Dictionary<string,Rect> thumbnailBounds=new System.Collections.Generic.Dictionary<string,Rect>();
        void BuildThumbnails()
        {
            foreach(var item in Catalog.All){
                var image=studio.Thumbnail(item);thumbnails[item.id]=image;
                var gray=new Texture2D(image.width,image.height,TextureFormat.RGBA32,false){filterMode=FilterMode.Point};var pixels=image.GetPixels();
                int minX=image.width,minY=image.height,maxX=-1,maxY=-1;
                for(int i=0;i<pixels.Length;i++){
                    if(pixels[i].a>.05f){int x=i%image.width,y=i/image.width;minX=Mathf.Min(minX,x);maxX=Mathf.Max(maxX,x);minY=Mathf.Min(minY,y);maxY=Mathf.Max(maxY,y);}
                    float value=pixels[i].grayscale*.55f;pixels[i]=new Color(value,value,value,pixels[i].a);
                }
                if(maxX>=minX){minX=Mathf.Max(0,minX-3);minY=Mathf.Max(0,minY-3);maxX=Mathf.Min(image.width-1,maxX+3);maxY=Mathf.Min(image.height-1,maxY+3);thumbnailBounds[item.id]=new Rect(minX/(float)image.width,minY/(float)image.height,(maxX-minX+1)/(float)image.width,(maxY-minY+1)/(float)image.height);}
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
            if(creatorPetPreview!=null)ClearCreatorPet();
            EndLiveTitle();
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
            uiFont=Resources.Load<Font>("Fonts/SpaceGrotesk-Regular");uiStrong=Resources.Load<Font>("Fonts/SpaceGrotesk-Bold");titleFont=Resources.Load<Font>("Fonts/ArchivoBlack-Regular");Font.textureRebuilt+=FontRebuilt;
            display=new GUIStyle(GUI.skin.label){font=titleFont,fontSize=86,fontStyle=FontStyle.Normal,wordWrap=false};
            heading=new GUIStyle(GUI.skin.label){font=titleFont,fontSize=30,fontStyle=FontStyle.Normal,wordWrap=true};
            copy=new GUIStyle(GUI.skin.label){font=uiFont,fontSize=21,wordWrap=true};
            caption=new GUIStyle(copy){fontSize=17};tiny=new GUIStyle(copy){fontSize=13};
            action=new GUIStyle(copy){font=uiStrong,fontSize=17,fontStyle=FontStyle.Normal,alignment=TextAnchor.MiddleCenter};
            nameEntry=new GUIStyle(GUI.skin.textField){font=uiFont,fontSize=18,alignment=TextAnchor.MiddleLeft,padding=new RectOffset(10,10,6,6)};
            WarmFont();
        }
        void Fill(Rect r,Color c)
        {var old=GUI.color;GUI.color=c;GUI.DrawTexture(r,Texture2D.whiteTexture);GUI.color=old;}
        void Txt(Rect r,string text,GUIStyle style,Color? color=null)
        {SetLabelInk(style,color??paper);GUI.Label(r,text,style);}
        static void SetLabelInk(GUIStyle style,Color ink)
        {
            // Label hover inherits the skin color unless explicitly overridden.
            style.normal.textColor=ink;style.hover.textColor=ink;style.active.textColor=ink;style.focused.textColor=ink;
            style.onNormal.textColor=ink;style.onHover.textColor=ink;style.onActive.textColor=ink;style.onFocused.textColor=ink;
        }
        void Rule(float x,float y,float w){Fill(new Rect(x,y,w,1),line);}
        bool Tap(Rect r,string label,bool primary=false,bool enabled=true)
        {
            KaosButtonFace(r,label,primary,enabled);
            bool active=GUI.enabled;GUI.enabled=active&&enabled;
            bool clicked=GUI.Button(r,GUIContent.none,GUIStyle.none);GUI.enabled=active;if(clicked)Sound(0);return clicked;
        }
        void OnGUI()
        {
            if(ProofMode&&speechProofActive){DrawSpeechProof();return;}
            if(loadingActive){DrawLoadingScreen();return;}
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
            ControlsKeyInput(e);
            if(e.type==EventType.KeyDown&&e.keyCode==KeyCode.Escape){
#if !UNITY_WEBGL || UNITY_EDITOR
                HandleEscapeAction();
#endif
                // Web leaves Escape to the browser; P controls the game's pause.
                e.Use();
            }
            bool modal=paused || settingsOpen || renderedStage==Stage.Confirm||pendingDeleteSlot>=0;
            GUI.enabled=!modal;
            if(renderedStage==Stage.Title)TitleUI();else{
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
                Fill(new Rect(0,0,1440,900),new Color(.025f,.03f,.025f,.88f));
                DrawKaosPanel(new Rect(440,280,560,320));Fill(new Rect(478,301,36,3),CreatorRed);
                Txt(new Rect(478,316,490,55),"Esse é o visual?",heading);
                Txt(new Rect(478,381,475,65),"Depois do envio, o cliente sai com as peças que você escolheu.",copy,muted);
                if(Tap(new Rect(478,485,222,60),"Voltar ao provador"))stage=Stage.Dressing;
                if(Tap(new Rect(718,485,242,60),"Enviar visual",true))Submit();
            }
            if(paused&&!settingsOpen)PauseMenuUI();
            if(settingsOpen)SettingsUI();
            DrawAchievement();
            DrawPresentation();
            DrawAffordanceResponse();
            if(loadingActive)DrawLoadingScreen();
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
            string key=customer.ToString();if(speechRequestKey!=key){speechRequestKey=key;speechAge=0;}string answer=key+"/"+question;if(speechAnswerKey!=answer){speechAnswerKey=answer;answerAge=0;}
            CustomerBadge();
            Fill(new Rect(1000,88,440,812),surface);
            Txt(new Rect(1032,123,370,38),"Qual é a ocasião?",heading);
            ComicBubble(new Rect(1016,181,408,313),Request,Customer.name,speechAge);
            Rule(1032,505,366);
            if(Timed)Txt(new Rect(50,715,850,70),Customer.name+" está com pressa: você terá 120 segundos no provador. Pausa interrompe o relógio.",copy,violet);
            if(question<0){
                Txt(new Rect(1032,526,365,45),"Se quiser, pergunte um pouco mais.",caption,muted);
                for(int i=0;i<2;i++)if(Tap(new Rect(1032,581+i*89,366,76),questions[i]))question=i;
            }else{
                Txt(new Rect(1032,526,366,47),questions[question],caption,violet);
                ComicBubble(new Rect(1016,580,408,170),answers[question],Customer.name,answerAge);
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
                Fill(new Rect(38,343,315,375),KaosPanel);
                string guide=showRequest?"“"+Request+"”":DropCatalog.Guide;
                float guideHeight=caption.CalcHeight(new GUIContent(guide),247);
                detailsScroll=GUI.BeginScrollView(new Rect(54,361,282,335),detailsScroll,new Rect(0,0,255,Mathf.Max(680,guideHeight+24)));
                Txt(new Rect(0,0,247,guideHeight),guide,caption);
                if(showRequest&&question>=0)Txt(new Rect(0,450,247,220),answers[question],caption,violet);
                GUI.EndScrollView();
            }
            if(campaignBeforePreview!=null&&!showRequest&&!showDrops){
                Txt(new Rect(38,585,240,55),"Corpo compacto\nExpressão e cabelo ao seu jeito.",caption,muted);
                if(Tap(new Rect(38,711,250,48),"Expressão: "+studio.ExpressionName+"  ↔"))studio.CycleExpression();
                if(Tap(new Rect(38,772,250,48),"Cabelo: "+studio.HairName+"  ↔")){studio.CycleHair();Apply();}
            }
            Fill(new Rect(325,827,565,57),KaosPanel);
            Txt(new Rect(340,840,535,30),"ARRASTE PARA GIRAR   /   ROLE PARA APROXIMAR",tiny,paper);
            DrawWardrobeShelf();
            if(Tap(new Rect(910,836,66,38),"↺")){rotation=-12;distance=7.2f;UpdateCamera();}
            HandlePreview();
        }
        void ClothingIcon(Rect r,Garment g,bool locked)
        {
            if(thumbnails.ContainsKey(g.id)){
                var image=locked?lockedThumbnails[g.id]:thumbnails[g.id];
                if(thumbnailBounds.TryGetValue(g.id,out var uv)){
                    float aspect=image.width*uv.width/(image.height*uv.height),width=Mathf.Min(r.width,r.height*aspect),height=width/aspect;
                    GUI.DrawTextureWithTexCoords(new Rect(r.center.x-width/2,r.center.y-height/2,width,height),image,uv,true);
                }else GUI.DrawTexture(r,image,ScaleMode.ScaleToFit,true);
                return;
            }
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
            FeedStyles();
            Fill(new Rect(0,0,1440,900),FeedBackground);
            DrawFitFeedLogo(new Rect(60,52,246,54));
            Txt(new Rect(60,111,390,31),campaignBeforePreview!=null?"PRÉVIA DO VISUAL":"PUBLICAÇÃO DA DRIPLAB",tiny,muted);
            if(campaignBeforePreview==null){
                if(Tap(new Rect(774,78,168,44),"← Anterior",false,customer>0)){customer--;record=progress.posts[customer];Continue();}
                if(Tap(new Rect(953,78,168,44),"Próxima →",false,customer+1<progress.posts.Count)){customer++;record=progress.posts[customer];Continue();}
                if(fromHome&&Tap(new Rect(1132,78,245,44),"← Voltar ao feed")){stage=Stage.Retrospective;return;}
            }
            RoundedPanel(new Rect(60,158,614,679),FeedCardColor,13);
            FeedByline(new Rect(82,169,568,68),customer);
            FitFeedImage(new Rect(101,250,532,532),photo);
            Txt(new Rect(84,790,566,39),Customer.caption,caption,paper);

            RoundedPanel(new Rect(705,158,672,598),FeedCardColor,13);
            Txt(new Rect(735,180,594,36),"AVALIAÇÃO DO LOOK",caption,muted);
            Txt(new Rect(730,217,510,112),record.score+" / 10",display,paper);
            Fill(new Rect(738,335,602,5),FeedInset);
            if(record.score>0)Fill(new Rect(738,335,602*record.score/10f,5),CreatorRed);
            Txt(new Rect(736,366,550,36),"O pedido do cliente",heading,paper);
            var criteria=DayOne.Evaluate(customer,record.outfit);
            for(int i=0;i<3;i++){
                float y=421+i*66;
                RoundedPanel(new Rect(735,y,610,56),FeedInset,8);
                Txt(new Rect(752,y+11,37,34),criteria[i]?"✓":"–",action,criteria[i]?FeedAccent:muted);
                Txt(new Rect(799,y+10,424,39),Customer.criteria[i],caption,paper);
                Txt(new Rect(1230,y+10,104,36),criteria[i]?(i==0?"+4 pontos":"+3 pontos"):"0 pontos",action,criteria[i]?FeedAccent:muted);
            }
            Rule(735,634,610);
            Txt(new Rect(736,652,602,38),campaignBeforePreview!=null?"PRÉVIA  /  SEM ALTERAR A REPUTAÇÃO":"REPUTAÇÃO  "+(record.reputation>=0?"+":"")+record.reputation+"  •  TOTAL "+progress.reputation,copy,FeedAccent);
            Txt(new Rect(736,701,602,42),message,tiny,muted);
            if(campaignBeforePreview!=null){
                if(Tap(new Rect(705,777,420,60),"Continuar experimentando",true))stage=Stage.Dressing;
                if(Tap(new Rect(1145,777,232,60),"Menu"))ReturnToTitle();
            }else if(fromHome){
                if(Tap(new Rect(705,777,420,60),"Refazer nas condições originais",true))Replay();
                if(Tap(new Rect(1145,777,232,60),"Voltar ao quarto"))CloseHomePanel();
            }else{
                if(Tap(new Rect(705,777,420,60),progress.DayComplete?"Fechar expediente  →":"Próximo cliente  →",true)){if(progress.DayComplete)stage=Stage.Summary;else if(shopService){shopService=false;EnterShop();}else Begin();}
                if(Tap(new Rect(1145,777,232,60),"Menu"))ReturnToTitle();
            }
        }
    }
}
