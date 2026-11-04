using UnityEditor;
using UnityEngine;
namespace FitCheck.Editor
{
    public class ArtworkImporter:AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            if(!assetPath.StartsWith("Assets/Resources/Artwork/"))return;
            var importer=(TextureImporter)assetImporter;importer.textureType=TextureImporterType.Default;importer.sRGBTexture=true;importer.maxTextureSize=512;importer.npotScale=TextureImporterNPOTScale.None;importer.mipmapEnabled=true;importer.wrapMode=TextureWrapMode.Clamp;importer.filterMode=FilterMode.Bilinear;importer.anisoLevel=4;importer.textureCompression=TextureImporterCompression.CompressedHQ;
        }
    }
}
