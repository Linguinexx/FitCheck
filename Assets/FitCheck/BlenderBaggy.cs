using System;
using UnityEngine;
namespace FitCheck
{
    [Serializable] public class BlenderGarmentPart {public string name;public int side;public Vector3[] vertices,normals;public Vector2[] uv;public int[] triangles;public Color color;public bool denim,textured;}
    [Serializable] public class BlenderGarmentAsset {public string source,id;public int slot;public Color sleeveColor;public BlenderGarmentPart[] parts;}
    public partial class StudioView
    {
        static BlenderGarmentAsset baggySource;
        bool BuildBlenderBaggy(Garment item)
        {
            if(BuildStreetwearBottom(item))return true;
            if(item.id!="CG_BOTTOM_01")return false;
            if(baggySource==null){var json=Resources.Load<TextAsset>("Garments/BaggyDenim01");if(json==null)throw new InvalidOperationException("Jeans Blender não exportada");baggySource=JsonUtility.FromJson<BlenderGarmentAsset>(json.text);}
            foreach(var part in baggySource.parts){
                var mesh=new Mesh{name=part.name+" — Blender Baggy"};mesh.vertices=part.vertices;mesh.normals=part.normals;mesh.uv=part.uv;mesh.triangles=part.triangles;mesh.RecalculateBounds();Own(mesh);
                var mat=new Material(Resources.Load<Shader>("GarmentFabric"));mat.color=part.denim?Color.white:part.color;mat.mainTexture=part.denim?Resources.Load<Texture2D>("Garments/BaggyDenim01"):Texture2D.whiteTexture;
                mat.SetFloat("_Motion",0);mat.SetFloat("_FabricWeight",0);mat.SetFloat("_SmoothLight",1);mat.SetFloat("_Outline",0);Own(mat);
                var node=new GameObject(part.name);node.transform.SetParent(clothes,false);node.AddComponent<MeshFilter>().sharedMesh=mesh;node.AddComponent<MeshRenderer>().sharedMaterial=mat;
            }
            return true;
        }
    }
}
