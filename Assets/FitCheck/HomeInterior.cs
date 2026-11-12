using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        [SerializeField] bool homeInterior;
        public bool IsHome=>homeInterior;
        public static readonly Rect HomeBounds=new Rect(-5.5f,-4.5f,11,9);
        public static readonly Vector2 HomeSpawn=new Vector2(0,.2f),HomeDoor=new Vector2(0,-3.5f),HomeBed=new Vector2(-1.5f,2.6f),HomeWardrobe=new Vector2(3,2),HomeDesk=new Vector2(-2.1f,-2.2f);
        public static readonly Vector3 HomePhonePosition=new Vector3(-3.3f,1.49f,-2.6f);
        public static readonly Vector3 HomeWardrobeCenter=new Vector3(4.4f,1.8f,3.875f);
        static readonly Vector3 HomeMirrorCenter=new Vector3(5.39f,1.83f,1.36f);
        const float HomeMirrorHalfWidth=.59f,HomeMirrorHalfHeight=1.62f;
        readonly Plane[] mirrorViewPlanes=new Plane[6];
        Camera homeMirrorCamera;
        Material homeMirrorMaterial;
        float nextHomeMirrorFrame;
        public static readonly Vector2 StreetHomeDoor=new Vector2(-124.8f,7.65f),StreetHomeThreshold=new Vector2(-124.8f,9.9f),StreetShopDoor=new Vector2(10.4f,2.1f);
        public void BuildHome(PetAppearance companion=null)
        {
            interior=true;homeInterior=true;transform.position=new Vector3(600,0,0);
            var petrol=new Color(.085f,.32f,.32f);var walnut=new Color(.44f,.24f,.125f);
            Ground(0,0,11,9,-.07f,new Color(.68f,.47f,.30f),"HomeFloor",1);
            Block(new Vector3(0,4.5f,0),new Vector3(11.3f,.18f,9.3f),new Color(.81f,.76f,.66f),"Plaster");
            foreach(float x in new[]{-5.6f,5.6f})Block(new Vector3(x,2.2f,0),new Vector3(.2f,4.5f,9.3f),Cream,"Plaster");
            foreach(float z in new[]{-4.6f,4.6f}){
                Block(new Vector3(0,2.2f,z),new Vector3(11.3f,4.5f,.2f),z>0?petrol:Cream,"Plaster");
                Block(new Vector3(0,.18f,z*.97f),new Vector3(11,.28f,.1f),Ink,"Wood");
            }
            // A compact lived-in room, at the same character scale as the shop.
            Block(new Vector3(-3.4f,.34f,2.3f),new Vector3(2.2f,.62f,3.4f),new Color(.33f,.20f,.13f),"Wood",.10f);
            Block(new Vector3(-3.4f,.76f,2.3f),new Vector3(2.13f,.31f,3.3f),Cream,"Cloth",.10f);
            Block(new Vector3(-3.4f,.96f,1.9f),new Vector3(2.16f,.15f,2.3f),new Color(.54f,.12f,.25f),"Cloth",.08f);
            Block(new Vector3(-3.4f,1.035f,2.94f),new Vector3(2.17f,.16f,.33f),new Color(.74f,.24f,.32f),"Cloth",.09f);
            foreach(float x in new[]{-3.91f,-2.89f})Block(new Vector3(x,1.08f,3.48f),new Vector3(.88f,.26f,.62f),Cream,"Cloth",.15f);
            Block(new Vector3(-3.4f,.85f,4.05f),new Vector3(2.3f,1.6f,.16f),walnut,"Wood");
            for(int i=0;i<10;i++)Block(new Vector3(-4.37f+i*.215f,1.11f,3.93f),new Vector3(.115f,.99f,.08f),walnut*1.16f,"Wood",.016f);
            obstacles.Add(new Rect(-4.5f,.6f,2.2f,3.5f));
            Block(HomeWardrobeCenter,new Vector3(1.65f,3.6f,1.25f),new Color(.35f,.23f,.15f),"Wood",.09f);
            Block(HomeWardrobeCenter+new Vector3(0,.1f,-.66f),new Vector3(.035f,3.3f,.04f),Ink,"CityMetal");
            foreach(float x in new[]{-.2f,.2f})Rod(HomeWardrobeCenter+new Vector3(x,-.15f,-.72f),HomeWardrobeCenter+new Vector3(x,.32f,-.72f),.03f,Cream,"CityMetal");
            obstacles.Add(new Rect(HomeWardrobeCenter.x-.85f,HomeWardrobeCenter.z-.65f,1.7f,1.3f));
            Block(new Vector3(-3.8f,1.32f,-2.65f),new Vector3(2.5f,.16f,1.3f),new Color(.61f,.43f,.27f),"Wood");
            foreach(float x in new[]{-4.8f,-2.8f})foreach(float z in new[]{-3.1f,-2.2f})Block(new Vector3(x,.65f,z),new Vector3(.09f,1.3f,.09f),Ink,"CityMetal");
            BuildHomePhone();
            BuildHomeDecorBook();
            Block(new Vector3(-4.5f,1.52f,-2.7f),new Vector3(.7f,.23f,.52f),Kaos,"Paper");
            obstacles.Add(new Rect(-5.05f,-3.3f,2.5f,1.3f));
            // Door, poster, night window, shelf, and a rug give each corner a purpose.
            Block(new Vector3(0,1.85f,-4.43f),new Vector3(1.9f,3.7f,.14f),Ink,"Wood");
            Rod(new Vector3(.62f,1.5f,-4.28f),new Vector3(.62f,1.95f,-4.28f),.035f,Cream,"CityMetal");
            HomeNightWindow();
            homeArtBed=FramedPrint("Quadro CREAM — quarto",new Vector3(-3.4f,2.9f,4.37f),0,2,new Vector2(1.45f,1.65f));
            homeArtSide=FramedPrint("Quadro Sol — quarto",new Vector3(-5.43f,2.8f,-.15f),-90,5,new Vector2(1.45f,1.45f));
            var rug=new Color(.30f,.09f,.25f);var trim=new Color(.75f,.36f,.22f);
            Ground(.2f,-.42f,4.05f,2.85f,-.045f,rug,"Rug",1);
            Ground(.2f,-.42f,3.78f,2.57f,-.042f,trim,"Rug",1);
            Ground(.2f,-.42f,3.57f,2.36f,-.039f,rug,"Rug",1);
            for(int i=0;i<5;i++){
                float x=-1.24f+i*.72f;
                Quad(new Vector3(x,-.036f,-.85f),new Vector3(x-.24f,-.036f,-.42f),new Vector3(x,-.036f,.01f),new Vector3(x+.24f,-.036f,-.42f),i%2==0?trim:Cream,"Rug");
            }
            Block(new Vector3(-3.8f,2.7f,-4.23f),new Vector3(2.4f,.12f,.50f),Ink,"Wood");
            foreach(float x in new[]{-4.6f,-3.0f}){
                Rod(new Vector3(x,2.30f,-4.44f),new Vector3(x,2.64f,-4.00f),.035f,Ink,"CityMetal");
                Rod(new Vector3(x,2.30f,-4.44f),new Vector3(x,2.67f,-4.44f),.035f,Ink,"CityMetal");
            }
            for(int i=0;i<3;i++)Block(new Vector3(-4.5f+i*.7f,2.92f,-4.23f),new Vector3(.45f,.30f,.42f),i%2==0?Kaos:Cream,"Paper");
            DecorateHome(walnut,petrol);
            BuildHomeMirror();
            BuildCat(new Vector3(-3.4f,1.055f,1.6f),240,companion??new PetAppearance());
            GeometryBatches=batches.Count;FinishGeometry();motionRigVersion=ResidentArtRevision;
        }
        void BuildHomeMirror()
        {
            var frameColor=new Color(.16f,.13f,.12f);
            Block(HomeMirrorCenter+Vector3.right*.07f,new Vector3(.13f,3.43f,1.37f),frameColor,"Wood",.038f);
            Block(HomeMirrorCenter+Vector3.right*.025f,new Vector3(.04f,3.31f,1.24f),new Color(.36f,.35f,.31f),"CityMetal",.012f);
            float w=HomeMirrorHalfWidth,h=HomeMirrorHalfHeight;
            var mesh=new Mesh{name="Espelho — face refletiva",
                vertices=new[]{new Vector3(0,-h,w),new Vector3(0,h,w),new Vector3(0,h,-w),new Vector3(0,-h,-w)},
                uv=new[]{Vector2.right,Vector2.one,Vector2.up,Vector2.zero},triangles=new[]{0,1,2,0,2,3}};
            mesh.RecalculateNormals();mesh.RecalculateBounds();assets.Add(mesh);
            var glass=new GameObject("Espelho do quarto");glass.transform.SetParent(transform,false);glass.transform.localPosition=HomeMirrorCenter;
            glass.AddComponent<MeshFilter>().sharedMesh=mesh;
            homeMirrorMaterial=new Material(Resources.Load<Shader>("Artwork")){mainTexture=Texture2D.blackTexture};
            assets.Add(homeMirrorMaterial);glass.AddComponent<MeshRenderer>().sharedMaterial=homeMirrorMaterial;
        }
        public bool RenderHomeMirror(Camera source)
        {
            if(!IsHome||homeMirrorMaterial==null||!PlayerBodyReady||Time.unscaledTime<nextHomeMirrorFrame)return false;
            var center=transform.TransformPoint(HomeMirrorCenter);
            var eye=source.transform.position;
            float distance=center.x-eye.x;
            if(distance<.1f)return false;
            GeometryUtility.CalculateFrustumPlanes(source,mirrorViewPlanes);
            if(!GeometryUtility.TestPlanesAABB(mirrorViewPlanes,new Bounds(center,new Vector3(.05f,2*HomeMirrorHalfHeight,2*HomeMirrorHalfWidth))))return false;
            nextHomeMirrorFrame=Time.unscaledTime+1f/15;
            if(homeMirrorCamera==null){
                var target=new RenderTexture(128,352,16,RenderTextureFormat.ARGB32){name="Reflexo do quarto",filterMode=FilterMode.Point};
                assets.Add(target);
                var cameraObject=new GameObject("Câmera do espelho");cameraObject.transform.SetParent(transform,false);
                homeMirrorCamera=cameraObject.AddComponent<Camera>();homeMirrorCamera.enabled=false;
                homeMirrorCamera.clearFlags=CameraClearFlags.SolidColor;homeMirrorCamera.backgroundColor=new Color(.065f,.06f,.07f);
                homeMirrorCamera.allowHDR=false;homeMirrorCamera.allowMSAA=false;homeMirrorCamera.targetTexture=target;
                homeMirrorMaterial.mainTexture=target;
            }
            var reflectedEye=eye+Vector3.right*(2*distance);
            homeMirrorCamera.transform.SetPositionAndRotation(reflectedEye,Quaternion.LookRotation(Vector3.left,Vector3.up));
            // Fit the frustum to the glass and clip the wall behind it.
            float near=distance+.015f,scale=near/distance;
            float x=center.z-eye.z,y=center.y-eye.y;
            homeMirrorCamera.cullingMask=source.cullingMask;
            float far=distance+Mathf.Sqrt(HomeBounds.width*HomeBounds.width+HomeBounds.height*HomeBounds.height)+2;
            homeMirrorCamera.nearClipPlane=near;homeMirrorCamera.farClipPlane=far;
            homeMirrorCamera.projectionMatrix=Matrix4x4.Frustum((x-HomeMirrorHalfWidth)*scale,(x+HomeMirrorHalfWidth)*scale,
                (y-HomeMirrorHalfHeight)*scale,(y+HomeMirrorHalfHeight)*scale,near,far);
            EnsurePlayerPose();FrameEmoteArms(true);
            try{
                foreach(var piece in playerHeadPieces)if(piece!=null)piece.SetActive(PlayerFaceVisible(piece));
                homeMirrorCamera.Render();
            }finally{FramePlayerForCamera(source,false);}
            return true;
        }
        void BuildHomePhone()
        {
            var graphite=new Color(.15f,.20f,.22f);
            var metalEdge=new Color(.30f,.37f,.39f);
            Block(HomePhonePosition-Vector3.up*.058f,new Vector3(.61f,.064f,.32f),Ink,"Rubber",.038f);
            Block(HomePhonePosition-Vector3.up*.022f,new Vector3(.62f,.028f,.33f),graphite,"CityMetal",.039f);
            Block(HomePhonePosition-Vector3.up*.009f,new Vector3(.614f,.010f,.324f),metalEdge,"CityMetal",.037f);
            Block(HomePhonePosition-Vector3.up*.005f,new Vector3(.599f,.010f,.309f),Ink,"Rubber",.033f);

            // Top and bottom bezels sit outside the wallpaper's existing rectangle.
            Block(HomePhonePosition+new Vector3(-.274f,.001f,0),new Vector3(.011f,.003f,.075f),new Color(.27f,.30f,.32f),"CityMetal",.003f);
            var lens=HomePhonePosition+new Vector3(-.274f,0,.065f);
            Rod(lens,lens+Vector3.up*.004f,.009f,new Color(.16f,.27f,.35f),"CityMetal",8);
            var button=HomePhonePosition+new Vector3(.273f,0,0);
            Rod(button,button+Vector3.up*.003f,.018f,metalEdge,"CityMetal",12);
            Rod(button,button+Vector3.up*.004f,.014f,graphite,"Rubber",12);
            foreach(float x in new[]{-.119f,-.061f})
                Block(HomePhonePosition+new Vector3(x,-.023f,.166f),new Vector3(.036f,.012f,.009f),metalEdge,"CityMetal",.003f);
            Block(HomePhonePosition+new Vector3(.311f,-.025f,0),new Vector3(.005f,.012f,.055f),Ink,"Rubber",.002f);

            var center=HomePhonePosition+Vector3.up*.002f;
            const float halfHeight=.245f,halfWidth=.13f;
            var mesh=new Mesh{name="Tela do celular — wallpaper",
                vertices=new[]{
                    new Vector3(halfHeight,0,-halfWidth),new Vector3(-halfHeight,0,-halfWidth),
                    new Vector3(-halfHeight,0,halfWidth),new Vector3(halfHeight,0,halfWidth)
                },
                uv=new[]{Vector2.zero,Vector2.up,Vector2.one,Vector2.right},triangles=new[]{0,1,2,0,2,3}};
            mesh.RecalculateNormals();mesh.RecalculateBounds();assets.Add(mesh);
            var screen=new GameObject("Tela do celular");
            screen.transform.SetParent(transform,false);screen.transform.localPosition=center;
            screen.AddComponent<MeshFilter>().sharedMesh=mesh;
            var material=new Material(Resources.Load<Shader>("Artwork"));
            material.mainTexture=Resources.Load<Texture2D>("Artwork/PhoneAndProfiles/Wallpaper");
            assets.Add(material);screen.AddComponent<MeshRenderer>().sharedMaterial=material;
        }
        void DecorateHome(Color walnut,Color petrol)
        {
            var amber=new Color(1,.65f,.32f);
            // Two practical lamps and a warm ceiling fixture frame the room's activity areas.
            Rod(new Vector3(0,4.38f,0),new Vector3(0,4.09f,0),.038f,Ink,"CityMetal");
            Block(new Vector3(0,4.03f,0),new Vector3(1.34f,.18f,.74f),walnut,"Wood",.10f);
            Block(new Vector3(0,3.93f,0),new Vector3(1.18f,.04f,.60f),Cream,"CityGlow");
            Block(new Vector3(-4.62f,1.43f,-2.99f),new Vector3(.38f,.07f,.30f),Ink,"CityMetal");
            Rod(new Vector3(-4.62f,1.46f,-2.99f),new Vector3(-4.62f,2.05f,-2.99f),.036f,Ink,"CityMetal");
            Block(new Vector3(-4.62f,2.12f,-2.99f),new Vector3(.57f,.22f,.49f),petrol,"CityMetal",.06f);
            Block(new Vector3(-4.62f,2.004f,-2.99f),new Vector3(.43f,.025f,.38f),amber,"CityGlow");
            Rod(new Vector3(-5.43f,2.04f,3.03f),new Vector3(-5.08f,2.04f,3.03f),.036f,Ink,"CityMetal");
            Block(new Vector3(-5.01f,2.01f,3.03f),new Vector3(.29f,.42f,.48f),walnut,"Wood",.06f);
            Block(new Vector3(-5.01f,1.79f,3.03f),new Vector3(.24f,.025f,.39f),amber,"CityGlow");

            // A low record console sits against the side wall, clear of the wardrobe and door.
            Block(new Vector3(5.02f,.56f,-.57f),new Vector3(.90f,.94f,2.35f),walnut,"Wood",.05f);
            Block(new Vector3(4.55f,.51f,-.57f),new Vector3(.035f,.69f,2.16f),Ink,"Wood");
            Block(new Vector3(4.53f,.55f,-.55f),new Vector3(.075f,.82f,.08f),walnut,"Wood");
            for(int i=0;i<8;i++)Block(new Vector3(4.51f,.47f,-1.52f+i*.11f),new Vector3(.065f,.56f,.074f),i%3==0?Cream:i%3==1?petrol:Kaos,"Paper");
            Block(new Vector3(4.94f,1.085f,-1.08f),new Vector3(.67f,.13f,.88f),Ink,"CityMetal");
            Rod(new Vector3(4.94f,1.15f,-1.16f),new Vector3(4.94f,1.175f,-1.16f),.265f,Ink,"Rubber",18);
            Rod(new Vector3(4.94f,1.175f,-1.16f),new Vector3(4.94f,1.182f,-1.16f),.085f,Kaos,"Paper",12);
            Rod(new Vector3(5.13f,1.185f,-.82f),new Vector3(4.96f,1.185f,-1.06f),.014f,Cream,"CityMetal");
            Block(new Vector3(5.01f,1.22f,.24f),new Vector3(.41f,.40f,.41f),Peach,"CityBrick",.04f);
            Foliage(new Vector3(5.01f,1.57f,.24f),new Vector3(.32f,.33f,.35f),LeafGreen,31);
            homeArtRecords=FramedPrint("Quadro — canto dos discos",new Vector3(5.44f,2.47f,-.67f),90,0,new Vector2(1.34f,1.52f));
            obstacles.Add(new Rect(4.56f,-1.75f,.95f,2.36f));
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
            stage=Stage.City;city=homeRoom;paused=false;cityFreeCursor=false;mannequin.gameObject.SetActive(true);studio.ApplyUniform(progress.EquippedUniform);city.PreparePlayerBody(studio);mannequin.gameObject.SetActive(false);city.Frame(cam,true,0);SyncCityCursor(true);
        }
        bool InteractHome()
        {
            if(loadingActive||paused||stage!=Stage.City||!city.IsHome)return false;
            int action=city.HomeInteraction;if(action==0)return false;
            if(action==1){if(DeferLoading(()=>{EnterStreetAt(CityView.StreetHomeThreshold,180);homeDoorExitRemaining=.95f;}))return true;EnterStreetAt(CityView.StreetHomeThreshold,180);homeDoorExitRemaining=.95f;return true;}
            if(action==4)fitFeedScroll=Vector2.zero;
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
