using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class StudioView
    {
        void CompactCivilianWardrobe()
        {
            CombineCivilianParts("Calça","Calça civil — Blender Baggy",clothes,0,0);
            CombineCivilianParts("Tênis","Tênis civil",clothes,1,-1);CombineCivilianParts("Tênis","Tênis civil",clothes,1,1);
            CombineCivilianParts("Detalhe civil — acabamento","Acabamento civil",garmentTorso,2,0);
        }
        void CombineCivilianParts(string name,string meshName,Transform parent,int category,int side)
        {
            var builder=new ClothingMeshBuilder();var map=new Dictionary<Material,int>();var materials=new List<Material>();var sources=new List<MeshRenderer>();var inverse=parent.worldToLocalMatrix;
            foreach(var r in clothes.GetComponentsInChildren<MeshRenderer>()){
                string n=r.name;bool match=category==0?n=="Calça"||n=="Bermuda"||n=="Cintura"||n=="Bolso":category==1?n.StartsWith("Tênis")||n.StartsWith("Sola")||n=="Cadarço":n=="Gola"||n.StartsWith("Detalhe civil");
                if(!match)continue;var mesh=r.GetComponent<MeshFilter>().sharedMesh;var matrix=inverse*r.transform.localToWorldMatrix;
                if(category==1&&Mathf.Sign(matrix.MultiplyPoint3x4(mesh.bounds.center).x)!=side)continue;
                var rotation=Quaternion.Inverse(parent.rotation)*r.transform.rotation;var v=mesh.vertices;var normals=mesh.normals;var uv=mesh.uv;var mats=r.sharedMaterials;
                for(int sub=0;sub<mesh.subMeshCount;sub++){
                    var material=mats[Mathf.Min(sub,mats.Length-1)];if(!map.TryGetValue(material,out int index)){index=materials.Count;map.Add(material,index);materials.Add(material);}
                    foreach(int i in mesh.GetTriangles(sub))builder.Add(matrix.MultiplyPoint3x4(v[i]),rotation*normals[i],uv.Length>i?uv[i]:Vector2.zero,index);
                }
                sources.Add(r);
            }
            if(sources.Count==0)return;var combined=builder.Build(meshName);garmentAssets.Add(combined);var node=new GameObject(name);node.transform.SetParent(parent,false);node.AddComponent<MeshFilter>().sharedMesh=combined;node.AddComponent<MeshRenderer>().sharedMaterials=materials.ToArray();
            foreach(var r in sources){r.gameObject.SetActive(false);Destroy(r.gameObject);}
        }
    }
}
