using UnityEditor;
using UnityEngine;
namespace FitCheck.Editor
{
    public class PaintedTextureImport : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            if(!assetPath.Contains("/Resources/Painted/"))return;
            var importer=(TextureImporter)assetImporter;
            bool flow=assetPath.EndsWith("/AbstractFlow.png"),street=assetPath.EndsWith("/CityStone.png");
            importer.filterMode=flow||street?FilterMode.Bilinear:FilterMode.Point;importer.wrapMode=TextureWrapMode.Repeat;
            importer.mipmapEnabled=street;importer.anisoLevel=street?4:1;importer.textureCompression=TextureImporterCompression.Uncompressed;
            importer.maxTextureSize=street?1024:flow?512:256;importer.npotScale=TextureImporterNPOTScale.None;
        }
    }
}
