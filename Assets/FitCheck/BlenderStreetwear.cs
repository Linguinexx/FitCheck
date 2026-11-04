using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class StudioView
    {
        static readonly Dictionary<string,BlenderGarmentAsset> streetwearModels=new Dictionary<string,BlenderGarmentAsset>();
        static BlenderGarmentAsset StreetwearModel(string id)
        {
            if(streetwearModels.TryGetValue(id,out var model))return model;
            var json=Resources.Load<TextAsset>("Garments/Streetwear/"+id);if(json==null)return null;
            model=JsonUtility.FromJson<BlenderGarmentAsset>(json.text);streetwearModels[id]=model;return model;
        }
        void StreetwearParts(BlenderGarmentAsset model,Vector3 offset)
        {
            foreach(var part in model.parts){
                var mesh=new Mesh{name=part.name+(model.slot==1?" — Blender Baggy":" — Blender Streetwear")};mesh.vertices=part.vertices;mesh.normals=part.normals;mesh.uv=part.uv;mesh.triangles=part.triangles;mesh.RecalculateBounds();Own(mesh);
                var material=new Material(Resources.Load<Shader>("GarmentFabric"));material.color=part.textured?Color.white:part.color;
                material.mainTexture=part.textured?Resources.Load<Texture2D>("Garments/Streetwear/"+model.id):Texture2D.whiteTexture;
                material.SetFloat("_Motion",0);material.SetFloat("_FabricWeight",0);material.SetFloat("_SmoothLight",1);material.SetFloat("_Outline",0);Own(material);
                var node=new GameObject(part.name);node.transform.SetParent(clothes,false);node.transform.localPosition=offset;node.AddComponent<MeshFilter>().sharedMesh=mesh;node.AddComponent<MeshRenderer>().sharedMaterial=material;
            }
        }
        bool BuildStreetwearTop(Garment top,Material sleeveMaterial)
        {
            var model=StreetwearModel(top.id);if(model==null)return false;
            sleeveMaterial.color=model.sleeveColor;StreetwearParts(model,Vector3.zero);return true;
        }
        bool BuildStreetwearBottom(Garment bottom)
        {
            var model=StreetwearModel(bottom.id);if(model==null)return false;StreetwearParts(model,Vector3.zero);return true;
        }
        bool BuildStreetwearShoes(Garment shoe,int side)
        {
            var model=StreetwearModel(shoe.id);if(model==null)return false;StreetwearParts(model,new Vector3(side*.24f,0,0));return true;
        }
    }
}
