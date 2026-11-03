using System;
using System.IO;
using System.Reflection;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        // Proof-only IMGUI container: draw the real controls into the same native camera target.
        void CaptureInterface(string path)
        {
            if(!ProofMode)throw new InvalidOperationException("Interface capture requires proof mode");
            var flags=BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;var gui=typeof(GUIUtility);
            var stateType=gui.Assembly.GetType("UnityEngine.ObjectGUIState");var state=Activator.CreateInstance(stateType,true);
            var begin=gui.GetMethod("BeginContainer",flags);var end=gui.GetMethod("EndContainer",flags);var beginGui=gui.GetMethod("BeginGUI",flags);
            var entity=typeof(UnityEngine.Object).GetMethod("GetEntityId",BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).Invoke(this,null);
            var previousTarget=cam.targetTexture;var active=RenderTexture.active;float aspect=cam.aspect;
            var rt=RenderTexture.GetTemporary(1440,900,24,RenderTextureFormat.ARGB32);var tex=new Texture2D(1440,900,TextureFormat.RGB24,false);bool container=false,pushed=false;
            float fade=arrivalFade,flash=photoFlash;var oldEvent=Event.current;
            try{
                arrivalFade=0;photoFlash=0;cam.targetTexture=rt;cam.aspect=1.6f;cam.Render();RenderTexture.active=rt;
                begin.Invoke(null,new[]{state});container=true;beginGui.Invoke(null,new object[]{0,entity,0});
                Event.current=new Event{type=EventType.Repaint,mousePosition=new Vector2(-100,-100)};
                GL.PushMatrix();pushed=true;GL.LoadPixelMatrix(0,1440,900,0);OnGUI();end.Invoke(null,null);container=false;
                GL.Flush();RenderTexture.active=rt;tex.ReadPixels(new Rect(0,0,1440,900),0,0);tex.Apply();File.WriteAllBytes(path,tex.EncodeToPNG());
                Debug.Log("FITCHECK_INTERFACE_CAPTURED: "+path);
            }finally{
                if(container)try{end.Invoke(null,null);}catch{}
                if(pushed)GL.PopMatrix();if(oldEvent!=null)Event.current=oldEvent;(state as IDisposable)?.Dispose();arrivalFade=fade;photoFlash=flash;
                cam.targetTexture=previousTarget;cam.aspect=aspect;RenderTexture.active=active;RenderTexture.ReleaseTemporary(rt);Destroy(tex);
            }
        }
        void CaptureKaosInterfaceReview(string folder)
        {
            var original=progress;string storage=storageOverride;float sensitivity=mouseSensitivity;
            try{
                Home();StartLiveTitle();FrameLiveTitle(.4f);CaptureInterface(Path.Combine(folder,"ui-title-live.png"));
                OpenSettings(false);CaptureInterface(Path.Combine(folder,"ui-settings.png"));settingsOpen=false;
                Home();progress=new DayProgress();storageOverride=Path.Combine(folder,"ui-session-"+Guid.NewGuid().ToString("N"));
                EnterHome();MoveProof(new Vector2(0,-2.3f),180,18);TickContext(10);CaptureInterface(Path.Combine(folder,"ui-home-interaction.png"));
                Home();EnterShop();OpenShopDecor();decorDraft.wall=4;decorDraft.accent=3;city.ApplyShopDecor(decorDraft);decorView=1;FrameShopDecor();CaptureInterface(Path.Combine(folder,"ui-shop-purple.png"));CloseShopDecor(false);
                SettleShopArrival();MoveProof(CityView.ShopCounter,180,0);InteractShop();question=1;shopCameraTime=.5f;FrameShopDialogue();
                TickContext(.5f);TickContext(.5f);CaptureInterface(Path.Combine(folder,"ui-comic-dialogue.png"));
                Debug.Log("FITCHECK_KAOS_INTERFACE_PASSED: live title, settings, proximity hint, vivid decor and comic dialogue native UI captures.");
            }finally{settingsOpen=false;Home();progress=original;storageOverride=storage;savedDecor=null;SetMouseSensitivity(sensitivity);}
        }
    }
}
