using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace FitCheck.Editor
{
    [InitializeOnLoad] public static class CompanionsReview
    {
        const string Key="FitCheck.CompanionsReview";const BindingFlags P=BindingFlags.Instance|BindingFlags.NonPublic;static int frames;
        static CompanionsReview(){EditorApplication.update+=Tick;}
        public static void Capture(){var args=Environment.GetCommandLineArgs();if(!Application.isBatchMode)throw new Exception("Isolated batch only");SessionState.SetString(Key,args[Array.IndexOf(args,"-companions-output")+1]);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("Disabled campaign").AddComponent<FirstCustomer>().enabled=false;EditorApplication.isPlaying=true;}
        static object G(object o,string n)=>o.GetType().GetField(n,P).GetValue(o);static void S(object o,string n,object v)=>o.GetType().GetField(n,P).SetValue(o,v);static object C(object o,string n,params object[] a)=>o.GetType().GetMethod(n,P).Invoke(o,a);
        static object Stage(string n)=>Enum.Parse(typeof(FirstCustomer).GetNestedType("Stage",BindingFlags.NonPublic),n);
        static void A(bool c,string m){if(!c)throw new Exception(m);}
        static void Tick(){
            string folder=SessionState.GetString(Key,"");if(folder.Length==0||!EditorApplication.isPlaying||EditorApplication.isCompiling||++frames<5)return;SessionState.EraseString(Key);
            try{
                Directory.CreateDirectory(folder);string library=Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N"));
                var controller=UnityEngine.Object.FindAnyObjectByType<FirstCustomer>();var studio=new GameObject("Source studio").AddComponent<StudioView>();studio.Build();studio.PlayerLook=new PlayerAppearance{created=true};studio.ApplyUniform(0);var camera=studio.Camera;camera.enabled=false;camera.GetComponent<RetroCamera>().Filter=false;camera.aspect=1.6f;
                S(controller,"studio",studio);S(controller,"cam",camera);S(controller,"mannequin",studio.Avatar);S(controller,"mutedAudio",true);S(controller,"storageOverride",SaveSlots.Folder(library,0));S(controller,"stage",Stage("Welcome"));
                var legacy=JsonUtility.FromJson<PlayerAppearance>("{\"version\":1,\"name\":\"Antigo\",\"created\":true}");legacy.Validate();A(legacy.pet!=null,"Legacy pet missing");
                var appearance=new PlayerAppearance{created=true,name="Teste",pet=new PetAppearance{name="Luma",species=0,breed=2,coat=3}};var clone=appearance.Copy();clone.pet.name="Outra";A(appearance.pet.name=="Luma","Pet copy aliases save");
                for(int slot=0;slot<3;slot++){var value=appearance.Copy();value.pet.name="Pet "+slot;value.pet.species=slot%2;value.pet.breed=slot;AppearanceStore.Save(Path.Combine(SaveSlots.Folder(library,slot),"player-appearance.json"),value);A(SaveSlots.Read(library,slot).appearance.pet.name=="Pet "+slot,"Slot pet isolation failed");}
                bool invalid=false;try{new PetAppearance{coat=99}.Validate();}catch(ArgumentException){invalid=true;}A(invalid,"Invalid pet accepted");
                S(controller,"playerAppearance",appearance);C(controller,"OpenCreator",Stage("Welcome"));S(controller,"creatorPetTab",true);C(controller,"RefreshCreatorPet");C(controller,"UpdateCamera");
                Draw(controller,camera,Path.Combine(folder,"creator-cat.png"),"CreatorUI");Draw(controller,camera,Path.Combine(folder,"creator-cat.png"),"CreatorUI");
                var draft=(PlayerAppearance)G(controller,"creatorDraft");draft.pet=new PetAppearance{name="Juca",species=1,breed=1,coat=0};C(controller,"RefreshCreatorPet");C(controller,"UpdateCamera");Draw(controller,camera,Path.Combine(folder,"creator-dog.png"),"CreatorUI");Draw(controller,camera,Path.Combine(folder,"creator-dog.png"),"CreatorUI");
                C(controller,"CancelCreator");A(((PlayerAppearance)G(controller,"playerAppearance")).pet.name=="Luma"&&G(controller,"creatorPetPreview")==null&&studio.Avatar.gameObject.activeSelf,"Creator cancel leaked pet/hidden player");
                C(controller,"OpenCreator",Stage("Welcome"));draft=(PlayerAppearance)G(controller,"creatorDraft");draft.pet=new PetAppearance{name="Juca",species=1,breed=1,coat=0};A((bool)C(controller,"SaveCreator"),"Creator save failed");A(AppearanceStore.Load(Path.Combine(SaveSlots.Folder(library,0),"player-appearance.json")).pet.name=="Juca","Creator did not persist pet");
                studio.Avatar.gameObject.SetActive(false);
                for(int species=0;species<2;species++)for(int breed=0;breed<3;breed++)for(int coat=0;coat<6;coat++){
                    var go=new GameObject("Animal review");go.transform.position=new Vector3(0,.14f,0);go.transform.localScale=Vector3.one*1.5f;go.transform.rotation=Quaternion.Euler(0,-20,0);var animal=go.AddComponent<ShopCat>();animal.Build(new PetAppearance{species=species,breed=breed,coat=coat,name="Companheiro"});
                    A(animal.Ready&&!animal.IsFuba&&animal.MeowClip!=null,"Animal/audio incomplete");A(go.GetComponentsInChildren<MeshFilter>().Sum(f=>f.sharedMesh.triangles.Length/3)<18000,"Pet exceeds mesh budget");
                    if(coat==3){camera.transform.position=new Vector3(.15f,1.3f,-3);camera.transform.LookAt(new Vector3(0,.63f,0));camera.fieldOfView=48;Save(camera,Path.Combine(folder,"pet-"+species+"-"+breed+".png"));}
                    A(animal.Pet(true)&&!animal.Pet(true),"Pet cooldown failed");float age=animal.Age;animal.Tick(1,true,false,true);A(animal.Age==age,"Paused pet animates");animal.Tick(8,false,false,true);A(animal.Pet(true),"Pet voice never recovers");go.SetActive(false);UnityEngine.Object.Destroy(go);
                }
                var home=new GameObject("Home review").AddComponent<CityView>();home.BuildHome(new PetAppearance{species=1,breed=2,coat=4,name="Fubá"});home.ApplyHomeDecor(new HomeDecor());home.PreparePlayerBody(studio);studio.Avatar.gameObject.SetActive(false);
                S(controller,"city",home);S(controller,"homeRoom",home);S(controller,"stage",Stage("City"));S(controller,"reducedMotion",true);var progress=(DayProgress)G(controller,"progress");progress.fubaPets=7;
                camera.transform.position=home.Cat.PetTarget+new Vector3(0,1,-1.6f);camera.transform.LookAt(home.Cat.PetTarget);A((bool)C(controller,"TryPetCat")&&progress.fubaPets==7,"Named companion counted as Fubá");
                var aim=typeof(HomeDecorReview).GetMethod("AimAtBook",BindingFlags.Static|BindingFlags.NonPublic);aim.Invoke(null,new object[]{home,camera,studio.Avatar});C(controller,"OpenShopDecor");
                C(controller,"FocusHomeDecor",0);var initial=camera.transform.position;S(controller,"reducedMotion",false);C(controller,"FocusHomeDecor",2);
                var positions=(Vector3[])typeof(FirstCustomer).GetField("HomeDecorPreviewPositions",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);var target=home.transform.position+positions[5];
                A(Vector3.Distance(camera.transform.position,initial)>0&&Vector3.Distance(camera.transform.position,target)>.001f,"Decor camera did not animate");
                float remaining=Vector3.Distance(camera.transform.position,target);for(int f=0;f<20;f++)C(controller,"FrameHomeDecor");float expected=remaining*Mathf.Exp(-Time.unscaledDeltaTime*9*20);A(Mathf.Abs(Vector3.Distance(camera.transform.position,target)-expected)<.01f&&Vector3.Distance(camera.transform.position,target)<remaining,"Decor camera smoothing inconsistent");S(controller,"reducedMotion",true);
                int[] homeViews={4,1,5,1,5,1,3};for(int item=0;item<7;item++){C(controller,"FocusHomeDecor",item);A((int)G(controller,"homeDecorView")==homeViews[item],"Home focus item mismatch");}
                C(controller,"FocusHomeDecor",2);Draw(controller,camera,Path.Combine(folder,"home-floor-focus.png"),"HomeDecorUI");C(controller,"CloseShopDecor",false);
                var shop=new GameObject("Shop review").AddComponent<CityView>();shop.BuildShop(studio,0);shop.ConfigureCompanion(new PetAppearance{name="Juca",species=1,breed=1,coat=0});A(!shop.Cat.IsFuba&&shop.Cat.DisplayName=="Juca","Shop companion wrong");S(controller,"city",shop);shop.Frame(camera,true,0);C(controller,"OpenShopDecor");
                int[] shopViews={0,4,1,2,3,3,5};for(int item=0;item<7;item++){C(controller,"FocusShopDecor",item);A((int)G(controller,"decorView")==shopViews[item],"Shop focus item mismatch");}
                C(controller,"FocusShopDecor",4);Draw(controller,camera,Path.Combine(folder,"shop-gif-focus.png"),"ShopDecorUI");C(controller,"CloseShopDecor",false);
                home.gameObject.SetActive(false);shop.gameObject.SetActive(false);studio.Avatar.gameObject.SetActive(true);var street=new GameObject("Street Fubá").AddComponent<CityView>();street.Build(studio);street.PreparePlayerBody(studio);studio.Avatar.gameObject.SetActive(false);S(controller,"city",street);
                var position=street.FubaStreetPosition+new Vector2(0,-1.7f);street.RestoreWalk(position,0,35,studio.Avatar);A(Vector2.Distance(street.Position,position)<.01f,"Street Fubá inaccessible");street.Frame(camera,true,0);camera.transform.LookAt(street.Cat.PetTarget);
                A(street.Cat.IsFuba&&(bool)C(controller,"TryPetCat")&&progress.fubaPets==8&&progress.UniformUnlocked(DayProgress.FubaUniform),"Street Fubá progression lost");A(ProgressStore.Load(Path.Combine(SaveSlots.Folder(library,0),"day-one.json")).fubaPets==8,"Fubá progress not saved");
                street.PlayerBodyVisible=false;camera.transform.position=street.Cat.transform.TransformPoint(new Vector3(-.9f,.95f,-1.9f));camera.transform.LookAt(street.Cat.PetTarget);camera.fieldOfView=43;Save(camera,Path.Combine(folder,"fuba-street.png"));
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"COMPANIONS_REVIEW_PASSED: 36 animal variants, cat/dog voices, pause/cooldown, legacy appearance, deep copies, three save slots, creator preview/cancel/save, focused decor cameras, distinct accessible street Fubá and preserved outfit unlock/save.");Debug.Log("COMPANIONS_REVIEW_PASSED");EditorApplication.Exit(0);
            }catch(Exception error){Debug.LogException(error);EditorApplication.Exit(1);}
        }
        static void Save(Camera camera,string path){var old=camera.targetTexture;var target=RenderTexture.GetTemporary(1440,900,24);camera.targetTexture=target;camera.Render();Read(target,path);camera.targetTexture=old;RenderTexture.ReleaseTemporary(target);}
        static void Read(RenderTexture target,string path){var old=RenderTexture.active;RenderTexture.active=target;var image=new Texture2D(1440,900,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,1440,900),0,0);image.Apply();File.WriteAllBytes(path,image.EncodeToPNG());UnityEngine.Object.Destroy(image);RenderTexture.active=old;}
        static void Draw(FirstCustomer controller,Camera camera,string path,string method){
            const BindingFlags flags=BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;var gui=typeof(GUIUtility);var state=Activator.CreateInstance(gui.Assembly.GetType("UnityEngine.ObjectGUIState"),true);var begin=gui.GetMethod("BeginContainer",flags);var end=gui.GetMethod("EndContainer",flags);var start=gui.GetMethod("BeginGUI",flags);var entity=typeof(UnityEngine.Object).GetMethod("GetEntityId",BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).Invoke(controller,null);
            var old=camera.targetTexture;var active=RenderTexture.active;var previous=Event.current;var target=RenderTexture.GetTemporary(1440,900,24);bool begun=false,pushed=false;
            try{camera.targetTexture=target;camera.Render();RenderTexture.active=target;begin.Invoke(null,new[]{state});begun=true;start.Invoke(null,new object[]{0,entity,0});GL.PushMatrix();pushed=true;GL.LoadPixelMatrix(0,1440,900,0);GUI.matrix=Matrix4x4.identity;Event.current=new Event{type=EventType.Layout,mousePosition=new Vector2(-100,-100)};C(controller,"InterfaceStyles");C(controller,method);Event.current=new Event{type=EventType.Repaint,mousePosition=new Vector2(-100,-100)};C(controller,"InterfaceStyles");C(controller,method);end.Invoke(null,null);begun=false;GL.Flush();Read(target,path);}
            finally{if(begun)end.Invoke(null,null);if(pushed)GL.PopMatrix();camera.targetTexture=old;RenderTexture.active=active;Event.current=previous;RenderTexture.ReleaseTemporary(target);}
        }
    }
}
