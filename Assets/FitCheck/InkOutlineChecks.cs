using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CaptureInkPair(string folder,string name,Action refresh=null){
            SetInkOutlinePreview(false);refresh?.Invoke();SaveFirstPersonFrame(Path.Combine(folder,name+"-original.png"),1440,900);
            int meshes=Resources.FindObjectsOfTypeAll<Mesh>().Length,materials=Resources.FindObjectsOfTypeAll<Material>().Length;
            SetInkOutlinePreview(true);refresh?.Invoke();if(InkOutlinePreview.MaterialCount<3)throw new Exception("Prévia não alcançou os personagens");
            if(Resources.FindObjectsOfTypeAll<Mesh>().Length!=meshes||Resources.FindObjectsOfTypeAll<Material>().Length!=materials)throw new Exception("Contorno duplicou modelos ou materiais");
            SaveFirstPersonFrame(Path.Combine(folder,name+"-contour.png"),1440,900);
            SetInkOutlinePreview(false);refresh?.Invoke();SaveFirstPersonFrame(Path.Combine(folder,name+"-restored.png"),1440,900);
        }
        InkMeasurements InkRenderSamples(){
            var target=RenderTexture.GetTemporary(1152,720,24,RenderTextureFormat.ARGB32);var previous=cam.targetTexture;var active=RenderTexture.active;
            var pixel=new Texture2D(1,1,TextureFormat.RGB24,false);var off=new double[24];var on=new double[24];
            Action draw=()=>{cam.Render();if(SystemInfo.supportsAsyncGPUReadback){var read=AsyncGPUReadback.Request(target,0,0,1,0,1,0,1);read.WaitForCompletion();if(read.hasError)throw new Exception("Medição de render indisponível");}else{RenderTexture.active=target;pixel.ReadPixels(new Rect(0,0,1,1),0,0);pixel.Apply();}};
            try{
                cam.targetTexture=target;
                for(int i=0;i<6;i++){SetInkOutlinePreview(i%2==1);draw();}
                for(int i=0;i<24;i++)for(int step=0;step<2;step++){
                    bool enabled=i%2==0?step==1:step==0;SetInkOutlinePreview(enabled);
                    var timer=Stopwatch.StartNew();draw();timer.Stop();(enabled?on:off)[i]=timer.Elapsed.TotalMilliseconds;
                }
            }finally{SetInkOutlinePreview(false);cam.targetTexture=previous;RenderTexture.active=active;RenderTexture.ReleaseTemporary(target);Destroy(pixel);}
            return new InkMeasurements{gpu=SystemInfo.graphicsDeviceName,api=SystemInfo.graphicsDeviceType.ToString(),baselineMedianMs=MedianInk(off),contourMedianMs=MedianInk(on),baselineSamples=off,contourSamples=on};
        }
        static double MedianInk(double[] samples){var copy=(double[])samples.Clone();Array.Sort(copy);return (copy[copy.Length/2-1]+copy[copy.Length/2])*.5;}
        void CheckInkOutline(string folder){
            Home();storageOverride=Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N"));Directory.CreateDirectory(storageOverride);progress=new DayProgress();paused=false;
            var retro=cam.GetComponent<RetroCamera>();retro.Filter=false;retro.Paused=true;retro.OverrideFrame=0;Shader.SetGlobalFloat("_PaintFrame",0);
            if(InkOutlinePreview.Enabled)throw new Exception("Contorno não começa desligado");
            Begin();EnterDressing();UpdateCamera();CaptureInkPair(folder,"fitting");
            SetInkOutlinePreview(true);Material clone=null;
            foreach(var renderer in mannequin.GetComponentsInChildren<MeshRenderer>(true))foreach(var material in renderer.sharedMaterials)if(clone==null&&material.shader.name.StartsWith("FitCheck/Preview/Ink "))clone=new Material(material);
            if(clone==null)throw new Exception("Material de contorno não foi encontrado");
            SetInkOutlinePreview(false);if(clone.shader.name.StartsWith("FitCheck/Preview/Ink "))throw new Exception("Cópia de roupa/cabelo reteve o contorno");Destroy(clone);
            var timing=InkRenderSamples();SetInkOutlinePreview(false);
            File.WriteAllText(Path.Combine(folder,"render-cost.json"),JsonUtility.ToJson(timing,true));
            Home();EnterShop();LookAtShopMoment(new Vector2(1.8f,3.4f),city.transform.InverseTransformPoint(city.ShopGuideTarget));city.FramePlayerForCamera(cam,false);CaptureInkPair(folder,"shop");
            Home();EnterHome();var pet=city.Cat.transform;var target=pet.TransformPoint(new Vector3(0,.38f,0));cam.transform.position=target+pet.TransformDirection(new Vector3(1.65f,1,-2.4f));cam.transform.LookAt(target);cam.fieldOfView=50;city.PlayerBodyVisible=false;CaptureInkPair(folder,"pet");
            MoveProof(new Vector2(2.75f,1.36f),90,10);city.PlayPlayerEmote(1);for(int i=0;i<18;i++)city.Step(Vector2.zero,.05f,false,mannequin);city.Frame(cam,true,0);city.FramePlayerForCamera(cam,false);
            CaptureInkPair(folder,"mirror",()=>{if(!city.RefreshInkPreviewMirror(cam)||city.PlayerHeadVisible)throw new Exception("Prévia alterou o espelho/cabeça em primeira pessoa");});
            MoveProof(CityView.HomeSpawn,310,12);city.CancelPlayerEmote(true);city.PlayPlayerEmote(1);for(int i=0;i<18;i++)city.Step(Vector2.zero,.05f,false,mannequin);city.Frame(cam,true,0);city.FramePlayerForCamera(cam,false);CaptureInkPair(folder,"first-person");
            if(progress.posts.Count!=0||progress.reputation!=0||InkOutlinePreview.Enabled)throw new Exception("Prévia mudou campanha ou permaneceu ativa");
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"INK_OUTLINE_PREVIEW_PASSED: default off; character/pet/clothing selection; no mesh/material duplication; original/contour/restored fitting, shop, pet, first-person and real mirror; 720p render/readback samples; isolated progress and restored shaders.");UnityEngine.Debug.Log(File.ReadAllText(Path.Combine(folder,"checks.txt")));
        }
        [Serializable] sealed class InkMeasurements {public string gpu,api;public double baselineMedianMs,contourMedianMs;public double[] baselineSamples,contourSamples;}
    }
}
