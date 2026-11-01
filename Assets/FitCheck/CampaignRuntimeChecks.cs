using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        public void CampaignSmokeCheck(string[][] solutions)
        {
            var oldProgress=progress;var oldRecord=record;var oldStorage=storageOverride;var oldPreviewCampaign=campaignBeforePreview;campaignBeforePreview=null;int oldCustomer=customer;bool oldPaused=paused;
            storageOverride=Path.Combine(Application.temporaryCachePath,"FitCheck-Campaign-"+Guid.NewGuid());Directory.CreateDirectory(storageOverride);
            try{
                progress=new DayProgress();paused=false;Begin();EnterDressing();
                float angle=rotation;showRequest=false;showDrops=false;
                ProcessPreview(new Event{type=EventType.MouseDown,button=0,mousePosition=new Vector2(600,450)});
                ProcessPreview(new Event{type=EventType.MouseDrag,button=0,mousePosition=new Vector2(680,450),delta=new Vector2(80,0)});
                ProcessPreview(new Event{type=EventType.MouseUp,button=0,mousePosition=new Vector2(680,450)});
                if(rotation==angle||showRequest)throw new Exception("Arrasto não girou ou abriu pedido");
                ProcessPreview(new Event{type=EventType.MouseDown,button=0,mousePosition=new Vector2(600,450)});
                ProcessPreview(new Event{type=EventType.MouseUp,button=0,mousePosition=new Vector2(600,450)});
                if(!showRequest)throw new Exception("Clique não releu pedido");
                showRequest=false;float oldDistance=distance;
                ProcessPreview(new Event{type=EventType.ScrollWheel,mousePosition=new Vector2(600,450),delta=new Vector2(0,1000)});
                if(distance!=8.2f||distance==oldDistance)throw new Exception("Zoom sem limite");
                paused=true;angle=rotation;ProcessPreview(new Event{type=EventType.MouseDrag,delta=new Vector2(40,0)});
                if(rotation!=angle)throw new Exception("Pausa permitiu arrasto");paused=false;
                for(int i=0;i<9;i++){
                    Begin();question=i%2;outfit=(string[])solutions[i].Clone();Apply();EnterDressing();
                    if(i==3){
                        paused=true;AdvanceClock(10);if(remaining!=120)throw new Exception("Pausa não congelou timer");paused=false;
                        stage=Stage.Confirm;AdvanceClock(5);if(remaining!=115)throw new Exception("Confirmação congelou timer");
                        AdvanceClock(115);if(progress.posts.Count!=4)throw new Exception("Tempo esgotado não enviou");
                    }else Submit();
                    var saved=ProgressStore.Load(DayPath);if(saved.posts.Count!=i+1||!File.Exists(saved.posts[i].photo))throw new Exception("Atendimento não persistido");
                    customer=i;record=saved.posts[i];Continue();if(photo==null||photo.width!=480)throw new Exception("Foto não recuperada");
                    if((i+1)%3==0){progress.Close();SaveDay();if(i<8)progress.Advance();}
                }
                if(!progress.Complete||progress.reputation!=900||progress.Available.Length!=21)throw new Exception("Campanha incompleta");
                customer=0;fromHome=true;record=progress.posts[0];Replay();Submit();
                if(progress.reputation!=900||progress.posts.Count!=9)throw new Exception("Replay alterou campanha");
                foreach(var garment in Catalog.All){outfit=Catalog.Default;outfit[garment.slot]=garment.id;Apply();}
                if(thumbnails.Count!=21||lockedThumbnails.Count!=21)throw new Exception("Miniaturas incompletas");
                progress=new DayProgress();Begin();EnterDressing();Submit();string oldPhoto=progress.posts[0].photo;
                customer=0;record=progress.posts[0];fromHome=true;Replay();outfit=(string[])solutions[0].Clone();Apply();Submit();
                var backup=ProgressStore.Load(DayPath+".bak");
                if(progress.posts[0].photo==oldPhoto||backup.posts[0].photo!=oldPhoto||!File.Exists(oldPhoto)||!File.Exists(progress.posts[0].photo))throw new Exception("Replay quebrou associação de foto/backup");
                Debug.Log("FITCHECK_CAMPAIGN_RUNTIME_PASSED: arrasto/clique/zoom, 9 fotos/saves, timer/pausa/expiração, 3 dias, 21 peças renderizadas e replay sem crédito duplicado.");
            }finally{
                var testFolder=storageOverride;
                foreach(var file in Directory.GetFiles(testFolder))File.Delete(file);Directory.Delete(testFolder);
                progress=oldProgress;record=oldRecord;storageOverride=oldStorage;campaignBeforePreview=oldPreviewCampaign;customer=oldCustomer;paused=oldPaused;
                replayIndex=-1;fromHome=false;inventory=null;outfit=Catalog.Default;question=-1;stage=Stage.Welcome;notices.Clear();notice="";message="";Apply();
            }
        }
    }
}
