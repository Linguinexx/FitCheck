using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        public bool CustomerHasEntered {get;private set;}
        public bool CustomerAtDestination=>residents[0].path==null;
        public void WalkCustomer(params Vector2[] path)
        {
            var actor=residents[0];actor.wallLean=false;actor.oneShot=true;
            actor.path=path;actor.target=0;actor.wait=0;actor.speed=0;
        }
        public void ArriveCustomer()
        {
            CustomerHasEntered=true;var actor=residents[0];
            actor.root.localPosition=new Vector3(1.1f,-.055f,-4.3f);
            WalkCustomer(new Vector2(1.1f,2.8f),new Vector2(0,6.7f),new Vector2(0,7.96f));
        }
        public void RestCustomer()
        {
            var actor=residents[0];var p=actor.root.localPosition;p.y=-.055f;actor.root.localPosition=p;
            actor.home=p;actor.blend=0;actor.walking=false;actor.speed=0;
            actor.facing=Quaternion.identity;actor.root.localRotation=actor.facing;ConfigureShopLean(actor);
        }
        public void DressServiceCustomer(StudioView studio,string[] outfit,int customer)
        {
            var old=residents[0];old.root.gameObject.SetActive(false);Destroy(old.root.gameObject);
            if(old.shadow!=null){old.shadow.gameObject.SetActive(false);Destroy(old.shadow.gameObject);}
            residents.Clear();bool visible=studio.Avatar.gameObject.activeSelf;
            try{studio.Avatar.gameObject.SetActive(true);BakeResident(studio,customer,outfit,new Vector3(-4.9f,-.055f,6.4f),0);}
            finally{studio.Avatar.gameObject.SetActive(visible);}
        }
    }
    public partial class FirstCustomer
    {
        enum ShopSequence { None, Arriving, WalkingToFitting, EnteringFitting, Exiting }
        ShopSequence shopSequence;bool shopService;
        Vector3 shopCameraStart;Quaternion shopCameraRotation;float shopCameraTime;
        bool ShopSequenceActive=>shopSequence!=ShopSequence.None;
        void StartShopArrival()
        {
            if(city.CustomerHasEntered||progress.DayComplete)return;
            city.ArriveCustomer();shopSequence=ShopSequence.Arriving;
        }
        void ProceedToFitting()
        {
            if(!shopService){EnterDressing();return;}
            stage=Stage.City;cityFreeCursor=false;shopSequence=ShopSequence.WalkingToFitting;
            shopCameraStart=cam.transform.position;shopCameraRotation=cam.transform.rotation;shopCameraTime=0;
            city.WalkCustomer(new Vector2(0,6.7f),new Vector2(-3.2f,4.1f),new Vector2(-4.9f,4.1f));
        }
        void FrameShopDialogue()
        {
            cityFreeCursor=true;SyncCityCursor(true);
            city.Frame(cam,reducedMotion,paused?0:Time.unscaledDeltaTime);
            city.PlayerBodyVisible=false;mannequin.gameObject.SetActive(false);
            var focus=city.transform.position+city.ResidentPosition(0)+Vector3.up*2.15f;
            cam.transform.position=city.transform.position+new Vector3(.5f,2.65f,4.25f);
            cam.transform.LookAt(focus);cam.fieldOfView=52;
        }
        void UpdateShopSequence(float delta)
        {
            if(!ShopSequenceActive||paused)return;
            shopCameraTime+=delta;city.PlayerBodyVisible=false;
            var origin=city.transform.position;
            if(shopSequence==ShopSequence.Arriving){
                cam.transform.position=origin+new Vector3(3,CityView.EyeHeight,2.7f);
                cam.transform.LookAt(origin+city.ResidentPosition(0)+Vector3.up*2.1f);
                if(city.CustomerAtDestination){city.RestCustomer();shopSequence=ShopSequence.None;city.PlayerBodyVisible=true;}
                return;
            }
            if(shopSequence==ShopSequence.Exiting){
                cam.transform.position=origin+new Vector3(-3.8f,2.8f,3.2f);
                cam.transform.LookAt(origin+city.ResidentPosition(0)+Vector3.up*1.6f);
                if(city.CustomerAtDestination){
                    ExitCity();shop.gameObject.SetActive(false);Destroy(shop.gameObject);shop=null;
                    shopSequence=ShopSequence.None;stage=Stage.Post;UpdateCamera();
                }
                return;
            }
            float travel=Mathf.SmoothStep(0,1,Mathf.Clamp01(shopCameraTime/3.2f));
            var destination=origin+new Vector3(-4.9f,2.65f,2.2f);
            var view=Quaternion.LookRotation(origin+new Vector3(-4.9f,2.0f,6.4f)-destination);
            cam.transform.position=Vector3.Lerp(shopCameraStart,destination,travel);
            cam.transform.rotation=Quaternion.Slerp(shopCameraRotation,view,travel);
            if(shopSequence==ShopSequence.WalkingToFitting&&city.CustomerAtDestination){
                city.OpenFittingCurtain();shopSequence=ShopSequence.EnteringFitting;
            }
            if(shopSequence==ShopSequence.EnteringFitting&&city.FittingOpen){
                var p=city.ResidentPosition(0);
                if(p.z<6.3f&&city.CustomerAtDestination)city.WalkCustomer(new Vector2(-4.9f,6.4f));
                else if(p.z>=6.3f&&city.CustomerAtDestination&&travel>=1){
                    ExitCity();shopSequence=ShopSequence.None;Apply();EnterDressing();UpdateCamera();
                }
            }
        }
        void StartShopExit()
        {
            EnterSpace(true);city.DressServiceCustomer(studio,outfit,customer);
            city.OpenFittingCurtain();shopSequence=ShopSequence.Exiting;
            city.WalkCustomer(new Vector2(-4.9f,3),new Vector2(1.1f,2.6f),new Vector2(1.1f,-4.3f));
        }
        string ShopSequenceLabel=>shopSequence==ShopSequence.Arriving?"O cliente está chegando…":shopSequence==ShopSequence.Exiting?"Visual pronto. Até a próxima!":shopSequence==ShopSequence.WalkingToFitting?"Acompanhando até o provador…":"Preparando o provador…";
    }
}
