using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        void ShopFittingFinishes()
        {
            // Reuse existing textures and static batches. No realtime lights or
            // interactive obstacles are added to the fitting path.
            var wood=new Color(.47f,.31f,.20f);
            var steel=new Color(.53f,.51f,.46f);
            var upholstery=new Color(.20f,.19f,.17f);
            Block(new Vector3(-4.9f,2.1f,8.68f),new Vector3(2.78f,3.82f,.1f),Ink,"CityMetal");
            for(int side=-1;side<=1;side+=2){
                Block(new Vector3(-4.9f+side*1.35f,2.1f,8.52f),new Vector3(.09f,3.74f,.12f),Ink,"CityMetal");
                for(int slat=0;slat<6;slat++){
                    float x=-4.9f+side*(1.50f+slat*.12f);
                    var grain=Get("Wood");int first=grain.uv.Count;
                    Block(new Vector3(x,2.28f,8.59f),new Vector3(.078f,2.55f,.075f),wood,"Wood",.01f);
                    for(int vertex=first;vertex<grain.uv.Count;vertex++){
                        var uv=grain.uv[vertex];grain.uv[vertex]=new Vector2(uv.y,uv.x+slat*.17f+side*.09f);
                    }
                }
                for(int end=0;end<2;end++){
                    float y=end==0?.27f:3.93f;
                    Rod(new Vector3(-4.9f+side*1.35f,y,8.455f),new Vector3(-4.9f+side*1.35f,y,8.425f),.023f,steel,"CityMetal",6);
                }
            }
            foreach(float y in new[]{.27f,3.93f})
                Block(new Vector3(-4.9f,y,8.52f),new Vector3(2.78f,.09f,.12f),Ink,"CityMetal");
            Block(new Vector3(-2.615f,.66f,7),new Vector3(.045f,1.16f,3.38f),wood,"Wood");
            Block(new Vector3(-2.635f,1.27f,7),new Vector3(.065f,.065f,3.44f),Ink,"CityMetal");
            Block(new Vector3(-2.62f,.08f,7),new Vector3(.07f,.15f,3.45f),Ink,"CityMetal");
            Block(new Vector3(-4.9f,.08f,8.59f),new Vector3(4.48f,.15f,.07f),Ink,"CityMetal");
            Ground(-4.9f,6.75f,2.28f,2.35f,-.037f,new Color(.18f,.17f,.155f),"Rug");
            foreach(float x in new[]{-6.015f,-3.785f})Ground(x,6.75f,.026f,2.25f,-.035f,Kaos*.78f,"Cloth");
            foreach(float z in new[]{5.61f,7.89f})Ground(-4.9f,z,2.20f,.026f,-.035f,Kaos*.78f,"Cloth");
            // Seat and connected metal legs remain inside the existing obstacle.
            Block(new Vector3(-6.7f,.44f,7.2f),new Vector3(.73f,.10f,1.58f),Ink,"CityMetal");
            Block(new Vector3(-6.7f,.56f,7.2f),new Vector3(.78f,.16f,1.64f),upholstery,"FittingFabric",.04f);
            for(int side=-1;side<=1;side+=2){
                float x=-6.7f+side*.27f;
                Rod(new Vector3(x,.635f,6.43f),new Vector3(x,.635f,7.97f),.009f,new Color(.52f,.49f,.42f),"Cloth",6);
                foreach(float z in new[]{6.65f,7.75f})
                    Block(new Vector3(x,.20f,z),new Vector3(.055f,.43f,.075f),Ink,"CityMetal",.01f);
                Rod(new Vector3(x,.10f,6.65f),new Vector3(x,.10f,7.75f),.025f,steel,"CityMetal",6);
            }
            Block(new Vector3(-2.67f,2.44f,7.18f),new Vector3(.08f,.14f,1.18f),Ink,"CityMetal");
            foreach(float z in new[]{6.80f,7.18f,7.56f}){
                Rod(new Vector3(-2.70f,2.44f,z),new Vector3(-2.92f,2.44f,z),.024f,steel,"CityMetal",6);
                Rod(new Vector3(-2.92f,2.44f,z),new Vector3(-2.92f,2.55f,z),.024f,steel,"CityMetal",6);
            }
            Block(new Vector3(-2.84f,1.33f,7.2f),new Vector3(.48f,.09f,1.28f),wood,"Wood");
            // Warm diffuser, shaded with the shop's existing lighting material.
            Block(new Vector3(-4.9f,4.19f,7.25f),new Vector3(4.45f,.12f,2.65f),Ink,"CityMetal");
            Block(new Vector3(-4.9f,4.08f,7.25f),new Vector3(1.65f,.10f,.42f),Ink,"CityMetal");
            Block(new Vector3(-4.9f,4.015f,7.25f),new Vector3(1.48f,.025f,.30f),new Color(1,.83f,.62f),"CityGlow");
        }
    }
}
