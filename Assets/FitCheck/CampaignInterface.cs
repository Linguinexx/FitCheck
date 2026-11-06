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
        static readonly string[] UniformAwards={"","Primeiro Expediente","Visual Perfeito","Sintonia com os Clientes"};
        static readonly string[] UniformNames={"Equipe DripLab","Primeiro Expediente","Visual Perfeito","Sintonia"};
        readonly Dictionary<string,Texture2D> feedPhotos=new Dictionary<string,Texture2D>();
        static readonly Color FeedBackground=new Color(.045f,.04f,.045f),FeedCardColor=new Color(.095f,.083f,.091f),
            FeedInset=new Color(.067f,.058f,.065f),FeedAccent=new Color(1f,.34f,.39f);
        Vector2 fitFeedScroll;
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
            SetMouseSensitivity(ProofMode?1:PlayerPrefs.GetFloat("FitCheck.MouseSensitivity",1));
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
        void Sound(int kind){if(kind==1)photoFlash=.16f;if(effects!=null&&!mutedAudio)effects.PlayOneShot(kind==0?clickSound:kind==1?photoSound:awardSound);}
        bool TrySave(){try{if(pendingPhotoBytes!=null)System.IO.File.WriteAllBytes(pendingPhotoPath,pendingPhotoBytes);SaveDay();pendingPhotoBytes=null;pendingPhotoPath=null;message="Progresso salvo.";return true;}catch(Exception ex){message="Não foi possível salvar. Tente novamente: "+ex.Message;return false;}}
        void NotifyAchievements(string[] before){foreach(string id in progress.achievements)if(!before.Contains(id))notices.Enqueue(id);}
        void DrawAchievement()
        {
            if(notice.Length==0&&notices.Count>0){notice=notices.Dequeue();noticeUntil=Time.unscaledTime+4;Sound(2);}
            if(notice.Length==0)return;
            float left=noticeUntil-Time.unscaledTime;if(left<=0){notice="";return;}
            float shift=Mathf.Clamp01((left-3.65f)/.35f)*420;
            var r=new Rect(994+shift,695,420,100);Fill(r,surface);Fill(new Rect(r.x,r.y,4,100),violet);
            Txt(new Rect(r.x+22,r.y+13,380,24),"CONQUISTA DESBLOQUEADA",tiny,violet);
            Txt(new Rect(r.x+22,r.y+43,380,45),notice,copy);
        }
        void HomeUI()
        {
            Fill(new Rect(0,88,600,812),new Color(.055f,.06f,.055f,.97f));
            Txt(new Rect(48,125,500,35),progress.Complete?"SEMANA CONCLUÍDA":"DIA 0"+progress.day+"  /  EM CASA",caption,violet);
            Txt(new Rect(48,178,510,85),progress.Complete?"Seu toque ficou.":"Seu canto.",heading);
            string intro=progress.Complete?"Os nove encontros viraram histórias no FitFeed. Você pode revisitar seus visuais e melhorar seus resultados.":progress.closed?"Expediente encerrado. Confira suas conquistas, reveja os posts e descanse para amanhã.":"Antes de sair, escolha seu uniforme. O bairro tem novas histórias esperando por você.";
            Txt(new Rect(48,260,500,125),intro,copy,muted);
            if(!showWardrobe){
                if(!progress.DayComplete&&Tap(new Rect(48,413,500,60),"Voltar ao quarto  →",true))CloseHomePanel();
                if(progress.DayComplete&&!progress.closed)Txt(new Rect(48,413,500,80),"Feche o expediente no balcão da DripLab antes de descansar.",copy,muted);
                if(progress.closed&&!progress.Complete&&Tap(new Rect(48,413,500,60),"Dormir e começar o próximo dia  →",true)){progress.Advance();TrySave();GoHome();}
                if(progress.Complete&&Tap(new Rect(48,413,500,60),"Receber recompensas pendentes",true)){progress.Deliver();TrySave();}
                Txt(new Rect(48,700,500,40),"Reputação "+progress.reputation+"   /   "+progress.Available.Length+" peças disponíveis",copy,violet);
                Txt(new Rect(48,755,500,44),progress.achievements.Count+" de 3 conquistas   /   "+progress.posts.Count+" de 9 publicações",caption,muted);
            }else{
                if(Tap(new Rect(845,806,210,48),"↶ Girar"))homeWardrobeRotation-=45;
                if(Tap(new Rect(1075,806,210,48),"Girar ↷"))homeWardrobeRotation+=45;
                for(int i=0;i<4;i++){
                    bool unlocked=i==0||progress.achievements.Contains(UniformAwards[i]);
                    if(Tap(new Rect(48,407+i*74,500,61),(progress.uniform==i?"✓ ":"")+UniformNames[i]+(unlocked?"":"  /  bloqueado"),progress.uniform==i,unlocked)){progress.uniform=i;studio.ApplyUniform(i);TrySave();}
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
            Txt(new Rect(90,145,1250,110),progress.day==3?"A semana virou história.":"Fim de expediente.",display);
            Txt(new Rect(96,278,1100,50),"Dia "+progress.day+"  /  Três ocasiões. Três histórias. Seu toque.",copy,muted);
            for(int j=0;j<3;j++){
                int i=(progress.day-1)*3+j;
                Fill(new Rect(95+j*423,376,395,145),new Color(.14f,.11f,.19f));
                Txt(new Rect(119+j*423,398,348,48),DayOne.Customers[i].name,heading);
                Txt(new Rect(119+j*423,463,348,43),progress.posts[i].score+" / 10",copy,violet);
            }
            Txt(new Rect(96,571,1000,60),"Reputação  /  "+progress.reputation,heading);
            if(!progress.closed){
                Txt(new Rect(96,645,1100,75),"As peças dos marcos alcançados chegam no fechamento.",copy,muted);
                if(Tap(new Rect(96,769,510,67),"Receber recompensas e fechar o dia",true)){
                    var before=progress.achievements.ToArray();progress.Close();NotifyAchievements(before);TrySave();Sound(2);
                }
            }else{
                Txt(new Rect(96,640,1200,90),progress.rewards.Count>0?"Peças conquistadas: "+string.Join("  /  ",progress.rewards):"Novos visuais podem melhorar sua Reputação. A campanha continua mesmo com notas baixas.",copy,violet);
                if(Tap(new Rect(96,769,510,67),"Voltar à loja  →",true)){shopService=false;EnterShop();}
            }
            Txt(new Rect(645,779,680,70),message,caption,muted);
        }
        void RetrospectiveUI()
        {
            Fill(new Rect(0,0,1440,900),FeedBackground);
            Fill(new Rect(96,126,38,4),CreatorRed);
            Txt(new Rect(92,145,386,108),"FitFeed",display,paper);
            Txt(new Rect(96,250,337,72),progress.Complete?"A semana no seu olhar.":"Os looks da DripLab pelo bairro.",copy,muted);
            RoundedPanel(new Rect(96,346,326,108),FeedCardColor,12);
            Txt(new Rect(114,366,137,44),progress.posts.Count.ToString("00"),heading,paper);
            Txt(new Rect(265,366,137,44),progress.reputation.ToString(),heading,paper);
            Txt(new Rect(116,412,137,26),"PUBLICAÇÕES",tiny,muted);
            Txt(new Rect(267,412,137,26),"REPUTAÇÃO",tiny,muted);
            if(Tap(new Rect(96,778,326,52),"← Voltar ao quarto")){CloseHomePanel();return;}
            Txt(new Rect(510,75,590,35),"PUBLICAÇÕES  /  MAIS RECENTES",caption,muted);
            if(progress.posts.Count==0){
                RoundedPanel(new Rect(510,125,644,270),FeedCardColor,13);
                Txt(new Rect(550,188,550,58),"Seu feed começa na loja.",heading,paper);
                Txt(new Rect(550,255,520,76),"Finalize o primeiro atendimento na DripLab para ver a foto e a avaliação do look aqui.",copy,muted);
                return;
            }
            const float cardHeight=760,viewportHeight=754;
            fitFeedScroll=GUI.BeginScrollView(new Rect(505,121,676,viewportHeight),fitFeedScroll,new Rect(0,0,646,progress.posts.Count*cardHeight),false,true);
            int opened=-1;
            for(int row=0;row<progress.posts.Count;row++){
                float y=row*cardHeight;
                if(y+cardHeight<fitFeedScroll.y||y>fitFeedScroll.y+viewportHeight)continue;
                int index=progress.posts.Count-1-row;
                var post=progress.posts[index];var client=DayOne.Customers[index];
                RoundedPanel(new Rect(5,y,634,747),FeedCardColor,13);
                FeedByline(new Rect(24,y+14,592,52),index);
                var image=new Rect(42,y+76,560,560);
                FitFeedImage(image,FeedPhoto(post.photo));
                if(GUI.Button(image,GUIContent.none,GUIStyle.none)){Sound(0);opened=index;}
                Txt(new Rect(27,y+644,574,44),client.caption,caption,paper);
                Txt(new Rect(27,y+698,195,35),"NOTA  "+post.score+" / 10",caption,FeedAccent);
                if(Tap(new Rect(358,y+692,261,42),"Ver avaliação  →"))opened=index;
            }
            GUI.EndScrollView();
            if(opened>=0){fromHome=true;customer=opened;record=progress.posts[opened];Continue();}
        }
        void FeedByline(Rect r,int index)
        {
            var client=DayOne.Customers[index];
            RoundedPanel(new Rect(r.x,r.y+4,44,44),CreatorRed,22);
            Txt(new Rect(r.x+2,r.y+5,40,42),client.name.Substring(0,1),action,paper);
            Txt(new Rect(r.x+55,r.y,r.width-62,28),client.name,copy,paper);
            Txt(new Rect(r.x+55,r.y+29,r.width-62,24),client.location+"  •  DIA 0"+(index/3+1),tiny,muted);
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
        void OnDestroy(){if(bubbleTail!=null)Destroy(bubbleTail);DisposePresentation();Font.textureRebuilt-=FontRebuilt;if(titlePhoto!=null)Destroy(titlePhoto);if(photo!=null)Destroy(photo);foreach(var t in lockedThumbnails.Values)Destroy(t);foreach(var t in feedPhotos.Values)Destroy(t);if(clickSound!=null)Destroy(clickSound);if(photoSound!=null)Destroy(photoSound);if(awardSound!=null)Destroy(awardSound);}
    }
}
