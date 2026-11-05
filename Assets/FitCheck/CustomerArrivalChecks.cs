using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public void CheckCustomerClothing(bool everyday,string shoeId="AH_SHOES_01")
        {
            var texture=Resources.Load<Texture2D>(everyday?"Garments/Civilian/CIV_SHOES_BASE":"Garments/Streetwear/"+shoeId);int shoes=0,civilian=0;
            foreach(var renderer in residents[0].root.GetComponentsInChildren<MeshRenderer>()){
                foreach(var material in renderer.sharedMaterials){
                    if(renderer.name.StartsWith("Tênis")){shoes++;if(material.mainTexture!=texture)throw new Exception("Customer footwear changed at the wrong stage");}
                    if(material.name.StartsWith("Civilian"))civilian++;
                }
            }
            if(shoes!=2||everyday&&civilian==0||!everyday&&civilian!=0)throw new Exception("Customer everyday/catalog clothing mismatch");
        }
        public string CustomerIdentitySignature()
        {
            string result=residents[0].expression.ToString();
            foreach(var renderer in residents[0].root.GetComponentsInChildren<MeshRenderer>())if(renderer.name=="Cabeça arredondada"||renderer.name=="Cabelo ajustado à cabeça")result+="/"+renderer.name+":"+renderer.GetComponent<MeshFilter>().sharedMesh.vertexCount+":"+renderer.sharedMaterial.color;
            return result;
        }
    }
    public partial class FirstCustomer
    {
        void CheckCustomerArrivals(string folder)
        {
            var tiles=new List<string>();var look=studio.PlayerLook;
            try{
                for(int id=0;id<9;id++){
                    studio.Apply(Catalog.Default,id);var expression=studio.CurrentExpression;string hair=studio.HairName;
                    studio.ApplyCivilian(id,true);
                    if(studio.CurrentExpression!=expression||studio.HairName!=hair)throw new Exception("Civilian arrival changed customer identity: "+id);
                    studio.ValidateArmFit();FrameCollection(new Vector3(1.1f,2.1f,-5.35f),new Vector3(0,1.65f,0));
                    string path=Path.Combine(folder,"arrival-look-"+id+".png");SaveFirstPersonFrame(path,360,480);tiles.Add(path);
                }
                CollectionSheet(tiles,3,360,480,Path.Combine(folder,"customer-arrivals.png"));
                CheckShopServiceFlow(folder);CheckSaveSlots(folder);
                File.WriteAllText(Path.Combine(folder,"checks.txt"),"SHOP_REVIEW_PASSED: nine everyday customer looks, original identities, basic footwear before fitting, selected outfit retained on departure, full service/cancellation/next-client and isolated saves.");
                Debug.Log("CUSTOMER_ARRIVALS_PASSED: nine distinct civilian looks; same identity; everyday shoes until fitting; selected catalog outfit on departure; service and saves.");
            }finally{studio.PlayerLook=look;Home();}
        }
    }
}
