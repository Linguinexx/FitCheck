using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace FitCheck.Editor
{
    [InitializeOnLoad] public static class FeedShadowReview
    {
        const string Key="FitCheck.FeedShadowReview";const BindingFlags P=BindingFlags.Instance|BindingFlags.NonPublic;static int frames;
        static FeedShadowReview(){EditorApplication.update+=Tick;}
        public static void Capture(){var args=Environment.GetCommandLineArgs();SessionState.SetString(Key,args[Array.IndexOf(args,"-feed-output")+1]);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("Campaign disabled").AddComponent<FirstCustomer>().enabled=false;EditorApplication.isPlaying=true;}
        static object G(object o,string n)=>o.GetType().GetField(n,P).GetValue(o);static void S(object o,string n,object v)=>o.GetType().GetField(n,P).SetValue(o,v);static object C(object o,string n,params object[] a)=>o.GetType().GetMethod(n,P).Invoke(o,a);
        static void A(bool c,string m){if(!c)throw new Exception(m);}
        static void Save(Camera camera,string path)=>typeof(HomeDecorReview).GetMethod("Save",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{camera,path});
        static void Draw(FirstCustomer c,Camera camera,string path)=>typeof(CompanionsReview).GetMethod("Draw",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{c,camera,path,"RetrospectiveUI"});
        static void NoDiscs(CityView city){A(city.GetComponentsInChildren<Transform>(true).All(t=>t.name!="Sombra do funcionário"&&t.name!="Sombra do morador"),"Gray character disc still exists");}
        static void Tick(){
            string folder=SessionState.GetString(Key,"");if(folder.Length==0||!EditorApplication.isPlaying||EditorApplication.isCompiling||++frames<5)return;SessionState.EraseString(Key);
            try{
                Directory.CreateDirectory(folder);CampaignChecks.Validate();var c=UnityEngine.Object.FindAnyObjectByType<FirstCustomer>();var studio=new GameObject("Studio").AddComponent<StudioView>();studio.Build();studio.PlayerLook=new PlayerAppearance{created=true};studio.ApplyUniform(0);var camera=studio.Camera;camera.enabled=false;camera.GetComponent<RetroCamera>().Filter=false;camera.aspect=1.6f;
                S(c,"studio",studio);S(c,"cam",camera);S(c,"mannequin",studio.Avatar);S(c,"storageOverride",Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N")));S(c,"mutedAudio",true);
                var shop=new GameObject("Shop without discs").AddComponent<CityView>();shop.BuildShop(studio);studio.ApplyUniform(0);shop.PreparePlayerBody(studio);studio.Avatar.gameObject.SetActive(false);S(c,"city",shop);NoDiscs(shop);int count=shop.ResidentCount;shop.SetCustomerVisible(true);A(shop.CustomerVisible,"Client no longer visible");shop.SetCustomerVisible(false);A(!shop.CustomerVisible&&shop.ResidentCount==count,"Client hiding damaged roles");
                typeof(HomeDecorReview).GetMethod("Aim",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{shop,camera,studio.Avatar,new Vector2(.4f,6.15f),shop.transform.TransformPoint(CityView.ShopMirrorCenter)});S(shop,"nextShopMirrorFrame",0f);A(shop.RenderShopMirror(camera),"Shop mirror regressed");Save(camera,Path.Combine(folder,"shop-no-discs.png"));
                shop.gameObject.SetActive(false);studio.Avatar.gameObject.SetActive(true);var street=new GameObject("Street without discs").AddComponent<CityView>();street.Build(studio);studio.ApplyUniform(0);street.PreparePlayerBody(studio);studio.Avatar.gameObject.SetActive(false);NoDiscs(street);street.RestoreWalk(new Vector2(-18,-3),20,5,studio.Avatar);street.Frame(camera,true,0);street.FramePlayerForCamera(camera,false);for(int f=0;f<60;f++)street.Frame(camera,false,1/30f);street.CheckResidents();NoDiscs(street);Save(camera,Path.Combine(folder,"street-no-discs.png"));street.gameObject.SetActive(false);
                var progress=new DayProgress();S(c,"progress",progress);Draw(c,camera,Path.Combine(folder,"feed-empty.png"));Draw(c,camera,Path.Combine(folder,"feed-empty.png"));var texture=(Texture2D)G(c,"fitFeedStars");A(texture!=null&&texture.width<=1024&&!texture.isReadable,"Backdrop import/budget wrong");
                string photo=Path.Combine(folder,"post.png");File.Copy(Path.Combine(folder,"shop-no-discs.png"),photo,true);
                for(int day=1;day<=6;day++){
                    for(int j=0;j<3;j++){int index=progress.posts.Count;progress.Add(new Attempt{outfit=CampaignChecks.Solutions[index],photo=photo});}
                    if(day<6){progress.Close();progress.Advance();}
                }
                Draw(c,camera,Path.Combine(folder,"feed-populated.png"));Draw(c,camera,Path.Combine(folder,"feed-populated.png"));A(ReferenceEquals(texture,G(c,"fitFeedStars")),"Background texture allocated again");S(c,"fitFeedScroll",new Vector2(0,1450));Draw(c,camera,Path.Combine(folder,"feed-scrolled.png"));A(progress.posts.Count==18&&progress.day==6,"Feed changed campaign data");
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"FEED_SHADOW_PASSED: no human gray discs in shop/street/player, client visibility/roles and walking preserved, shop mirror preserved, empty/populated/scrolled real FitFeed UI, cached readonly 1024 backdrop, 18 posts unmodified and campaign validation.");Debug.Log("FEED_SHADOW_PASSED");EditorApplication.Exit(0);
            }catch(Exception error){Debug.LogException(error);EditorApplication.Exit(1);}
        }
    }
}
