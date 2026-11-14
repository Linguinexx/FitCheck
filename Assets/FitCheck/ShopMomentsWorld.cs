using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public static readonly Vector3 ShopRadioSpot=new Vector3(5.45f,1.015f,-3.1f),ShopAlbumSpot=new Vector3(4.08f,2.005f,6.62f);
        Material shopMemoryMaterial;
        void BuildShopMoments(){
            ShopRoundedSolid(ShopRadioSpot,new Vector3(.88f,.47f,.27f),.07f,Ink,"CityMetal");
            foreach(float side in new[]{-1f,1f}){
                var p=ShopRadioSpot+new Vector3(side*.275f,0,.149f);
                ShopRing(p,Vector3.right*.14f,Vector3.up*.14f,.015f,Kaos);
                Block(p,new Vector3(.17f,.17f,.028f),Ink*.6f,"Leather",.025f);
            }
            Block(ShopRadioSpot+new Vector3(0,.025f,.145f),new Vector3(.19f,.11f,.025f),new Color(.12f,.69f,.54f),"CityGlow",.008f);
            Rod(ShopRadioSpot+new Vector3(-.25f,.24f,0),ShopRadioSpot+new Vector3(-.25f,.35f,0),.018f,Cream,"CityMetal");
            Rod(ShopRadioSpot+new Vector3(-.25f,.35f,0),ShopRadioSpot+new Vector3(.25f,.35f,0),.018f,Cream,"CityMetal");
            Rod(ShopRadioSpot+new Vector3(.25f,.35f,0),ShopRadioSpot+new Vector3(.25f,.24f,0),.018f,Cream,"CityMetal");
            // Small album/stand shares a clear part of the counter, between bags and printer.
            ShopRoundedSolid(new Vector3(4.08f,1.67f,6.62f),new Vector3(.55f,.055f,.40f),.04f,Kaos,"Paper");
            var frame=FramedPrint("Memória dos clientes — balcão",ShopAlbumSpot,180,0,new Vector2(.49f,.49f));
            frame.Find("Moldura").localScale=new Vector3(.61f,.61f,.055f);frame.Find("Passe-partout").localScale=new Vector3(.54f,.54f,.035f);
            shopMemoryMaterial=frame.Find("Arte fornecida").GetComponent<MeshRenderer>().sharedMaterial;
            frame.Find("Arte fornecida").localScale=new Vector3(.49f,.49f,1);
            Block(new Vector3(4.08f,1.71f,6.62f),new Vector3(.33f,.025f,.22f),Ink,"Wood");
        }
        public void SetShopMemory(Texture2D image){if(shopMemoryMaterial!=null)shopMemoryMaterial.mainTexture=image!=null?image:GalleryArt.Load(0);}
        public bool LookingAtShopRadio(Camera camera)=>IsInterior&&!IsHome&&ActivityLook(camera,ShopRadioSpot,3);
        public bool LookingAtShopAlbum(Camera camera)=>IsInterior&&!IsHome&&ActivityLook(camera,ShopAlbumSpot,2.6f);
    }
}
