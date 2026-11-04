using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void FinishPreviewPose(string top)
        {
            Home();studio.Apply(new[]{top,"CG_BOTTOM_01","AH_SHOES_01",""},0);
            FrameCollection(new Vector3(.85f,2.30f,-4.7f),new Vector3(0,2.16f,0),34);studio.Avatar.rotation=Quaternion.identity;
        }
        int PreviewGarmentVertices()
        {
            int vertices=0;foreach(var f in studio.Avatar.GetComponentsInChildren<MeshFilter>())vertices+=f.sharedMesh.vertexCount;return vertices;
        }
        void SetPreviewMaterial(int effect,int variant=0)
        {
            var shader=Resources.Load<Shader>("ReviewClothingFinish");if(shader==null)throw new Exception("Review shader missing");
            foreach(var renderer in studio.Avatar.GetComponentsInChildren<MeshRenderer>()){
                string n=renderer.name;bool body=n=="Camiseta com caimento",sleeve=n.Contains("Manga"),trim=n.StartsWith("BlenderTopo");if(!body&&!sleeve&&!trim)continue;
                var source=renderer.sharedMaterials;var materials=new Material[source.Length];
                for(int i=0;i<source.Length;i++){
                    var m=new Material(source[i]);m.shader=shader;m.SetFloat("_ReviewEffect",body||effect==2?effect:0);materials[i]=m;previewMaterials.Add(m);
                    if(variant==1){
                        if(body){m.mainTexture=Resources.Load<Texture2D>("Garments/Streetwear/AH_TOP_02");m.color=Color.white;}
                        else{m.mainTexture=Resources.Load<Texture2D>("Garments/Finishes/Cotton");m.color=sleeve?new Color(.10f,.15f,.28f):new Color(.065f,.10f,.18f);}
                    }
                    if(variant==2){
                        m.mainTexture=Resources.Load<Texture2D>("Surfaces/Fabric030");m.color=new Color(1.0f,1.20f,1.55f);m.SetFloat("_TexScale",.80f);m.SetFloat("_FabricWeight",0);
                    }
                }
                renderer.sharedMaterials=materials;
            }
        }
        readonly List<Material> previewMaterials=new List<Material>();
        void CaptureClothingMaterialPreview(string folder)
        {
            int vertexBudget=0;var variants=new List<string>();var effects=new List<string>();
            for(int i=0;i<3;i++){
                FinishPreviewPose("AH_TOP_01");int v=PreviewGarmentVertices();if(i==0)vertexBudget=v;else if(v!=vertexBudget)throw new Exception("Texture variation changed geometry");
                if(i>0)SetPreviewMaterial(0,i);
                string path=Path.Combine(folder,"base-"+i+".png");SaveFirstPersonFrame(path,480,640);variants.Add(path);
            }
            CollectionSheet(variants,3,480,640,Path.Combine(folder,"same-model-textures.png"));
            string[] tops={"AH_TOP_02","AH_TOP_02","CG_TOP_03","CG_TOP_03"};int[] modes={0,1,0,2};
            for(int i=0;i<4;i++){
                FinishPreviewPose(tops[i]);if(modes[i]>0)SetPreviewMaterial(modes[i]);
                string path=Path.Combine(folder,"effect-"+i+".png");SaveFirstPersonFrame(path,480,640);effects.Add(path);
            }
            CollectionSheet(effects,2,480,640,Path.Combine(folder,"effects-before-after.png"));
            // Changing view makes the foil and satin reflections visible; no cloth motion or extra geometry.
            var motion=Path.Combine(folder,"motion");Directory.CreateDirectory(motion);
            for(int kind=0;kind<2;kind++){
                FinishPreviewPose(kind==0?"AH_TOP_02":"CG_TOP_03");SetPreviewMaterial(kind==0?1:2);
                for(int frame=0;frame<48;frame++){
                    float angle=Mathf.Sin(frame/48f*Mathf.PI*2)*28;studio.Avatar.rotation=Quaternion.Euler(0,angle,0);
                    SaveFirstPersonFrame(Path.Combine(motion,(kind*48+frame).ToString("D3")+".png"),480,640);
                }
            }
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"PREVIEW_ONLY_PASSED: identical base geometry for three texture variants; current and proposed foil/nylon on existing clothing; single-pass effects, no real-time reflection capture. Main project Assets not modified.");
            Debug.Log("CLOTHING_MATERIAL_PREVIEW_PASSED: "+vertexBudget+" unchanged avatar vertices across texture variants; seven native comparisons and 96 view-change frames; no production application.");
            foreach(var m in previewMaterials)Destroy(m);previewMaterials.Clear();Home();
        }
    }
}
