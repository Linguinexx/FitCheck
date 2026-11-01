using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        [SerializeField] bool homeInterior;
        public bool IsHome=>homeInterior;
        public static readonly Rect HomeBounds=new Rect(-5.5f,-4.5f,11,9);
        public static readonly Vector2 HomeSpawn=new Vector2(0,.2f),HomeDoor=new Vector2(0,-3.5f),HomeBed=new Vector2(-1.5f,2.6f),HomeWardrobe=new Vector2(3,2),HomeDesk=new Vector2(-2.1f,-2.2f);
        public static readonly Vector2 StreetHomeDoor=new Vector2(21.8f,3.8f),StreetShopDoor=new Vector2(10.4f,2.1f);
        public void BuildHome()
        {
            interior=true;homeInterior=true;transform.position=new Vector3(600,0,0);
            Ground(0,0,11,9,-.07f,new Color(.64f,.47f,.32f),"Wood",2);
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
            Block(new Vector3(.7f,2.75f,4.43f),new Vector3(2.7f,1.9f,.1f),Ink,"CityMetal");
            Block(new Vector3(.7f,2.75f,4.35f),new Vector3(2.5f,1.7f,.03f),new Color(.12f,.035f,.07f),"CityGlow");
            for(int i=0;i<4;i++)Block(new Vector3(-.2f+i*.6f,2.5f,4.30f),new Vector3(.38f,.9f+i%2*.5f,.03f),new Color(.28f,.12f,.12f),"CityBrick");
            Block(new Vector3(.7f,2.75f,4.21f),new Vector3(.07f,1.8f,.05f),Cream,"Wood");
            Block(new Vector3(.7f,1.77f,4.21f),new Vector3(2.9f,.1f,.35f),Cream,"Wood");
            Block(new Vector3(-3.4f,2.9f,4.42f),new Vector3(1.45f,1.65f,.045f),Kaos,"Cloth");
            Words("DROP",new Vector3(-3.4f,2.95f,4.37f),.055f,Cream);
            Ground(.3f,-.5f,3.6f,2.4f,.005f,new Color(.30f,.10f,.15f),"Cloth",2);
            Block(new Vector3(4.5f,2.1f,-2.3f),new Vector3(1.45f,.12f,.65f),Ink,"Wood");
            for(int i=0;i<3;i++)Block(new Vector3(4.1f+i*.4f,2.3f,-2.3f),new Vector3(.30f,.30f,.46f),i%2==0?Kaos:Cream,"Paper");
            Block(new Vector3(0,4.25f,0),new Vector3(1.4f,.12f,.7f),Cream,"CityGlow");
            GeometryBatches=batches.Count;FinishGeometry();motionRigVersion=ResidentArtRevision;
        }
        public int HomeInteraction=>Vector2.Distance(Position,HomeDoor)<1.4f?1:Vector2.Distance(Position,HomeWardrobe)<1.35f?2:Vector2.Distance(Position,HomeBed)<1.25f?3:Vector2.Distance(Position,HomeDesk)<1.25f?4:0;
        public int StreetInteraction=>IsInterior?0:Vector2.Distance(Position,StreetShopDoor)<1.65f?1:Vector2.Distance(Position,StreetHomeDoor)<1.65f?2:0;
    }
    public partial class FirstCustomer
    {
        CityView homeRoom;
        void EnterHome(){shopService=false;EnterSpace(true,true);}
        void EnterStreetAt(Vector2 position,float yaw)
        {if(stage==Stage.Home&&city!=null&&city.IsHome)ExitCity();EnterCity();city.RestoreWalk(position,yaw,-4,mannequin);city.Frame(cam,true,0);}
        void CloseHomePanel()
        {stage=Stage.City;city=homeRoom;paused=false;cityFreeCursor=false;mannequin.gameObject.SetActive(true);studio.ApplyUniform(progress.uniform);city.PreparePlayerBody(studio);mannequin.gameObject.SetActive(false);SyncCityCursor(true);}
        bool InteractHome()
        {
            if(paused||stage!=Stage.City||!city.IsHome)return false;
            int action=city.HomeInteraction;if(action==0)return false;
            if(action==1){EnterStreetAt(CityView.StreetHomeDoor,180);return true;}
            showWardrobe=action==2;stage=Stage.Home;cityFreeCursor=true;SyncCityCursor(true);return true;
        }
        bool InteractStreet()
        {
            if(paused||stage!=Stage.City||city.IsInterior)return false;
            if(city.StreetInteraction==1){EnterShop();return true;}
            if(city.StreetInteraction==2){GoHome();return true;}return false;
        }
        string HomeActionLabel()=>city.HomeInteraction==1?"Sair de casa":city.HomeInteraction==2?"Abrir guarda-roupa":city.HomeInteraction==3?"Descansar / organizar o dia":city.HomeInteraction==4?"Celular / FitFeed":"";
    }
}
