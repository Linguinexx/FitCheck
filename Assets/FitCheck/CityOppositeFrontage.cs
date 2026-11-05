using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        // Static geometry reuses city batches, without new actors or lights.
        void OppositeShopFrontage()
        {
            PlaceCityModule(()=>OppositeCommerce(true),new Vector3(-9,0,-28),true);
            PlaceCityModule(()=>OppositeCommerce(false),new Vector3(12,0,-28),true);
            StreetPlanter(-19,-21,41);StreetPlanter(23,-21,42);
            Bench(-9,-20.5f);Bicycle(new Vector3(17,0,-21.8f));Lamp(1,-20.8f);
            FillMusicGap();
        }
        void OppositeCommerce(bool records)
        {
            var accent=records?new Color(.43f,.11f,.66f):new Color(.02f,.54f,.41f);
            var wall=records?new Color(.64f,.32f,.18f):new Color(.36f,.42f,.34f);
            Residence(0,0,records?2:3,17,10,wall,0,records?101:102,true);
            Block(new Vector3(0,2.05f,-5.18f),new Vector3(15.5f,4.1f,.18f),Ink,"CityMetal");
            foreach(float x in new[]{-5.1f,5.1f}){
                Block(new Vector3(x,2.20f,-5.31f),new Vector3(4.15f,2.8f,.10f),records?new Color(.26f,.17f,.10f):new Color(.16f,.24f,.20f),"CityWindow");
                foreach(float side in new[]{-1f,1f})Block(new Vector3(x+side*2.15f,2.2f,-5.40f),new Vector3(.13f,3.1f,.18f),accent,"Wood");
                Block(new Vector3(x,.68f,-5.43f),new Vector3(4.55f,.16f,.42f),Cream,"Plaster");
            }
            ResidenceDoor(0,-5.35f);
            Block(new Vector3(0,4.45f,-5.3f),new Vector3(16.4f,.83f,.25f),accent,"CityMetal");
            Words(records?"SIDE B":"OFICINA",new Vector3(0,4.45f,-5.46f),.11f,Cream);
            Awning(0,-5.1f,3.73f,16.4f,records?new Color(.72f,.44f,.06f):accent);
            if(records){
                foreach(float x in new[]{-6.3f,-4.9f,-3.5f,3.8f,5.5f}){
                    float y=x<0?2.2f:2.45f;
                    Block(new Vector3(x,y,-5.46f),new Vector3(1.12f,1.12f,.07f),x<0?Cream:accent,"Paper");
                    Rod(new Vector3(x,y,-5.51f),new Vector3(x,y,-5.56f),.46f,Ink,"Rubber",16);
                    Rod(new Vector3(x,y,-5.565f),new Vector3(x,y,-5.58f),.13f,x<0?Kaos:Cream,"Paper",12);
                }
                Block(new Vector3(5.05f,1.18f,-5.44f),new Vector3(3.8f,.40f,.07f),Cream,"Paper");
                Words("SO MAIS UM DROP",new Vector3(5.05f,1.18f,-5.49f),.033f,Ink);
            }else{
                Block(new Vector3(-5.1f,1.35f,-5.45f),new Vector3(3.8f,.16f,.17f),new Color(.62f,.34f,.12f),"Wood");
                foreach(float x in new[]{3.9f,6.0f})for(int i=0;i<16;i++){
                    float a=i*Mathf.PI*2/16,b=(i+1)*Mathf.PI*2/16;var center=new Vector3(x,2.2f,-5.5f);
                    var u=center+new Vector3(Mathf.Cos(a)*.68f,Mathf.Sin(a)*.68f,0);var v=center+new Vector3(Mathf.Cos(b)*.68f,Mathf.Sin(b)*.68f,0);
                    Rod(u,v,.045f,Ink,"Rubber",5);if(i%4==0)Rod(center,u,.012f,Cream,"CityMetal",4);
                }
                for(int i=0;i<5;i++)Block(new Vector3(-6.45f+i*.63f,2.1f,-5.5f),new Vector3(.11f,.65f+i%2*.25f,.07f),Cream,"CityMetal");
            }
            foreach(float x in new[]{-7.5f,7.5f})Rod(new Vector3(x,.12f,-5.50f),new Vector3(x,8.4f,-5.50f),.045f,Ink,"CityMetal",6);
        }
    }
}
