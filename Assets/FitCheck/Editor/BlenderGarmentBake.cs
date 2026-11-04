using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace FitCheck.Editor
{
    public class BlenderGarmentImporter:AssetPostprocessor
    {
        void OnPreprocessModel(){if(!assetPath.EndsWith("Garments/BaggyDenim01.fbx"))return;var m=(ModelImporter)assetImporter;m.isReadable=true;m.importAnimation=false;m.globalScale=1;m.meshCompression=ModelImporterMeshCompression.Off;}
        void OnPreprocessTexture(){if(!assetPath.EndsWith("Garments/BaggyDenim01.png"))return;var t=(TextureImporter)assetImporter;t.filterMode=FilterMode.Point;t.textureCompression=TextureImporterCompression.Uncompressed;t.mipmapEnabled=true;}
    }
    public static class BlenderGarmentBake
    {
        [MenuItem("Tools/Fit Check/Bake Blender jeans")]
        public static void Bake()
        {
            const string path="Assets/Resources/Garments/BaggyDenim01.fbx";AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceUpdate);
            var model=AssetDatabase.LoadAssetAtPath<GameObject>(path);if(model==null)throw new Exception("FBX Blender ausente");var parts=new List<BlenderGarmentPart>();
            foreach(var f in model.GetComponentsInChildren<MeshFilter>()){
                var mesh=f.sharedMesh;var matrix=model.transform.worldToLocalMatrix*f.transform.localToWorldMatrix;var vertices=mesh.vertices;var normals=mesh.normals;var uv=mesh.uv;var tris=mesh.triangles;var material=f.GetComponent<MeshRenderer>().sharedMaterial;
                var groups=new Dictionary<int,List<int>>{{-1,new List<int>()},{0,new List<int>()},{1,new List<int>()}};
                for(int i=0;i<tris.Length;i+=3){var c=matrix.MultiplyPoint3x4((vertices[tris[i]]+vertices[tris[i+1]]+vertices[tris[i+2]])/3);int side=c.y<1.19f?(c.x<0?-1:1):0;groups[side].AddRange(new[]{tris[i],tris[i+1],tris[i+2]});}
                foreach(var pair in groups){if(pair.Value.Count==0)continue;int count=pair.Value.Count;var v=new Vector3[count];var n=new Vector3[count];var u=new Vector2[count];var t=new int[count];
                    for(int i=0;i<count;i++){int index=pair.Value[i];v[i]=matrix.MultiplyPoint3x4(vertices[index]);n[i]=matrix.MultiplyVector(normals[index]).normalized;u[i]=uv.Length>index?uv[index]:Vector2.zero;t[i]=i;}
                    parts.Add(new BlenderGarmentPart{name=pair.Key==0?"Cintura":"Calça",side=pair.Key,vertices=v,normals=n,uv=u,triangles=t,color=material!=null?material.color:Color.white,denim=material!=null&&material.name.Contains("Jeans azul")});
                }
            }
            if(parts.Count==0)throw new Exception("FBX sem malhas");float min=100,max=-100;foreach(var p in parts)foreach(var v in p.vertices){min=Mathf.Min(min,v.y);max=Mathf.Max(max,v.y);}if(min<.15f||max<1.3f||max>1.5f)throw new Exception("Eixos/escala FBX inválidos: "+min+" / "+max);
            File.WriteAllText("Assets/Resources/Garments/BaggyDenim01.json",JsonUtility.ToJson(new BlenderGarmentAsset{source="Blender — BaggyDenim01.blend",parts=parts.ToArray()}));AssetDatabase.Refresh();Debug.Log("BLENDER_GARMENT_BAKED: "+parts.Count+" parts; height="+min+".."+max);
        }
        public static void BakeAndBuild(){Bake();CitySliceBuild.Build();}
    }
}
