using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace FitCheck.Editor
{
    [InitializeOnLoad] public static class PendingReview
    {
        const string Key="FitCheck.PendingReview";const BindingFlags P=BindingFlags.Instance|BindingFlags.NonPublic;static int frames;
        static PendingReview(){EditorApplication.update+=Tick;}
        public static void Capture(){var args=Environment.GetCommandLineArgs();SessionState.SetString(Key,args[Array.IndexOf(args,"-pending-output")+1]);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("Disabled campaign").AddComponent<FirstCustomer>().enabled=false;EditorApplication.isPlaying=true;}
        static object G(object o,string n)=>o.GetType().GetField(n,P).GetValue(o);static void S(object o,string n,object v)=>o.GetType().GetField(n,P).SetValue(o,v);static object C(object o,string n,params object[] a)=>o.GetType().GetMethod(n,P).Invoke(o,a);
        static object Stage(string n)=>Enum.Parse(typeof(FirstCustomer).GetNestedType("Stage",BindingFlags.NonPublic),n);
        static void A(bool c,string m){if(!c)throw new Exception(m);}
        static void Draw(FirstCustomer c,Camera camera,string path,string method)=>typeof(CompanionsReview).GetMethod("Draw",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{c,camera,path,method});
        static void Save(Camera camera,string path)=>typeof(CompanionsReview).GetMethod("Save",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{camera,path});
        static void Tick(){
            string folder=SessionState.GetString(Key,"");if(folder.Length==0||!EditorApplication.isPlaying||EditorApplication.isCompiling||++frames<5)return;SessionState.EraseString(Key);
            try{
                Directory.CreateDirectory(folder);CampaignChecks.Validate();DropCatalogChecks.Validate();
                A(DayOne.Customers.Length==21&&DayProgress.TotalDays==7,"Campaign expanded unexpectedly");
                A(DayOne.Customers[11].name=="Iara Ponteiro"&&(int)typeof(DayOne).GetMethod("CustomerProfile",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{11})==11&&(int)typeof(DayOne).GetMethod("CustomerProfile",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{14})==12&&(int)typeof(DayOne).GetMethod("CustomerProfile",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{15})==13,"Replacement clients missing");
                var controller=UnityEngine.Object.FindAnyObjectByType<FirstCustomer>();var studio=new GameObject("Review studio").AddComponent<StudioView>();studio.Build();studio.PlayerLook=new PlayerAppearance{created=true};studio.ApplyUniform(0);var camera=studio.Camera;camera.enabled=false;camera.GetComponent<RetroCamera>().Filter=false;camera.aspect=1.6f;
                S(controller,"studio",studio);S(controller,"cam",camera);S(controller,"mannequin",studio.Avatar);S(controller,"mutedAudio",true);S(controller,"storageOverride",Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N")));S(controller,"reducedMotion",true);
                foreach(int i in new[]{3,11,14,15}){
                    S(controller,"customer",i);S(controller,"stage",Stage("Dialogue"));C(controller,"EnterDressing");float seconds=i==3?30:i==11?45:120;
                    A((float)G(controller,"remaining")==seconds,"Wrong fitting duration");S(controller,"paused",true);C(controller,"AdvanceClock",5f);A((float)G(controller,"remaining")==seconds,"Paused timer moved");S(controller,"paused",false);C(controller,"AdvanceClock",1f);A((float)G(controller,"remaining")==seconds-(i==3||i==11?1:0),"Wrong timer progression");
                    studio.Apply(Catalog.ArrivalOutfit,i);studio.Pose(-12,6.8f,0);camera.fieldOfView=45;
                    S(controller,"stage",Stage("Dialogue"));Draw(controller,camera,Path.Combine(folder,"client-"+i+".png"),"DialogueUI");Draw(controller,camera,Path.Combine(folder,"client-"+i+".png"),"DialogueUI");
                    if(i!=3){camera.transform.position=new Vector3(0,2.8f,-3.2f);camera.transform.LookAt(new Vector3(0,2.8f,0));camera.fieldOfView=36;Save(camera,Path.Combine(folder,"portrait-"+i+".png"));}
                }
                studio.Avatar.gameObject.SetActive(false);
                for(int breed=0;breed<3;breed++){
                    var go=new GameObject("White cat");var pet=go.AddComponent<ShopCat>();pet.Build(new PetAppearance{coat=4,breed=breed});A(new PetAppearance{coat=4}.CoatName=="Branco","Wrong cat label");A(go.GetComponentsInChildren<MeshRenderer>().All(r=>r.name!="Mancha bicolor"),"Cat still patched");A(go.GetComponentsInChildren<MeshRenderer>().First(r=>r.name=="Cabeça arredondada").sharedMaterial.color.r>.9f,"White Siamese still masked");A(pet.Pet(true),"Pet failed");pet.Tick(.1f,true,false,true);A(pet.Age==0,"Paused pet ages");pet.Tick(2,false,false,true);var purr=(AudioSource)G(pet,"purr");A(purr!=null&&purr.clip!=null&&purr.mute,"Purr not connected/muted");pet.Tick(3,false,false,true);A(pet.Pet(true)&&pet.MeowClip.name=="cat_meow_alt_02","No alternate meow");camera.transform.position=new Vector3(0,.85f,-2.3f);camera.transform.LookAt(new Vector3(0,.33f,0));camera.fieldOfView=45;Save(camera,Path.Combine(folder,"white-cat-"+breed+".png"));go.SetActive(false);UnityEngine.Object.Destroy(go);
                }
                var dog=new GameObject("Bicolor dog").AddComponent<ShopCat>();dog.Build(new PetAppearance{species=1,coat=4});A(dog.GetComponentsInChildren<MeshRenderer>().Any(r=>r.name=="Mancha bicolor"),"Dog coat changed");dog.gameObject.SetActive(false);
                var home=new GameObject("Mirror review").AddComponent<CityView>();home.BuildHome();studio.Avatar.gameObject.SetActive(true);home.PreparePlayerBody(studio);studio.Avatar.gameObject.SetActive(false);S(controller,"city",home);S(controller,"stage",Stage("City"));
                var legacy=JsonUtility.FromJson<HomeDecor>("{\"version\":1}");legacy.Validate();A(legacy.mirror==0,"Legacy frame changed");
                typeof(HomeDecorReview).GetMethod("AimAtBook",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{home,camera,studio.Avatar});C(controller,"OpenShopDecor");
                int meshCount=home.GetComponentsInChildren<MeshFilter>(true).Length;var draft=(HomeDecor)G(controller,"homeDecorDraft");S(controller,"homeDecorTab",3);S(controller,"homeDecorView",6);
                for(int style=1;style<=2;style++){draft.mirror=style;home.ApplyHomeDecor(draft);C(controller,"FrameHomeDecor");Draw(controller,camera,Path.Combine(folder,"mirror-ui-"+style+".png"),"HomeDecorUI");Draw(controller,camera,Path.Combine(folder,"mirror-ui-"+style+".png"),"HomeDecorUI");A(home.HomeMirrorStyle==style&&home.GetComponentsInChildren<MeshFilter>(true).Length==meshCount,"Frame change grew scene");}
                C(controller,"CloseShopDecor",false);A(home.HomeMirrorStyle==0,"Cancel did not restore frame");C(controller,"OpenHomeDecor");
                // Re-aim because close restored the normal view.
                if(!(bool)G(controller,"showShopDecor")){typeof(HomeDecorReview).GetMethod("AimAtBook",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{home,camera,studio.Avatar});C(controller,"OpenShopDecor");}
                draft=(HomeDecor)G(controller,"homeDecorDraft");draft.mirror=1;home.ApplyHomeDecor(draft);C(controller,"CloseShopDecor",true);A(HomeDecorStore.Load(Path.Combine((string)G(controller,"storageOverride"),"home-style.json")).mirror==1,"Frame not saved");
                home.ApplyHomeDecor(new HomeDecor{mirror=1});home.RestoreWalk(new Vector2(1.8f,1.36f),90,0,studio.Avatar);home.Frame(camera,true,0);home.FramePlayerForCamera(camera,false);camera.transform.LookAt(home.transform.position+new Vector3(5.39f,1.83f,1.36f));S(home,"nextHomeMirrorFrame",0f);A(home.RenderHomeMirror(camera),"Retro mirror not reflecting");Save(camera,Path.Combine(folder,"paint-reflection.png"));
                var mirror=(Camera)G(home,"homeMirrorCamera");A(mirror.targetTexture.width==128&&mirror.targetTexture.height==352,"Mirror budget changed");A(((Transform)G(home,"homeMirrorTrim")).GetComponent<MeshFilter>().sharedMesh.triangles.Length==24,"Frame is not four strips");
                C(controller,"BuildAdditionalSounds");var spots=(Array)G(controller,"streetSounds");A(spots.Length==8,"Street banks missing");foreach(var spot in spots){var bank=(AudioClip[])spot.GetType().GetField("clips").GetValue(spot);A(bank.All(clip=>clip!=null),"Missing city sound");}
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"PENDING_REVIEW_PASSED: 21 encounters, 3 replacement characters, stable scores/history, 30/45 second timers and pause, white cats/dog coat, purr/alternate voices, 8 local sound banks, mirror migration/cancel/save/real reflection and unchanged render budget.");Debug.Log("PENDING_REVIEW_PASSED");EditorApplication.Exit(0);
            }catch(Exception error){Debug.LogException(error);EditorApplication.Exit(1);}
        }
    }
}
