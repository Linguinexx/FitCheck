using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class StudioView
    {
        static readonly Dictionary<string,BlenderGarmentAsset> streetwearModels=new Dictionary<string,BlenderGarmentAsset>();
        static BlenderGarmentAsset StreetwearModel(string id)
        {
            if(streetwearModels.TryGetValue(id,out var model))return model;
            var json=Resources.Load<TextAsset>("Garments/Streetwear/"+id);if(json==null)return null;
            model=JsonUtility.FromJson<BlenderGarmentAsset>(json.text);streetwearModels[id]=model;return model;
        }
        void StreetwearParts(BlenderGarmentAsset model,Vector3 offset)
        {
            BuildCachedClothing(model,model.id,"Garments/Streetwear/"+model.id,model.slot,offset);
        }
        bool BuildStreetwearTop(Garment top,Material sleeveMaterial)
        {
            var model=StreetwearModel(top.id);if(model==null)return false;
            sleeveMaterial.color=model.sleeveColor;StreetwearParts(model,Vector3.zero);return true;
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
