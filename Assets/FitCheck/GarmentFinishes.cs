using System;
using UnityEngine;
namespace FitCheck
{
    public partial class StudioView
    {
        static void ConfigureGarmentFinish(Material material,string id,bool printedBody)
        {
            int finish=id=="AH_TOP_02"&&printedBody?1:id=="CG_TOP_03"?2:0;
            if(finish==0)return;
            material.shader=Resources.Load<Shader>("GarmentFinishEffects");material.SetFloat("_GarmentFinish",finish);
        }
        static BlenderGarmentAsset SharedTeeVariant(BlenderGarmentAsset variant,BlenderGarmentAsset basis)
        {
            // The two wide tees share cut/UV geometry; IDs, artwork and trim colors stay independent.
            var parts=new BlenderGarmentPart[variant.parts.Length];
            for(int i=0;i<parts.Length;i++){
                var target=variant.parts[i];BlenderGarmentPart source=null;
                foreach(var p in basis.parts)if(p.name==target.name||p.name.Contains("Gola")&&target.name.Contains("Gola")||p.name.Contains("Barra")&&target.name.Contains("Barra")){source=p;break;}
                if(source==null)throw new Exception("Shared tee part missing: "+target.name);
                parts[i]=new BlenderGarmentPart{name=target.name,side=target.side,vertices=source.vertices,normals=source.normals,uv=source.uv,triangles=source.triangles,color=target.color,textured=target.textured,denim=target.denim};
            }
            return new BlenderGarmentAsset{id=variant.id,slot=variant.slot,sleeveColor=variant.sleeveColor,source=variant.source+" / shared wide tee base",parts=parts};
        }
        public bool ValidateSharedTeeBase()
        {
            var a=StreetwearModel("AH_TOP_01");var b=StreetwearModel("AH_TOP_02");
            for(int i=0;i<a.parts.Length;i++)if(!ReferenceEquals(a.parts[i].vertices,b.parts[i].vertices)||!ReferenceEquals(a.parts[i].triangles,b.parts[i].triangles)||!ReferenceEquals(a.parts[i].uv,b.parts[i].uv))throw new Exception("Tee family duplicated base geometry");
            if(clothingTemplates.TryGetValue("AH_TOP_01",out var first)&&clothingTemplates.TryGetValue("AH_TOP_02",out var second))foreach(var t in first){
                bool shared=false;foreach(var other in second)if(other.name==t.name&&ReferenceEquals(other.mesh,t.mesh))shared=true;if(!shared)throw new Exception("Wide tee mesh was duplicated");
            }
            return true;
        }
    }
    public partial class FirstCustomer
    {
        bool garmentFinishes=true;
        void SetGarmentFinishes(bool enabled,bool save=false)
        {
            garmentFinishes=enabled;Shader.SetGlobalFloat("_GarmentFinishesEnabled",enabled?1:0);
            if(save&&!ProofMode){PlayerPrefs.SetInt("FitCheck.GarmentFinishes",enabled?1:0);PlayerPrefs.Save();}
        }
    }
}
