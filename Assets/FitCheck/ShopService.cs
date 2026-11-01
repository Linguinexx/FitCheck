using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        public static readonly Vector3 ShopDressingPosition=new Vector3(-4.9f,-.055f,6.4f);
        public static string[] ShopArrivalOutfit=>new[]{"SB_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""};
        public bool CustomerHasEntered {get;private set;}
        bool customerWaiting;
        public bool CustomerWaitingAtCounter=>customerWaiting&&CustomerVisible;
        public bool CustomerAtDestination=>residents[0].path==null;
        public bool CustomerSettled=>residents[0].blend<.01f;
        public bool CustomerVisible=>residents[0].root.gameObject.activeSelf;
        public bool CustomerShadowVisible=>residents[0].shadow!=null&&residents[0].shadow.gameObject.activeSelf;
        public void WalkCustomer(params Vector2[] path)
        {
            var actor=residents[0];actor.wallLean=false;actor.oneShot=true;customerWaiting=false;
            actor.path=path;actor.target=0;actor.wait=0;actor.speed=0;
        }
        public void SetCustomerVisible(bool visible,bool keepShadow=false)
        {
            var actor=residents[0];actor.root.gameObject.SetActive(visible);
            if(actor.shadow!=null)actor.shadow.gameObject.SetActive(visible||keepShadow);
        }
        public bool TurnCustomer(float yaw,float delta)
        {
            var actor=residents[0];var goal=Quaternion.Euler(0,yaw,0);
            actor.facing=Quaternion.RotateTowards(actor.facing,goal,delta*170);
            actor.root.localRotation=actor.facing;
            return Quaternion.Angle(actor.facing,goal)<1;
        }
        public void ArriveCustomer()
        {
            CustomerHasEntered=true;var actor=residents[0];
            actor.root.localPosition=new Vector3(.65f,-.055f,-4.3f);
            SetCustomerVisible(true);
            WalkCustomer(new Vector2(1.1f,-1.8f),new Vector2(1.1f,2.7f),new Vector2(2.6f,4.0f),ShopCustomerQueue);
        }
        public void WaitCustomerAtCounter()
        {
            var actor=residents[0];var p=actor.root.localPosition;p.y=-.055f;actor.root.localPosition=p;
            actor.home=p;actor.blend=0;actor.walking=false;actor.speed=0;
            actor.wallLean=false;actor.leanBlend=0;actor.leanTarget=0;customerWaiting=true;
            actor.facing=Quaternion.Euler(0,180,0);actor.root.localRotation=actor.facing;
        }
        public void DressServiceCustomer(StudioView studio,string[] outfit,int customer)
        {
            var old=residents[0];old.root.gameObject.SetActive(false);Destroy(old.root.gameObject);
            if(old.shadow!=null){old.shadow.gameObject.SetActive(false);Destroy(old.shadow.gameObject);}
            residents.RemoveAt(0);bool visible=studio.Avatar.gameObject.activeSelf;
            try{
                studio.Avatar.gameObject.SetActive(true);BakeResident(studio,customer,outfit,ShopDressingPosition,0);
                var dressed=residents[residents.Count-1];residents.RemoveAt(residents.Count-1);residents.Insert(0,dressed);
                dressed.root.name="Cliente — "+DayOne.Customers[customer].name;
            }
            finally{studio.Avatar.gameObject.SetActive(visible);}
        }
    }
    public partial class FirstCustomer
    {
        enum ShopSequence { None, Arriving, SettlingArrival, ReturningToPlayer, LeavingRest, WalkingToFitting, EnteringFitting, PresentingFit, ShowingFinishedFit, Exiting }
        ShopSequence shopSequence;bool shopService,shopDressingInStore,shopCustomerEntering;
        Vector3 shopCameraStart;Quaternion shopCameraRotation;float shopCameraTime,shopCameraStartFov,shopDressingTime,shopDepartureTime;
        bool ShopSequenceActive=>shopSequence!=ShopSequence.None;
        void CaptureShopCamera()
        {shopCameraStart=cam.transform.position;shopCameraRotation=cam.transform.rotation;shopCameraStartFov=cam.fieldOfView;shopCameraTime=0;}
        void StartShopSequence(ShopSequence phase){shopSequence=phase;CaptureShopCamera();}
        static float ShopEase(float t){t=Mathf.Clamp01(t);return t*t*t*(t*(t*6-15)+10);}
        void BlendShopCamera(Vector3 position,Vector3 focus,float fov,float duration,float time)
        {
            float f=ShopEase(time/duration);
            cam.transform.position=Vector3.Lerp(shopCameraStart,position,f);
            cam.transform.rotation=Quaternion.Slerp(shopCameraRotation,Quaternion.LookRotation(focus-position),f);
            cam.fieldOfView=Mathf.Lerp(shopCameraStartFov,fov,f);
        }
        void StartShopArrival()
        {
            if(city.CustomerHasEntered||progress.DayComplete)return;
            city.ArriveCustomer();cam.transform.position=city.transform.position+new Vector3(3,3.0f,2.1f);
            cam.transform.LookAt(city.transform.position+city.ResidentPosition(0)+Vector3.up*1.9f);cam.fieldOfView=62;
            StartShopSequence(ShopSequence.Arriving);
        }
        void StartShopConversation()
        {
            var p=cam.transform.position;var r=cam.transform.rotation;float fov=cam.fieldOfView;
            stage=Stage.Welcome;Begin();shopService=stage==Stage.Dialogue;
            if(!shopService)return;
            outfit=CityView.ShopArrivalOutfit;Apply();
            cam.transform.SetPositionAndRotation(p,r);cam.fieldOfView=fov;CaptureShopCamera();FrameShopDialogue();
        }
        void ProceedToFitting()
        {
            if(!shopService){EnterDressing();return;}
            stage=Stage.City;cityFreeCursor=false;WalkClientToFitting();
        }
        void WalkClientToFitting()
        {
            city.WalkCustomer(new Vector2(2.6f,4.1f),new Vector2(.8f,4.0f),new Vector2(-2.3f,4.1f),new Vector2(-4.9f,4.1f));
            StartShopSequence(ShopSequence.WalkingToFitting);
        }
        void FrameShopDialogue()
        {
            cityFreeCursor=true;SyncCityCursor(true);
            city.Frame(cam,reducedMotion,paused?0:Time.unscaledDeltaTime);
            city.PlayerBodyVisible=false;mannequin.gameObject.SetActive(false);
            var focus=city.transform.position+city.ResidentPosition(0)+Vector3.up*2.15f;
            BlendShopCamera(city.transform.position+new Vector3(4.4f,2.65f,7.65f),focus,52,.45f,shopCameraTime);
        }
        void EnterShopDressing()
        {
            StartShopSequence(ShopSequence.None);shopDressingTime=0;shopDressingInStore=true;
            city.SetCustomerVisible(false,true);city.PlayerBodyVisible=false;
            studio.SetHome(false);Apply();mannequin.position=city.transform.position+CityView.ShopDressingPosition;
            mannequin.gameObject.SetActive(true);rotation=0;EnterDressing();FrameShopDressing();SyncCityCursor(true);
        }
        void FrameShopDressing()
        {
            city.Frame(cam,reducedMotion,0);city.PlayerBodyVisible=false;
            mannequin.position=city.transform.position+CityView.ShopDressingPosition;
            mannequin.rotation=Quaternion.Euler(0,rotation,0);
            BlendShopCamera(ShopDressingCameraPosition(),mannequin.position+new Vector3(.65f,1.75f,0),52,.70f,shopDressingTime);
        }
        Vector3 ShopDressingCameraPosition()
        {
            float zoom=Mathf.InverseLerp(5.3f,8.2f,distance),depth=Mathf.Lerp(4.2f,5.6f,zoom);
            return city.transform.position+CityView.ShopDressingPosition+new Vector3(.9f,2.5f,-depth);
        }
        void FrameWalkToFitting(Vector3 focus)
        {
            float t=ShopEase(shopCameraTime/4.8f),u=1-t;var origin=city.transform.position;
            var a=origin+new Vector3(8.5f,3.3f,2.0f);var b=origin+new Vector3(1.5f,3.0f,-1.5f);var end=origin+new Vector3(-4.9f,2.65f,.8f);
            cam.transform.position=u*u*u*shopCameraStart+3*u*u*t*a+3*u*t*t*b+t*t*t*end;
            cam.transform.rotation=Quaternion.Slerp(shopCameraRotation,Quaternion.LookRotation(focus-cam.transform.position),ShopEase(shopCameraTime/.5f));
            cam.fieldOfView=Mathf.Lerp(shopCameraStartFov,64,ShopEase(shopCameraTime/.8f));
        }
        void UpdateShopSequence(float delta)
        {
            if(!ShopSequenceActive||paused)return;
            shopCameraTime+=delta;city.PlayerBodyVisible=false;
            var origin=city.transform.position;var customerFocus=origin+city.ResidentPosition(0)+Vector3.up*1.9f;
            if(shopSequence==ShopSequence.Arriving||shopSequence==ShopSequence.SettlingArrival){
                BlendShopCamera(origin+new Vector3(3,3.0f,2.1f),customerFocus,62,.50f,shopCameraTime);
                if(shopSequence==ShopSequence.Arriving&&city.CustomerAtDestination)StartShopSequence(ShopSequence.SettlingArrival);
                else if(shopSequence==ShopSequence.SettlingArrival&&city.TurnCustomer(180,delta)&&city.CustomerSettled){
                    city.WaitCustomerAtCounter();StartShopSequence(ShopSequence.ReturningToPlayer);
                }
                return;
            }
            if(shopSequence==ShopSequence.ReturningToPlayer){
                var position=origin+new Vector3(city.Position.x,CityView.EyeHeight,city.Position.y);
                var rotation=Quaternion.Euler(city.ViewPitch,city.ViewYaw,0);
                BlendShopCamera(position,position+rotation*Vector3.forward,CityView.ViewFieldOfView,.75f,shopCameraTime);
                if(shopCameraTime>=.75f){shopSequence=ShopSequence.None;city.PlayerBodyVisible=true;}
                return;
            }
            if(shopSequence==ShopSequence.LeavingRest){
                cam.transform.SetPositionAndRotation(shopCameraStart,shopCameraRotation);cam.fieldOfView=shopCameraStartFov;
                WalkClientToFitting();
                return;
            }
            if(shopSequence==ShopSequence.ShowingFinishedFit){
                BlendShopCamera(origin+new Vector3(-4.9f,2.5f,1.7f),customerFocus,52,.65f,shopCameraTime);
                if(shopCameraTime>=.65f){
                    city.WalkCustomer(new Vector2(-4.9f,4.1f),new Vector2(-2.9f,3.6f),new Vector2(1.1f,2.6f),new Vector2(1.1f,-2.7f),new Vector2(.65f,-4.3f));
                    shopDepartureTime=0;StartShopSequence(ShopSequence.Exiting);
                }
                return;
            }
            if(shopSequence==ShopSequence.Exiting){
                BlendShopCamera(origin+new Vector3(5.2f,3.0f,2.8f),customerFocus,60,2.8f,shopCameraTime);
                if(city.CustomerAtDestination){
                    city.SetCustomerVisible(false);shopDepartureTime+=delta;
                    if(shopDepartureTime>=.35f){
                        ExitCity();if(shop!=null){shop.gameObject.SetActive(false);Destroy(shop.gameObject);shop=null;}stage=Stage.Post;UpdateCamera();
                    }
                }
                return;
            }
            bool walking=shopSequence==ShopSequence.WalkingToFitting;
            bool presenting=shopSequence==ShopSequence.PresentingFit;
            var destination=presenting?ShopDressingCameraPosition():origin+(walking?new Vector3(-4.9f,2.65f,.8f):new Vector3(-4.9f,2.5f,2.2f));
            var focus=presenting?origin+CityView.ShopDressingPosition+new Vector3(.65f,1.75f,0):customerFocus;
            if(walking)FrameWalkToFitting(customerFocus);else BlendShopCamera(destination,focus,52,presenting?.8f:1.4f,shopCameraTime);
            if(walking&&city.CustomerAtDestination){
                city.OpenFittingCurtain();shopCustomerEntering=false;StartShopSequence(ShopSequence.EnteringFitting);
            }else if(shopSequence==ShopSequence.EnteringFitting&&city.FittingOpen){
                if(!shopCustomerEntering){city.WalkCustomer(new Vector2(-4.9f,6.4f));shopCustomerEntering=true;}
                else if(city.CustomerAtDestination)StartShopSequence(ShopSequence.PresentingFit);
            }else if(shopSequence==ShopSequence.PresentingFit){
                if(city.TurnCustomer(0,delta)&&city.CustomerSettled&&shopCameraTime>=.35f)EnterShopDressing();
            }
        }
        void StartShopExit()
        {
            var p=cam.transform.position;var r=cam.transform.rotation;float fov=cam.fieldOfView;
            if(shopDressingInStore)ExitCity();
            EnterSpace(true);city.DressServiceCustomer(studio,outfit,customer);city.OpenFittingCurtain();
            cam.transform.SetPositionAndRotation(p,r);cam.fieldOfView=fov;StartShopSequence(ShopSequence.ShowingFinishedFit);
        }
        string ShopSequenceLabel=>shopSequence==ShopSequence.Arriving?"O cliente está chegando…":
            shopSequence==ShopSequence.SettlingArrival||shopSequence==ShopSequence.ReturningToPlayer?"Cliente pronto para o atendimento.":
            shopSequence==ShopSequence.ShowingFinishedFit||shopSequence==ShopSequence.Exiting?"Visual pronto. Até a próxima!":
            shopSequence==ShopSequence.LeavingRest||shopSequence==ShopSequence.WalkingToFitting?"Vamos até o provador…":"Preparando o provador…";
    }
}
