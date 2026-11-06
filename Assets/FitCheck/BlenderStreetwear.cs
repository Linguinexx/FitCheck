using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class StudioView
    {
        static readonly Dictionary<string,BlenderGarmentAsset> streetwearModels=new Dictionary<string,BlenderGarmentAsset>();
        static string StreetwearResource(string id)=>(id.StartsWith("PLAYER_")?"Garments/Player/":"Garments/Streetwear/")+id;
        static BlenderGarmentAsset StreetwearModel(string id)
        {
            if(streetwearModels.TryGetValue(id,out var model))return model;
            var json=Resources.Load<TextAsset>(StreetwearResource(id));if(json==null)return null;
            model=JsonUtility.FromJson<BlenderGarmentAsset>(json.text);if(id=="AH_TOP_02")model=SharedTeeVariant(model,StreetwearModel("AH_TOP_01"));streetwearModels[id]=model;return model;
        }
        void StreetwearParts(BlenderGarmentAsset model,Vector3 offset)
        {
            BuildCachedClothing(model,model.id,StreetwearResource(model.id),model.slot,offset);
        }
        bool BuildStreetwearTop(Garment top,Material sleeveMaterial)
        {
            var model=StreetwearModel(top.id);if(model==null)return false;
            FinishCollectionSleeve(sleeveMaterial,model);
            if(top.id=="PLAYER_RED_TOP_03"){
                sleeveMaterial.color=Color.white;
                sleeveMaterial.mainTexture=Resources.Load<Texture2D>("Garments/Player/RedSleeve");
            }
            StreetwearParts(model,Vector3.zero);return true;
        }
        bool BuildStreetwearBottom(Garment bottom)
        {
            var model=StreetwearModel(bottom.id);if(model==null)return false;StreetwearParts(model,Vector3.zero);return true;
        }
        bool BuildStreetwearShoes(Garment shoe,int side)
        {
            var model=StreetwearModel(shoe.id);if(model==null)return false;StreetwearParts(model,new Vector3(side*.24f,0,0));return true;
        }
    }
}
