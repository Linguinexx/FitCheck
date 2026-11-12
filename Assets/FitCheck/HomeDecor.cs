using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    [Serializable] public class HomeDecor
    {
        public int version=1,wall,accent,floor,bedding,rug,poster=2,posterSide=5,posterRecords;
        public bool plants=true;
        public HomeDecor Copy()=>new HomeDecor{version=version,wall=wall,accent=accent,floor=floor,bedding=bedding,rug=rug,poster=poster,posterSide=posterSide,posterRecords=posterRecords,plants=plants};
        public void Validate(){
            if(version!=1||wall<0||wall>7||accent<0||accent>7||floor<0||floor>2||bedding<0||bedding>7||rug<0||rug>7||poster<0||poster>=GalleryArt.Names.Length||posterSide<0||posterSide>=GalleryArt.Names.Length||posterRecords<0||posterRecords>=GalleryArt.Names.Length)throw new ArgumentException("Decoração do quarto inválida");
        }
    }
    public static class HomeDecorStore
    {
        static HomeDecor Read(string path){var value=JsonUtility.FromJson<HomeDecor>(File.ReadAllText(path));if(value==null)throw new ArgumentException("Decoração vazia");value.Validate();return value;}
        public static HomeDecor Load(string path){if(!File.Exists(path)&&!File.Exists(path+".bak"))return new HomeDecor();try{return Read(path);}catch{if(File.Exists(path+".bak"))return Read(path+".bak");throw;}}
        public static void Save(string path,HomeDecor value){
            value.Validate();Directory.CreateDirectory(Path.GetDirectoryName(path));string temporary=path+".tmp";File.WriteAllText(temporary,JsonUtility.ToJson(value,true));Read(temporary);
            if(File.Exists(path)){bool valid=true;try{Read(path);}catch{valid=false;}PlatformFiles.ReplaceValidated(temporary,path,valid?path+".bak":null);}else File.Move(temporary,path);
        }
    }
    public partial class CityView
    {
        public static readonly Vector3 HomeDecorBook=new Vector3(-4.02f,1.47f,-2.25f);
        [SerializeField] Transform homeArtBed,homeArtSide,homeArtRecords;
        [SerializeField] HomeDecor activeHomeDecor;
        readonly Dictionary<Mesh,Color[]> homeDecorOriginal=new Dictionary<Mesh,Color[]>();
        public HomeDecor ActiveHomeDecor=>activeHomeDecor;
        public static readonly Color[] HomeDecorColors={new Color(.94f,.91f,.82f),new Color(.12f,.67f,.51f),new Color(.32f,.23f,.82f),new Color(.90f,.22f,.43f),new Color(.10f,.43f,.88f),new Color(.96f,.62f,.14f),new Color(.76f,.21f,.12f),new Color(.21f,.23f,.26f)};
        public bool LookingAtHomeDecor(Camera camera){
            if(!IsHome||camera==null)return false;var offset=transform.TransformPoint(HomeDecorBook)-camera.transform.position;
            return offset.sqrMagnitude<10&&Vector3.Dot(camera.transform.forward,offset.normalized)>.94f;
        }
        void BuildHomeDecorBook(){
            Block(HomeDecorBook,new Vector3(.64f,.075f,.44f),Kaos,"Paper",.02f);
            Block(HomeDecorBook+Vector3.up*.041f,new Vector3(.55f,.015f,.37f),Cream,"Paper",.015f);
            Block(HomeDecorBook+new Vector3(0,.051f,.04f),new Vector3(.39f,.009f,.12f),Kaos,"Paper",.006f);
            for(int i=0;i<3;i++)Block(HomeDecorBook+new Vector3(-.08f+i*.08f,.051f,-.09f),new Vector3(.042f,.009f,.055f),Ink,"Paper",.003f);
        }
        public void ApplyHomeDecor(HomeDecor value){
            if(!IsHome)return;value.Validate();
            foreach(var renderer in GetComponentsInChildren<MeshRenderer>(true)){
                if(!renderer.name.StartsWith("Arquitetura — "))continue;
                var filter=renderer.GetComponent<MeshFilter>();if(filter==null)continue;var mesh=filter.sharedMesh;
                if(renderer.name=="Arquitetura — CityLeaves")renderer.gameObject.SetActive(value.plants);
                if(renderer.name=="Arquitetura — CityBrick"&&mesh.bounds.center.x>4.5f)renderer.gameObject.SetActive(value.plants);
                if(renderer.name=="Arquitetura — HomeFloor"&&(activeHomeDecor==null||activeHomeDecor.floor!=value.floor)){
                    if(!homeDecorOriginal.TryGetValue(mesh,out var floorOriginal)){floorOriginal=mesh.colors;homeDecorOriginal.Add(mesh,floorOriginal);}
                    var floorColors=(Color[])floorOriginal.Clone();if(value.floor>0)for(int i=0;i<floorColors.Length;i++)floorColors[i]=(value.floor==1?new Color(.34f,.24f,.16f):Cream)*(floorOriginal[i].r/.68f);mesh.colors=floorColors;
                    foreach(var material in renderer.sharedMaterials){
                    if(value.floor==0)AmbientSurfaces.Apply(material,"HomeFloor",true);
                    else{material.mainTexture=Resources.Load<Texture2D>("Surfaces/"+(value.floor==1?"WoodFloor040":"Tiles107"));material.SetFloat("_SingleSurface",1);material.SetFloat("_SurfaceMean",value.floor==1?.55374473f:.90368061f);material.SetFloat("_SurfaceContrast",3);material.SetFloat("_TexScale",value.floor==1?.24f:.55f);material.SetColor("_SurfaceTint",Color.white);}
                    }
                }
                if(renderer.name!="Arquitetura — CityStucco"&&renderer.name!="Arquitetura — Cloth"&&renderer.name!="Arquitetura — Rug")continue;
                if(!homeDecorOriginal.TryGetValue(mesh,out var original)){original=mesh.colors;homeDecorOriginal.Add(mesh,original);}
                var colors=(Color[])original.Clone();var vertices=mesh.vertices;
                for(int i=0;i<colors.Length;i++){
                    var color=original[i];var point=vertices[i];
                    if(renderer.name=="Arquitetura — CityStucco"&&point.y<4.3f&&(Mathf.Abs(point.x)>5.45f||Mathf.Abs(point.z)>4.45f)){
                        bool back=color.g>color.r*1.6f&&color.b>color.r*1.6f;int choice=back?value.accent:value.wall;
                        if(choice>0)colors[i]=HomeDecorColors[choice]*(color.maxColorComponent/(back?.32f:.94f));
                    }else if(renderer.name=="Arquitetura — Cloth"&&point.x<-2.2f&&point.z>.6f&&point.y>.84f&&point.y<1.2f&&color.r>color.g*1.5f&&value.bedding>0){
                        bool fold=color.r>.6f;var target=fold?Color.Lerp(HomeDecorColors[value.bedding],Cream,.2f):HomeDecorColors[value.bedding];colors[i]=target*(color.maxColorComponent/(fold?.74f:.54f));
                    }else if(renderer.name=="Arquitetura — Rug"&&value.rug>0){
                        if(color.maxColorComponent<.4f)colors[i]=HomeDecorColors[value.rug]*.72f;
                        else if(color.r>.5f&&color.g<.6f)colors[i]=Color.Lerp(HomeDecorColors[value.rug],Cream,.35f);
                    }
                }
                mesh.colors=colors;
            }
            if(homeArtBed!=null)SetFramedPrint(homeArtBed,value.poster,new Vector2(1.45f,1.65f));
            if(homeArtSide!=null)SetFramedPrint(homeArtSide,value.posterSide,new Vector2(1.45f,1.45f));
            if(homeArtRecords!=null)SetFramedPrint(homeArtRecords,value.posterRecords,new Vector2(1.34f,1.52f));
            activeHomeDecor=value.Copy();
        }
    }
    public partial class FirstCustomer
    {
        HomeDecor homeDecorDraft,homeDecorBefore,savedHomeDecor;string homeDecorPathLoaded,homeDecorError="";int homeDecorView,homeDecorTab,homeDecorArt;float homeDecorSavedUntil;
        static readonly Vector3[] HomeDecorPreviewPositions={new Vector3(3.7f,3.4f,-2.9f),new Vector3(.6f,3.05f,.15f),new Vector3(.4f,2.8f,-.25f),new Vector3(1.7f,2.8f,-2.5f),new Vector3(-1.5f,2.8f,.2f),new Vector3(3.2f,3.8f,-2.8f)};
        static readonly Vector3[] HomeDecorPreviewTargets={new Vector3(-.55f,1.65f,1.45f),new Vector3(-3.4f,1.4f,2.6f),new Vector3(-3.8f,1.6f,-2.65f),new Vector3(4.9f,1.8f,-.4f),new Vector3(-5.4f,2.8f,-.15f),new Vector3(.2f,-.03f,-.42f)};
        string HomeDecorPath=>Path.Combine(SessionFolder,"home-style.json");
        HomeDecor CurrentHomeDecor{get{
            if(savedHomeDecor==null||homeDecorPathLoaded!=HomeDecorPath){homeDecorPathLoaded=HomeDecorPath;try{savedHomeDecor=HomeDecorStore.Load(homeDecorPathLoaded);}catch{savedHomeDecor=new HomeDecor();message="Não foi possível ler a decoração do quarto.";}}
            return savedHomeDecor;
        }}
        void OpenHomeDecor(){
            if(stage!=Stage.City||!city.IsHome||paused||loadingActive||!city.LookingAtHomeDecor(cam))return;
            homeDecorBefore=CurrentHomeDecor.Copy();homeDecorDraft=homeDecorBefore.Copy();homeDecorView=homeDecorTab=homeDecorArt=0;homeDecorError="";
            // Reuse the shared decoration modal state, including all input locks.
            decorCameraReady=false;showShopDecor=true;city.CancelPlayerEmote(true);city.StopWalk();cityFreeCursor=true;cityDragLook=false;SyncCityCursor(true);Sound(0);FrameHomeDecor();
        }
        void FrameHomeDecor(){
            var positions=HomeDecorPreviewPositions;var targets=HomeDecorPreviewTargets;
            var position=city.transform.position+positions[homeDecorView];var target=city.transform.position+targets[homeDecorView];var right=Quaternion.LookRotation(target-position)*Vector3.right;
            FrameDecorCamera(position,target-right*(homeDecorView==0?1.4f:.9f),homeDecorView==0?75:60);
            city.PlayerBodyVisible=false;city.RenderHomeMirror(cam);city.PlayerBodyVisible=false;
        }
        void CloseHomeDecor(bool save){
            if(!showShopDecor||homeDecorDraft==null)return;
            if(save)try{HomeDecorStore.Save(HomeDecorPath,homeDecorDraft);savedHomeDecor=homeDecorDraft.Copy();message="Seu quarto foi salvo.";homeDecorSavedUntil=Time.unscaledTime+3;}catch(Exception error){homeDecorError="Não foi possível salvar. Tente novamente ou cancele.\n"+error.Message;return;}
            city.ApplyHomeDecor(save?homeDecorDraft:homeDecorBefore);showShopDecor=false;homeDecorDraft=null;cityFreeCursor=false;cityDragLook=false;cityLookCaptured=false;decorCameraReady=false;
            city.Frame(cam,reducedMotion,0);city.FramePlayerForCamera(cam,false);
        }
        void DrawHomeDecorNotice(){
            if(city==null||!city.IsHome||paused||showShopDecor||Time.unscaledTime>=homeDecorSavedUntil)return;
            DrawKaosPanel(new Rect(38,108,386,54));Txt(new Rect(54,122,354,30),"Decoração do quarto salva.",caption,paper);
        }
        void HomeDecorUI(){
            DrawKaosPanel(new Rect(30,108,570,766));Txt(new Rect(55,130,510,48),"Seu canto. Seu jeito.",heading);
            Txt(new Rect(55,191,510,50),"Cores, tecidos e artes.\nVeja tudo no próprio quarto.",caption,muted);
            for(int i=0;i<3;i++)if(Tap(new Rect(55+i*173,248,165,42),new[]{"Ambiente","Têxteis","Artes"}[i],homeDecorTab==i)){homeDecorTab=i;homeDecorView=i==1?1:i==2?(homeDecorArt==0?1:homeDecorArt==1?4:3):0;FrameHomeDecor();}
            var names=new[]{"Original","Jade","Uva","Rosa","Azul","Âmbar","Terracota","Carvão"};
            if(homeDecorTab<2){
                int[] rows=homeDecorTab==0?new[]{0,1,2}:new[]{3,4};float y=315;
                foreach(int row in rows){
                    Txt(new Rect(55,y,510,26),new[]{"Paredes","Parede da cama","Piso","Roupa de cama","Tapete"}[row],caption);
                    var choices=row==2?new[]{"Original","Escuro","Cerâmica"}:names;
                    int selected=row==0?homeDecorDraft.wall:row==1?homeDecorDraft.accent:row==2?homeDecorDraft.floor:row==3?homeDecorDraft.bedding:homeDecorDraft.rug;
                    for(int i=0;i<choices.Length;i++)if(Tap(new Rect(55+i%4*130,y+28+i/4*40,122,34),choices[i],selected==i)){
                        if(row==0)homeDecorDraft.wall=i;else if(row==1)homeDecorDraft.accent=i;else if(row==2)homeDecorDraft.floor=i;else if(row==3)homeDecorDraft.bedding=i;else homeDecorDraft.rug=i;
                        city.ApplyHomeDecor(homeDecorDraft);FocusHomeDecor(row);
                    }
                    y+=44+Mathf.CeilToInt(choices.Length/4f)*40;
                }
                if(homeDecorTab==0&&Tap(new Rect(55,666,510,42),homeDecorDraft.plants?"Planta: presente":"Planta: recolhida")){homeDecorDraft.plants=!homeDecorDraft.plants;city.ApplyHomeDecor(homeDecorDraft);FocusHomeDecor(6);}
                if(homeDecorTab==1)Txt(new Rect(55,604,510,60),"As texturas e os detalhes dos tecidos\ncontinuam presentes em cada cor.",caption,muted);
            }else{
                for(int i=0;i<3;i++)if(Tap(new Rect(55+i*173,314,165,40),new[]{"Da cama","Da parede","Dos discos"}[i],homeDecorArt==i)){homeDecorArt=i;homeDecorView=i==0?1:i==1?4:3;FrameHomeDecor();}
                Txt(new Rect(55,370,510,26),"Quadro",caption);
                int selected=homeDecorArt==0?homeDecorDraft.poster:homeDecorArt==1?homeDecorDraft.posterSide:homeDecorDraft.posterRecords;
                for(int i=0;i<GalleryArt.Names.Length;i++)if(Tap(new Rect(55+i%4*130,400+i/4*40,122,34),GalleryArt.Names[i],selected==i)){
                    if(homeDecorArt==0)homeDecorDraft.poster=i;else if(homeDecorArt==1)homeDecorDraft.posterSide=i;else homeDecorDraft.posterRecords=i;city.ApplyHomeDecor(homeDecorDraft);FocusHomeDecor(5);
                }
            }
            if(homeDecorError.Length>0)Txt(new Rect(55,716,510,58),homeDecorError,caption,new Color(1,.55f,.45f));
            if(Tap(new Rect(55,782,246,55),"Salvar decoração",true))CloseHomeDecor(true);
            if(Tap(new Rect(319,782,246,55),"Cancelar"))CloseHomeDecor(false);
            for(int i=0;i<6;i++)if(Tap(new Rect(620+i*132,713,124,48),new[]{"Geral","Cama","Mesa","Discos","Parede","Piso"}[i],homeDecorView==i)){homeDecorView=i;FrameHomeDecor();}
            RoundedPanel(new Rect(620,782,792,56),new Color(.035f,.03f,.035f,.86f),10);
            Txt(new Rect(636,797,760,30),"Olhe a prévia. As mudanças pertencem a este save.",caption,paper);
        }
    }
}
