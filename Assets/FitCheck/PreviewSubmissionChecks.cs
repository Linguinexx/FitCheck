using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        public void PreviewSubmissionSmokeCheck()
        {
            var oldProgress=progress;var oldCampaign=campaignBeforePreview;var oldRecord=record;var oldOutfit=outfit;var oldInventory=inventory;
            var oldStorage=storageOverride;var oldStage=stage;int oldCustomer=customer,oldReplay=replayIndex,oldQuestion=question;
            bool oldFromHome=fromHome,oldPaused=paused,oldPreview=studio.PreviewProportions,oldTall=studio.TallPreview;
            int oldHair=studio.HairOverride,oldExpression=studio.ExpressionOverride;var oldPending=pendingPhotoBytes;var oldPendingPath=pendingPhotoPath;
            string original=JsonUtility.ToJson(progress);int cases=0;
            try{
                pendingPhotoBytes=null;pendingPhotoPath=null;campaignBeforePreview=null;paused=false;
                PreviewSession();EnterDressing();string previewState=JsonUtility.ToJson(progress);
                var files=Directory.GetFiles(storageOverride).ToDictionary(p=>p,p=>File.GetLastWriteTimeUtc(p));
                foreach(var garment in Catalog.All){
                    outfit=Catalog.Default;outfit[garment.slot]=garment.id;Apply();stage=Stage.Confirm;Submit();
                    if(stage!=Stage.Leaving||photo==null||!record.available.Contains(garment.id)||!record.outfit.SequenceEqual(outfit))throw new Exception("Envio da prévia falhou: "+garment.id);
                    if(JsonUtility.ToJson(progress)!=previewState||pendingPhotoBytes!=null||record.reputation!=0)throw new Exception("Prévia alterou progresso ou tentou salvar");cases++;
                }
                if(Directory.GetFiles(storageOverride).Length!=files.Count||files.Any(p=>File.GetLastWriteTimeUtc(p.Key)!=p.Value))throw new Exception("Prévia gravou arquivos de progresso/foto");
                Home();if(!ReferenceEquals(progress,oldProgress)||JsonUtility.ToJson(progress)!=original)throw new Exception("Retorno da prévia alterou a campanha");
                bool rejected=false;try{DayProgress.CheckOutfit(new[]{"AH_TOP_01","AH_BOTTOM_01","SB_SHOES_01","SB_HEAD_01"},new DayProgress().Available);}catch(ArgumentException){rejected=true;}
                if(!rejected)throw new Exception("Peça bloqueada foi liberada na campanha normal");
                Debug.Log("FITCHECK_PREVIEW_SUBMISSION_PASSED: "+cases+" peças enviadas, incluindo bloqueadas na campanha; fotos em memória, zero arquivos alterados e campanha restaurada; bloqueios normais preservados.");
            }finally{
                progress=oldProgress;campaignBeforePreview=oldCampaign;record=oldRecord;outfit=oldOutfit;inventory=oldInventory;storageOverride=oldStorage;
                stage=oldStage;customer=oldCustomer;replayIndex=oldReplay;question=oldQuestion;fromHome=oldFromHome;paused=oldPaused;
                studio.PreviewProportions=oldPreview;studio.TallPreview=oldTall;studio.HairOverride=oldHair;studio.ExpressionOverride=oldExpression;
                pendingPhotoBytes=oldPending;pendingPhotoPath=oldPendingPath;Apply();UpdateCamera();
            }
        }
    }
}
