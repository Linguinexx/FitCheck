using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        [SerializeField] bool homeInterior;
        public bool IsHome=>homeInterior;
        public static readonly Rect HomeBounds=new Rect(-5.5f,-4.5f,11,9);
        public static readonly Vector2 HomeSpawn=new Vector2(0,.2f),HomeDoor=new Vector2(0,-3.5f),HomeBed=new Vector2(-1.5f,2.6f),HomeWardrobe=new Vector2(3,2),HomeDesk=new Vector2(-2.1f,-2.2f);
        public static readonly Vector2 StreetHomeDoor=new Vector2(-124.8f,7.65f),StreetHomeThreshold=new Vector2(-124.8f,9.9f),StreetShopDoor=new Vector2(10.4f,2.1f);
        public void BuildHome()
        {
            interior=true;homeInterior=true;transform.position=new Vector3(600,0,0);
            Ground(0,0,11,9,-.07f,new Color(.64f,.47f,.32f),"HomeFloor",1);
            Block(new Vector3(0,4.5f,0),new Vector3(11.3f,.18f,9.3f),Cream,"Plaster");
            foreach(float x in new[]{-5.6f,5.6f})Block(new Vector3(x,2.2f,0),new Vector3(.2f,4.5f,9.3f),Cream,"Plaster");
            foreach(float z in new[]{-4.6f,4.6f}){
                Block(new Vector3(0,2.2f,z),new Vector3(11.3f,4.5f,.2f),Cream,"Plaster");
                Block(new Vector3(0,.18f,z*.97f),new Vector3(11,.28f,.1f),Ink,"Wood");
            }
            // A compact lived-in room, at the same character scale as the shop.
            Block(new Vector3(-3.4f,.34f,2.3f),new Vector3(2.2f,.62f,3.4f),new Color(.33f,.20f,.13f),"Wood",.10f);
            Block(new Vector3(-3.4f,.76f,2.3f),new Vector3(2.13f,.31f,3.3f),Cream,"Cloth",.10f);
            Block(new Vector3(-3.4f,.96f,1.9f),new Vector3(2.16f,.15f,2.3f),Kaos,"Cloth",.08f);
            Block(new Vector3(-3.4f,1.10f,3.48f),new Vector3(1.40f,.24f,.62f),Cream,"Cloth",.10f);
            Block(new Vector3(-3.4f,.85f,4.05f),new Vector3(2.3f,1.6f,.16f),Ink,"Wood");
            obstacles.Add(new Rect(-4.5f,.6f,2.2f,3.5f));
            Block(new Vector3(4.4f,1.8f,2.9f),new Vector3(1.65f,3.6f,1.25f),new Color(.35f,.23f,.15f),"Wood",.09f);
            Block(new Vector3(4.4f,1.9f,2.24f),new Vector3(.035f,3.3f,.04f),Ink,"CityMetal");
            foreach(float x in new[]{4.2f,4.6f})Rod(new Vector3(x,1.65f,2.18f),new Vector3(x,2.12f,2.18f),.03f,Cream,"CityMetal");
            obstacles.Add(new Rect(3.55f,2.25f,1.7f,1.3f));
            Block(new Vector3(-3.8f,1.32f,-2.65f),new Vector3(2.5f,.16f,1.3f),new Color(.61f,.43f,.27f),"Wood");
            foreach(float x in new[]{-4.8f,-2.8f})foreach(float z in new[]{-3.1f,-2.2f})Block(new Vector3(x,.65f,z),new Vector3(.09f,1.3f,.09f),Ink,"CityMetal");
            Block(new Vector3(-3.3f,1.46f,-2.6f),new Vector3(.33f,.06f,.62f),Ink,"Rubber",.04f);
            Block(new Vector3(-3.3f,1.50f,-2.6f),new Vector3(.26f,.01f,.49f),new Color(.45f,.15f,.50f),"CityGlow");
            Block(new Vector3(-4.5f,1.52f,-2.7f),new Vector3(.7f,.23f,.52f),Kaos,"Paper");
            obstacles.Add(new Rect(-5.05f,-3.3f,2.5f,1.3f));
            // Door, poster, night window, shelf, and a rug give each corner a purpose.
            Block(new Vector3(0,1.85f,-4.43f),new Vector3(1.9f,3.7f,.14f),Ink,"Wood");
            Rod(new Vector3(.62f,1.5f,-4.28f),new Vector3(.62f,1.95f,-4.28f),.035f,Cream,"CityMetal");
            HomeNightWindow();
            FramedPrint("Quadro CREAM — quarto",new Vector3(-3.4f,2.9f,4.37f),0,2,new Vector2(1.45f,1.65f));
            FramedPrint("Quadro Sol — quarto",new Vector3(-5.43f,2.8f,-.15f),-90,5,new Vector2(1.45f,1.45f));
            Ground(.3f,-.5f,3.6f,2.4f,.005f,new Color(.30f,.10f,.15f),"Rug",1);
            Block(new Vector3(-3.8f,2.7f,-4.23f),new Vector3(2.4f,.12f,.50f),Ink,"Wood");
            foreach(float x in new[]{-4.6f,-3.0f}){
                Rod(new Vector3(x,2.30f,-4.44f),new Vector3(x,2.64f,-4.00f),.035f,Ink,"CityMetal");
                Rod(new Vector3(x,2.30f,-4.44f),new Vector3(x,2.67f,-4.44f),.035f,Ink,"CityMetal");
            }
            for(int i=0;i<3;i++)Block(new Vector3(-4.5f+i*.7f,2.92f,-4.23f),new Vector3(.45f,.30f,.42f),i%2==0?Kaos:Cream,"Paper");
            Block(new Vector3(0,4.25f,0),new Vector3(1.4f,.12f,.7f),Cream,"CityGlow");
            BuildCat(new Vector3(-3.4f,1.055f,1.6f),240);
            GeometryBatches=batches.Count;FinishGeometry();motionRigVersion=ResidentArtRevision;
        }
        public int HomeInteraction=>Vector2.Distance(Position,HomeDoor)<1.4f?1:Vector2.Distance(Position,HomeWardrobe)<1.35f?2:Vector2.Distance(Position,HomeBed)<1.25f?3:Vector2.Distance(Position,HomeDesk)<1.25f?4:0;
        public int StreetInteraction=>IsInterior?0:Vector2.Distance(Position,StreetShopDoor)<1.65f?1:Vector2.Distance(Position,StreetHomeDoor)<1.65f?2:0;
    }
    public partial class FirstCustomer
    {
        CityView homeRoom;
        float homeDoorExitRemaining,homeWardrobeRotation;
        static readonly Vector3 HomeWardrobePreviewPosition=new Vector3(2.8f,-.055f,.7f);
        void FrameHomeWardrobe()
        {
            city.Frame(cam,reducedMotion,0);city.PlayerBodyVisible=false;mannequin.gameObject.SetActive(true);
            mannequin.position=city.transform.position+HomeWardrobePreviewPosition;mannequin.rotation=Quaternion.Euler(0,homeWardrobeRotation,0);
            cam.transform.position=mannequin.position+new Vector3(.35f,2.6f,-5.0f);cam.transform.LookAt(mannequin.position+new Vector3(-.9f,1.75f,0));cam.fieldOfView=52;
        }
        void EnterHome(){shopService=false;EnterSpace(true,true);}
        void EnterStreetAt(Vector2 position,float yaw)
        {if(DeferLoading(()=>EnterStreetAt(position,yaw)))return;if(stage==Stage.Home&&city!=null&&city.IsHome)ExitCity();EnterCity();city.RestoreWalk(position,yaw,-4,mannequin);city.Frame(cam,true,0);}
        void CloseHomePanel()
        {
            if(homeRoom!=null&&!homeRoom.gameObject.activeSelf){var position=homeRoom.Position;float yaw=homeRoom.ViewYaw,pitch=homeRoom.ViewPitch;EnterHome();city.RestoreWalk(position,yaw,pitch,mannequin);}
            stage=Stage.City;city=homeRoom;paused=false;cityFreeCursor=false;mannequin.gameObject.SetActive(true);studio.ApplyUniform(progress.uniform);city.PreparePlayerBody(studio);mannequin.gameObject.SetActive(false);city.Frame(cam,true,0);SyncCityCursor(true);
        }
        bool InteractHome()
        {
            if(loadingActive||paused||stage!=Stage.City||!city.IsHome)return false;
            int action=city.HomeInteraction;if(action==0)return false;
            if(action==1){if(DeferLoading(()=>{EnterStreetAt(CityView.StreetHomeThreshold,180);homeDoorExitRemaining=.95f;}))return true;EnterStreetAt(CityView.StreetHomeThreshold,180);homeDoorExitRemaining=.95f;return true;}
            showWardrobe=action==2;homeWardrobeRotation=-12;fromHome=true;stage=action==4?Stage.Retrospective:Stage.Home;cityFreeCursor=true;SyncCityCursor(true);return true;
        }
        bool InteractStreet()
        {
            if(loadingActive||paused||stage!=Stage.City||city.IsInterior||homeDoorExitRemaining>0)return false;
            if(city.StreetInteraction==1){EnterShop();return true;}
            if(city.StreetInteraction==2){GoHome();return true;}return false;
        }
        string HomeActionLabel()=>city.HomeInteraction==1?"Sair de casa":city.HomeInteraction==2?"Abrir guarda-roupa":city.HomeInteraction==3?"Descansar / organizar o dia":city.HomeInteraction==4?"Celular / FitFeed":"";
    }
}
