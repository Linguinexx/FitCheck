using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public void CheckNightLightingField()
        {
            if(nightLightField==null||nightLightField.width!=256||nightLightField.height!=160||nightSources.Count<20||nightHalos==null)throw new Exception("Night lighting field incomplete");
            if(nightHalos.GetComponentsInChildren<MeshRenderer>().Length!=1||nightHalos.GetComponent<MeshFilter>().sharedMesh.vertexCount!=nightSources.Count*4)throw new Exception("Night halos exceeded their single batch");
            if(GetComponentsInChildren<Light>().Length!=0)throw new Exception("Lighting introduced per-pixel Unity light passes");
            foreach(float x in new[]{FacadePlanterLeft,FacadePlanterRight}){
                if(Mathf.Abs(x-17.8f)<2||x<-.65f||x>15.65f)throw new Exception("Facade planters overlap palm or leave shop corners");
                if(WorldFree(new Vector2(x,2.2f)))throw new Exception("Moved planter lost collision");
            }
            var warm=nightLightField.GetPixelBilinear((-1.75f+150)/180,(-4+50)/115);
            if(warm.r<warm.b*1.4f)throw new Exception("Street lamp lost warm color");
            var violet=nightLightField.GetPixelBilinear((-9+150)/180,(-22.5f+50)/115);
            if(violet.b<violet.g*1.3f)throw new Exception("Record storefront lost violet LED pool");
            BindNightLighting();Debug.Log("NIGHT_LIGHT_FIELD_PASSED: "+nightSources.Count+" sources, 256x160 shared light field, one halo renderer, zero realtime light/shadow passes, corner planters and distinct warm/violet pools.");
        }
    }
    public partial class FirstCustomer
    {
        void CheckNightLighting(string folder)
        {
            CheckRoadPolish(folder);Home();EnterCity();city.CheckNightLightingField();
            MoveProof(new Vector2(7,-6),0,-8);city.Frame(cam,false,.05f);
            city.PreviewNightLighting(false);SaveFirstPersonFrame(Path.Combine(folder,"lighting-off.png"));
            city.PreviewNightLighting(true);SaveFirstPersonFrame(Path.Combine(folder,"lighting-on.png"));
            MoveProof(new Vector2(-8,-15.5f),180,-9);SaveFirstPersonFrame(Path.Combine(folder,"led-street.png"));
            city.RestoreWalk(new Vector2(-124.8f,-2),0,-10,mannequin);city.Frame(cam,false,0);SaveFirstPersonFrame(Path.Combine(folder,"warm-residential.png"));
            city.RestoreWalk(new Vector2(-25,-17),313,-30,mannequin);city.Frame(cam,false,0);SaveFirstPersonFrame(Path.Combine(folder,"moon-night.png"));
            MoveProof(new Vector2(-2,-6),0,0);var target=RenderTexture.GetTemporary(1280,720,24);var probe=new Texture2D(1,1,TextureFormat.RGB24,false);
            string timings="";
            try{foreach(bool enabled in new[]{false,true,true,false}){city.PreviewNightLighting(enabled);double ms=TimeGarmentRender(target,probe,24);timings+=(enabled?"on":"off")+"="+ms.ToString("F4",System.Globalization.CultureInfo.InvariantCulture)+" ms\n";}}
            finally{RenderTexture.ReleaseTemporary(target);Destroy(probe);city.PreviewNightLighting(true);}
            File.WriteAllText(Path.Combine(folder,"lighting-render-readback.txt"),timings);Debug.Log("NIGHT_LIGHT_RENDER_PROBE: "+timings.Replace("\n","; "));
            Home();EnterHome();if(Shader.GetGlobalFloat("_CityLightActive")!=0)throw new Exception("Street lighting leaked into home");
            Home();EnterShop();if(Shader.GetGlobalFloat("_CityLightActive")!=0)throw new Exception("Street lighting leaked into shop");
        }
    }
}
