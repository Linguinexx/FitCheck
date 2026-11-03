using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        [SerializeField] bool interior;
        [SerializeField] int shopCustomerIndex;
        public bool IsInterior=>interior;
        public int ShopCustomerIndex=>shopCustomerIndex;
        public static readonly Rect ShopBounds=new Rect(-7.7f,-4.8f,15.4f,13.6f);
        public static readonly Vector2 ShopSpawn=new Vector2(0,-3.5f);
        public static readonly Vector2 ShopCounter=new Vector2(4.4f,7.65f),ShopCustomerQueue=new Vector2(4.4f,4.5f),ShopFitting=new Vector2(-4.9f,4.1f);
        static readonly Color FittingPurple=new Color(.29f,.37f,.29f);
        public void BuildShop(StudioView studio,int customer=0)
        {
            interior=true;shopCustomerIndex=customer;transform.position=new Vector3(400,0,0);
            ShopShell();ShopFixtures();ShopFittingRoom();ShopMerchandise(studio);
            BakeResident(studio,customer,ShopArrivalOutfit,new Vector3(.65f,-.055f,-4.3f),0);
            residents[0].root.name="Cliente — "+DayOne.Customers[customer].name;
            SetCustomerVisible(false);
            BakeResident(studio,0,new[]{"CG_TOP_01","SB_BOTTOM_01","SB_SHOES_01",""},new Vector3(-.8f,-.055f,7.96f),0,true);
            residents[1].root.name="Frequentador fixo — encostado na parede";
            Ground(-4.6f,-2.6f,1.5f,1.15f,-.048f,Kaos*.45f,"Rug");
            BuildCat(new Vector3(-4.6f,-.045f,-2.6f),270);obstacles.Add(new Rect(-5.30f,-3.12f,1.4f,1.04f));
            motionRigVersion=ResidentArtRevision;GeometryBatches=batches.Count;FinishGeometry();ResetFittingCurtain();
        }
        void ShopShell()
        {
            // A worn concrete floor with an inlaid red route ties the store to the street.
            Ground(0,2,16,14,-.055f,Ink,"Leather");
            for(int x=-7;x<=7;x+=2)for(int z=-4;z<=8;z+=2){
                float shade=(x+z+1)%4==0?.96f:1;
                Ground(x,z,1.975f,1.975f,-.052f,Cream*shade,"ShopFloor");
            }
            Ground(0,-2.1f,2.6f,4.4f,-.046f,Ink,"CityMetal",1.4f);
            Ground(-1.36f,-2.1f,.08f,4.4f,-.043f,Kaos,"Leather");
            Ground(1.36f,-2.1f,.08f,4.4f,-.043f,Kaos,"Leather");
            Block(new Vector3(-7.9f,2.35f,2),new Vector3(.3f,4.7f,14.0f),Cream,"CityStucco");
            Block(new Vector3(7.9f,2.35f,2),new Vector3(.3f,4.7f,14.0f),Ink,"CityBrick");
            Block(new Vector3(0,2.35f,9),new Vector3(16,4.7f,.3f),Cream,"CityStucco");
            Block(new Vector3(0,4.8f,2),new Vector3(16,.2f,14),Ink,"CityMetal");
            for(int side=-1;side<=1;side+=2){
                // Night-time shop windows: solid silhouettes behind glass preserve the indoor boundary.
                Block(new Vector3(side*4.85f,2.35f,-5),new Vector3(6.3f,4.7f,.25f),Ink,"CityMetal");
                Block(new Vector3(side*4.85f,2.4f,-4.84f),new Vector3(5.9f,3.9f,.08f),new Color(.12f,.105f,.10f),"CityWindow");
                Block(new Vector3(side*4.85f,2.4f,-4.74f),new Vector3(.10f,4,.1f),Cream,"CityMetal");
                Block(new Vector3(side*4.85f,.3f,-4.72f),new Vector3(6.3f,.6f,.3f),Ink,"CityMetal");
                Block(new Vector3(side*7.68f,.2f,2),new Vector3(.1f,.4f,13.5f),Kaos,"Leather");
            }
            Block(new Vector3(0,2.15f,-5),new Vector3(2.8f,4.3f,.18f),new Color(.16f,.15f,.14f),"CityWindow");
            for(int side=-1;side<=1;side+=2){
                Block(new Vector3(side*1.42f,2.3f,-4.8f),new Vector3(.12f,4.6f,.16f),Cream,"CityMetal");
                Block(new Vector3(side*.18f,1.8f,-4.72f),new Vector3(.05f,.66f,.1f),Cream,"CityMetal");
            }
            Block(new Vector3(0,4.4f,-4.8f),new Vector3(3,.18f,.18f),Kaos,"CityGlow");
            for(int x=-5;x<=5;x+=5){
                Block(new Vector3(x,4.56f,2),new Vector3(.08f,.12f,11.5f),Cream,"CityMetal");
                for(int z=-2;z<=7;z+=3){
                    Block(new Vector3(x,4.38f,z),new Vector3(.62f,.22f,.55f),Ink,"CityMetal");
                    Block(new Vector3(x,4.25f,z),new Vector3(.48f,.045f,.42f),new Color(1,.86f,.69f),"CityGlow");
                }
            }
            // Red pipes and conduits under the low ceiling add a recognisable retail silhouette.
            Rod(new Vector3(-7.5f,4.38f,7.9f),new Vector3(7.5f,4.38f,7.9f),.065f,Kaos,"CityMetal");
            Rod(new Vector3(7.5f,4.38f,7.9f),new Vector3(7.5f,.2f,7.9f),.065f,Kaos,"CityMetal");
        }
        void ShopFixtures()
        {
            // Back counter, with access around its left side rather than a blocked full-width wall.
            ShopDetailedCounter();
            Block(new Vector3(2.1f,1.675f,6.35f),new Vector3(.7f,.065f,.8f),Kaos,"Paper");
            Block(new Vector3(2.1f,1.715f,6.35f),new Vector3(.61f,.015f,.7f),Cream,"Paper");
            Block(new Vector3(2.1f,1.727f,6.35f),new Vector3(.45f,.01f,.08f),Kaos,"Leather");
            Block(new Vector3(0,1.1f,8.70f),new Vector3(2.2f,2.2f,.30f),Cream,"CityStucco");
            obstacles.Add(new Rect(-1.1f,8.55f,2.2f,.4f));
            obstacles.Add(new Rect(1.45f,5.45f,5.9f,1.6f));
            Words("DRIPLAB",new Vector3(3.6f,3.45f,8.80f),.145f,Kaos,"CityGlow");
            Block(new Vector3(3.6f,2.65f,8.76f),new Vector3(6.8f,.06f,.05f),Ink,"CityMetal");
            // Centre table and folded garments leave two clear circulation lanes.
            ShopDetailedTable();
            obstacles.Add(new Rect(-3.3f,-.75f,3.4f,2.1f));
            // Wall rail, hangers and shoe shelving.
            for(int z=-2;z<=4;z+=3){
                Rod(new Vector3(-6.9f,.05f,z-1.35f),new Vector3(-6.9f,3.1f,z-1.35f),.035f,Ink,"CityMetal");
                Rod(new Vector3(-6.9f,.05f,z+1.35f),new Vector3(-6.9f,3.1f,z+1.35f),.035f,Ink,"CityMetal");
                Rod(new Vector3(-6.9f,3.1f,z-1.35f),new Vector3(-6.9f,3.1f,z+1.35f),.035f,Ink,"CityMetal");
            }
            obstacles.Add(new Rect(-7.7f,-3.25f,1.8f,8.6f));
            for(int level=0;level<3;level++){
                float y=.75f+level*1.05f;
                ShopRoundedSolid(new Vector3(7.15f,y,1.1f),new Vector3(1.05f,.13f,7.9f),.16f,Cream,"Wood");
                ShopShoeBox(new Vector3(7.15f,y+.065f,-1.4f),level==0?Ink:level==1?new Color(.20f,.34f,.22f):Cream);
                ShopShoeBox(new Vector3(7.15f,y+.065f,3.6f),level==0?Ink:level==1?new Color(.20f,.34f,.22f):Cream);
            }
            obstacles.Add(new Rect(6.55f,-3,1.15f,8.0f));
            // In-world posters use the same printed atlas as the city signs.
            for(int i=0;i<3;i++){
                float z=-2+i*3;var center=new Vector3(-7.70f,3.85f,z);
                Block(center,new Vector3(.07f,.8f,2.1f),i==1?Kaos:Ink,"Paper");
                string text=i==0?"SIDE B":i==1?"AFTER HOURS":"COMMON GROUND";
                var printed=Get("CitySign");int first=printed.v.Count;
                Words(text,center+Vector3.right*.05f,i==0?.042f:i==1?.028f:.023f,Cream);
                var rotation=Quaternion.Euler(0,-90,0);var pivot=center+Vector3.right*.05f;
                for(int v=first;v<printed.v.Count;v++){printed.v[v]=pivot+rotation*(printed.v[v]-pivot);printed.n[v]=Vector3.right;}
            }
            // Seat near the entrance, outside the central path.
            ShopFixtureDetails();
            obstacles.Add(new Rect(3.5f,-3.8f,2.6f,1.2f));
        }
        void ShopFittingRoom()
        {
            Block(new Vector3(-4.9f,2.1f,8.72f),new Vector3(4.6f,4.2f,.16f),new Color(.055f,.035f,.075f),"CityStucco");
            Block(new Vector3(-2.5f,1.8f,7),new Vector3(.18f,3.6f,3.6f),Ink,"CityMetal");
            obstacles.Add(new Rect(-2.6f,5.2f,.2f,3.6f));
            Ground(-4.9f,7,4.6f,3.4f,-.045f,new Color(.13f,.07f,.19f),"CityStone",3);
            for(int side=-1;side<=1;side+=2){
                BuildFittingCurtain(side);
            }
            Rod(new Vector3(-7.4f,3.65f,5.15f),new Vector3(-2.5f,3.65f,5.15f),.045f,Cream,"CityMetal");
            Block(new Vector3(-4.9f,4.0f,5.15f),new Vector3(4.8f,.53f,.12f),Ink,"CityMetal");
            Words("PROVADOR",new Vector3(-4.9f,4.0f,5.06f),.058f,new Color(.90f,.80f,.57f),"CityGlow");
            // An abstract printed panel connects this entrance to the existing fitting-room stage.
            var panel=LiveBox("Painel abstrato do provador",new Vector3(-4.9f,2.1f,8.59f),new Vector3(2.3f,3.3f,.04f),Color.white,"CityMetal");
            var abstractMaterial=new Material(Resources.Load<Shader>("AbstractBackdrop"));abstractMaterial.mainTexture=Resources.Load<Texture2D>("Painted/AbstractFlow");assets.Add(abstractMaterial);
            panel.GetComponent<MeshRenderer>().sharedMaterial=abstractMaterial;
            for(int side=-1;side<=1;side+=2)Block(new Vector3(-4.9f+side*1.25f,2.1f,8.50f),new Vector3(.045f,3.5f,.06f),FittingPurple,"CityGlow");
            Block(new Vector3(-6.7f,.55f,7.2f),new Vector3(.75f,.18f,1.5f),Cream,"Wood");
            obstacles.Add(new Rect(-7.15f,6.35f,.9f,1.7f));
        }
        void ShopMerchandise(StudioView studio)
        {
            var rotation=studio.Avatar.rotation;studio.Avatar.rotation=Quaternion.identity;
            try{
                string[] drops={"SB","AH","CG"};
                for(int drop=0;drop<3;drop++)for(int piece=0;piece<3;piece++){
                    float z=-2+drop*3+(piece-1)*.87f;
                    var outfit=new[]{drops[drop]+"_TOP_0"+(piece+1),"SB_BOTTOM_01","SB_SHOES_01",""};
                    ShopGarment(studio,outfit,new Vector3(-6.9f,.70f,z),-90,false);
                    Rod(new Vector3(-6.9f,3.1f,z),new Vector3(-6.9f,2.98f,z),.018f,Cream,"CityMetal");
                    var apex=new Vector3(-6.9f,2.98f,z);var a=new Vector3(-6.9f,2.78f,z-.26f);var b=new Vector3(-6.9f,2.78f,z+.26f);
                    Rod(apex,a,.022f,new Color(.68f,.50f,.33f),"Wood");Rod(a,b,.022f,new Color(.68f,.50f,.33f),"Wood");Rod(b,apex,.022f,new Color(.68f,.50f,.33f),"Wood");
                }
                string[] shoes={"SB_SHOES_01","CG_SHOES_01","AH_SHOES_01"};
                for(int level=0;level<3;level++)
                    ShopGarment(studio,new[]{"SB_TOP_01","SB_BOTTOM_01",shoes[level],""},new Vector3(7.0f,.77f+level*1.05f,1.1f),90,true);
            }finally{studio.Avatar.rotation=rotation;}
        }
        void ShopGarment(StudioView studio,string[] outfit,Vector3 position,float yaw,bool shoes)
        {
            studio.Apply(outfit,0);var display=CopyAvatar(studio,shoes?"Tênis em exposição":"Peça na arara",position,yaw,true);
            display.root.name+=(" — "+(shoes?outfit[2]:outfit[0]));
            if(!shoes)display.root.localScale=new Vector3(.65f,1,1);
            foreach(var renderer in display.root.GetComponentsInChildren<MeshRenderer>()){
                string n=renderer.name;
                if(!shoes&&n=="Capuz")renderer.transform.localPosition-=Vector3.up*.32f;
                bool top=n=="Camiseta com caimento"||n.Contains("Manga")||n=="Gola"||n.Contains("Barra")||n.Contains("Estampa")||n.Contains("Carretel")||n.Contains("Grafismo")||n=="Capuz";
                bool foot=n.StartsWith("Tênis")||n.StartsWith("Sola")||n=="Cadarço";
                if(!(shoes?foot:top))renderer.gameObject.SetActive(false);
            }
        }
        public int ShopInteraction
        {
            get{
                if(!IsInterior)return 0;
                if(Mathf.Abs(Position.x+4.9f)<2&&Position.y>5.2f)return 2;
                if(Vector2.Distance(Position,ShopFitting)<1.8f)return 2;
                if(Vector2.Distance(Position,ShopCounter)<1.9f)return 1;
                if(Vector2.Distance(Position,ShopSpawn)<1.45f&&ViewYaw>110&&ViewYaw<250)return 3;
                return 0;
            }
        }
    }
}
