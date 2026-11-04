using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckNormalFlow(string folder)
        {
            var original=progress;string storage=storageOverride;
            try{
                Home();progress=new DayProgress();storageOverride=Path.Combine(folder,"normal-flow-"+Guid.NewGuid().ToString("N"));
                EnterShop();string before=JsonUtility.ToJson(progress);
                MoveProof(CityView.ShopFitting,0,4);
                for(int cycle=0;cycle<4;cycle++){
                    bool opening=cycle%2==0;
                    if(!InteractShop()||stage!=Stage.City||!shopFittingOpening)throw new Exception("Interação vazia não alternou cortina");
                    paused=true;float frozen=city.FittingProgress;city.Frame(cam,true,0);FinishShopFittingEntryIfReady();if(city.FittingProgress!=frozen)throw new Exception("Pausa moveu cortina");paused=false;
                    for(int i=0;i<30;i++){city.Frame(cam,true,1/30f);FinishShopFittingEntryIfReady();}
                    if(city.FittingOpen!=opening||shopFittingOpening||stage!=Stage.City||city.CustomerVisible||ShopOpen||shopService||campaignBeforePreview!=null||JsonUtility.ToJson(progress)!=before)throw new Exception("Provador vazio criou cliente ou mudou campanha");
                }
                OpenShopDecor();decorTab=1;decorView=3;decorDraft.backdrop=4;decorDraft.led=1;city.ApplyShopDecor(decorDraft);FrameShopDecor();
                CheckFittingPalette(4,1);CaptureInterface(Path.Combine(folder,"ui-fitting-lights.png"));CloseShopDecor(false);CheckFittingPalette(0,0);
                if(File.Exists(ShopDecorPath))throw new Exception("Cancelar persistiu luzes");
                OpenShopDecor();decorDraft.backdrop=2;decorDraft.led=4;city.ApplyShopDecor(decorDraft);CloseShopDecor(true);
                var loaded=ShopDecorStore.Load(ShopDecorPath);if(loaded.backdrop!=2||loaded.led!=4)throw new Exception("Cores não persistiram");
                var legacy=JsonUtility.FromJson<ShopDecor>("{\"version\":1,\"wall\":2}");legacy.Validate();if(legacy.backdrop!=0||legacy.led!=0)throw new Exception("Legado perdeu cores padrão");
                Home();savedDecor=null;EnterShop();CheckFittingPalette(2,4);
                MoveProof(new Vector2(2.2f,3),42,14);CaptureInterface(Path.Combine(folder,"ui-counter-clean.png"));
                paused=true;CaptureInterface(Path.Combine(folder,"ui-pause-simple.png"));paused=false;
                EnterCity();city.CheckMapFinishing();MoveProof(new Vector2(-94,24),85,23);CaptureInterface(Path.Combine(folder,"ui-plaza-no-strip.png"));
                Debug.Log("FITCHECK_NORMAL_FLOW_PASSED: repeated empty curtain open/close without customer, campaign unchanged, legacy settings, independent GIF/LED tint, cancel/save/reentry, native UI captures and removed overlapping asphalt.");
            }finally{Home();paused=false;progress=original;storageOverride=storage;savedDecor=null;decorPathLoaded=null;}
        }
        void CheckFittingPalette(int backdrop,int led)
        {
            var panel=city.transform.Find("Painel abstrato do provador").GetComponent<MeshRenderer>().sharedMaterial;
            if(panel.GetColor("_BackdropColor")!=CityView.FittingLightColor(backdrop))throw new Exception("Fundo não mudou cor");
            foreach(int side in new[]{-1,1})if(city.transform.Find("LED do provador "+side).GetComponent<MeshRenderer>().sharedMaterial.GetColor("_DecorTint")!=new Color(.29f,.37f,.29f))throw new Exception("Acabamento do provador mudou junto ao GIF");
        }
    }
}
