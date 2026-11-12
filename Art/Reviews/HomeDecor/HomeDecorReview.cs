using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace FitCheck.Editor
{
    [InitializeOnLoad] public static class HomeDecorReview
    {
        const string Key="FitCheck.HomeDecorReview";static int frames;
        const BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
        static HomeDecorReview(){EditorApplication.update+=Tick;}
        public static void Capture(){
            if(!Application.isBatchMode)throw new Exception("Isolated batch only");var args=Environment.GetCommandLineArgs();
            SessionState.SetString(Key,args[Array.IndexOf(args,"-home-decor-output")+1]);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            new GameObject("Disabled campaign").AddComponent<FirstCustomer>().enabled=false;EditorApplication.isPlaying=true;
        }
        static object Get(object target,string name)=>target.GetType().GetField(name,Private).GetValue(target);
        static void Set(object target,string name,object value)=>target.GetType().GetField(name,Private).SetValue(target,value);
        static object Call(object target,string name,params object[] args)=>target.GetType().GetMethod(name,Private).Invoke(target,args);
        static HomeDecor Current(FirstCustomer target)=>(HomeDecor)target.GetType().GetProperty("CurrentHomeDecor",Private).GetValue(target);
        static void Assert(bool condition,string message){if(!condition)throw new Exception(message);}
        static void Tick(){
            string folder=SessionState.GetString(Key,"");if(folder.Length==0||!EditorApplication.isPlaying||EditorApplication.isCompiling||++frames<5)return;SessionState.EraseString(Key);
            try{
                Directory.CreateDirectory(folder);string data=Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N"));var controller=UnityEngine.Object.FindAnyObjectByType<FirstCustomer>();
                var studio=new GameObject("Source avatar").AddComponent<StudioView>();studio.Build();studio.PlayerLook=new PlayerAppearance{created=true};studio.ApplyUniform(0);
                var city=new GameObject("Isolated home").AddComponent<CityView>();city.BuildHome();city.ApplyHomeDecor(new HomeDecor());city.PreparePlayerBody(studio);studio.Avatar.gameObject.SetActive(false);
                var camera=studio.Camera;camera.enabled=false;camera.GetComponent<RetroCamera>().Filter=false;camera.farClipPlane=40;camera.aspect=1.6f;
                Set(controller,"studio",studio);Set(controller,"cam",camera);Set(controller,"mannequin",studio.Avatar);Set(controller,"city",city);Set(controller,"homeRoom",city);Set(controller,"storageOverride",Path.Combine(data,"session-A"));Set(controller,"mutedAudio",true);
                var stage=typeof(FirstCustomer).GetField("stage",Private);stage.SetValue(controller,Enum.Parse(stage.FieldType,"City"));
                foreach(var position in new[]{new Vector2(-4.9f,-.2f),new Vector2(-1.2f,3.7f),new Vector2(-2,-3.7f),new Vector2(4.8f,-3.7f),new Vector2(4.8f,2.2f)}){
                    Aim(city,camera,studio.Avatar,position,city.transform.position+new Vector3(5.39f,1.83f,1.36f));
                    Assert(Vector2.Distance(city.Position,position)<.01f,"Mirror position blocked: "+position);
                    typeof(CityView).GetField("nextHomeMirrorFrame",Private).SetValue(city,0f);Assert(city.RenderHomeMirror(camera),"Mirror missing at "+position);
                    var reflected=(Camera)typeof(CityView).GetField("homeMirrorCamera",Private).GetValue(city);
                    float distance=5.39f-position.x;Assert(reflected.farClipPlane>distance+11,"Mirror clips far side of room");
                    Assert(reflected.targetTexture.width==128&&reflected.targetTexture.height==352,"Mirror resolution increased");Assert(!city.PlayerHeadVisible,"Mirror leaked head");
                    Save(camera,Path.Combine(folder,"mirror-"+position.x+"-"+position.y+".png"));
                }
                AimAtBook(city,camera,studio.Avatar);Assert(city.LookingAtHomeDecor(camera),"Book cannot be targeted");
                var hint=Call(controller,"CurrentWorldHint");Assert((string)hint.GetType().GetField("key").GetValue(hint)=="F","Book lacks F affordance");Save(camera,Path.Combine(folder,"book.png"));
                long original=ColorHash(city);int renderers=city.GetComponentsInChildren<Renderer>(true).Length,triangles=Triangles(city);
                string pathA=Path.Combine(data,"session-A/home-style.json");bool existed=File.Exists(pathA);
                Call(controller,"OpenShopDecor");Assert((bool)Get(controller,"showShopDecor"),"Home modal not opened by shared entry");
                var originalPosition=city.Position;Call(controller,"UpdateCity",.05f);Assert(city.Position==originalPosition,"Decoration moved the player");
                var draft=(HomeDecor)Get(controller,"homeDecorDraft");draft.wall=4;draft.accent=3;draft.floor=2;draft.bedding=3;draft.rug=1;draft.poster=20;draft.posterSide=14;draft.posterRecords=7;draft.plants=false;city.ApplyHomeDecor(draft);
                Assert(ColorHash(city)!=original,"Preview does not change the room");Assert(city.GetComponentsInChildren<Renderer>(true).Length==renderers&&Triangles(city)==triangles,"Preview creates duplicate geometry");city.CheckPrints();
                for(int tab=0;tab<3;tab++){
                    Set(controller,"homeDecorTab",tab);Set(controller,"homeDecorView",tab==1?1:tab==2?4:0);Set(controller,"homeDecorArt",tab==2?1:0);Call(controller,"FrameHomeDecor");
                    DrawUI(controller,camera,Path.Combine(folder,"ui-"+tab+".png"));DrawUI(controller,camera,Path.Combine(folder,"ui-"+tab+".png"));
                }
                Call(controller,"CloseShopDecor",false);Assert(!(bool)Get(controller,"showShopDecor")&&city.PlayerBodyVisible,"Cancel did not restore player/control mode");
                Assert(ColorHash(city)==original,"Cancel did not restore original colors");Assert(File.Exists(pathA)==existed,"Cancel wrote a save");
                for(int color=0;color<8;color++){var value=new HomeDecor{wall=color,accent=color,bedding=color,rug=color,floor=color%3,poster=color,posterSide=(color+3)%GalleryArt.Names.Length,posterRecords=(color+5)%GalleryArt.Names.Length,plants=color%2==0};city.ApplyHomeDecor(value);city.CheckPrints();}
                city.ApplyHomeDecor(new HomeDecor());Assert(ColorHash(city)==original,"Repeated previews accumulated tint");
                AimAtBook(city,camera,studio.Avatar);Call(controller,"OpenShopDecor");draft=(HomeDecor)Get(controller,"homeDecorDraft");draft.accent=4;draft.bedding=5;draft.rug=2;draft.poster=22;draft.floor=1;var saved=draft.Copy();city.ApplyHomeDecor(draft);Call(controller,"CloseShopDecor",true);
                Assert(JsonUtility.ToJson(HomeDecorStore.Load(pathA))==JsonUtility.ToJson(saved),"Save/reload lost style");string bytesA=File.ReadAllText(pathA);
                AimAtBook(city,camera,studio.Avatar);Call(controller,"OpenShopDecor");draft=(HomeDecor)Get(controller,"homeDecorDraft");draft.bedding=7;city.ApplyHomeDecor(draft);Call(controller,"HandleEscapeAction");
                Assert(city.ActiveHomeDecor.bedding==5&&File.ReadAllText(pathA)==bytesA,"Back did not cancel to saved style");
                Set(controller,"storageOverride",Path.Combine(data,"session-B"));city.ApplyHomeDecor(Current(controller));Assert(city.ActiveHomeDecor.bedding==0,"Second save inherited first style");
                AimAtBook(city,camera,studio.Avatar);Call(controller,"OpenShopDecor");draft=(HomeDecor)Get(controller,"homeDecorDraft");draft.wall=1;city.ApplyHomeDecor(draft);Call(controller,"CloseShopDecor",true);
                Set(controller,"storageOverride",Path.Combine(data,"session-A"));city.ApplyHomeDecor(Current(controller));Assert(JsonUtility.ToJson(city.ActiveHomeDecor)==JsonUtility.ToJson(saved)&&File.ReadAllText(pathA)==bytesA,"Save isolation failed");
                string recovery=Path.Combine(data,"recovery/home-style.json");HomeDecorStore.Save(recovery,new HomeDecor{wall=1});HomeDecorStore.Save(recovery,new HomeDecor{wall=2});File.WriteAllText(recovery,"{bad");Assert(HomeDecorStore.Load(recovery).wall==1,"Backup recovery failed");
                bool rejected=false;try{HomeDecorStore.Save(recovery,new HomeDecor{wall=99});}catch(ArgumentException){rejected=true;}Assert(rejected,"Invalid values accepted");
                string bad=Path.Combine(data,"not-a-folder");File.WriteAllText(bad,"keep");Set(controller,"storageOverride",bad);city.ApplyHomeDecor(Current(controller));AimAtBook(city,camera,studio.Avatar);Call(controller,"OpenShopDecor");Call(controller,"CloseShopDecor",true);
                Assert((bool)Get(controller,"showShopDecor")&&((string)Get(controller,"homeDecorError")).Length>0,"Save error closed preview or lacked feedback");Call(controller,"CloseShopDecor",false);Assert(File.ReadAllText(bad)=="keep","Save error modified unrelated file");
                var shop=new GameObject("Shop regression").AddComponent<CityView>();shop.BuildShop(studio,0);Set(controller,"city",shop);Set(controller,"storageOverride",Path.Combine(data,"shop"));Call(controller,"OpenShopDecor");Assert((bool)Get(controller,"showShopDecor")&&Get(controller,"homeDecorDraft")==null,"Shop route broken");Call(controller,"CloseShopDecor",false);Assert(!(bool)Get(controller,"showShopDecor"),"Shop cancel broken");
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"HOME_DECOR_REVIEW_PASSED: mirror across room/far plane/head restore, real UI, preview/cancel/back, save/reload, independent slots, backup recovery, invalid values, save error, all artwork, no geometry growth. Isolated data only. Renderers="+renderers+" triangles="+triangles);
                Debug.Log("HOME_DECOR_REVIEW_PASSED");EditorApplication.Exit(0);
            }catch(Exception error){Debug.LogException(error);EditorApplication.Exit(1);}
        }
        static void AimAtBook(CityView city,Camera camera,Transform avatar)=>Aim(city,camera,avatar,CityView.HomeDesk,city.transform.TransformPoint(CityView.HomeDecorBook));
        static void Aim(CityView city,Camera camera,Transform avatar,Vector2 position,Vector3 target){
            var direction=target-(city.transform.position+new Vector3(position.x,CityView.EyeHeight,position.y));float yaw=Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg,pitch=-Mathf.Atan2(direction.y,new Vector2(direction.x,direction.z).magnitude)*Mathf.Rad2Deg;
            city.RestoreWalk(position,yaw,pitch,avatar);city.Frame(camera,true,0);city.FramePlayerForCamera(camera,false);
        }
        static int Triangles(CityView city)=>city.GetComponentsInChildren<MeshFilter>(true).Sum(f=>f.sharedMesh.triangles.Length/3);
        static long ColorHash(CityView city){unchecked{long hash=17;foreach(var f in city.GetComponentsInChildren<MeshFilter>(true))if(f.name.StartsWith("Arquitetura — "))foreach(var c in f.sharedMesh.colors){hash=hash*31+c.r.GetHashCode();hash=hash*31+c.g.GetHashCode();hash=hash*31+c.b.GetHashCode();}return hash;}}
        static void Save(Camera camera,string path){var target=RenderTexture.GetTemporary(1440,900,24);var old=camera.targetTexture;camera.targetTexture=target;camera.Render();Read(target,path);camera.targetTexture=old;RenderTexture.ReleaseTemporary(target);}
        static void Read(RenderTexture target,string path){var active=RenderTexture.active;RenderTexture.active=target;var texture=new Texture2D(1440,900,TextureFormat.RGB24,false);texture.ReadPixels(new Rect(0,0,1440,900),0,0);texture.Apply();File.WriteAllBytes(path,texture.EncodeToPNG());UnityEngine.Object.Destroy(texture);RenderTexture.active=active;}
        static void DrawUI(FirstCustomer controller,Camera camera,string path){
            const BindingFlags flags=BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;var gui=typeof(GUIUtility);var state=Activator.CreateInstance(gui.Assembly.GetType("UnityEngine.ObjectGUIState"),true);
            var begin=gui.GetMethod("BeginContainer",flags);var end=gui.GetMethod("EndContainer",flags);var beginGui=gui.GetMethod("BeginGUI",flags);
            var entity=typeof(UnityEngine.Object).GetMethod("GetEntityId",BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).Invoke(controller,null);
            var target=RenderTexture.GetTemporary(1440,900,24);var old=camera.targetTexture;var active=RenderTexture.active;var previous=Event.current;bool begun=false,pushed=false;
            try{camera.targetTexture=target;camera.Render();RenderTexture.active=target;begin.Invoke(null,new[]{state});begun=true;beginGui.Invoke(null,new object[]{0,entity,0});GL.PushMatrix();pushed=true;GL.LoadPixelMatrix(0,1440,900,0);GUI.matrix=Matrix4x4.identity;
                Event.current=new Event{type=EventType.Layout,mousePosition=new Vector2(-100,-100)};Call(controller,"InterfaceStyles");Call(controller,"HomeDecorUI");
                Event.current=new Event{type=EventType.Repaint,mousePosition=new Vector2(-100,-100)};Call(controller,"InterfaceStyles");Call(controller,"HomeDecorUI");end.Invoke(null,null);begun=false;GL.Flush();Read(target,path);}
            finally{if(begun)end.Invoke(null,null);if(pushed)GL.PopMatrix();Event.current=previous;camera.targetTexture=old;RenderTexture.active=active;RenderTexture.ReleaseTemporary(target);}
        }
    }
}
