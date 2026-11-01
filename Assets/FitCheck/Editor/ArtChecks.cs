using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FitCheck.Editor
{
    public static class ArtChecks
    {
        public static readonly string[] Atlases={"Cloth","Plaster","Paper","Wood","Skin","Backdrop"};
        public static readonly string[] StableAtlases={"Hair","Leather","Rubber","Knit"};
        [MenuItem("Tools/Fit Check/Validate retro art")]
        public static void Validate()
        {
            var flowShader=Resources.Load<Shader>("AbstractBackdrop");if(flowShader==null||!flowShader.isSupported)throw new Exception("Shader do fundo ausente");
            foreach(var message in ShaderUtil.GetShaderMessages(flowShader))if(message.severity.ToString()=="Error")throw new Exception(message.message);
            var flow=Resources.Load<Texture2D>("Painted/AbstractFlow");var flowImport=(TextureImporter)AssetImporter.GetAtPath("Assets/Resources/Painted/AbstractFlow.png");
            if(flow==null||flow.width!=256||flow.height!=512||flowImport.filterMode!=FilterMode.Bilinear||flowImport.mipmapEnabled||flowImport.wrapMode!=TextureWrapMode.Repeat||flowImport.textureCompression!=TextureImporterCompression.Uncompressed)throw new Exception("Textura de fluxo mal importada");
            var shader=Resources.Load<Shader>("RetroPaint");if(shader==null||!shader.isSupported)throw new Exception("Shader retrô indisponível");
            foreach(var message in ShaderUtil.GetShaderMessages(shader))if(message.severity.ToString()=="Error")throw new Exception(message.message);
            foreach(var name in Atlases){
                var path="Assets/Resources/Painted/"+name+".png";
                var texture=Resources.Load<Texture2D>("Painted/"+name);
                if(texture==null||texture.width!=256||texture.height!=64)throw new Exception("Atlas incompleto: "+name);
                var importer=(TextureImporter)AssetImporter.GetAtPath(path);
                if(importer.filterMode!=FilterMode.Point||importer.mipmapEnabled||importer.textureCompression!=TextureImporterCompression.Uncompressed)throw new Exception("Importação borrada: "+name);
                var cpu=new Texture2D(2,2);cpu.LoadImage(File.ReadAllBytes(path));
                try{
                    var pixels=cpu.GetPixels32();long mean0=0,mean1=0;int changed=0;
                    for(int y=0;y<64;y++)for(int x=0;x<64;x++){
                        var a=pixels[y*256+x];var b=pixels[y*256+x+64];mean0+=a.r+a.g+a.b;mean1+=b.r+b.g+b.b;
                        if(a.r!=b.r||a.g!=b.g||a.b!=b.b)changed++;
                    }
                    if(changed==0)throw new Exception("Textura sem quadros distintos: "+name);
                    if(name=="Cloth"&&Math.Abs(mean0-mean1)/(64f*64*3)>3)throw new Exception("Animação muda demais a cor da roupa");
                }finally{UnityEngine.Object.DestroyImmediate(cpu);}
            }
            foreach(var name in StableAtlases){
                string path="Assets/Resources/Painted/"+name+".png";
                var texture=Resources.Load<Texture2D>("Painted/"+name);
                if(texture==null||texture.width!=256||texture.height!=64)throw new Exception("Material de avatar ausente: "+name);
                var importer=(TextureImporter)AssetImporter.GetAtPath(path);
                if(importer.filterMode!=FilterMode.Point||importer.mipmapEnabled||importer.textureCompression!=TextureImporterCompression.Uncompressed)throw new Exception("Importação incorreta: "+name);
                var cpu=new Texture2D(2,2);cpu.LoadImage(File.ReadAllBytes(path));
                try{
                    var pixels=cpu.GetPixels32();
                    for(int y=0;y<64;y++)for(int x=0;x<64;x++)for(int frame=1;frame<4;frame++)
                        if(!pixels[y*256+x].Equals(pixels[y*256+x+frame*64]))throw new Exception("Detalhe deveria ficar estável: "+name);
                }finally{UnityEngine.Object.DestroyImmediate(cpu);}
            }
            Debug.Log("FITCHECK_ART_PASSED: shader suportado, 6 atlas animados e 4 atlas estáveis de avatar, filtro pontual e tecido com cor média estável.");
        }
        [MenuItem("Tools/Fit Check/Run retro art runtime check")]
        public static void Runtime()
        {
            var game=UnityEngine.Object.FindAnyObjectByType<FirstCustomer>();if(!Application.isPlaying||game==null)throw new Exception("Inicie Play Mode");game.ArtSmokeCheck();game.RoundedAvatarSmokeCheck();game.HairFitSmokeCheck();game.PostureSmokeCheck();game.RefinementSmokeCheck();game.EyelidSmokeCheck();
        }
        public static void ConfigureImports()
        {
            foreach(var name in Atlases.Concat(StableAtlases)){
                var importer=(TextureImporter)AssetImporter.GetAtPath("Assets/Resources/Painted/"+name+".png");
                importer.filterMode=FilterMode.Point;importer.wrapMode=TextureWrapMode.Repeat;importer.mipmapEnabled=false;
                importer.textureCompression=TextureImporterCompression.Uncompressed;importer.maxTextureSize=256;importer.SaveAndReimport();
            }
        }
    }
}
