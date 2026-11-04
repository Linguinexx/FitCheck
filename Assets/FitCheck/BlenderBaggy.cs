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
            BuildCachedClothing(baggySource,"BaggyDenim01","Garments/BaggyDenim01",1,Vector3.zero);
            return true;
        }
    }
}
