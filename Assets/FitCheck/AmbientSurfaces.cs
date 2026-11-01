using UnityEngine;

namespace FitCheck
{
    public static class AmbientSurfaces
    {
        public static readonly string[] Assets={"Wood051","Fabric030","Metal032","Concrete034","PaintedPlaster017","Bricks074","Cardboard001","Asphalt012","WoodFloor040","Carpet016","Tiles107","Fabric084"};
        static Texture2D fabric;
        public static void ClothDetail(Material material,bool enabled)
        {
            if(!enabled){material.SetFloat("_FabricWeight",0);return;}
            if(fabric==null)fabric=Resources.Load<Texture2D>("Surfaces/Fabric030");
            material.SetTexture("_FabricTex",fabric);material.SetFloat("_FabricWeight",.65f);
        }
        public static void Apply(Material material,string key,bool interior)
        {
            string asset=null;float mean=.5f,contrast=3,scale=1;Color tint=Color.white;
            switch(key){
                case "Wood":asset="Wood051";mean=.2156f;contrast=3.5f;scale=.62f;break;
                case "HomeFloor":asset="WoodFloor040";mean=.5537f;contrast=2.5f;scale=.24f;break;
                case "Rug":asset="Carpet016";mean=.6920f;contrast=4;scale=.75f;break;
                case "EntryTile":asset="Tiles107";mean=.9037f;contrast=3;scale=.55f;break;
                case "Cloth":case "CityBanners":asset="Fabric030";mean=.3554f;contrast=5;scale=.45f;break;
                case "Paper":asset="Cardboard001";mean=.588f;contrast=1.8f;scale=.65f;break;
                case "ShopFloor":asset="Concrete034";mean=.7223f;contrast=2.7f;scale=.38f;tint=new Color(.74f,.73f,.70f);break;
                case "CityStucco":if(interior){asset="PaintedPlaster017";mean=.7457f;contrast=3.5f;scale=.26f;tint=new Color(.90f,.89f,.86f);}break;
                case "CityBrick":asset="Bricks074";mean=.457f;contrast=4;scale=.20f;tint=interior?Color.white:new Color(.67f,.50f,.40f);break;
                case "CityMetal":if(interior){asset="Metal032";mean=.5272f;contrast=7;scale=.70f;}break;
                case "CityAsphalt":asset="Asphalt012";mean=.264f;contrast=3.5f;scale=.36f;tint=new Color(.29f,.28f,.28f);break;
            }
            if(asset==null)return;
            var texture=Resources.Load<Texture2D>("Surfaces/"+asset);
            if(texture==null){Debug.LogError("Material ambientCG ausente: "+asset);return;}
            material.mainTexture=texture;material.SetFloat("_SingleSurface",1);material.SetFloat("_SurfaceTile",0);
            material.SetFloat("_SurfaceMean",mean);material.SetFloat("_SurfaceContrast",contrast);material.SetFloat("_TexScale",scale);
            material.SetFloat("_TextureWeight",1);material.SetColor("_SurfaceTint",tint);
            material.name+=" / ambientCG "+asset;
        }
        public static void Validate()
        {
            foreach(var id in Assets){
                var t=Resources.Load<Texture2D>("Surfaces/"+id);
                if(t==null||t.width>512||t.height>512||t.mipmapCount<2||t.wrapMode!=TextureWrapMode.Repeat)
                    throw new System.Exception("Textura ambientCG sem importação adequada: "+id);
            }
            Debug.Log("FITCHECK_AMBIENT_SURFACES_PASSED: "+Assets.Length+" CC0 color maps, bounded resolution, mipmaps and repeat sampling.");
        }
    }
}
