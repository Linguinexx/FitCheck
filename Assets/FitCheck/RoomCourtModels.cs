using System;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        [Serializable] sealed class FurnishingModel { public FurnishingGroup[] groups; }
        [Serializable] sealed class FurnishingGroup { public string texture; public float[] p,n,c; }
        static FurnishingModel bedModel,hoopModel;
        Transform homeBedVisual;
        static FurnishingModel ReadFurnishing(string name){
            var file=Resources.Load<TextAsset>("Environment/RoomCourt/"+name);
            if(file==null)throw new InvalidOperationException("Modelo de ambiente ausente: "+name);
            return JsonUtility.FromJson<FurnishingModel>(file.text);
        }
        void AppendFurnishing(FurnishingModel model,Vector3 origin){
            foreach(var g in model.groups){var b=Get(g.texture);int first=b.v.Count;
                for(int i=0;i<g.p.Length;i+=3){var p=origin+new Vector3(g.p[i],g.p[i+1],g.p[i+2]);var n=new Vector3(g.n[i],g.n[i+1],g.n[i+2]);
                    b.v.Add(p);b.n.Add(n);b.c.Add(new Color(g.c[i],g.c[i+1],g.c[i+2]));b.uv.Add(ProjectedUV(p,n));b.t.Add(first+i/3);
                }
            }
        }
        void BuildHomeBedModel(){
            FinishGeometry();homeBedVisual=new GameObject("Cama — estrutura, colchão e roupa de cama").transform;homeBedVisual.SetParent(transform,false);int first=transform.childCount;
            if(bedModel==null)bedModel=ReadFurnishing("Bed");AppendFurnishing(bedModel,new Vector3(-3.4f,0,2.3f));FinishGeometry();
            while(transform.childCount>first)transform.GetChild(first).SetParent(homeBedVisual,false);
            var shader=Resources.Load<Shader>("FurnitureInk");
            foreach(var r in homeBedVisual.GetComponentsInChildren<MeshRenderer>())r.sharedMaterial.shader=shader;
        }
        void BuildCourtHoopModel(){if(hoopModel==null)hoopModel=ReadFurnishing("Hoop");AppendFurnishing(hoopModel,new Vector3(-76,0,17.35f));}
    }
}
