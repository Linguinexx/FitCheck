using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace FitCheck.Editor {
    [InitializeOnLoad] public static class EmoteReview {
        const string Key="FitCheck.IsolatedEmoteReview";static int frames;
        static EmoteReview(){EditorApplication.update+=Tick;}
        public static void Capture(){
            if(!Application.isBatchMode)throw new Exception("Isolated batch only");
            var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-emote-review-output");
            SessionState.SetString(Key,args[i+1]);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            new GameObject("Campaign disabled for review").AddComponent<FirstCustomer>().enabled=false;EditorApplication.isPlaying=true;
        }
        static void Tick(){
            string folder=SessionState.GetString(Key,"");if(folder.Length==0||!EditorApplication.isPlaying||EditorApplication.isCompiling||++frames<5)return;
            SessionState.EraseString(Key);
            try{
                Directory.CreateDirectory(folder);
                var studio=new GameObject("Avatar source").AddComponent<StudioView>();studio.Build();studio.PlayerLook=new PlayerAppearance{created=true,hair=0,skin=0};studio.ApplyUniform(0);studio.Camera.enabled=false;
                var city=new GameObject("Emotes — isolated home").AddComponent<CityView>();city.BuildHome();
                city.RestoreWalk(new Vector2(3.2f,1.36f),90,0,studio.Avatar);city.PreparePlayerBody(studio);studio.Avatar.gameObject.SetActive(false);
                var camera=new GameObject("First person").AddComponent<Camera>();var portrait=new GameObject("Pose review").AddComponent<Camera>();
                camera.clearFlags=portrait.clearFlags=CameraClearFlags.SolidColor;camera.farClipPlane=portrait.farClipPlane=30;
                var flags=System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic;
                for(int emote=1;emote<=6;emote++){
                    city.CancelPlayerEmote(true);if(!city.PlayPlayerEmote(emote))throw new Exception("Emote unavailable");
                    for(int f=0;f<21;f++)city.Step(Vector2.zero,1f/30,false,studio.Avatar);
                    float age=city.PlayerEmoteTime;city.Step(Vector2.zero,.15f,true,studio.Avatar);if(city.PlayerEmoteTime!=age)throw new Exception("Pause advanced emote");
                    city.Frame(camera,true,0);city.FramePlayerForCamera(camera,false);
                    Save(camera,Path.Combine(folder,"first-person-"+emote+".png"));
                    int visible=VisibleHands(city,camera);
                    if(visible<30)throw new Exception("Hands outside normal first-person view: "+emote+" / "+visible);
                    var palm=(Quaternion)typeof(CityView).GetMethod("EmotePalmRotation",flags).Invoke(city,new object[]{1f});
                    if((emote==3||emote==6)&&Vector3.Dot(palm*Vector3.back,Vector3.back)<.9f)throw new Exception("Palm faces player instead of outward: "+emote);
                    if(emote==4&&Vector3.Dot(palm*Vector3.back,Vector3.up)<.95f)throw new Exception("67 palms do not face up");
                    if(emote==1&&Vector3.Dot(palm*Vector3.right,Vector3.up)<.98f)throw new Exception("Thumb does not point up");
                    typeof(CityView).GetField("nextHomeMirrorFrame",flags).SetValue(city,0f);
                    if(!city.RenderHomeMirror(camera))throw new Exception("Mirror did not render");
                    Save(camera,Path.Combine(folder,"mirror-"+emote+".png"));
                    if(emote==2){
                        for(int f=0;f<7;f++)city.Step(Vector2.zero,1f/30,false,studio.Avatar);
                        city.FramePlayerForCamera(camera,false);Save(camera,Path.Combine(folder,"clap-contact.png"));
                    }
                    if(city.PlayerHeadVisible)throw new Exception("Mirror left first-person head visible");
                    portrait.transform.position=city.transform.position+new Vector3(5.03f,1.8f,1.36f);portrait.transform.LookAt(city.PlayerBodyPosition+Vector3.up*1.75f);portrait.fieldOfView=82;
                    city.FramePlayerForCamera(portrait,true);Save(portrait,Path.Combine(folder,"pose-"+emote+".png"));city.FramePlayerForCamera(camera,false);
                    if(emote==4){
                        for(int f=0;f<5;f++)city.Step(Vector2.zero,1f/30,false,studio.Avatar);
                        city.FramePlayerForCamera(camera,false);Save(camera,Path.Combine(folder,"67-phase-A.png"));float a=HandHeightDifference(city);
                        for(int f=0;f<10;f++)city.Step(Vector2.zero,1f/30,false,studio.Avatar);
                        city.FramePlayerForCamera(camera,false);Save(camera,Path.Combine(folder,"67-phase-B.png"));float b=HandHeightDifference(city);
                        if(a*b>=-.001f)throw new Exception("67 hands do not alternate heights: "+a+" / "+b);
                    }
                    if(emote==5){var part=typeof(CityView).GetField("gestureHand",flags).GetValue(city);if(part==null)throw new Exception("Alternate hand missing");}
                    for(int f=0;f<90;f++)city.Step(Vector2.zero,1f/30,false,studio.Avatar);
                    if(city.PlayerEmoteActive)throw new Exception("Emote never returned to rest");
                }
                if(city.PlayerClapSerial!=5)throw new Exception("Claps out of sync: "+city.PlayerClapSerial);
                var clap=Resources.Load<AudioClip>("Audio/Game/emote_clap");if(clap==null||clap.length<.1f||clap.length>.3f)throw new Exception("Clap audio missing");
                city.PlayPlayerEmote(1);city.PlayPlayerEmote(2);for(int f=0;f<15;f++)city.Step(Vector2.zero,1f/30,false,studio.Avatar);
                if(city.PlayerEmoteId!=2)throw new Exception("Queued emote did not blend");
                for(int f=0;f<12;f++)city.Step(Vector2.up,.05f,false,studio.Avatar);
                if(city.PlayerEmoteActive)throw new Exception("Walking did not cancel emote");
                foreach(float pitch in new[]{-35f,0f,40f}){
                    city.CancelPlayerEmote(true);city.SetView(90,pitch);city.PlayPlayerEmote(1);
                    for(int f=0;f<21;f++)city.Step(Vector2.zero,1f/30,false,studio.Avatar);
                    city.Frame(camera,true,0);city.FramePlayerForCamera(camera,false);
                    if(VisibleHands(city,camera)<30)throw new Exception("Hands lost when pitching: "+pitch);
                    Save(camera,Path.Combine(folder,"pitch-"+pitch+".png"));
                }
                city.CancelPlayerEmote(true);city.FramePlayerForCamera(camera,false);
                foreach(var mesh in city.GetComponentsInChildren<MeshFilter>())if(mesh.name=="Mão"&&mesh.transform.localPosition.sqrMagnitude>.00001f)throw new Exception("Viewmodel transform persisted after cancel");
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"EMOTES_REVIEW_PASSED: six poses, first-person visibility, outward wave/cinema palms, 67 upward palms and alternating heights, thumb direction, clap timing/audio, actual mirror, head restore, pause, expiry, queued blend and walking cancellation. Saves untouched.");
                Debug.Log("EMOTES_REVIEW_PASSED");EditorApplication.Exit(0);
            }catch(Exception error){Debug.LogException(error);EditorApplication.Exit(1);}
        }
        static int VisibleHands(CityView city,Camera camera){
            int visible=0;foreach(var mesh in city.GetComponentsInChildren<MeshFilter>())if((mesh.name=="Mão"||mesh.name=="Gesto alternativo — mão")&&mesh.GetComponent<Renderer>().enabled)foreach(var vertex in mesh.sharedMesh.vertices){var p=camera.WorldToViewportPoint(mesh.transform.TransformPoint(vertex));if(p.z>.08f&&p.z<2&&p.x>.05f&&p.x<.95f&&p.y>.05f&&p.y<.95f)visible++;}return visible;
        }
        static float HandHeightDifference(CityView city){
            float a=float.NaN,b=float.NaN;foreach(var renderer in city.GetComponentsInChildren<MeshRenderer>())if(renderer.name=="Gesto alternativo — mão"&&renderer.enabled){if(float.IsNaN(a))a=renderer.bounds.center.y;else b=renderer.bounds.center.y;}
            if(float.IsNaN(a)||float.IsNaN(b))throw new Exception("67 requires both hands");return a-b;
        }
        static void Save(Camera camera,string path){
            var target=new RenderTexture(1440,900,24);target.Create();camera.targetTexture=target;camera.Render();RenderTexture.active=target;
            var image=new Texture2D(1440,900,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,1440,900),0,0);image.Apply();File.WriteAllBytes(path,image.EncodeToPNG());
            camera.targetTexture=null;RenderTexture.active=null;UnityEngine.Object.Destroy(image);target.Release();UnityEngine.Object.Destroy(target);
        }
    }
}
