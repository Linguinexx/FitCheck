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
        static readonly Garment[] InitialPlayerPieces={null,PlayerWashedDenim,null,null};
        static readonly Garment[] RedPlayerPieces={
            new Garment("PLAYER_RED_TOP_03","Corta-vento vermelho","CIVIL",0,new Color(.67f,.043f,.065f),false,true),
            PlayerBlueDenim,new Garment("PLAYER_RED_SHOES","Runner vermelho","CIVIL",2,new Color(.72f,.035f,.055f),false),
            new Garment("PLAYER_RED_BEANIE","Gorro preto","CIVIL",3,new Color(.08f,.075f,.085f),true)
        };
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
