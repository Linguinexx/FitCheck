using System;
using UnityEngine;
namespace FitCheck
{
    [Serializable] public class BlenderGarmentPart {public string name;public int side;public Vector3[] vertices,normals;public Vector2[] uv;public int[] triangles;public Color color;public bool denim,textured;}
    [Serializable] public class BlenderGarmentAsset {public string source,id;public int slot;public Color sleeveColor;public BlenderGarmentPart[] parts;}
    public partial class StudioView
    {
        static BlenderGarmentAsset baggySource;
        static BlenderGarmentAsset playerDenimSource;
        static readonly Garment PlayerWashedDenim=new Garment("PlayerWashedDenim","Jeans preto lavado","CIVIL",1,new Color(.19f,.19f,.20f),true,true);
        static readonly Garment PlayerBlueDenim=new Garment("PlayerBlueDenim","Jeans índigo lavado","CIVIL",1,new Color(.17f,.24f,.25f),true,true);
        static readonly Garment[] InitialPlayerPieces={
            new Garment("PLAYER_STANDARD_TOP_03","Jaqueta esportiva preta","CIVIL",0,new Color(.085f,.087f,.095f),true,true),
            PlayerWashedDenim,new Garment("PLAYER_STANDARD_SHOES","Tênis preto de sola baixa","CIVIL",2,new Color(.082f,.084f,.09f),true),null
        };
        static readonly Garment[] RedPlayerPieces={
            new Garment("PLAYER_RED_TOP_03","Corta-vento vermelho","CIVIL",0,new Color(.67f,.043f,.065f),false,true),
            PlayerBlueDenim,new Garment("PLAYER_RED_SHOES","Runner vermelho","CIVIL",2,new Color(.72f,.035f,.055f),false),
            new Garment("PLAYER_RED_BEANIE","Gorro preto","CIVIL",3,new Color(.08f,.075f,.085f),true)
        };
        static readonly Garment[] PinkPlayerPieces={
            new Garment("PLAYER_PINK_TOP_01","Jersey rosa 27","CIVIL",0,new Color(.74f,.26f,.43f),false,true),
            new Garment("PLAYER_PINK_BOTTOM","Bermuda de carpa","CIVIL",1,new Color(.13f,.21f,.23f),true,true){shorts=true},
            new Garment("PLAYER_PINK_SHOES","Runner rosa","CIVIL",2,new Color(.86f,.52f,.63f),false),
            new Garment("PLAYER_PINK_GLASSES","Óculos pretos","CIVIL",3,Color.black,true)
        };
        static readonly Garment[] BluePlayerPieces={
            new Garment("PLAYER_BLUE_TOP_03","Camiseta azul em camadas","CIVIL",0,new Color(.06f,.24f,.67f),false,true),
            new Garment("PLAYER_BLUE_BOTTOM","Bermuda jeans de recortes","CIVIL",1,new Color(.13f,.21f,.23f),true,true){shorts=true},
            new Garment("PLAYER_BLUE_SHOES","Tênis marrom","CIVIL",2,new Color(.28f,.20f,.16f),true),
            new Garment("PLAYER_BLUE_CAP","Boné jeans","CIVIL",3,new Color(.23f,.34f,.46f),true){cap=true}
        };
        const string FubaTop="PLAYER_FUBA_TOP_03",FubaHood="PLAYER_FUBA_HOOD";
        static readonly Color FubaFur=new Color(.78f,.38f,.12f);
        static readonly Garment[] FubaPlayerPieces={
            new Garment(FubaTop,"Macacão felino","CIVIL",0,FubaFur,false,true),
            new Garment("PLAYER_FUBA_BOTTOM","Macacão felino","CIVIL",1,FubaFur,false,true),
            null,
            new Garment(FubaHood,"Capuz felino","CIVIL",3,FubaFur,false)
        };
        const string NeighbourhoodTop="PLAYER_POSTER_TOP_03",NeighbourhoodCap="PLAYER_POSTER_CAP";
        static readonly Garment[] NeighbourhoodPieces={
            new Garment(NeighbourhoodTop,"Jaqueta lambe-lambe","CIVIL",0,new Color(.15f,.18f,.17f),true,true),
            PlayerWashedDenim,InitialPlayerPieces[2],
            new Garment(NeighbourhoodCap,"Boné de adesivos","CIVIL",3,new Color(.13f,.15f,.16f),true){cap=true}
        };
        const string GarageDjTop="PLAYER_GARAGE_DJ_TOP",GarageDjHeadphones="PLAYER_GARAGE_DJ_HEADPHONES";
        static readonly Garment[] GarageDjPieces={
            new Garment(GarageDjTop,"Jaqueta Garage","CIVIL",0,new Color(.29f,.14f,.49f),true,true),
            PlayerBlueDenim,
            new Garment("PLAYER_GARAGE_DJ_SHOES","Runner lima","CIVIL",2,new Color(.43f,.63f,.12f),true),
            new Garment(GarageDjHeadphones,"Fone de estúdio","CIVIL",3,new Color(.08f,.075f,.11f),true)
        };
        const string GhostTop="PLAYER_GHOST_TOP",GhostHood="PLAYER_GHOST_HOOD";
        static readonly Color GhostFabric=new Color(.84f,.80f,.89f);
        static readonly Garment[] GhostPieces={
            new Garment(GhostTop,"Moletom assombrado","CIVIL",0,GhostFabric,true,true),
            new Garment("PLAYER_GHOST_BOTTOM","Calça névoa","CIVIL",1,new Color(.35f,.29f,.47f),true,true),
            new Garment("PLAYER_GHOST_SHOES","Tênis espectral","CIVIL",2,new Color(.66f,.74f,.85f),true),
            new Garment(GhostHood,"Capuz espectral","CIVIL",3,GhostFabric,true)
        };
        static readonly Garment[][] PlayerUniforms={InitialPlayerPieces,RedPlayerPieces,PinkPlayerPieces,BluePlayerPieces,FubaPlayerPieces,NeighbourhoodPieces,GarageDjPieces,GhostPieces};
        bool BuildBlenderBaggy(Garment item)
        {
            if(item==PlayerWashedDenim||item==PlayerBlueDenim){
                const string path="Garments/PlayerWashedDenim";
                if(playerDenimSource==null){
                    var json=Resources.Load<TextAsset>(path);
                    if(json==null)throw new InvalidOperationException("Jeans do jogador não exportada");
                    playerDenimSource=JsonUtility.FromJson<BlenderGarmentAsset>(json.text);
                }
                BuildCachedClothing(playerDenimSource,item.id,"Garments/"+item.id,1,Vector3.zero);
                return true;
            }
            if(BuildStreetwearBottom(item))return true;
            if(item.id!="CG_BOTTOM_01")return false;
            if(baggySource==null){var json=Resources.Load<TextAsset>("Garments/BaggyDenim01");if(json==null)throw new InvalidOperationException("Jeans Blender não exportada");baggySource=JsonUtility.FromJson<BlenderGarmentAsset>(json.text);}
            BuildCachedClothing(baggySource,"BaggyDenim01","Garments/BaggyDenim01",1,Vector3.zero);
            return true;
        }
    }
}
