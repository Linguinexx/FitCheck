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
        void InvalidateFeedPhoto(string path){if(feedPhotos.TryGetValue(path,out var tex)){Destroy(tex);feedPhotos.Remove(path);}}
        Texture2D FeedPhoto(string path)
        {
            if(string.IsNullOrEmpty(path))return null;
            if(feedPhotos.TryGetValue(path,out var cached))return cached;
            try{if(!System.IO.File.Exists(path))return null;var tex=new Texture2D(2,2);tex.LoadImage(System.IO.File.ReadAllBytes(path));feedPhotos[path]=tex;return tex;}catch{return null;}
        }
        void BuildAudio()
        {
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
                if(progress.DayComplete&&!progress.closed&&Tap(new Rect(48,413,500,60),"Concluir fechamento  →",true))stage=Stage.Summary;
                if(progress.closed&&!progress.Complete&&Tap(new Rect(48,413,500,60),"Dormir e começar o próximo dia  →",true)){progress.Advance();TrySave();GoHome();}
                if(progress.Complete&&Tap(new Rect(48,413,500,60),"Receber recompensas pendentes",true)){progress.Deliver();TrySave();}
                if(Tap(new Rect(48,495,500,56),"Celular  /  FitFeed",false,progress.posts.Count>0)){fromHome=true;stage=Stage.Retrospective;}
                if(Tap(new Rect(48,572,500,56),"Guarda-roupa  /  Uniformes"))showWardrobe=true;
                if(Tap(new Rect(48,640,500,48),"Editar meu personagem"))OpenCreator(Stage.Home);
                Txt(new Rect(48,700,500,40),"Reputação "+progress.reputation+"   /   "+progress.Available.Length+" peças disponíveis",copy,violet);
                Txt(new Rect(48,755,500,44),progress.achievements.Count+" de 3 conquistas   /   "+progress.posts.Count+" de 9 publicações",caption,muted);
            }else{
                if(Tap(new Rect(845,806,210,48),"↶ Girar"))homeWardrobeRotation-=45;
                if(Tap(new Rect(1075,806,210,48),"Girar ↷"))homeWardrobeRotation+=45;
                for(int i=0;i<4;i++){
                    bool unlocked=i==0||progress.achievements.Contains(UniformAwards[i]);
                    if(Tap(new Rect(48,407+i*74,500,61),(progress.uniform==i?"✓ ":"")+UniformNames[i]+(unlocked?"":"  /  bloqueado"),progress.uniform==i,unlocked)){progress.uniform=i;studio.ApplyUniform(i);TrySave();}
                }
                if(Tap(new Rect(48,725,500,50),"Voltar"))showWardrobe=false;
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
                if(Tap(new Rect(96,769,510,67),"Voltar para casa  →",true))GoHome();
            }
            Txt(new Rect(645,779,680,70),message,caption,muted);
        }
        void RetrospectiveUI()
        {
            Fill(new Rect(0,88,1440,812),surface);
            Txt(new Rect(48,117,900,55),progress.Complete?"A semana no seu olhar.":"FitFeed  /  Suas histórias",heading);
            Txt(new Rect(48,174,1110,35),progress.posts.Count+" publicações   /   Reputação "+progress.reputation+"   /   Abra uma foto para rever ou refazer em casa.",caption,muted);
            if(Tap(new Rect(1190,120,200,48),"Voltar para casa"))GoHome();
            for(int i=0;i<progress.posts.Count;i++){
                var rect=new Rect(48+(i%3)*453,232+(i/3)*211,430,193);Fill(rect,new Color(.14f,.115f,.18f));
                var tex=FeedPhoto(progress.posts[i].photo);if(tex!=null)GUI.DrawTexture(new Rect(rect.x+10,rect.y+10,173,173),tex,ScaleMode.ScaleToFit);
                Txt(new Rect(rect.x+201,rect.y+16,213,53),DayOne.Customers[i].name,copy);
                Txt(new Rect(rect.x+201,rect.y+82,213,35),"Nota "+progress.posts[i].score+" / 10",copy,violet);
                Txt(new Rect(rect.x+201,rect.y+129,210,55),"DIA "+(i/3+1)+"  /  "+DayOne.Customers[i].location,tiny,muted);
                if(GUI.Button(rect,GUIContent.none,GUIStyle.none)){Sound(0);customer=i;record=progress.posts[i];Continue();}
            }
        }
        void OnApplicationFocus(bool focus){
            if(!focus&&(stage==Stage.Dressing||stage==Stage.Confirm)){paused=true;dragging=false;}
            if(!focus&&stage==Stage.City){dragging=false;SyncCityCursor();}
        }
        void OnDestroy(){if(bubbleTail!=null)Destroy(bubbleTail);DisposePresentation();Font.textureRebuilt-=FontRebuilt;if(titlePhoto!=null)Destroy(titlePhoto);if(photo!=null)Destroy(photo);foreach(var t in lockedThumbnails.Values)Destroy(t);foreach(var t in feedPhotos.Values)Destroy(t);if(clickSound!=null)Destroy(clickSound);if(photoSound!=null)Destroy(photoSound);if(awardSound!=null)Destroy(awardSound);}
    }
}
