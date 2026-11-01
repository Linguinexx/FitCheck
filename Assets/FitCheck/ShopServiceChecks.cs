using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        void SettleShopArrival()
        {
            int frames=0;
            while(ShopSequenceActive&&frames++<1400){city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);city.CheckResidents();city.ValidateShopRoles();}
            if(ShopSequenceActive)throw new Exception("Cliente não terminou de entrar na loja");
            city.Frame(cam,true,0);
        }
        void CheckShopServiceFlow(string folder)
        {
            var original=progress;var originalRecord=record;string originalStorage=storageOverride;
            try{
                Home();progress=new DayProgress();storageOverride=Path.Combine(folder,"service-session");Directory.CreateDirectory(storageOverride);
                EnterShop();SettleShopArrival();
                city.ValidateShopRoles();var waitingPosition=city.ResidentPosition(0);
                if(!city.CustomerWaitingAtCounter||Vector2.Distance(new Vector2(waitingPosition.x,waitingPosition.z),CityView.ShopCustomerQueue)>.06f)throw new Exception("Cliente não parou na frente do balcão");
                for(int frame=0;frame<120;frame++){city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);city.ValidateShopRoles();}
                if(stage!=Stage.City||shopService||!city.CustomerWaitingAtCounter||Vector3.Distance(waitingPosition,city.ResidentPosition(0))>.001f)throw new Exception("Cliente não aguardou atendimento do jogador");
                var ids=new System.Collections.Generic.HashSet<string>();
                foreach(Transform child in city.transform)if(child.name.StartsWith("Peça na arara — ")){
                    string id=child.name.Substring("Peça na arara — ".Length);
                    if(!ids.Add(id))throw new Exception("Peça repetida na arara: "+id);
                    int drop=id.StartsWith("SB")?0:id.StartsWith("AH")?1:2;
                    if(Mathf.Abs(child.localPosition.z-(-2+drop*3))>1.0f||Mathf.Abs(child.localPosition.x+6.9f)>.01f)throw new Exception("Peça fora do trilho: "+id);
                }
                if(ids.Count!=9)throw new Exception("Araras sem os nove tops dos três drops");
                MoveProof(new Vector2(0,6.3f),0,0);
                if(city.ShopInteraction==1||InteractShop())throw new Exception("Frequentador fixo abriu atendimento de cliente");
                MoveProof(CityView.ShopCounter,180,0);InteractShop();question=1;
                if(!shopService||stage!=Stage.Dialogue)throw new Exception("Conversa não inicia atendimento da loja");
                shopCameraTime=.5f;FrameShopDialogue();SaveFirstPersonFrame(Path.Combine(folder,"service-conversation.png"));
                ProceedToFitting();
                if(stage!=Stage.City||shopSequence!=ShopSequence.WalkingToFitting)throw new Exception("Cliente não foi ao provador depois da conversa");
                int originalCustomer=customer;customer=3;AdvanceClock(4);customer=originalCustomer;
                if(remaining!=120)throw new Exception("Cronômetro começou durante a caminhada");
                paused=true;var p=city.ResidentPosition(0);float elapsed=shopCameraTime;var pausedPosition=cam.transform.position;float pausedFov=cam.fieldOfView;
                UpdateCamera();UpdateShopSequence(1);
                if(city.ResidentPosition(0)!=p||shopCameraTime!=elapsed||cam.transform.position!=pausedPosition||cam.fieldOfView!=pausedFov)throw new Exception("Pausa alterou sequência/câmera");paused=false;
                bool movie=Array.IndexOf(Environment.GetCommandLineArgs(),"-service-motion")>=0;int movieFrame=0;
                string movieFolder=Path.Combine(folder,"service-motion");if(movie)Directory.CreateDirectory(movieFolder);
                float maxCameraStep=0,maxCameraTurn=0;
                for(int i=0;i<1400&&stage==Stage.City;i++){
                    var beforeCamera=cam.transform.position;var beforeRotation=cam.transform.rotation;
                    city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);
                    city.CheckResidents();
                    city.ValidateShopRoles();
                    maxCameraStep=Mathf.Max(maxCameraStep,Vector3.Distance(beforeCamera,cam.transform.position));
                    maxCameraTurn=Mathf.Max(maxCameraTurn,Quaternion.Angle(beforeRotation,cam.transform.rotation));
                    if(movie&&movieFrame<600)SaveFirstPersonFrame(Path.Combine(movieFolder,(movieFrame++).ToString("D4")+".png"),960,540);
                    if(i==65)SaveFirstPersonFrame(Path.Combine(folder,"service-camera-approach.png"));
                    if(i==190)SaveFirstPersonFrame(Path.Combine(folder,"service-fitting-entry.png"));
                }
                if(stage!=Stage.Dressing||campaignBeforePreview!=null||question!=1||progress.posts.Count!=0||remaining!=120)throw new Exception("Provador perdeu campanha/pergunta ou começou relógio antes da montagem");
                if(!shopDressingInStore||!city.gameObject.activeSelf||city.CustomerVisible||!city.CustomerShadowVisible||city.PlayerBodyVisible||Vector3.Distance(mannequin.position,city.transform.position+CityView.ShopDressingPosition)>.001f)throw new Exception("Montagem saiu do provador ou perdeu a presença do cliente");
                for(int frame=0;frame<30;frame++){
                    var beforeCamera=cam.transform.position;shopDressingTime+=1/30f;FrameShopDressing();
                    maxCameraStep=Mathf.Max(maxCameraStep,Vector3.Distance(beforeCamera,cam.transform.position));
                    if(movie&&movieFrame<600)SaveFirstPersonFrame(Path.Combine(movieFolder,(movieFrame++).ToString("D4")+".png"),960,540);
                }
                if(maxCameraStep>.40f||maxCameraTurn>22)throw new Exception("Corte brusco na câmera do atendimento: "+maxCameraStep+" / "+maxCameraTurn);
                SaveFirstPersonFrame(Path.Combine(folder,"service-dressing-in-store.png"));
                customer=3;AdvanceClock(.25f);paused=true;float clock=remaining;AdvanceClock(5);paused=false;customer=originalCustomer;
                if(clock!=119.75f||remaining!=clock)throw new Exception("Cronômetro do provador/pausa incorreto");remaining=120;
                var dressingPosition=mannequin.position;rotation=90;FrameShopDressing();
                if(mannequin.position!=dressingPosition||Quaternion.Angle(mannequin.rotation,Quaternion.Euler(0,90,0))>.01f)throw new Exception("Rotação levou o cliente para fora do provador");rotation=0;
                outfit=new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""};Apply();Submit();
                if(shopSequence!=ShopSequence.ShowingFinishedFit||stage!=Stage.City||!city.RuntimeReady)throw new Exception("Cliente não apresenta o fit antes de sair");
                for(int i=0;i<1600&&stage==Stage.City;i++){
                    city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);
                    city.ValidateShopRoles();
                    if(i==170)SaveFirstPersonFrame(Path.Combine(folder,"service-customer-exit.png"));
                }
                if(stage!=Stage.Post||progress.posts.Count!=1||!File.Exists(DayPath))throw new Exception("Saída não chegou ao FitFeed com um atendimento salvo");
                if(shopDressingInStore||mannequin.position!=Vector3.zero)throw new Exception("Provador físico não foi encerrado ao finalizar");
                GoHome();if(shopService)throw new Exception("Atendimento vazou para a casa/repetição");Home();EnterShop();SettleShopArrival();
                if(city.ShopCustomerIndex!=1)throw new Exception("Próximo cliente não corresponde ao próximo pedido");
                MoveProof(CityView.ShopCounter,0,0);InteractShop();ProceedToFitting();
                for(int i=0;i<30;i++){city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);}
                Home();if(shop!=null||shopDressingInStore||mannequin.position!=Vector3.zero||progress.posts.Count!=1)throw new Exception("Cancelar caminhada deixou atendimento parcial na loja");
                EnterShop();SettleShopArrival();if(!city.CustomerVisible)throw new Exception("Cliente ficou invisível ao retomar");
                shopService=true;EnterShopDressing();Home();
                if(shop!=null||shopDressingInStore||mannequin.position!=Vector3.zero||progress.posts.Count!=1)throw new Exception("Cancelar montagem deixou provador ativo");
                Debug.Log("FITCHECK_SHOP_SERVICE_PASSED: fixed regular retained, separate client arrives and waits at counter until player interaction; in-store dressing, continuous camera max step="+maxCameraStep+" / turn="+maxCameraTurn+"; departure, saved FitFeed, next client and cancellation.");
            }finally{Home();progress=original;record=originalRecord;storageOverride=originalStorage;Home();}
        }
    }
}
