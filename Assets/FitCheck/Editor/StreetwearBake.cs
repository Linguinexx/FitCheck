using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace FitCheck.Editor
{
    public class StreetwearImporter:AssetPostprocessor
    {
        void OnPreprocessModel(){if(!assetPath.Contains("Garments/Streetwear/"))return;var m=(ModelImporter)assetImporter;m.isReadable=true;m.importAnimation=false;m.globalScale=1;m.meshCompression=ModelImporterMeshCompression.Off;}
        void OnPreprocessTexture(){if(!assetPath.Contains("Garments/Streetwear/"))return;var t=(TextureImporter)assetImporter;t.filterMode=FilterMode.Point;t.textureCompression=TextureImporterCompression.Compressed;t.maxTextureSize=512;t.mipmapEnabled=true;}
    }
    public static class StreetwearBake
    {
        static readonly Color[] sleeves={new Color(.87f,.84f,.76f),new Color(.88f,.85f,.78f),new Color(.23f,.23f,.24f),new Color(.10f,.15f,.28f),new Color(.09f,.44f,.23f),new Color(.89f,.87f,.81f),new Color(.12f,.11f,.125f),new Color(.09f,.29f,.61f),new Color(.11f,.10f,.13f)};
        static readonly string[] tops={"AH_TOP_01","CG_TOP_01","SB_TOP_01","AH_TOP_02","CG_TOP_02","SB_TOP_02","AH_TOP_03","CG_TOP_03","SB_TOP_03"};
        [MenuItem("Tools/Fit Check/Bake Blender streetwear collection")]
        public static void Bake()
        {
            int assets=0,total=0;
            foreach(var raw in Directory.GetFiles("Assets/Resources/Garments/Streetwear","*.fbx")){
                string path=raw.Replace('\\','/');string id=Path.GetFileNameWithoutExtension(path);int slot=id.Contains("_TOP_")?0:id.Contains("_BOTTOM_")?1:2;
                AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceUpdate);var model=AssetDatabase.LoadAssetAtPath<GameObject>(path);var parts=new List<BlenderGarmentPart>();int triangles=0;
                foreach(var filter in model.GetComponentsInChildren<MeshFilter>()){
                    if(filter.name.StartsWith("BIND_PREVIEW"))continue;
                    var mesh=filter.sharedMesh;var matrix=model.transform.worldToLocalMatrix*filter.transform.localToWorldMatrix;var v=mesh.vertices;var n=mesh.normals;var uv=mesh.uv;var tris=mesh.triangles;var mat=filter.GetComponent<MeshRenderer>().sharedMaterial;
                    var groups=new Dictionary<int,List<int>>{{-1,new List<int>()},{0,new List<int>()},{1,new List<int>()}};
                    for(int i=0;i<tris.Length;i+=3){var c=matrix.MultiplyPoint3x4((v[tris[i]]+v[tris[i+1]]+v[tris[i+2]])/3);int group=slot==1&&c.y<1.19f?(c.x<0?-1:1):0;groups[group].AddRange(new[]{tris[i],tris[i+1],tris[i+2]});}
                    foreach(var pair in groups){int count=pair.Value.Count;if(count==0)continue;var pv=new Vector3[count];var pn=new Vector3[count];var pu=new Vector2[count];var pt=new int[count];
                        for(int i=0;i<count;i++){int j=pair.Value[i];pv[i]=matrix.MultiplyPoint3x4(v[j]);pn[i]=matrix.MultiplyVector(n[j]).normalized;pu[i]=uv.Length>j?uv[j]:Vector2.zero;if(slot==1)pu[i].x=1-pu[i].x;pt[i]=i;}
                        string name=slot==0?(filter.name.StartsWith("Camiseta_com_caimento")?"Camiseta com caimento":"BlenderTopo "+filter.name):slot==1?(pair.Key==0?"Cintura":"Calça"):filter.name.StartsWith("Sola")?"Sola "+filter.name:filter.name.StartsWith("Cadarço")?"Cadarço":"Tênis "+filter.name;
                        parts.Add(new BlenderGarmentPart{name=name,side=pair.Key,vertices=pv,normals=pn,uv=pu,triangles=pt,color=mat!=null?mat.color:Color.white,textured=mat!=null&&mat.name.StartsWith("PRINT_")});triangles+=count/3;
                    }
                }
                if(parts.Count==0||triangles>20000)throw new Exception("Malha inválida: "+id+" / "+triangles);
                float min=100,max=-100;foreach(var p in parts)foreach(var point in p.vertices){min=Mathf.Min(min,point.y);max=Mathf.Max(max,point.y);}
                if(slot==0&&(min<1.20f||max>2.60f)||slot==1&&(min<.15f||max>1.50f)||slot==2&&(min<.05f||max>.65f))throw new Exception("Eixos/escala incorretos: "+id+" "+min+".."+max);
                int index=Array.IndexOf(tops,id);var asset=new BlenderGarmentAsset{id=id,slot=slot,source="Blender — StreetwearCollection.blend",sleeveColor=index>=0?sleeves[index]:Color.white,parts=parts.ToArray()};
                File.WriteAllText(Path.ChangeExtension(path,".json"),JsonUtility.ToJson(asset));assets++;total+=triangles;Debug.Log("STREETWEAR_BAKED: "+id+"; "+triangles+" triangles");
            }
            if(assets!=17)throw new Exception("Coleção incompleta: "+assets);AssetDatabase.Refresh();Debug.Log("STREETWEAR_COLLECTION_READY: "+assets+" pieces; "+total+" triangles total");
        }
        public static void BakeAndBuild(){Bake();CitySliceBuild.Build();}
    }
}
