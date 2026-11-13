using UnityEditor;
using UnityEngine;
namespace FitCheck.Editor
{
    public sealed class FitFeedBackdropImporter:AssetPostprocessor
    {
        void OnPreprocessTexture(){
            if(assetPath!="Assets/Resources/UI/FitFeedStars.png")return;
            var texture=(TextureImporter)assetImporter;texture.textureType=TextureImporterType.Default;texture.sRGBTexture=true;texture.maxTextureSize=1024;
            texture.npotScale=TextureImporterNPOTScale.None;texture.mipmapEnabled=false;texture.wrapMode=TextureWrapMode.Clamp;texture.filterMode=FilterMode.Bilinear;texture.anisoLevel=0;
            texture.textureCompression=TextureImporterCompression.CompressedHQ;texture.isReadable=false;
        }
    }
}
