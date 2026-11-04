using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    [Serializable] public class ShopDecor
    {
        public int version=1,wall,floor,accent,poster;public bool plants=true;
        public ShopDecor Copy(){return new ShopDecor{version=version,wall=wall,floor=floor,accent=accent,poster=poster,plants=plants};}
        public void Validate(){if(version!=1||wall<0||wall>7||floor<0||floor>2||accent<0||accent>7||poster<0||poster>7)throw new ArgumentException("Decoração inválida");}
    }
    public static class ShopDecorStore
    {
        static ShopDecor Read(string path){var v=JsonUtility.FromJson<ShopDecor>(File.ReadAllText(path));if(v==null)throw new ArgumentException("Decoração vazia");v.Validate();return v;}
        public static ShopDecor Load(string path){if(!File.Exists(path)&&!File.Exists(path+".bak"))return new ShopDecor();try{return Read(path);}catch{if(File.Exists(path+".bak"))return Read(path+".bak");throw;}}
        public static void Save(string path,ShopDecor value){
            value.Validate();Directory.CreateDirectory(Path.GetDirectoryName(path));string temp=path+".tmp";File.WriteAllText(temp,JsonUtility.ToJson(value,true));Read(temp);
            if(File.Exists(path)){bool valid=true;try{Read(path);}catch{valid=false;}File.Replace(temp,path,valid?path+".bak":null);}else File.Move(temp,path);
        }
    }
    public partial class CityView
    {
        [SerializeField] ShopDecor activeDecor;[SerializeField] Transform decorPlants,decorArtwork;
        readonly System.Collections.Generic.Dictionary<Mesh,Color[]> decorOriginal=new System.Collections.Generic.Dictionary<Mesh,Color[]>();
        public ShopDecor ActiveDecor=>activeDecor;
        public void ApplyShopDecor(ShopDecor value)
        {
            if(!IsInterior||IsHome)return;value.Validate();activeDecor=value.Copy();
            var washes=new[]{Color.white,new Color(.54f,.88f,.52f),new Color(1,.80f,.39f),new Color(.70f,.74f,.75f),new Color(.66f,.36f,.94f),new Color(.28f,.52f,1),new Color(1,.32f,.21f),new Color(.10f,.88f,.68f)};
            var accents=new[]{new Color(.08f,.46f,.25f),new Color(.90f,.54f,.08f),new Color(.82f,.22f,.11f),new Color(.49f,.10f,.91f),new Color(.025f,.72f,.59f),new Color(.89f,.024f,.075f),new Color(.95f,.06f,.43f),new Color(.07f,.23f,.89f)};
            foreach(var r in GetComponentsInChildren<MeshRenderer>(true)){
                if(r.name=="Arquitetura — CityStucco"||r.name=="Arquitetura — CityBrick")foreach(var m in r.sharedMaterials)m.SetColor("_DecorTint",washes[value.wall]);
                if(r.name=="Arquitetura — ShopFloor")foreach(var m in r.sharedMaterials){
                    if(value.floor==0)AmbientSurfaces.Apply(m,"ShopFloor",true);
                    else{
                        m.mainTexture=Resources.Load<Texture2D>("Surfaces/"+(value.floor==1?"WoodFloor040":"Tiles107"));
                        m.SetFloat("_SingleSurface",1);m.SetFloat("_SurfaceMean",value.floor==1?.55374473f:.90368061f);m.SetFloat("_SurfaceContrast",3);m.SetFloat("_TexScale",value.floor==1?.24f:.55f);m.SetColor("_SurfaceTint",Color.white);
                    }
                }
                if(r.name.Contains("Cortina")){
                    var filter=r.GetComponent<MeshFilter>();if(filter==null)continue;var mesh=filter.sharedMesh;Color[] original;
                    if(!decorOriginal.TryGetValue(mesh,out original)){original=mesh.colors;decorOriginal[mesh]=original;}
                    var colors=(Color[])original.Clone();foreach(int index in mesh.GetTriangles(0))colors[index]=accents[value.accent];
                    mesh.colors=colors;
                }
            }
            if(decorArtwork==null||decorArtwork.Find("Arte fornecida")==null){
                if(decorArtwork!=null){decorArtwork.gameObject.SetActive(false);Destroy(decorArtwork.gameObject);}
                decorArtwork=FramedPrint("Quadro personalizável",new Vector3(-.8f,4.0f,8.74f),0,value.poster,new Vector2(2.2f,1.05f));
            }else SetFramedPrint(decorArtwork,value.poster,new Vector2(2.2f,1.05f));
            if(decorPlants==null){
                int first=transform.childCount;
                foreach(float z in new[]{-3.2f,8.1f}){Block(new Vector3(7.5f,.35f,z),new Vector3(.5f,.7f,.5f),Peach,"CityBrick");Foliage(new Vector3(7.5f,1.3f,z),new Vector3(.40f,.95f,.4f),new Color(.22f,.38f,.19f),3);}
                FinishGeometry();decorPlants=new GameObject("Plantas da decoração").transform;decorPlants.SetParent(transform,false);
                var children=new System.Collections.Generic.List<Transform>();for(int i=first;i<transform.childCount-1;i++)children.Add(transform.GetChild(i));foreach(var child in children)child.SetParent(decorPlants,false);
            }
            decorPlants.gameObject.SetActive(value.plants);
        }
    }
    public partial class FirstCustomer
    {
        bool showShopDecor;int decorView;ShopDecor decorDraft,decorBefore;string decorPathLoaded;ShopDecor savedDecor;
        string ShopDecorPath=>Path.Combine(SessionFolder,"shop-style.json");
        ShopDecor CurrentShopDecor {
            get{if(savedDecor==null||decorPathLoaded!=ShopDecorPath){decorPathLoaded=ShopDecorPath;try{savedDecor=ShopDecorStore.Load(decorPathLoaded);}catch{savedDecor=new ShopDecor();message="Não foi possível ler a decoração. Você pode escolher outra.";}}return savedDecor;}
        }
        void OpenShopDecor()
        {
            if(stage!=Stage.City||!city.IsInterior||city.IsHome||ShopOpen||ShopSequenceActive||paused)return;
            decorBefore=CurrentShopDecor.Copy();decorDraft=decorBefore.Copy();decorView=0;showShopDecor=true;city.StopWalk();cityFreeCursor=true;SyncCityCursor(true);Sound(0);
            FrameShopDecor();
        }
        void FrameShopDecor(){
            var positions=new[]{new Vector3(.6f,3.8f,-4.1f),new Vector3(1.5f,3.1f,.2f),new Vector3(-3,3,3)};
            var targets=new[]{new Vector3(-.5f,2.1f,4.6f),new Vector3(-4.9f,1.8f,5.1f),new Vector3(-.8f,3.9f,8.7f)};
            cam.transform.position=city.transform.position+positions[decorView];cam.transform.LookAt(city.transform.position+targets[decorView]);cam.fieldOfView=decorView==0?66:55;city.FramePlayerForCamera(cam,true);
        }
        void CloseShopDecor(bool save)
        {
            if(!showShopDecor)return;
            if(save)try{ShopDecorStore.Save(ShopDecorPath,decorDraft);savedDecor=decorDraft.Copy();message="Sua loja foi salva.";}catch(Exception e){message="Não foi possível salvar: "+e.Message;return;}
            city.ApplyShopDecor(save?decorDraft:decorBefore);showShopDecor=false;decorDraft=null;cityFreeCursor=false;
        }
        void ShopDecorUI()
        {
            Fill(new Rect(30,108,570,766),new Color(.055f,.06f,.055f,.98f));
            Txt(new Rect(55,130,510,48),"Sua loja. Seu jeito.",heading);
            Txt(new Rect(55,191,510,50),"Escolha os acabamentos antes de abrir.\nA prévia aparece na própria loja.",caption,muted);
            string[][] names={new[]{"Original","Verde","Sol","Concreto","Uva","Azul","Coral","Turquesa"},new[]{"Original","Madeira","Cerâmica"},new[]{"Verde","Ocre","Terracota","Roxo","Turquesa","Kaos","Rosa","Azul"},GalleryArt.Names};
            string[] labels={"Paredes","Piso","Cortina","Quadro"};int[] values={decorDraft.wall,decorDraft.floor,decorDraft.accent,decorDraft.poster};
            float y=246;for(int row=0;row<4;row++){
                Txt(new Rect(55,y,510,26),labels[row],caption);
                for(int i=0;i<names[row].Length;i++)if(Tap(new Rect(55+i%4*130,y+28+i/4*40,122,34),names[row][i],values[row]==i)){
                    if(row==0)decorDraft.wall=i;else if(row==1)decorDraft.floor=i;else if(row==2)decorDraft.accent=i;else decorDraft.poster=i;city.ApplyShopDecor(decorDraft);
                }
                y+=40+Mathf.CeilToInt(names[row].Length/4f)*40;
            }
            if(Tap(new Rect(55,708,510,45),decorDraft.plants?"Plantas: presentes":"Plantas: recolhidas")){decorDraft.plants=!decorDraft.plants;city.ApplyShopDecor(decorDraft);}
            if(Tap(new Rect(55,782,246,55),"Salvar decoração",true))CloseShopDecor(true);
            if(Tap(new Rect(319,782,246,55),"Cancelar"))CloseShopDecor(false);
            for(int i=0;i<3;i++)if(Tap(new Rect(630+i*240,713,222,48),new[]{"Geral","Provador","Quadro"}[i],decorView==i)){decorView=i;FrameShopDecor();}
            Txt(new Rect(620,790,700,40),"Olhe a prévia. As mudanças pertencem a este save.",caption,paper);
        }
    }
}
