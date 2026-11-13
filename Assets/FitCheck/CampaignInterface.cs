using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool showWardrobe, mutedAudio,reducedMotion,retroFilter; AudioSource effects;
        AudioClip clickSound,photoSound,awardSound;Queue<string> notices=new Queue<string>();string notice="";float noticeUntil;
        const string FubaUniformName="Modo Fubá";
        static readonly string[] UniformNames={"Traje padrão","Corta-vento Rubro","Carpa Rosa","Azul Elétrico",FubaUniformName,DayProgress.NeighbourhoodAward,DayProgress.GarageDjAward,DayProgress.FittingGhostAward};
        readonly Dictionary<string,Texture2D> feedPhotos=new Dictionary<string,Texture2D>();
        static readonly Color FeedBackground=new Color32(23,21,25,255),FeedCardColor=new Color32(33,30,35,255),
            FeedPaper=new Color32(247,244,239,255),FeedMuted=new Color32(165,159,168,255),
            FeedDivider=new Color32(56,51,58,255),
            FeedInset=new Color(.067f,.058f,.065f),FeedAccent=new Color(1f,.34f,.39f);
        Vector2 fitFeedScroll;
        Texture2D fitFeedLogo;
        GUIStyle feedTitle, feedName, feedMetric, feedIntro, feedCopy, feedSmall, feedTag, feedRightTag, feedScore;

        void FeedStyles()
        {
            if(feedTitle!=null)return;
            feedTitle=FeedStyle(display,57);
            feedName=FeedStyle(heading,24);
            feedMetric=FeedStyle(display,32);
            feedIntro=FeedStyle(copy,27);
            feedCopy=FeedStyle(copy,16);
            feedSmall=FeedStyle(copy,14);
            feedTag=FeedStyle(tiny,11);
            feedRightTag=FeedStyle(tiny,11);
            feedRightTag.alignment=TextAnchor.UpperRight;
            feedScore=FeedStyle(action,19);
            feedScore.alignment=TextAnchor.UpperLeft;
        }

        static GUIStyle FeedStyle(GUIStyle basis,int size)
        {
            return new GUIStyle(basis){fontSize=size,padding=new RectOffset()};
        }

        void DrawFitFeedLogo(Rect bounds)
        {
            if(fitFeedLogo==null)fitFeedLogo=Resources.Load<Texture2D>("Brand/FitFeed-Flash-de-Esquina");
            if(fitFeedLogo!=null)GUI.DrawTexture(bounds,fitFeedLogo,ScaleMode.ScaleToFit,true);
            else Txt(bounds,"FitFeed",feedTitle,FeedPaper);
        }
        static readonly string[] FeedPortraitNames={"Kaio","Lia","Breno","Julia","SeuJota","NinaFaisca",null,"Dandara","Ravi","Joana","Teo","Iara","Dante","Cora"};
        Texture2D[] feedPortraits;
        void InvalidateFeedPhoto(string path){if(feedPhotos.TryGetValue(path,out var tex)){Destroy(tex);feedPhotos.Remove(path);}}
        Texture2D FeedPhoto(string path)
        {
            if(string.IsNullOrEmpty(path))return null;
            if(feedPhotos.TryGetValue(path,out var cached))return cached;
            Texture2D tex=null;
            try{
                if(!System.IO.File.Exists(path))return null;
                tex=new Texture2D(2,2);
                if(tex.LoadImage(System.IO.File.ReadAllBytes(path),true)){feedPhotos[path]=tex;return tex;}
            }catch(Exception ex) when(ex is System.IO.IOException||ex is UnauthorizedAccessException){}
            if(tex!=null)Destroy(tex);
            return null;
        }
        void BuildAudio()
        {
            SetGarmentFinishes(ProofMode||PlayerPrefs.GetInt("FitCheck.GarmentFinishes",1)==1);
            LoadMouseSensitivity();
            masterVolume=ProofMode?1:Mathf.Clamp01(PlayerPrefs.GetFloat(MasterVolumePreference,1));
            musicVolume=ProofMode?1:Mathf.Clamp01(PlayerPrefs.GetFloat(MusicVolumePreference,1));
            mutedAudio=PlayerPrefs.GetInt("FitCheck.Mute",0)==1;
            reducedMotion=PlayerPrefs.GetInt("FitCheck.ReducedMotion",0)==1;
            if(!ProofMode&&PlayerPrefs.GetInt("FitCheck.VisualDefaultsVersion",0)<2){
                PlayerPrefs.SetInt("FitCheck.Retro",0);PlayerPrefs.SetInt("FitCheck.VisualDefaultsVersion",2);PlayerPrefs.Save();
            }
            retroFilter=!ProofMode&&PlayerPrefs.GetInt("FitCheck.Retro",0)==1;
            effects=gameObject.AddComponent<AudioSource>();effects.playOnAwake=false;effects.spatialBlend=0;effects.volume=.2f;
            clickSound=Tone("Seleção",660,.055f);photoSound=Tone("Publicação",880,.15f);awardSound=Tone("Conquista",1046,.32f);
            if(FindAnyObjectByType<AudioListener>()==null)cam.gameObject.AddComponent<AudioListener>();
        }
        AudioClip Tone(string name,float frequency,float duration)
        {
            int count=Mathf.CeilToInt(22050*duration);var data=new float[count];
            for(int i=0;i<count;i++){float t=i/22050f;float env=Mathf.Min(1,t/.008f)*Mathf.Pow(1-t/duration,2);data[i]=Mathf.Sin(t*frequency*Mathf.PI*2)*env*.45f;}
            var clip=AudioClip.Create(name,count,1,22050,false);clip.SetData(data,0);return clip;
        }
        void Sound(int kind){if(kind==1)photoFlash=.16f;if(effects!=null&&!mutedAudio)PlayClip(kind==0?NextClickSound()??clickSound:kind==1?Clip("photo_capture")??photoSound:Clip("ui_confirm")??awardSound);}
        bool TrySave(){try{if(pendingPhotoBytes!=null)System.IO.File.WriteAllBytes(pendingPhotoPath,pendingPhotoBytes);SaveDay();pendingPhotoBytes=null;pendingPhotoPath=null;message="Progresso salvo.";return true;}catch(Exception ex){message="Não foi possível salvar. Tente novamente: "+ex.Message;return false;}}
        void NotifyAchievements(string[] before){foreach(string id in progress.achievements)if(!before.Contains(id))notices.Enqueue(id);}
        void DrawAchievement()
        {
            if(notice.Length==0&&notices.Count>0){notice=notices.Dequeue();noticeUntil=Time.unscaledTime+4;PlayClip(Clip("achievement")??awardSound);}
            if(notice.Length==0)return;
            float left=noticeUntil-Time.unscaledTime;if(left<=0){notice="";return;}
            float shift=Mathf.Clamp01((left-3.65f)/.35f)*420;
            var r=new Rect(994+shift,695,420,100);Fill(r,surface);Fill(new Rect(r.x,r.y,4,100),violet);
            Txt(new Rect(r.x+22,r.y+13,380,24),"CONQUISTA DESBLOQUEADA",tiny,violet);
            Txt(new Rect(r.x+22,r.y+43,380,45),notice,copy);
        }
        void HomeUI()
        {
            Fill(new Rect(0,88,600,812),KaosPanel);
            Txt(new Rect(48,125,500,35),progress.Complete?"SEMANA CONCLUÍDA":"DIA 0"+progress.day+"  /  EM CASA",caption,violet);
            Txt(new Rect(48,178,510,85),progress.Complete?"Seu toque ficou.":"Seu canto.",heading);
            string intro=progress.Complete?"Os encontros da semana viraram histórias no FitFeed. Você pode revisitar seus visuais e melhorar seus resultados.":progress.closed?"Expediente encerrado. Confira suas conquistas, reveja os posts e descanse para amanhã.":"Antes de sair, escolha seu uniforme. O bairro tem novas histórias esperando por você.";
            Txt(new Rect(48,260,500,125),intro,copy,muted);
            if(!showWardrobe){
                if(!progress.DayComplete&&Tap(new Rect(48,413,500,60),"Voltar ao quarto  →",true))CloseHomePanel();
                if(progress.DayComplete&&!progress.closed)Txt(new Rect(48,413,500,80),"Feche o expediente no balcão da DripLab antes de descansar.",copy,muted);
                if(progress.closed&&!progress.Complete&&Tap(new Rect(48,413,500,60),"Dormir e começar o próximo dia  →",true)){progress.Advance();TrySave();GoHome();}
                if(progress.Complete&&Tap(new Rect(48,413,500,60),"Receber recompensas pendentes",true)){int rewards=progress.rewards.Count;progress.Deliver();TrySave();if(progress.rewards.Count>rewards)PlayClip(Clip("unlock_drop"),.65f);}
                Txt(new Rect(48,700,500,40),"Reputação "+progress.reputation+"   /   "+progress.Available.Length+" peças disponíveis",copy,violet);
                int visibleAwards=3;
                for(int i=DayProgress.FubaUniform;i<UniformNames.Length;i++)
                    if(progress.UniformUnlocked(i))visibleAwards++;
                Txt(new Rect(48,755,500,44),progress.achievements.Count+" de "+visibleAwards+" conquistas   /   "+progress.posts.Count+" de "+DayProgress.TotalCustomers+" publicações",caption,muted);
            }else{
                if(Tap(new Rect(845,806,210,48),"↶ Girar"))homeWardrobeRotation-=45;
                if(Tap(new Rect(1075,806,210,48),"Girar ↷"))homeWardrobeRotation+=45;
                for(int i=0,row=0;i<UniformNames.Length;i++){
                    bool unlocked=progress.UniformUnlocked(i),selected=progress.EquippedUniform==i;
                    if(i>=DayProgress.FubaUniform&&!unlocked)continue;
                    if(Tap(new Rect(48,396+row++*40,500,36),(selected?"✓ ":"")+UniformNames[i]+(unlocked?"":"  /  bloqueado"),selected,unlocked)){progress.uniform=i;studio.ApplyUniform(i);TrySave();}
                }
                if(Tap(new Rect(48,725,500,50),"Editar meu personagem"))OpenCreator(Stage.Home);
            }
            if(Tap(new Rect(48,817,200,45),"Menu principal"))ReturnToTitle();
            if(Tap(new Rect(260,817,288,45),"Voltar ao quarto"))CloseHomePanel();
            Txt(new Rect(48,872,500,26),message,tiny,muted);
        }
        void SummaryUI()
        {
            Fill(new Rect(0,88,1440,812),surface);
            Txt(new Rect(90,145,1250,110),progress.day==DayProgress.TotalDays?"A semana virou história.":"Fim de expediente.",display);
            Txt(new Rect(96,278,1100,50),"Dia "+progress.day+"  /  Três ocasiões. Três histórias. Seu toque.",copy,muted);
            for(int j=0;j<DayProgress.CustomersPerDay;j++){
                int i=progress.FirstCustomerOfDay+j;
                RoundedPanel(new Rect(95+j*423,376,395,145),KaosControl,12);
                Txt(new Rect(119+j*423,398,348,48),DayOne.Customers[i].name,heading);
                Txt(new Rect(119+j*423,463,348,43),progress.posts[i].score+" / 10",copy,violet);
            }
            Txt(new Rect(96,571,1000,60),"Reputação  /  "+progress.reputation,heading);
            if(!progress.closed){
                Txt(new Rect(96,645,1100,75),"As peças dos marcos alcançados chegam no fechamento.",copy,muted);
                if(Tap(new Rect(96,769,510,67),"Receber recompensas e fechar o dia",true)){
                    int rewards=progress.rewards.Count;var before=progress.achievements.ToArray();progress.Close();NotifyAchievements(before);TrySave();if(progress.rewards.Count>rewards)PlayClip(Clip("unlock_drop"),.65f);else Sound(2);
                }
            }else{
                Txt(new Rect(96,640,1200,90),progress.rewards.Count>0?"Peças conquistadas: "+string.Join("  /  ",progress.rewards):"Novos visuais podem melhorar sua Reputação. A campanha continua mesmo com notas baixas.",copy,violet);
                if(Tap(new Rect(96,769,510,67),"Voltar à loja  →",true)){shopService=false;EnterShop();}
            }
            Txt(new Rect(645,779,680,70),message,caption,muted);
        }
        void RetrospectiveUI()
        {
            FeedStyles();
            int count=progress.posts.Count;
            DrawFeedBackdrop();
            Fill(new Rect(423,0,1,900),FeedDivider);
            DrawFitFeedLogo(new Rect(48,77,300,64));
            Txt(new Rect(48,160,328,40),"Da loja pra rua",feedIntro,FeedPaper);

            Fill(new Rect(48,230,328,1),FeedDivider);
            Txt(new Rect(48,259,156,22),"Publicações",feedSmall,FeedMuted);
            Txt(new Rect(48,287,156,42),count.ToString("00"),feedMetric,FeedPaper);
            Txt(new Rect(48,334,156,18),"looks no bairro",feedTag,FeedMuted);
            Txt(new Rect(220,259,156,22),"Nota média",feedSmall,FeedMuted);
            string average=count==0?"—":progress.posts.Average(post=>post.score)
                .ToString("0.#",System.Globalization.CultureInfo.GetCultureInfo("pt-BR"))+"/10";
            Txt(new Rect(220,287,156,42),average,feedMetric,FeedPaper);
            Txt(new Rect(220,334,156,18),"média das avaliações",feedTag,FeedMuted);
            Fill(new Rect(48,375,328,1),FeedDivider);

            Txt(new Rect(48,408,328,86),"Cada look tem uma história. Reveja os clientes e descubra como foi o atendimento na DripLab.",feedCopy,FeedMuted);
            if(Tap(new Rect(48,775,328,48),"← Voltar ao quarto")){CloseHomePanel();return;}
            Txt(new Rect(48,847,328,20),"ESTILO LOCAL. IMPACTO REAL.",feedTag,FeedMuted);

            const float cardX=656,cardWidth=552,firstCardY=52,viewportHeight=848;
            Txt(new Rect(cardX,24,200,17),"PELO BAIRRO",feedTag,FeedMuted);
            Txt(new Rect(cardX+252,24,300,17),"MAIS RECENTES PRIMEIRO",feedRightTag,FeedMuted);
            if(count==0){
                RoundedPanel(new Rect(cardX,firstCardY,cardWidth,180),FeedCardColor,8);
                Txt(new Rect(cardX+20,80,512,34),"Seu feed começa na loja.",feedName,FeedPaper);
                Txt(new Rect(cardX+20,124,512,70),"Finalize o primeiro atendimento na DripLab para ver a foto e a avaliação do look aqui.",feedCopy,FeedMuted);
                return;
            }

            const float cardHeight=738;
            float contentHeight=count*cardHeight-16+24;
            fitFeedScroll=GUI.BeginScrollView(new Rect(424,firstCardY,1016,viewportHeight),fitFeedScroll,
                new Rect(0,0,1016,contentHeight),false,false,GUIStyle.none,GUIStyle.none);
            int opened=-1;
            for(int row=0;row<count;row++){
                float y=row*cardHeight;
                if(y+cardHeight<fitFeedScroll.y||y>fitFeedScroll.y+viewportHeight)continue;
                int index=count-1-row;
                var post=progress.posts[index];
                const float left=232;
                RoundedPanel(new Rect(left,y,cardWidth,722),FeedCardColor,8);
                FeedByline(new Rect(left+20,y+20,512,68),index);
                var photoRect=new Rect(left+20,y+100,512,512);
                FitFeedImage(photoRect,FeedPhoto(post.photo));
                if(GUI.Button(photoRect,GUIContent.none,GUIStyle.none)){Sound(0);opened=index;}
                Txt(new Rect(left+20,y+626,512,34),DayOne.Customers[index].caption,feedCopy,FeedPaper);
                Txt(new Rect(left+20,y+664,186,14),"ATENDIMENTO",feedTag,FeedMuted);
                Txt(new Rect(left+20,y+678,186,24),"Nota "+post.score+"/10",feedScore,FeedPaper);
                if(Tap(new Rect(left+380,y+662,152,40),"Ver avaliação →",true))opened=index;
            }
            GUI.EndScrollView();

            const float trackHeight=852;
            Fill(new Rect(1414,24,2,trackHeight),FeedCardColor);
            if(contentHeight>viewportHeight){
                float thumbHeight=trackHeight*viewportHeight/contentHeight;
                float thumbY=24+(trackHeight-thumbHeight)*Mathf.Clamp01(fitFeedScroll.y/(contentHeight-viewportHeight));
                Fill(new Rect(1414,thumbY,2,thumbHeight),FeedMuted);
            }
            if(opened>=0){fromHome=true;customer=opened;record=progress.posts[opened];Continue();}
        }
        void FeedByline(Rect r,int index)
        {
            if(feedPortraits==null)feedPortraits=FeedPortraitNames.Select(name=>name==null?null:Resources.Load<Texture2D>("Artwork/PhoneAndProfiles/"+name)).ToArray();
            var client=DayOne.Customers[index];
            var avatar=new Rect(r.x,r.y,68,68);
            int profile=DayOne.CustomerProfile(index);
            var portrait=profile<feedPortraits.Length?feedPortraits[profile]:null;
            if(portrait!=null)GUI.DrawTexture(avatar,portrait,ScaleMode.ScaleToFit,true);
            else{
                RoundedPanel(avatar,FeedInset,34);
                Txt(avatar,client.name.Substring(0,1),action,paper);
            }
            FeedStyles();
            Txt(new Rect(r.x+84,r.y+8,r.width-84,30),client.name,feedName,FeedPaper);
            Txt(new Rect(r.x+84,r.y+42,r.width-84,23),client.location+" · Dia "+(index/DayProgress.CustomersPerDay+1).ToString("00"),feedSmall,FeedMuted);
        }
        void FitFeedImage(Rect r,Texture2D image)
        {
            Fill(r,FeedInset);
            if(image!=null)GUI.DrawTexture(r,image,ScaleMode.ScaleToFit);
            else Txt(new Rect(r.x+20,r.center.y-20,r.width-40,40),"Foto indisponível",action,muted);
        }
        void OnApplicationFocus(bool focus){
            if(!focus&&(stage==Stage.Dressing||stage==Stage.Confirm)){paused=true;dragging=false;}
            if(!focus&&stage==Stage.City){dragging=false;SyncCityCursor();}
        }
        void OnDestroy(){if(bubbleTail!=null)Destroy(bubbleTail);DisposePresentation();Font.textureRebuilt-=FontRebuilt;if(titlePhoto!=null)Destroy(titlePhoto);if(titleScrim!=null)Destroy(titleScrim);if(photo!=null)Destroy(photo);foreach(var t in lockedThumbnails.Values)Destroy(t);foreach(var t in feedPhotos.Values)Destroy(t);if(clickSound!=null)Destroy(clickSound);if(photoSound!=null)Destroy(photoSound);if(awardSound!=null)Destroy(awardSound);}
    }
}
