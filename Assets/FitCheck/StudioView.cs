using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FitCheck
{
    // Geometric art authored for this scene. Clothes have their own volume and trim.
    public partial class StudioView : MonoBehaviour
    {
        public Camera Camera {get;private set;}
        public Transform Avatar {get;private set;}
        Transform clothes, hair; Material skin, ink;
        Transform house; bool inHome;
        public bool ReducedMotion;float framingNow=-.95f,idle;
        [SerializeField] List<Object> owned=new List<Object>();
        [SerializeField] List<Object> garmentAssets=new List<Object>(); bool buildingGarments;
        void Own(Object o){if(buildingGarments)garmentAssets.Add(o);else owned.Add(o);}
        Color purple=new Color(.46f,.54f,.39f);
        Material Material(Color c, bool glow=false)
        {
            var m=new Material(Resources.Load<Shader>("RetroPaint"));m.color=c;
            m.SetFloat("_CharacterLit",buildingGarments?1:0);
            m.mainTexture=Resources.Load<Texture2D>("Painted/Cloth");m.SetFloat("_Motion",1);m.SetFloat("_Glow",glow?.65f:0);
            if(buildingGarments)m.SetFloat("_SmoothLight",1);
            AmbientSurfaces.ClothDetail(m,buildingGarments&&!glow);
            Own(m);return m;
        }
        GameObject Box(string name, Vector3 p, Vector3 s, Material m, Transform parent=null)
        {
            var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=name;o.transform.SetParent(parent??transform,false);
            o.transform.localPosition=p;o.transform.localScale=s;o.GetComponent<Renderer>().sharedMaterial=m;
            if(name.Contains("Parede")||name.Contains("Ladrilho")||name.Contains("Piso"))m.mainTexture=Resources.Load<Texture2D>("Painted/Plaster");
            if(name.Contains("Pôster")||name.Contains("Recorte"))m.mainTexture=Resources.Load<Texture2D>("Painted/Paper");
            if(name.Contains("Tábua")||name.Contains("Guarda-roupa")||name=="Cama")m.mainTexture=Resources.Load<Texture2D>("Painted/Wood");
            Destroy(o.GetComponent<Collider>());return o;
        }
        GameObject Form(string name,Vector3 p,Vector3 s,Material m,Transform parent,int sides=10)
        {
            return Rings(name,p,s,m,parent,new[]{.0f,.12f,.35f,.7f,.9f,1f},new[]{.48f,.83f,1f,1f,.8f,.4f},sides);
        }
        GameObject Rings(string name,Vector3 p,Vector3 s,Material m,Transform parent,float[] ys,float[] widths,int sides=8)
        {
            if(Avatar!=null&&parent!=null&&(parent==Avatar||parent.IsChildOf(Avatar)))
                return RoundedLoft(name,p,s,m,parent,ys,widths,Mathf.Max(24,sides));
            var v=new List<Vector3>();var t=new List<int>();
            for(int r=0;r<ys.Length;r++)for(int i=0;i<sides;i++){
                float a=2*Mathf.PI*i/sides+Mathf.PI/8;
                float z=Mathf.Sin(a)*.5f*widths[r];
                if(name=="Cabeça")z=Mathf.Max(z,-.41f*widths[r]);
                v.Add(new Vector3(Mathf.Cos(a)*.5f*widths[r],ys[r]-.5f,z));
            }
            for(int r=0;r<ys.Length-1;r++)for(int i=0;i<sides;i++){
                int a=r*sides+i,b=r*sides+(i+1)%sides,c=a+sides,d=b+sides;
                t.Add(a);t.Add(c);t.Add(b);t.Add(b);t.Add(c);t.Add(d);
            }
            for(int i=1;i<sides-1;i++){t.Add(0);t.Add(i);t.Add(i+1);int a=(ys.Length-1)*sides;t.Add(a);t.Add(a+i+1);t.Add(a+i);}
            // Duplicate triangle vertices for deliberate flat shading.
            var flat=new Vector3[t.Count];var indices=new int[t.Count];var uv=new Vector2[t.Count];
            for(int i=0;i<t.Count;i++){flat[i]=v[t[i]];indices[i]=i;uv[i]=new Vector2((t[i]%sides)/(float)sides,ys[t[i]/sides]);}
            for(int i=0;i<uv.Length;i+=3){float lo=Mathf.Min(uv[i].x,Mathf.Min(uv[i+1].x,uv[i+2].x));float hi=Mathf.Max(uv[i].x,Mathf.Max(uv[i+1].x,uv[i+2].x));if(hi-lo>.5f)for(int j=0;j<3;j++)if(uv[i+j].x<.5f)uv[i+j].x+=1;}
            var mesh=new Mesh{name=name};mesh.vertices=flat;mesh.triangles=indices;mesh.uv=uv;mesh.RecalculateNormals();mesh.RecalculateBounds();Own(mesh);
            var o=new GameObject(name);o.transform.SetParent(parent,false);o.transform.localPosition=p;o.transform.localScale=s;
            o.AddComponent<MeshFilter>().sharedMesh=mesh;o.AddComponent<MeshRenderer>().sharedMaterial=m;return o;
        }
        public void Build()
        {
            var dark=Material(new Color(.055f,.06f,.055f));var panel=Material(new Color(.105f,.11f,.10f));
            var metal=Material(new Color(.28f,.25f,.32f));var neon=Material(purple,true);var ivory=Material(new Color(.82f,.79f,.72f));
            Box("Piso",new Vector3(0,-.14f,0),new Vector3(14,.2f,12),dark);
            for(int x=-7;x<=7;x++)for(int z=-5;z<=4;z++)
                Box("Ladrilho",new Vector3(x*.7f,-.025f,z*.7f),new Vector3(.69f,.025f,.69f),((x+z)%2==0)?panel:dark);
            Box("Parede do estúdio",new Vector3(0,2.35f,2.8f),new Vector3(14,5,.25f),dark);
            BuildAbstractBackdrop();
            for(int side=-1;side<=1;side+=2){
                Box("Moldura",new Vector3(side*1.86f,2.2f,2.42f),new Vector3(.07f,4.4f,.12f),neon);
                Box("Luz de camarim",new Vector3(side*1.64f,2.3f,2.38f),new Vector3(.035f,3.7f,.05f),Material(new Color(.85f,.8f,.95f),true));
            }
            Box("Travessa de luz",new Vector3(0,4.39f,2.42f),new Vector3(3.8f,.07f,.12f),neon);
            Rings("Tablado octogonal",new Vector3(0,.035f,0),new Vector3(2.55f,.15f,2.2f),metal,transform,new[]{0f,1f},new[]{1f,1f},12);
            Rings("Borda violeta",new Vector3(0,-.005f,0),new Vector3(2.6f,.035f,2.25f),neon,transform,new[]{0f,1f},new[]{1f,1f},12);
            // Curtain, folds and a clothing rail establish a real fitting room.
            for(int i=0;i<15;i++)Box("Dobra da cortina",new Vector3(-3.3f+i*.11f,2,2.1f+(i%2)*.09f),new Vector3(.12f,3.9f,.17f),Material(i%2==0?new Color(.2f,.11f,.29f):new Color(.14f,.075f,.21f)));
            Box("Arara",new Vector3(3.4f,2.9f,1.4f),new Vector3(2.2f,.06f,.06f),metal);
            for(int side=-1;side<=1;side+=2)Box("Pé da arara",new Vector3(3.4f+side*1.05f,1.45f,1.4f),new Vector3(.06f,2.9f,.06f),metal);
            for(int i=0;i<4;i++){
                float x=2.65f+i*.5f;var fabric=Material(new[]{new Color(.8f,.77f,.66f),new Color(.18f,.28f,.2f),new Color(.15f,.17f,.26f),new Color(.4f,.2f,.16f)}[i]);
                Box("Cabide",new Vector3(x,2.67f,1.4f),new Vector3(.38f,.04f,.05f),ivory);
                Box("Roupa na arara",new Vector3(x,2.16f,1.4f),new Vector3(.42f,.9f,.22f),fabric);
            }
            Box("Banco de prova",new Vector3(-2.8f,.5f,-.3f),new Vector3(1.7f,.16f,.65f),metal);
            for(int side=-1;side<=1;side+=2)Box("Pé do banco",new Vector3(-2.8f+side*.6f,.23f,-.3f),new Vector3(.06f,.5f,.5f),dark);
            Avatar=new GameObject("Cliente").transform;Avatar.SetParent(transform,false);
            skin=Material(new Color(.62f,.39f,.25f));ink=Material(new Color(.035f,.025f,.035f));
            skin.SetFloat("_CharacterLit",1);ink.SetFloat("_CharacterLit",1);
            BuildFace();
            for(int side=-1;side<=1;side+=2){
                Form("Canela",new Vector3(side*.207f,.62f,0),new Vector3(.24f,.85f,.26f),skin,Avatar,6);
            }
            Rings("Pescoço",new Vector3(0,2.31f,.025f),new Vector3(.30f,.32f,.29f),skin,Avatar,new[]{0f,1f},new[]{1.1f,.92f},6);
            CacheCalves();
            hair=new GameObject("Cabelo").transform;hair.SetParent(Avatar,false);hair.localPosition=Vector3.zero;
            Camera=new GameObject("Câmera do provador").AddComponent<Camera>();Camera.tag="MainCamera";Camera.depth=0;
            Camera.backgroundColor=new Color(.045f,.035f,.065f);Camera.clearFlags=CameraClearFlags.SolidColor;Camera.fieldOfView=36;
            Camera.gameObject.AddComponent<RetroCamera>();
            var key=new GameObject("Luz frontal neutra").AddComponent<Light>();key.type=LightType.Directional;key.intensity=.85f;key.transform.rotation=Quaternion.Euler(28,-25,0);
            RenderSettings.ambientLight=new Color(.36f,.36f,.36f);
            BuildHouse();
        }
        void BuildHouse()
        {
            house=new GameObject("Casa — quitinete").transform;house.SetParent(transform);house.localPosition=new Vector3(100,0,0);
            var wall=Material(new Color(.43f,.37f,.36f));var wood=Material(new Color(.25f,.16f,.13f));var cream=Material(new Color(.83f,.77f,.63f));
            Box("Piso de casa",new Vector3(0,-.09f,0),new Vector3(10,.15f,9),wood,house);
            for(int i=-5;i<=5;i++)Box("Tábua",new Vector3(i*.8f,.0f,0),new Vector3(.03f,.012f,9),wall,house);
            Box("Parede de casa",new Vector3(0,2.2f,3),new Vector3(10,4.5f,.15f),wall,house);
            Box("Janela",new Vector3(.2f,2.7f,2.88f),new Vector3(2.1f,1.6f,.08f),Material(new Color(.15f,.22f,.34f)),house);
            Box("Travessa da janela",new Vector3(.2f,2.7f,2.8f),new Vector3(.05f,1.6f,.08f),cream,house);
            Box("Peitoril",new Vector3(.2f,1.9f,2.78f),new Vector3(2.3f,.1f,.25f),cream,house);
            Box("Cama",new Vector3(-2.6f,.3f,1.3f),new Vector3(1.9f,.5f,2.6f),wood,house);
            Box("Colchão",new Vector3(-2.6f,.65f,1.3f),new Vector3(1.85f,.3f,2.5f),cream,house);
            Box("Coberta",new Vector3(-2.6f,.82f,.9f),new Vector3(1.9f,.12f,1.7f),Material(new Color(.46f,.22f,.2f)),house);
            Box("Travesseiro",new Vector3(-2.6f,.88f,2.1f),new Vector3(1.15f,.2f,.5f),cream,house);
            Box("Guarda-roupa",new Vector3(3.4f,1.5f,2.2f),new Vector3(1.9f,3,.9f),wood,house);
            for(int side=-1;side<=1;side+=2)Box("Puxador",new Vector3(3.4f+side*.12f,1.7f,1.72f),new Vector3(.04f,.3f,.04f),cream,house);
            Box("Pôster",new Vector3(-2.6f,2.8f,2.87f),new Vector3(1.2f,1.4f,.03f),Material(purple),house);
            for(int i=0;i<4;i++)Box("Recorte do pôster",new Vector3(-2.9f+i*.2f,2.7f+i*.12f,2.83f),new Vector3(.1f,.6f,.02f),cream,house);
            Box("Vaso",new Vector3(2.2f,.25f,1.5f),new Vector3(.48f,.5f,.48f),Material(new Color(.55f,.23f,.12f)),house);
            for(int i=0;i<5;i++){var leaf=Form("Folha",new Vector3(2.2f+(i-2)*.12f,.95f,1.5f),new Vector3(.18f,.9f,.12f),Material(new Color(.12f,.29f,.16f)),house);leaf.transform.localRotation=Quaternion.Euler(0,0,(i-2)*-18);}
        }
        public bool LegacyHomeVisible=>house!=null&&house.gameObject.activeSelf;
        public void SetHome(bool home)
        {if(house!=null)house.gameObject.SetActive(home);if(home==inHome)return;inHome=home;Avatar.position=home?new Vector3(100,0,0):Vector3.zero;framingNow=home?-.95f:.9f;}
        public void ApplyUniform(int uniform)
        {
            Apply(Catalog.Default,-1,PlayerUniforms[Mathf.Clamp(uniform,0,PlayerUniforms.Length-1)]);
        }
        public PlayerAppearance PlayerLook;
        bool applyingPlayer;
        public void Apply(string[] outfit,int customer){Apply(outfit,customer,null);}
        void Apply(string[] outfit,int customer,Garment[] playerPieces)
        {
            applyingPlayer=customer<0&&PlayerLook!=null;
            RestoreCalfBindings();clothingInstanceMaterials.Clear();
            if(clothes!=null){clothes.gameObject.SetActive(false);Destroy(clothes.gameObject);}
            foreach(var asset in garmentAssets)if(asset!=null)Destroy(asset);garmentAssets.Clear();buildingGarments=true;
            clothes=new GameObject("Vestuário 3D").transform;clothes.SetParent(Avatar,false);clothes.localPosition=Vector3.zero;
            foreach(Transform c in hair){c.gameObject.SetActive(false);Destroy(c.gameObject);}
            int profile=customer<0?6:DayOne.CustomerProfile(Mathf.Clamp(customer,0,DayOne.Customers.Length-1));
            var appearance=applyingPlayer?PlayerLook:WeekExpansion.Appearance(profile);
            feminineAppearance=appearance!=null?appearance.appearanceBase==1:profile==1||profile==3||profile==5;
            skin.color=appearance!=null?PlayerAppearance.SkinColors[appearance.skin]:
                new[]{new Color(.67f,.40f,.23f),new Color(.94f,.72f,.58f),new Color(.25f,.135f,.085f),new Color(.76f,.60f,.40f),new Color(.53f,.33f,.23f),new Color(.38f,.205f,.13f),new Color(.62f,.4f,.27f)}[profile];
            SetFaceProfile(profile);
            if(appearance!=null&&(applyingPlayer||ExpressionOverride<0))ApplyExpression((AvatarExpression)appearance.expression);
            var headwear=playerPieces?[3]??Catalog.Get(outfit[3]);
            BuildHair(profile,headwear,appearance);
            int civilianProfile=civilianWardrobe>=0?civilianWardrobe:Mathf.Max(0,customer);
            bool originalTop=Catalog.IsArrivalPiece(outfit[0]),originalBottom=Catalog.IsArrivalPiece(outfit[1]),originalShoes=Catalog.IsArrivalPiece(outfit[2]);
            var t=playerPieces?[0]??(originalTop?CivilianTop(civilianProfile):Catalog.Get(outfit[0]));bool fubaSuit=t.id==FubaTop;
            var cloth=fubaSuit?SurfaceMaterial(t.color,"Knit"):Material(t.color);
            FinishCivilianMaterial(cloth,ClothingFabric(t.id),originalTop);
            cloth.SetFloat("_Outline",0);
            bool blenderTop=originalTop?BuildCivilianTop(t,cloth,civilianProfile):BuildStreetwearTop(t,cloth);
            if(fubaSuit){
                ShirtBody(t,cloth);
                BuildFubaSuitDetails(cloth);
            }else if(t.id==NeighbourhoodTop){
                ShirtBody(t,cloth);
                BuildPosterJacket();
            }else if(t.id==GarageDjTop){
                ShirtBody(t,cloth);
                BuildGarageDjJacket();
            }else if(t.id==GhostTop){
                ShirtBody(t,cloth);
                BuildFittingGhostShirt();
            }else if(!blenderTop&&!BuildWeekTop(t,cloth)){
ShirtBody(t,cloth);
            Rings("Gola",new Vector3(0,2.217f,0),new Vector3(.34f,.045f,.32f),Material(t.id=="CG_TOP_01"?new Color(.12f,.3f,.19f):t.color*.68f),clothes,new[]{0f,1f},new[]{1f,1f},12);
            var print=Material(t.id=="SB_TOP_01"?new Color(.7f,.06f,.07f):new Color(.16f,.26f,.22f));
            Box("Estampa discreta",new Vector3(t.id=="SB_TOP_01"?0:.19f,1.98f,-.286f),new Vector3(t.id=="SB_TOP_01"?.22f:.115f,.085f,.018f),print,clothes);
            if(t.id=="SB_TOP_01")for(int side=-1;side<=1;side+=2)Box("Carretel da fita",new Vector3(side*.055f,1.98f,-.299f),new Vector3(.028f,.028f,.01f),Material(new Color(.8f,.77f,.69f)),clothes);
            if(!t.discreet){
                var accent=Material(t.drop=="AFTER HOURS"?new Color(.91f,.58f,.14f):t.drop=="COMMON GROUND"?new Color(.82f,.76f,.59f):new Color(.75f,.08f,.12f));
                for(int i=0;i<5;i++){
                    var graphic=Box("Grafismo da coleção",new Vector3((i-2)*.115f,1.84f+(i%2)*.09f,-.3f),new Vector3(.065f,.34f,.022f),accent,clothes);
                    graphic.transform.localRotation=Quaternion.Euler(0,0,t.drop=="AFTER HOURS"?32:t.drop=="SIDE B"?i*13:0);
                }
            }
            if(t.id.EndsWith("TOP_03")){
                if(t.drop!="COMMON GROUND")Form("Capuz",new Vector3(0,2.38f,.26f),new Vector3(.61f,.62f,.32f),cloth,clothes);
            }
            }
            var b=playerPieces?[1]??(originalBottom?CivilianBottom(civilianProfile):Catalog.Get(outfit[1]));
            var shoes=playerPieces?[2]??Catalog.Get(outfit[2]);
            var denim=fubaSuit?cloth:Material(b.color);FinishCivilianMaterial(denim,originalBottom?"Denim":ClothingFabric(b.id),originalBottom);
            foreach(Transform part in Avatar){
                if(part.name=="Canela")part.gameObject.SetActive(b.shorts);
            }
            bool blenderBaggy=!originalBottom&&BuildBlenderBaggy(b);
            if(!blenderBaggy)Rings("Cintura",new Vector3(0,1.25f,0),new Vector3(.77f,.32f,.48f),denim,clothes,new[]{0f,1f},new[]{1f,1f});
            for(int side=-1;side<=1;side+=2){
                if(!blenderBaggy){
                Rings(b.shorts?"Bermuda":"Calça",new Vector3(side*.23f,b.shorts?1.03f:.83f,0),new Vector3(b.shorts?.46f:.39f,b.shorts?.54f:.95f,.47f),denim,clothes,new[]{0f,.12f,.43f,.66f,.86f,1f},new[]{.95f,1f,1.03f,1.04f,1.06f,1.02f});
                if(!fubaSuit&&t.id!=GhostTop)Box("Bolso",new Vector3(side*.3f,1.13f,-.232f),new Vector3(.15f,.17f,.018f),Material(b.color*.82f),clothes);
                if(b.id=="CG_BOTTOM_01")Box("Faixa lateral",new Vector3(side*.423f,.88f,0),new Vector3(.018f,.82f,.08f),Material(new Color(.8f,.75f,.6f)),clothes);
                if(b.id=="SB_BOTTOM_02")Box("Painel Remix",new Vector3(side*.23f,1.03f,-.244f),new Vector3(.32f,.3f,.02f),Material(side<0?new Color(.32f,.32f,.34f):new Color(.65f,.65f,.67f)),clothes);
                if(b.id=="AH_BOTTOM_03"||b.id=="CG_BOTTOM_03")BuildWeekBottomTrim(b,side);
                }
                if(fubaSuit)BuildFubaPaw(side,cloth);
                else if(originalShoes)BuildCivilianShoes(civilianProfile,side);else BuildShoes(shoes,side);
            }
            FinishProceduralClothing(t,b);
            BuildHeadwear(headwear);
            foreach(var eye in eyes)eye.gameObject.SetActive(headwear?.id!=PrismaGlassesId);
            var headScale=HeadProportion(profile);
            foreach(Transform part in hair){
                var p=part.localPosition;p.x*=headScale.x;p.y=2.8f+(p.y-2.8f)*headScale.y+HeadLift(profile);part.localPosition=p;
                part.localScale=Vector3.Scale(part.localScale,headScale);
            }
            // Fit the catalog to the shared body; the root supplies each client's
            // overall proportions so skin, clothes, head and shoes remain aligned.
            foreach(Transform piece in clothes){
                if(piece.localPosition.y>=2.5f){
                    var headPosition=piece.localPosition;headPosition.x*=headScale.x;headPosition.y=2.8f+(headPosition.y-2.8f)*headScale.y+HeadLift(profile);piece.localPosition=headPosition;
                    piece.localScale=Vector3.Scale(piece.localScale,headScale);continue;
                }
                var p=piece.localPosition;p.x*=.9f;p.z*=.90f;piece.localPosition=p;
                var scale=piece.localScale;scale.x*=.9f;scale.z*=.90f;piece.localScale=scale;
            }
            FitAppearanceSilhouette();
            FitRelaxedBody(t,cloth,(shoes?.id==CloudSlideId||shoes?.id==BrasaClogId)&&!b.shorts);
            buildingGarments=false;applyingPlayer=false;
            if(originalTop&&originalBottom&&originalShoes&&string.IsNullOrEmpty(outfit[3]))CompactCivilianWardrobe();
        }
        public void Pose(float rotation,float distance,float framing)
        {
            framingNow=ReducedMotion?framing:Mathf.Lerp(framingNow,framing,1-Mathf.Exp(-Time.unscaledDeltaTime*8));framing=framingNow;
            float origin=inHome?100:0;
            Camera.transform.position=new Vector3(origin+.25f+framing,2.5f,-distance-FramingPadding);
            Camera.transform.LookAt(new Vector3(origin+framing,1.85f,0));Avatar.localRotation=Quaternion.Euler(0,rotation,0);
        }
        public void Animate(float delta,bool active)
        {if(!EnsureFaceBindings())return;if(active){blinkClock+=delta;if(!ReducedMotion)idle+=delta;}Avatar.localScale=bodyScale;PoseRelaxedBody();AnimateFace();}
        public Texture2D Thumbnail(Garment item)
        {
            var outfit=Catalog.Default;outfit[item.slot]=item.id;Apply(outfit,0);CacheShopDisplay(item);
            foreach(var r in clothes.GetComponentsInChildren<Renderer>()){
                string n=r.name;
                bool visible=item.slot==0?(n=="Camiseta com caimento"||n.Contains("Manga")||n=="Gola"||n.Contains("Barra")||n.Contains("Estampa")||n.Contains("Carretel")||n.Contains("Grafismo")||n=="Capuz"||n.StartsWith("BlenderTopo")):
                    item.slot==1?(n=="Cintura"||n=="Calça"||n=="Bermuda"||n=="Bolso"||n.Contains("Faixa")||n.Contains("Painel")):
                    item.slot==2?(n.StartsWith("Tênis")||n.StartsWith("Sola")||n=="Cadarço"):(n.Contains("Boné")||n.Contains("boné")||n=="Gorro"||n.Contains("gorro")||n.StartsWith("Óculos"));
                r.gameObject.layer=visible?31:30;
            }
            int height=item.id==PrismaGlassesId?64:128;
            var rt=RenderTexture.GetTemporary(128,height,24,RenderTextureFormat.ARGB32);
            var oldTarget=Camera.targetTexture;var oldActive=RenderTexture.active;var pos=Camera.transform.position;var rot=Camera.transform.rotation;
            int mask=Camera.cullingMask;var bg=Camera.backgroundColor;float aspect=Camera.aspect;
            bool orthographic=Camera.orthographic;float orthographicSize=Camera.orthographicSize;
            var oldPose=Avatar.rotation;Avatar.rotation=Quaternion.identity;
            try{
                float y=new[]{1.9f,.94f,.25f,3.23f}[item.slot];float distance=new[]{2.6f,2.3f,1.5f,2.15f}[item.slot];
                Camera.cullingMask=1<<31;Camera.backgroundColor=Color.clear;Camera.aspect=1;
                Camera.transform.position=Avatar.position+new Vector3(.08f,y+.06f,-distance);Camera.transform.LookAt(Avatar.position+new Vector3(0,y,0));
                if(item.id==PrismaGlassesId){
                    var renderers=clothes.GetComponentsInChildren<Renderer>().Where(r=>r.gameObject.layer==31).ToArray();
                    var bounds=renderers[0].bounds;
                    foreach(var renderer in renderers)bounds.Encapsulate(renderer.bounds);
                    Camera.aspect=2;Camera.orthographic=true;
                    Camera.orthographicSize=Mathf.Max(bounds.extents.x/2,bounds.extents.y)*1.14f;
                    Camera.transform.position=bounds.center+Vector3.back*2;Camera.transform.LookAt(bounds.center);
                }
                Camera.targetTexture=rt;Camera.Render();RenderTexture.active=rt;
                var image=new Texture2D(128,height,TextureFormat.RGBA32,false){filterMode=FilterMode.Point};
                image.ReadPixels(new Rect(0,0,128,height),0,0);image.Apply();Own(image);return image;
            }finally{
                Avatar.rotation=oldPose;Camera.targetTexture=oldTarget;Camera.cullingMask=mask;Camera.backgroundColor=bg;Camera.ResetAspect();
                Camera.orthographic=orthographic;Camera.orthographicSize=orthographicSize;
                Camera.transform.SetPositionAndRotation(pos,rot);RenderTexture.active=oldActive;RenderTexture.ReleaseTemporary(rt);
            }
        }
        void OnDestroy(){foreach(var o in owned)if(o!=null)Destroy(o);foreach(var o in garmentAssets)if(o!=null)Destroy(o);}
    }
}
