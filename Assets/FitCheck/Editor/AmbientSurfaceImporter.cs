using UnityEditor;
using UnityEngine;

namespace FitCheck.Editor
{
    public class AmbientSurfaceImporter : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            if(!assetPath.StartsWith("Assets/Resources/Surfaces/"))return;
            var importer=(TextureImporter)assetImporter;
            Configure(importer,assetPath);
        }
        static void Configure(TextureImporter importer,string path)
        {
            importer.textureType=TextureImporterType.Default;importer.sRGBTexture=true;
            importer.maxTextureSize=path.Contains("Fabric030")||path.Contains("Fabric084")?512:256;
            importer.mipmapEnabled=true;importer.filterMode=FilterMode.Bilinear;importer.anisoLevel=4;
            importer.wrapMode=TextureWrapMode.Repeat;importer.npotScale=TextureImporterNPOTScale.None;
            importer.isReadable=false;importer.textureCompression=TextureImporterCompression.CompressedHQ;
        }
        public static void PrepareAll()
        {
            foreach(string id in AmbientSurfaces.Assets){
                string path="Assets/Resources/Surfaces/"+id+".jpg";
                var importer=AssetImporter.GetAtPath(path) as TextureImporter;
                if(importer==null)throw new System.Exception("Mapa ambientCG ausente: "+id);
                int size=id=="Fabric030"||id=="Fabric084"?512:256;
                if(importer.maxTextureSize==size&&importer.mipmapEnabled&&importer.wrapMode==TextureWrapMode.Repeat&&importer.filterMode==FilterMode.Bilinear)continue;
                Configure(importer,path);importer.SaveAndReimport();
            }
        }
    }
}
