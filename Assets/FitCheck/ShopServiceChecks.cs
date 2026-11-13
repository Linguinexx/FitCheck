using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        void SettleShopArrival()
        {
            if(!ShopOpen&&!progress.DayComplete){MoveProof(CityView.ShopCounter,180,0);InteractShop();}
            int frames=0;
            while((ShopSequenceActive||ShopArrivalActive||ShopOpen&&shopArrivalDelay>0)&&frames++<1400){city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);city.CheckResidents();city.ValidateShopRoles();}
            if(ShopSequenceActive||ShopArrivalActive)throw new Exception("Cliente não terminou de entrar na loja");
            city.Frame(cam,true,0);
        }
        void CheckShopServiceFlow(string folder)
        {
            var original=progress;var originalRecord=record;string originalStorage=storageOverride;
            try{
                Home();progress=new DayProgress();storageOverride=Path.Combine(folder,"service-session");Directory.CreateDirectory(storageOverride);
                EnterShop();
                if(ShopOpen||ShopSequenceActive||ShopArrivalActive||city.CustomerVisible)throw new Exception("Loja recebeu cliente antes de abrir");
                for(int i=0;i<120;i++){city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);}
                if(city.CustomerVisible||progress.posts.Count!=0)throw new Exception("Tempo na loja fechada iniciou atendimento");
                MoveProof(CityView.ShopCounter,180,0);var openingPosition=cam.transform.position;var openingRotation=cam.transform.rotation;float openingFov=cam.fieldOfView;
                paused=true;if(InteractShop()||ShopOpen)throw new Exception("Pausa abriu a loja");paused=false;
                if(!InteractShop()||!ShopOpen||!ShopArrivalActive||ShopSequenceActive)throw new Exception("Abrir a loja não iniciou chegada com câmera livre");
                for(int i=0;i<50;i++){
                    city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);
                    if(cam.transform.position!=openingPosition||Quaternion.Angle(cam.transform.rotation,openingRotation)>.001f||cam.fieldOfView!=openingFov)throw new Exception("Chegada tomou a câmera do jogador");
                }
                city.Look(new Vector2(12,-6));city.Step(new Vector2(-1,0),.05f,false,mannequin);city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);
                if(Vector2.Distance(city.Position,CityView.ShopCounter)<.1f||cam.transform.position==openingPosition)throw new Exception("Jogador não pode andar/olhar durante chegada");
                SettleShopArrival();
                city.ValidateShopRoles();city.CheckCustomerClothing(true);string arrivalIdentity=city.CustomerIdentitySignature();var waitingPosition=city.ResidentPosition(0);
                if(!city.CustomerWaitingAtCounter||Vector2.Distance(new Vector2(waitingPosition.x,waitingPosition.z),CityView.ShopCustomerQueue)>.06f)throw new Exception("Cliente não parou na frente do balcão");
                for(int frame=0;frame<120;frame++){city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);city.ValidateShopRoles();}
                if(stage!=Stage.City||shopService||!city.CustomerWaitingAtCounter||Vector3.Distance(waitingPosition,city.ResidentPosition(0))>.001f)throw new Exception("Cliente não aguardou atendimento do jogador");
                var ids=new System.Collections.Generic.HashSet<string>();
                foreach(Transform child in city.transform)if(child.name.StartsWith("Peça na arara — ")){
                    string id=child.name.Substring("Peça na arara — ".Length);
                    if(!ids.Add(id))throw new Exception("Peça repetida na arara: "+id);
                    int drop=id.StartsWith("SB")?0:id.StartsWith("AH")?1:2;
                    if(Mathf.Abs(child.localPosition.z-CityView.ShopRackCenter(drop))>CityView.ShopRackHalfLength-.25f||Mathf.Abs(child.localPosition.x+6.9f)>.01f)throw new Exception("Peça fora do trilho: "+id);
                }
                if(ids.Count!=9)throw new Exception("Araras sem os nove tops dos três drops");
                MoveProof(new Vector2(0,6.3f),0,0);
                cam.transform.LookAt(city.ShopGuideTarget);
                if(!InteractShop()||!shopGuideOpen||shopService||stage!=Stage.City||progress.posts.Count!=0)
                    throw new Exception("Guia não abriu ajuda independente do atendimento");
                CloseShopGuide();
                MoveProof(CityView.ShopCounter,180,0);InteractShop();question=1;
                if(!shopService||stage!=Stage.Dialogue)throw new Exception("Conversa não inicia atendimento da loja");
                shopCameraTime=.5f;FrameShopDialogue();SaveFirstPersonFrame(Path.Combine(folder,"service-conversation.png"));
                city.CheckCustomerClothing(true);ProceedToFitting();
                var employeePosition=city.PlayerBodyPosition;
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
                    city.CheckResidents();if(city.CustomerVisible)city.CheckCustomerClothing(true);
                    city.ValidateShopRoles();
                    if(!city.PlayerBodyVisible||Vector3.Distance(employeePosition,city.PlayerBodyPosition)>.001f)throw new Exception("Jogador sumiu ou saiu do balcão durante acompanhamento");
                    if(i==65&&!city.PlayerHeadVisible)throw new Exception("Jogador ficou sem cabeça na câmera externa");
                    maxCameraStep=Mathf.Max(maxCameraStep,Vector3.Distance(beforeCamera,cam.transform.position));
                    maxCameraTurn=Mathf.Max(maxCameraTurn,Quaternion.Angle(beforeRotation,cam.transform.rotation));
                    if(movie&&movieFrame<600)SaveFirstPersonFrame(Path.Combine(movieFolder,(movieFrame++).ToString("D4")+".png"),960,540);
                    if(i==65){
                        SaveFirstPersonFrame(Path.Combine(folder,"service-camera-approach.png"));
                        var shotPosition=cam.transform.position;var shotRotation=cam.transform.rotation;float shotFov=cam.fieldOfView;
                        cam.transform.position=city.transform.position+new Vector3(1.8f,3.5f,4.2f);cam.transform.LookAt(employeePosition+Vector3.up*2.0f);cam.fieldOfView=60;
                        SaveFirstPersonFrame(Path.Combine(folder,"service-player-at-counter.png"));
                        cam.transform.SetPositionAndRotation(shotPosition,shotRotation);cam.fieldOfView=shotFov;
                    }
                    if(i==190)SaveFirstPersonFrame(Path.Combine(folder,"service-fitting-entry.png"));
                }
                if(stage!=Stage.Dressing||campaignBeforePreview!=null||question!=1||progress.posts.Count!=0||remaining!=120)throw new Exception("Provador perdeu campanha/pergunta ou começou relógio antes da montagem");
                if(!shopDressingInStore||!city.gameObject.activeSelf||city.CustomerVisible||!city.PlayerBodyVisible||!city.PlayerHeadVisible||Vector3.Distance(mannequin.position,city.transform.position+CityView.ShopDressingPosition)>.001f)throw new Exception("Montagem saiu do provador, recriou sombra artificial ou perdeu a presença do cliente/jogador");
                for(int slot=0;slot<3;slot++)if(outfit[slot]!=Catalog.ArrivalPiece(slot).id)throw new Exception("Peça de chegada trocada ao abrir o provador");
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
                city.CheckCustomerClothing(false);if(city.CustomerIdentitySignature()!=arrivalIdentity)throw new Exception("Clothing change replaced customer identity");
                if(Vector3.Distance(employeePosition,city.PlayerBodyPosition)>.001f)throw new Exception("Preparar saída teleportou jogador para a porta");
                for(int i=0;i<1600&&stage==Stage.City;i++){
                    city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);
                    city.ValidateShopRoles();
                    if(stage==Stage.City&&(!city.PlayerBodyVisible||Vector3.Distance(employeePosition,city.PlayerBodyPosition)>.001f))throw new Exception("Jogador saiu do balcão durante saída do cliente");
                    if(i==170)SaveFirstPersonFrame(Path.Combine(folder,"service-customer-exit.png"));
                }
                if(stage!=Stage.Post||progress.posts.Count!=1||!File.Exists(DayPath))throw new Exception("Saída não chegou ao FitFeed com um atendimento salvo");
                if(shopDressingInStore||mannequin.position!=Vector3.zero)throw new Exception("Provador físico não foi encerrado ao finalizar");
                EnterShop();if(Vector2.Distance(new Vector2(employeePosition.x,employeePosition.z),new Vector2(city.PlayerBodyPosition.x,city.PlayerBodyPosition.z))>.001f)throw new Exception("Próximo cliente teleportou jogador para a porta");
                UpdateShopSequence(BetweenCustomersDelay-.25f);
                if(city.CustomerVisible||ShopArrivalActive)throw new Exception("Próximo cliente chegou antes do intervalo");
                UpdateShopSequence(.25f);
                if(!ShopArrivalActive)throw new Exception("Próximo cliente não chegou após o intervalo");
                EnterCity();
                var postedCustomer=city.transform.Find("FitFeed — "+DayOne.Customers[0].name);
                if(!ShopOpen||!city.RuntimeReady||city.ResidentCount!=CityView.StreetResidentCount+1||postedCustomer==null||!postedCustomer.gameObject.activeInHierarchy)
                    throw new Exception("Cliente atendido ausente do bairro com a loja aberta");
                EnterShop();EnterCity();
                if(city.ResidentCount!=CityView.StreetResidentCount+1||city.transform.Find(postedCustomer.name)!=postedCustomer)
                    throw new Exception("Cliente do FitFeed duplicado ao retornar ao bairro");
                Home();GoHome();if(shopService)throw new Exception("Atendimento vazou para a casa/repetição");Home();EnterShop();SettleShopArrival();
                if(city.ShopCustomerIndex!=1)throw new Exception("Próximo cliente não corresponde ao próximo pedido");city.CheckCustomerClothing(true);
                MoveProof(CityView.ShopCounter,0,0);InteractShop();ProceedToFitting();
                for(int i=0;i<30;i++){city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);}
                Home();if(shop!=null||shopDressingInStore||mannequin.position!=Vector3.zero||progress.posts.Count!=1)throw new Exception("Cancelar caminhada deixou atendimento parcial na loja");
                EnterShop();SettleShopArrival();if(!city.CustomerVisible)throw new Exception("Cliente ficou invisível ao retomar");
                shopService=true;EnterShopDressing();Home();
                if(shop!=null||shopDressingInStore||mannequin.position!=Vector3.zero||progress.posts.Count!=1)throw new Exception("Cancelar montagem deixou provador ativo");
                progress=new DayProgress();shopOpenedCampaign=progress;shopOpenedDay=1;
                for(int i=0;i<3;i++)progress.Add(new Attempt{outfit=(string[])Catalog.Default.Clone(),question=1});progress.Close();
                EnterCity();if(city.ResidentCount!=CityView.StreetResidentCount+3)throw new Exception("Fechar a loja removeu os clientes do dia");
                Home();progress.Advance();EnterCity();
                if(city.ResidentCount!=CityView.StreetResidentCount)throw new Exception("Clientes do dia anterior ficaram no bairro");
                EnterShop();if(ShopOpen||city.CustomerVisible||ShopArrivalActive)throw new Exception("Novo dia abriu automaticamente");
                MoveProof(CityView.ShopCounter,180,0);InteractShop();if(!ShopOpen||!ShopArrivalActive)throw new Exception("Não foi possível abrir o novo dia");
                Debug.Log("FITCHECK_SHOP_SERVICE_PASSED: manual daily opening; free first-person arrival and waiting client; full player retained at counter during fitting camera; in-store dressing, continuous camera max step="+maxCameraStep+" / turn="+maxCameraTurn+"; pause, departure, saved FitFeed, next client, cancellation and next-day opening.");
            }finally{Home();progress=original;record=originalRecord;storageOverride=originalStorage;Home();}
        }
    }
}
