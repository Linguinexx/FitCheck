using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace FitCheck.Editor
{
    [InitializeOnLoad] public static class ShopMirrorReview
    {
        const string Key="FitCheck.ShopMirrorReview";const BindingFlags P=BindingFlags.Instance|BindingFlags.NonPublic;static int frames;
        static ShopMirrorReview(){EditorApplication.update+=Tick;}
        public static void Capture(){var args=Environment.GetCommandLineArgs();SessionState.SetString(Key,args[Array.IndexOf(args,"-mirror-output")+1]);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("Campaign disabled").AddComponent<FirstCustomer>().enabled=false;EditorApplication.isPlaying=true;}
        static object G(object o,string n)=>o.GetType().GetField(n,P).GetValue(o);static void S(object o,string n,object v)=>o.GetType().GetField(n,P).SetValue(o,v);static void A(bool condition,string message){if(!condition)throw new Exception(message);}
        static void Save(Camera camera,string path)=>typeof(HomeDecorReview).GetMethod("Save",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{camera,path});
        static void Aim(CityView city,Camera camera,Transform avatar,Vector2 position,Vector3 target)=>typeof(HomeDecorReview).GetMethod("Aim",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{city,camera,avatar,position,target});
        static void Tick(){
            string folder=SessionState.GetString(Key,"");if(folder.Length==0||!EditorApplication.isPlaying||EditorApplication.isCompiling||++frames<5)return;SessionState.EraseString(Key);
            try{
                Directory.CreateDirectory(folder);var studio=new GameObject("Studio").AddComponent<StudioView>();studio.Build();studio.PlayerLook=new PlayerAppearance{created=true};studio.ApplyUniform(0);var camera=studio.Camera;camera.enabled=false;camera.GetComponent<RetroCamera>().Filter=false;camera.aspect=1.6f;
                var shop=new GameObject("Shop mirror review").AddComponent<CityView>();shop.BuildShop(studio);studio.ApplyUniform(0);shop.PreparePlayerBody(studio);studio.Avatar.gameObject.SetActive(false);var target=shop.transform.TransformPoint(CityView.ShopMirrorCenter);
                foreach(var position in new[]{new Vector2(.4f,6.15f),new Vector2(1.05f,4.4f),new Vector2(.4f,6.5f)}){
                    Aim(shop,camera,studio.Avatar,position,target);A(Vector2.Distance(shop.Position,position)<.01f,"Mirror standing spot blocked: "+position);S(shop,"nextShopMirrorFrame",0f);A(shop.RenderShopMirror(camera),"Shop mirror not visible at "+position);
                    A(!shop.PlayerHeadVisible,"Mirror left head visible");Save(camera,Path.Combine(folder,"shop-"+position.x+"-"+position.y+".png"));
                }
                var reflected=(Camera)G(shop,"shopMirrorCamera");var texture=reflected.targetTexture;A(texture.width==128&&texture.height==352&&!reflected.enabled,"Mirror budget/camera changed");
                Aim(shop,camera,studio.Avatar,new Vector2(.4f,6.15f),target);S(shop,"nextShopMirrorFrame",0f);A(shop.RenderShopMirror(camera),"Cannot refresh mirror");A(!shop.RenderShopMirror(camera),"Mirror ignores 15 Hz cap");
                foreach(int emote in new[]{1,2,3,4,6}){
                    shop.CancelPlayerEmote(true);A(shop.PlayPlayerEmote(emote),"Emote rejected");
                    var tick=typeof(CityView).GetMethod("TickPlayerEmote",P);for(int f=0;f<26;f++)tick.Invoke(shop,new object[]{1/30f,0f});
                    shop.FramePlayerForCamera(camera,false);S(shop,"nextShopMirrorFrame",0f);A(shop.RenderShopMirror(camera),"Emote mirror failed");A(!shop.PlayerHeadVisible,"Emote leaked head");Save(camera,Path.Combine(folder,"emote-"+emote+".png"));
                }
                shop.CancelPlayerEmote(true);camera.transform.position+=Vector3.right*1.8f;camera.transform.LookAt(target);shop.FramePlayerForCamera(camera,true);A(shop.PlayerHeadVisible,"External body hidden before reflection");S(shop,"nextShopMirrorFrame",0f);shop.RenderShopMirror(camera,true);A(shop.PlayerHeadVisible,"External camera lost player head");
                Aim(shop,camera,studio.Avatar,new Vector2(.4f,6.15f),target+Vector3.right*20);S(shop,"nextShopMirrorFrame",0f);A(!shop.RenderShopMirror(camera),"Mirror rendered behind viewer");
                Aim(shop,camera,studio.Avatar,new Vector2(-4.9f,6.4f),target);A(!shop.RenderShopMirror(camera),"Mirror rendered through back of partition");
                shop.OpenFittingCurtain();for(int f=0;f<60;f++)shop.Frame(camera,true,1/30f);A(shop.FittingOpen,"Mirror obstructs curtain");
                shop.gameObject.SetActive(false);studio.Avatar.gameObject.SetActive(true);studio.ApplyUniform(0);var home=new GameObject("Home regression").AddComponent<CityView>();home.BuildHome();home.PreparePlayerBody(studio);studio.Avatar.gameObject.SetActive(false);home.ApplyHomeDecor(new HomeDecor{mirror=1});
                Aim(home,camera,studio.Avatar,new Vector2(-4.9f,-.2f),home.transform.TransformPoint(new Vector3(5.39f,1.83f,1.36f)));S(home,"nextHomeMirrorFrame",0f);A(home.RenderHomeMirror(camera),"Shared renderer broke home mirror");A(!home.PlayerHeadVisible,"Home head not restored");Save(camera,Path.Combine(folder,"home-regression.png"));
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"SHOP_MIRROR_PASSED: accessible outer partition, three standing views, five animated emotes, head/first-person/external restoration, 128x352/15Hz budget, offscreen/backface skip, curtain access and home Paint mirror regression. No real saves.");Debug.Log("SHOP_MIRROR_PASSED");EditorApplication.Exit(0);
            }catch(Exception error){Debug.LogException(error);EditorApplication.Exit(1);}
        }
    }
}
