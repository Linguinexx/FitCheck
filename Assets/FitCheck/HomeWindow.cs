using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        void HomeNightWindow()
        {
            var metal=new Color(.24f,.25f,.23f);var night=new Color(.035f,.045f,.07f);
            Block(new Vector3(.7f,2.75f,4.40f),new Vector3(3.0f,2.25f,.17f),metal,"CityMetal",.04f);
            Block(new Vector3(.7f,2.75f,4.29f),new Vector3(2.72f,1.98f,.04f),night,"CityWindow",.005f);
            // Layered silhouette beyond the pane, instead of bright blocks glued to the wall.
            for(int i=0;i<7;i++){
                float x=-.47f+i*.38f,h=.45f+(i%3)*.19f;
                Block(new Vector3(x,1.79f+h*.5f,4.245f),new Vector3(.32f,h,.018f),new Color(.06f,.065f,.08f),"CityMetal",.002f);
                for(int j=0;j<2;j++)Block(new Vector3(x-.075f+j*.15f,1.94f,4.228f),new Vector3(.055f,.07f,.008f),new Color(.65f,.29f,.08f),"CityGlow",.001f);
            }
            foreach(float x in new[]{-.71f,.7f,2.11f})Block(new Vector3(x,2.75f,4.19f),new Vector3(.065f,2.08f,.12f),metal,"CityMetal",.008f);
            foreach(float y in new[]{1.72f,3.78f})Block(new Vector3(.7f,y,4.18f),new Vector3(2.9f,.07f,.13f),metal,"CityMetal",.008f);
            Rod(new Vector3(.78f,2.50f,4.10f),new Vector3(.78f,2.79f,4.10f),.027f,Cream,"CityMetal");
            Block(new Vector3(.7f,1.64f,4.08f),new Vector3(3.2f,.15f,.48f),Cream,"Plaster",.035f);
            Rod(new Vector3(-1.15f,4.05f,4.04f),new Vector3(2.55f,4.05f,4.04f),.04f,metal,"CityMetal");
            foreach(float side in new[]{-1f,1f})for(int i=0;i<7;i++){
                float x=.7f+side*(1.35f+i*.07f);float z=3.94f+Mathf.Sin(i*1.6f)*.06f;
                Block(new Vector3(x,2.77f,z),new Vector3(.095f,2.48f,.10f),new Color(.48f,.12f,.16f)*(i%2==0?1:.78f),"Cloth",.018f);
            }
        }
    }
}
