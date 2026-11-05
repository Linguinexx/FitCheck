using UnityEngine;
namespace FitCheck
{
    public partial class StudioView
    {
        int civilianWardrobe=-1;
        public void ApplyCivilian(int id){int previous=civilianWardrobe;try{civilianWardrobe=Mathf.Abs(id);Apply(Catalog.Default,-1);CompactCivilianWardrobe();}finally{civilianWardrobe=previous;}}
        static readonly Color[] CivilianColors={new Color(.82f,.78f,.66f),new Color(.09f,.39f,.27f),new Color(.13f,.29f,.53f),new Color(.67f,.22f,.13f),new Color(.75f,.46f,.10f),new Color(.13f,.46f,.45f),new Color(.54f,.19f,.40f),new Color(.26f,.28f,.33f),new Color(.75f,.70f,.56f)};
        Garment CivilianTop(int id)
        {
            int style=id%5;return new Garment(style==2||style==3?"CIV_TOP_03":"CIV_TOP_01","Roupa cotidiana","CIVIL",0,CivilianColors[id%9],true,style==0||style==2){discreet=true};
        }
        Garment CivilianBottom(int id)
        {
            var colors=new[]{new Color(.12f,.22f,.32f),new Color(.46f,.34f,.20f),new Color(.19f,.22f,.24f),new Color(.31f,.36f,.25f)};
            return new Garment("CIV_BOTTOM","Calça cotidiana","CIVIL",1,colors[id%4],true){shorts=id==6||id==8};
        }
        void FinishCivilianMaterial(Material material,string fabric="Cotton")
        {
            material.name="Civilian — "+fabric;material.shader=Resources.Load<Shader>("GarmentFabric");material.mainTexture=Resources.Load<Texture2D>("Garments/Finishes/"+fabric);material.SetFloat("_Motion",0);material.SetFloat("_SmoothLight",1);material.SetFloat("_Outline",0);material.SetFloat("_FabricWeight",0);
        }
        bool BuildCivilianTop(Garment top,Material fabric)
        {
            int style=civilianWardrobe%5;FinishCivilianMaterial(fabric,style==4?"Stripes":"Cotton");ShirtBody(top,fabric);
            var trim=Material(top.color*.75f);FinishCivilianMaterial(trim);
            Rings("Gola",new Vector3(0,2.217f,0),new Vector3(.34f,.045f,.32f),trim,clothes,new[]{0f,1f},new[]{1f,1f},12);
            if(style==1||style==3){
                for(int side=-1;side<=1;side+=2){var collar=Box("Detalhe civil — colarinho",new Vector3(side*.09f,2.17f,-.145f),new Vector3(.14f,.12f,.02f),trim,clothes);collar.transform.localRotation=Quaternion.Euler(0,0,side*18);}
                Box("Detalhe civil — vista",new Vector3(0,style==3?1.8f:2.07f,-.275f),new Vector3(.032f,style==3?.62f:.15f,.012f),trim,clothes);
                var button=Material(new Color(.55f,.52f,.43f));FinishCivilianMaterial(button);
                for(int i=0;i<(style==3?4:2);i++)Box("Detalhe civil — botão",new Vector3(0,2.10f-i*(style==3?.15f:.065f),-.288f),new Vector3(.014f,.014f,.009f),button,clothes);
                if(style==3)for(int side=-1;side<=1;side+=2)Box("Detalhe civil — bolso",new Vector3(side*.23f,1.91f,-.275f),new Vector3(.16f,.16f,.012f),trim,clothes);
            }
            return true;
        }
        void BuildCivilianShoes(int id,int side)
        {
            var item=new Garment("CIV_SHOES","Tênis cotidiano","CIVIL",2,Color.white,true);var shape=Sneaker(item);float x=side*.24f;
            // A simple court silhouette with a stitched sole, suede/cloth upper and laces.
            shape.w=new[]{.115f,.16f,.175f,.185f,.18f,.14f,.085f};shape.h=new[]{.08f,.13f,.12f,.09f,.06f,.035f,.022f};shape.y=new[]{.285f,.30f,.29f,.26f,.235f,.225f,.225f};
            var upper=Material(id%3==0?new Color(.30f,.23f,.18f):id%3==1?new Color(.20f,.23f,.28f):new Color(.77f,.76f,.69f));FinishCivilianMaterial(upper);
            var sole=Material(new Color(.77f,.75f,.67f));FinishCivilianMaterial(sole,"Canvas");
            ShoeVolume("Tênis",x,upper,shape,false);ShoeVolume("Sola",x,sole,shape,true);
            var lace=Material(new Color(.85f,.83f,.75f));FinishCivilianMaterial(lace);
            for(int i=0;i<3;i++){float z=.025f-i*.075f;Tube("Cadarço",new[]{new Vector3(x-.07f,ShoeTop(shape,-.07f,z)+.008f,z),new Vector3(x,ShoeTop(shape,0,z)+.008f,z+.012f),new Vector3(x+.07f,ShoeTop(shape,.07f,z)+.008f,z)},.006f,lace,clothes);}
        }
        static void FinishCollectionMaterial(Material material,string id)
        {
            var weave=Resources.Load<Texture2D>("Surfaces/Fabric030");material.SetTexture("_FabricTex",weave);material.SetFloat("_FabricWeight",id.Contains("_SHOES_")?.025f:id=="CG_TOP_03"?.20f:id=="AH_TOP_03"?.055f:id=="AH_TOP_01"?.18f:.11f);material.SetFloat("_Motion",0);
        }
        void FinishCollectionSleeve(Material material,BlenderGarmentAsset model)
        {
            material.shader=Resources.Load<Shader>("GarmentFabric");material.mainTexture=Resources.Load<Texture2D>("Garments/Finishes/Cotton");material.color=model.sleeveColor;material.SetFloat("_SmoothLight",1);material.SetFloat("_Outline",0);FinishCollectionMaterial(material,model.id);ConfigureGarmentFinish(material,model.id,false);
        }
    }
}
