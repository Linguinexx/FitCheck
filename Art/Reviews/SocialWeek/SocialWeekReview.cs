using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace FitCheck.Editor
{
    [InitializeOnLoad] public static class SocialWeekReview
    {
        const string Key="FitCheck.SocialWeekReview";const BindingFlags P=BindingFlags.Instance|BindingFlags.NonPublic;static int frames;
        static SocialWeekReview(){EditorApplication.update+=Tick;}
        public static void Capture(){var args=Environment.GetCommandLineArgs();SessionState.SetString(Key,args[Array.IndexOf(args,"-social-output")+1]);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("Isolated disabled campaign").AddComponent<FirstCustomer>().enabled=false;EditorApplication.isPlaying=true;}
        static object G(object o,string n)=>o.GetType().GetField(n,P).GetValue(o);static void S(object o,string n,object v)=>o.GetType().GetField(n,P).SetValue(o,v);static object C(object o,string n,params object[] a)=>o.GetType().GetMethod(n,P).Invoke(o,a);
        static object Stage(string n)=>Enum.Parse(typeof(FirstCustomer).GetNestedType("Stage",BindingFlags.NonPublic),n);
        static void A(bool c,string m){if(!c)throw new Exception(m);}
        static void Save(Camera camera,string path)=>typeof(CompanionsReview).GetMethod("Save",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{camera,path});
        static void Draw(FirstCustomer c,Camera camera,string path,string method)=>typeof(CompanionsReview).GetMethod("Draw",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{c,camera,path,method});
        static void Tick(){
            string folder=SessionState.GetString(Key,"");if(folder.Length==0||!EditorApplication.isPlaying||EditorApplication.isCompiling||++frames<5)return;SessionState.EraseString(Key);
            try{
                Directory.CreateDirectory(folder);CampaignChecks.Validate();DropCatalogChecks.Validate();
                var c=UnityEngine.Object.FindAnyObjectByType<FirstCustomer>();var studio=new GameObject("Studio").AddComponent<StudioView>();studio.Build();studio.PlayerLook=new PlayerAppearance{created=true,pet=new PetAppearance{breed=3,coat=3}};studio.ApplyUniform(0);var camera=studio.Camera;camera.enabled=false;camera.GetComponent<RetroCamera>().Filter=false;camera.aspect=1.6f;
                S(c,"studio",studio);S(c,"cam",camera);S(c,"mannequin",studio.Avatar);S(c,"playerAppearance",studio.PlayerLook);S(c,"mutedAudio",true);S(c,"storageOverride",Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N")));S(c,"stage",Stage("City"));
                studio.Avatar.gameObject.SetActive(false);int petCases=0;
                for(int species=0;species<2;species++)for(int breed=0;breed<(species==0?4:3);breed++)for(int coat=0;coat<6;coat++){
                    var look=new PetAppearance{species=species,breed=breed,coat=coat};look.Validate();var go=new GameObject("Pet test");var pet=go.AddComponent<ShopCat>();pet.Build(look);
                    for(int f=0;f<12;f++)pet.Tick(.05f,false,false,true);
                    A(pet.EyeOpen>.8f,"Eyes never opened");if(species==0&&coat==4)A(pet.EyeColor.b>.9f&&pet.EyeColor.r<.2f,"White cat eyes not blue");
                    float age=pet.Age,eyes=pet.EyeOpen;pet.Tick(2,true,false,true);A(pet.Age==age&&pet.EyeOpen==eyes,"Pause moved pet");
                    int triangles=go.GetComponentsInChildren<MeshFilter>(true).Sum(m=>m.sharedMesh.triangles.Length/3);A(triangles<22000,"Pet too dense "+triangles);
                    if(species==0&&(breed==3&&coat==3||breed==0&&coat==4)){
                        camera.transform.position=new Vector3(-1,.75f,-2.2f);camera.transform.LookAt(new Vector3(-.25f,.39f,0));camera.fieldOfView=38;Save(camera,Path.Combine(folder,breed==3?"sphynx.png":"white-blue-eyes.png"));
                    }
                    if(breed==3)A(go.GetComponentsInChildren<MeshRenderer>().First(r=>r.name=="Cabeça arredondada").sharedMaterial.GetFloat("_TextureWeight")==0,"Sphynx has fur");
                    go.SetActive(false);UnityEngine.Object.Destroy(go);petCases++;
                }
                var progress=new DayProgress();for(int day=1;day<=7;day++){
                    for(int j=0;j<3;j++)progress.Add(new Attempt{outfit=CampaignChecks.Solutions[progress.posts.Count],photo=""});progress.Close();if(day<7)progress.Advance();
                }progress.Validate();A(progress.posts.Count==21&&progress.Complete,"Campaign invalid");S(c,"progress",progress);
                S(c,"shopService",true);S(c,"customer",10);S(c,"outfit",(string[])CampaignChecks.Solutions[10].Clone());A(!(bool)C(c,"SpecialOrderSatisfied"),"Fixed top not required");A(!(bool)C(c,"AllowSpecialOrderPiece",Catalog.Get("AH_TOP_01")),"Fixed top change allowed");var fit=(string[])G(c,"outfit");fit[0]=Catalog.ArrivalPiece(0).id;A((bool)C(c,"SpecialOrderSatisfied"),"Own top rejected");
                S(c,"customer",12);S(c,"outfit",new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""});A((bool)C(c,"SpecialOrderSatisfied"),"AH fit rejected");((string[])G(c,"outfit"))[2]="CG_SHOES_01";A(!(bool)C(c,"SpecialOrderSatisfied"),"Mixed drop accepted");
                S(c,"customer",19);S(c,"outfit",(string[])CampaignChecks.Solutions[19].Clone());((string[])G(c,"outfit"))[2]=progress.posts[18].outfit[2];A((bool)C(c,"SpecialOrderSatisfied"),"Partner footwear rejected");((string[])G(c,"outfit"))[2]=progress.posts[18].outfit[2]=="AH_SHOES_01"?"CG_SHOES_01":"AH_SHOES_01";S(c,"stage",Stage("Confirm"));C(c,"Submit");A(G(c,"stage").ToString()=="Dressing"&&progress.posts.Count==21,"Invalid special order submitted");S(c,"replayIndex",19);A((bool)C(c,"SpecialOrderSatisfied"),"Legacy replay blocked");S(c,"replayIndex",-1);S(c,"shopService",false);
                camera.transform.position=new Vector3(0,2.8f,-7);camera.transform.LookAt(new Vector3(0,1.9f,0));Draw(c,camera,Path.Combine(folder,"feed-comments.png"),"RetrospectiveUI");Draw(c,camera,Path.Combine(folder,"feed-comments.png"),"RetrospectiveUI");
                studio.Avatar.gameObject.SetActive(true);var city=new GameObject("Street").AddComponent<CityView>();city.Build(studio);studio.ApplyUniform(0);city.PreparePlayerBody(studio);studio.Avatar.gameObject.SetActive(false);S(c,"city",city);int obstacleCount=((ICollection)G(city,"obstacles")).Count;
                for(int day=1;day<=7;day++){city.ApplyWeekScenery(day);A(city.SceneryDay==day&&((Transform[])G(city,"weekScenery")).Count(root=>root.gameObject.activeSelf)==1,"Week state wrong");A(((ICollection)G(city,"obstacles")).Count==obstacleCount,"Week blocks route");city.RestoreWalk(new Vector2(-97,17),0,0,studio.Avatar);city.Frame(camera,true,0);camera.transform.position=city.transform.position+new Vector3(-97,3,16.2f);camera.transform.LookAt(city.transform.position+new Vector3(-97,3.8f,21.5f));if(day==1||day==7)Save(camera,Path.Combine(folder,"street-day-"+day+".png"));}
                var point=city.ResidentPosition(1);city.RestoreWalk(new Vector2(point.x,point.z-3),0,0,studio.Avatar);A(city.PlayPlayerEmote(3),"Emote not started");city.Frame(camera,false,.05f);A(city.NeighbourReacting,"Nearby visible resident did not react");float reaction=city.NeighbourReactionAge;city.Frame(camera,false,0);A(city.NeighbourReactionAge==reaction,"Paused reaction advanced");Save(camera,Path.Combine(folder,"resident-wave-start.png"));for(int f=0;f<22;f++)city.Frame(camera,false,.05f);Save(camera,Path.Combine(folder,"resident-wave.png"));for(int f=0;f<50;f++)city.Frame(camera,false,.05f);A(!city.NeighbourReacting,"Reaction never ended");city.gameObject.SetActive(false);
                var before=JsonUtility.ToJson(progress);S(c,"stage",Stage("Summary"));C(c,"CaptureWeekMemory");A(File.Exists(Path.Combine((string)G(c,"storageOverride"),"week-group.png")),"Photo not stored");A(before==JsonUtility.ToJson(progress),"Portrait changed campaign");File.Copy(Path.Combine((string)G(c,"storageOverride"),"week-group.png"),Path.Combine(folder,"week-group.png"),true);C(c,"OpenWeekMemory");A(G(c,"stage").ToString()=="WeekMemory","Album not opened");Draw(c,camera,Path.Combine(folder,"week-album.png"),"WeekMemoryUI");Draw(c,camera,Path.Combine(folder,"week-album.png"),"WeekMemoryUI");C(c,"CloseWeekMemory");A(G(c,"stage").ToString()=="Summary","Album return wrong");
                var home=new GameObject("Home").AddComponent<CityView>();home.BuildHome(studio.PlayerLook.pet);home.SetWeekFrame((Texture2D)G(c,"weekMemoryPhoto"));home.RestoreWalk(new Vector2(-2.4f,-1.1f),0,0,studio.Avatar);home.Frame(camera,true,0);camera.transform.LookAt(home.transform.TransformPoint(CityView.WeekFramePosition));A(home.LookingAtWeekFrame(camera),"Frame unreachable");Save(camera,Path.Combine(folder,"bedroom-memory.png"));home.gameObject.SetActive(false);
                var memoryRenderer=home.GetComponentsInChildren<MeshRenderer>().First(r=>r.name=="Fotografia da semana");A(Mathf.Abs(memoryRenderer.transform.localScale.x/memoryRenderer.transform.localScale.y-16f/9)<.01f,"Framed photo stretched");
                string firstFolder=(string)G(c,"storageOverride");S(c,"storageOverride",Path.Combine(folder,"other-slot"));A(C(c,"LoadWeekMemory")==null,"Photo leaked across save slots");S(c,"storageOverride",firstFolder);A(C(c,"LoadWeekMemory")!=null,"Original photo lost");
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"SOCIAL_WEEK_PASSED: 42 pet variants; blue eyes, Sphynx, pause and mesh budget; 21 historical encounter scores; three live order constraints/replay exemption; FitFeed comments; 7 daily scenery states without route/light changes; NPC emote reaction/pause/end; final group portrait, album, bedroom frame and save-slot isolation.");Debug.Log("SOCIAL_WEEK_PASSED");EditorApplication.Exit(0);
            }catch(Exception error){Debug.LogException(error);EditorApplication.Exit(1);}
        }
    }
}
