using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public void CheckGalleryFacade()
        {
            if(watcher==null||watcher.root.localPosition.y<5.5f)throw new Exception("Observador sem apoio na sacada");
            int legs=0,body=0;foreach(var p in watcher.parts)if(!p.head&&p.mouthKind==0){if(!p.node.activeSelf)throw new Exception("Observador com corpo oculto");body++;if(p.leg)legs++;}
            if(body<4||legs<2)throw new Exception("Observador sem corpo completo");
            var brick=transform.Find("Arquitetura — CityBrick").GetComponent<MeshFilter>().sharedMesh;
            bool plinth=false;foreach(var v in brick.vertices)if(v.x> -104&&v.x< -89&&v.z>35.3f&&v.z<35.5f&&v.y>.4f&&v.y<.57f)plinth=true;
            if(!plinth)throw new Exception("Base da galeria continua aberta");
        }
    }
    public partial class FirstCustomer
    {
        void CheckGifGallery(string folder)
        {
            var original=progress;string storage=storageOverride;
            try{
                Home();progress=new DayProgress();storageOverride=Path.Combine(folder,"gif-"+Guid.NewGuid().ToString("N"));
                EnterCity();city.CheckGalleryFacade();city.CheckMapFinishing();
                cam.transform.position=city.transform.position+new Vector3(-101,2.7f,29.3f);cam.transform.LookAt(city.transform.position+new Vector3(-100.3f,6.7f,35.25f));cam.fieldOfView=54;SaveFirstPersonFrame(Path.Combine(folder,"gallery-balcony-fixed.png"));
                cam.transform.position=city.transform.position+new Vector3(-99,.75f,32);cam.transform.LookAt(city.transform.position+new Vector3(-96.5f,.6f,35.4f));cam.fieldOfView=66;SaveFirstPersonFrame(Path.Combine(folder,"gallery-base-closed.png"));
                EnterShop();OpenShopDecor();decorTab=1;decorView=3;
                for(int i=0;i<8;i++){
                    decorDraft.backdropBase=i;decorDraft.backdrop=(i+3)%8;city.ApplyShopDecor(decorDraft);
                    var m=city.transform.Find("Painel abstrato do provador").GetComponent<MeshRenderer>().sharedMaterial;
                    if(m.GetColor("_BackdropBase")!=CityView.FittingBaseColor(i)||m.GetColor("_BackdropColor")!=CityView.FittingLightColor((i+3)%8))throw new Exception("GIF não recebeu duas cores independentes");
                }
                decorDraft.backdropBase=2;decorDraft.backdrop=5;city.ApplyShopDecor(decorDraft);FrameShopDecor();CaptureInterface(Path.Combine(folder,"gif-purple-green.png"));CloseShopDecor(false);
                if(city.ActiveDecor.backdropBase!=0||city.ActiveDecor.backdrop!=0||File.Exists(ShopDecorPath))throw new Exception("Cancelar alterou combinação original");
                OpenShopDecor();decorTab=1;decorView=3;decorDraft.backdropBase=1;decorDraft.backdrop=2;city.ApplyShopDecor(decorDraft);FrameShopDecor();CaptureInterface(Path.Combine(folder,"gif-navy-pink.png"));CloseShopDecor(true);
                Home();savedDecor=null;EnterShop();var loaded=ShopDecorStore.Load(ShopDecorPath);if(loaded.backdropBase!=1||loaded.backdrop!=2||city.ActiveDecor.backdropBase!=1)throw new Exception("Combinação não persistiu");
                var legacy=JsonUtility.FromJson<ShopDecor>("{\"version\":1,\"backdrop\":4,\"led\":1}");legacy.Validate();if(legacy.backdropBase!=0||legacy.backdrop!=4)throw new Exception("Migração de combinação antiga falhou");
                Debug.Log("FITCHECK_GIF_GALLERY_PASSED: complete balcony observer, closed gallery base, eight independent background/shape colors, original black/purple preserved, cancel/save/reentry and legacy settings.");
            }finally{Home();progress=original;storageOverride=storage;savedDecor=null;decorPathLoaded=null;}
        }
    }
}
