using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public void FrameGarmentTest(StudioView studio,Camera camera,string top)
        {
            DressServiceCustomer(studio,new[]{top,"CG_BOTTOM_01","AH_SHOES_01",""},0);SetCustomerVisible(true);OpenFittingCurtain();AdvanceFittingCurtain(1);
            camera.transform.position=transform.position+ShopDressingPosition+new Vector3(.75f,2.3f,-4.0f);camera.transform.LookAt(transform.position+ShopDressingPosition+new Vector3(0,1.95f,0));camera.fieldOfView=38;
        }
    }
    public partial class FirstCustomer
    {
        [Serializable] sealed class GarmentRenderSample{public string scene;public bool finishes;public double median_render_readback_ms;public int width=1280,height=720,frames=24;}
        [Serializable] sealed class GarmentRenderReport{public string method="Same executable, ABBA off/on/on/off, warmed native camera renders plus synchronous 1-pixel GPU readback; not complete gameplay FPS";public List<GarmentRenderSample> samples=new List<GarmentRenderSample>();}
        double TimeGarmentRender(RenderTexture target,Texture2D probe,int frames)
        {
            var previous=cam.targetTexture;var active=RenderTexture.active;float aspect=cam.aspect;var times=new List<double>();
            try{
                cam.targetTexture=target;cam.aspect=1280f/720;
                for(int i=0;i<frames+6;i++){
                    var watch=Stopwatch.StartNew();cam.Render();RenderTexture.active=target;probe.ReadPixels(new Rect(0,0,1,1),0,0,false);watch.Stop();if(i>=6)times.Add(watch.Elapsed.TotalMilliseconds);
                }
            }finally{cam.targetTexture=previous;cam.aspect=aspect;RenderTexture.active=active;}
            times.Sort();return times[times.Count/2];
        }
        IEnumerator CheckGarmentRenderPerformance(string folder)
        {
            bool original=garmentFinishes;bool wasPaused=paused;var report=new GarmentRenderReport();
            var target=RenderTexture.GetTemporary(1280,720,24,RenderTextureFormat.ARGB32);var probe=new Texture2D(1,1,TextureFormat.RGB24,false);
            Home();EnterShop();paused=true;
            for(int scene=0;scene<3;scene++){
                if(scene==0){city.SetView(180,0);city.Frame(cam,true,0);}else city.FrameGarmentTest(studio,cam,scene==1?"AH_TOP_02":"CG_TOP_03");
                yield return null;
                foreach(bool enabled in new[]{false,true,true,false}){
                    SetGarmentFinishes(enabled);yield return null;
                    double time=TimeGarmentRender(target,probe,24);report.samples.Add(new GarmentRenderSample{scene=scene==0?"shop":scene==1?"foil-client":"nylon-client",finishes=enabled,median_render_readback_ms=time});
                    UnityEngine.Debug.Log("GARMENT_RENDER_PERFORMANCE: "+report.samples[report.samples.Count-1].scene+" finishes="+enabled+"; median="+time.ToString("F3")+" ms");
                }
            }
            RenderTexture.ReleaseTemporary(target);Destroy(probe);SetGarmentFinishes(original);Home();paused=wasPaused;
            File.WriteAllText(Path.Combine(folder,"render-performance.json"),JsonUtility.ToJson(report,true));
            File.WriteAllText(Path.Combine(folder,"render-performance-checks.txt"),"RENDER_PERFORMANCE_CAPTURED: effects off/on in shop and two close client views, same executable and geometry, synchronous GPU readback.");
        }
        void CheckGarmentFinishes(string folder)
        {
            bool original=garmentFinishes;
            try{
                studio.ValidateSharedTeeBase();SetGarmentFinishes(false,true);if(Shader.GetGlobalFloat("_GarmentFinishesEnabled")!=0)throw new Exception("Finish toggle did not disable");SetGarmentFinishes(true,true);if(Shader.GetGlobalFloat("_GarmentFinishesEnabled")!=1)throw new Exception("Finish toggle did not enable");
                foreach(string top in new[]{"AH_TOP_01","AH_TOP_02","CG_TOP_03","SB_TOP_03"}){
                    Home();studio.Apply(new[]{top,"CG_BOTTOM_01","AH_SHOES_01",""},0);int fx=0;
                    foreach(var renderer in studio.Avatar.GetComponentsInChildren<MeshRenderer>())foreach(var m in renderer.sharedMaterials)if(m.HasProperty("_GarmentFinish")&&m.GetFloat("_GarmentFinish")>0){fx++;if(m.passCount!=1)throw new Exception("Effect added a render pass");}
                    if((top=="AH_TOP_02"||top=="CG_TOP_03")!=(fx>0))throw new Exception("Finish scope incorrect: "+top);
                }
                Home();EnterCity();city.CheckCivilianWardrobe();
                Home();EnterShop();
                var tiles=new List<string>();
                foreach(string top in new[]{"AH_TOP_02","CG_TOP_03"}){
                    city.FrameGarmentTest(studio,cam,top);
                    foreach(bool enabled in new[]{false,true}){SetGarmentFinishes(enabled);string path=Path.Combine(folder,top+(enabled?"-on":"-off")+".png");SaveFirstPersonFrame(path,640,800);tiles.Add(path);}
                }
                CollectionSheet(tiles,2,640,800,Path.Combine(folder,"in-shop-finishes.png"));
                OpenSettings(true);CaptureInterface(Path.Combine(folder,"settings-finishes.png"));settingsOpen=false;paused=false;
                CheckWardrobeFinish(folder);
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"SHOP_REVIEW_PASSED: selective foil and nylon in the real store, global immediate graphics toggle, shared wide-tee geometry with independent artwork, licensed textile, unchanged catalog IDs and civilian basics, 63 fits, sleeves, blinks, service and independent saves.");
                UnityEngine.Debug.Log("GARMENT_FINISHES_PASSED: two effect items, single pass, immediate toggle, shared geometry, real store captures and complete wardrobe/service/save checks.");
            }finally{settingsOpen=false;SetGarmentFinishes(original);Home();}
        }
    }
}
