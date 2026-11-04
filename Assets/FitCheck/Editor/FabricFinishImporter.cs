using UnityEditor;
using UnityEngine;
namespace FitCheck.Editor
{
    public sealed class FabricFinishImporter:AssetPostprocessor
    {
        void OnPreprocessTexture(){if(!assetPath.Contains("Garments/Finishes/"))return;var t=(TextureImporter)assetImporter;t.maxTextureSize=256;t.mipmapEnabled=true;t.filterMode=FilterMode.Bilinear;t.anisoLevel=1;t.textureCompression=TextureImporterCompression.Compressed;}
    }
}
