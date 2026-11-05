using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public int MusicInfillTriangles {get;private set;}
        public int MusicInfillBatches {get;private set;}
        void FillMusicGap()
        {
            int before=0,count=batches.Count;foreach(var b in batches.Values)before+=b.t.Count/3;
            PlaceCityModule(MusicStudioFront,new Vector3(-30,0,-29),true);
            foreach(float x in new[]{-36.5f,-23.7f}){
                Block(new Vector3(x,.36f,-22.8f),new Vector3(.85f,.72f,.85f),Peach,"CityBrick");
                Foliage(new Vector3(x,1.02f,-22.8f),new Vector3(.48f,.72f,.48f),LeafGreen,181);
                obstacles.Add(new Rect(x-.48f,-23.28f,.96f,.96f));
            }
            // The clear lane at X=-21.5 joins the promenade to the rear music pocket.
            foreach(float x in new[]{-22.6f,-18.0f}){
                Rod(new Vector3(x,0,-25),new Vector3(x,5.3f,-25),.055f,Ink,"CityMetal");
                obstacles.Add(new Rect(x-.12f,-25.12f,.24f,.24f));
            }
            Rod(new Vector3(-22.6f,5.3f,-25),new Vector3(-18,5.3f,-25),.045f,Ink,"CityMetal");
            PlaceCityModule(()=>{Block(new Vector3(0,4.65f,0),new Vector3(4.1f,.7f,.13f),new Color(.41f,.12f,.63f),"CityMetal");Words("BATALHA DE RIMA",new Vector3(0,4.65f,-.09f),.045f,Cream);},new Vector3(-20.3f,0,-25),true);
            // Wires and pennants remain above the walkable entrance.
            for(int line=0;line<2;line++){
                float z=-29-line*7;
                Rod(new Vector3(-36,5.4f,z),new Vector3(-18,5.0f,z),.015f,Ink,"CityMetal",5);
                for(int i=0;i<8;i++){
                    float x=-35+i*2.2f,y=5.4f-(x+36)/18*.4f;
                    Triangle(new Vector3(x,y,z),new Vector3(x+.8f,y-.65f,z),new Vector3(x+1.6f,y,z),i%3==0?Kaos:i%3==1?Cream:new Color(.10f,.64f,.46f),"CityBanners");
                }
            }
            MusicCourtyard();
            AddNightSource(new Vector3(-30,3.64f,-23.10f),new Color(.18f,.77f,.64f),8,.43f,.22f);
            Block(new Vector3(-26,4.05f,-43.83f),new Vector3(.72f,.18f,.42f),Ink,"CityMetal");
            Block(new Vector3(-26,3.94f,-43.64f),new Vector3(.46f,.04f,.24f),new Color(.67f,.24f,1),"CityGlow");
            AddNightSource(new Vector3(-26,3.88f,-43.57f),new Color(.67f,.24f,1),9,.44f,.18f);
            int after=0;foreach(var b in batches.Values)after+=b.t.Count/3;MusicInfillTriangles=after-before;MusicInfillBatches=batches.Count-count;
        }
        void MusicStudioFront()
        {
            var blue=new Color(.20f,.30f,.43f);var green=new Color(.11f,.56f,.42f);
            Residence(0,0,2,14,10,blue,0,184,true);
            Block(new Vector3(0,2,-5.17f),new Vector3(13.5f,4,.18f),Ink,"CityMetal");
            ResidenceDoor(4.4f,-5.35f);
            Block(new Vector3(-1.4f,2.1f,-5.31f),new Vector3(8.5f,2.75f,.12f),new Color(.24f,.33f,.30f),"CityWindow");
            foreach(float x in new[]{-5.7f,2.9f})Block(new Vector3(x,2.1f,-5.42f),new Vector3(.13f,3.0f,.18f),green,"Wood");
            Block(new Vector3(-1.4f,.68f,-5.44f),new Vector3(8.8f,.18f,.36f),green,"Wood");
            // Microphone and mixing console are readable silhouettes behind the display.
            Rod(new Vector3(-3.8f,.78f,-5.43f),new Vector3(-3.8f,2.45f,-5.43f),.035f,Cream,"CityMetal",6);
            Rod(new Vector3(-3.8f,2.45f,-5.43f),new Vector3(-3.45f,2.60f,-5.43f),.12f,Ink,"Rubber",10);
            Block(new Vector3(-.1f,1.25f,-5.46f),new Vector3(3.8f,.75f,.12f),Ink,"CityMetal");
            for(int i=0;i<8;i++){
                float x=-1.5f+i*.43f;
                Block(new Vector3(x,1.30f,-5.55f),new Vector3(.045f,.43f,.035f),Cream,"CityMetal");
                Block(new Vector3(x,1.28f+(i%3)*.09f,-5.59f),new Vector3(.18f,.07f,.05f),i%3==0?Kaos:green,"CityMetal");
            }
            Block(new Vector3(0,4.55f,-5.3f),new Vector3(13.6f,1.15f,.26f),green,"CityMetal");
            Words("SOM",new Vector3(-1.2f,4.60f,-5.46f),.13f,Cream);
            Words("LADO B",new Vector3(4.5f,4.60f,-5.46f),.055f,Cream);
            Awning(0,-5.1f,3.68f,13.2f,blue);
            Block(new Vector3(0,3.66f,-5.85f),new Vector3(12.5f,.065f,.16f),new Color(.27f,.92f,.64f),"CityGlow");
            // A compact crate links the studio window to SIDE B without occupying the lane.
            Block(new Vector3(-3.1f,.64f,-6.1f),new Vector3(2.2f,1.28f,.75f),new Color(.51f,.28f,.12f),"Wood");
            for(int i=0;i<4;i++){
                float x=-3.85f+i*.5f;
                Block(new Vector3(x,1.25f,-6.1f),new Vector3(.42f,.65f,.06f),i%2==0?Cream:green,"Paper");
                Rod(new Vector3(x,1.30f,-6.15f),new Vector3(x,1.30f,-6.18f),.17f,Ink,"Rubber",12);
            }
            Words("SO MAIS UM DROP",new Vector3(-3.1f,.64f,-6.50f),.025f,Cream);
            obstacles.Add(new Rect(-4.25f,-6.55f,2.3f,.9f));
        }
        void MusicCourtyard()
        {
            PlaceCityModule(()=>Bench(0,0),new Vector3(-28.8f,0,-41.6f),true);
            RootedTree(new Vector3(-36.2f,0,-41.1f),5.1f,189);
            Block(new Vector3(-36.2f,.25f,-41.1f),new Vector3(1.1f,.5f,1.1f),Peach,"CityBrick");obstacles.Add(new Rect(-36.8f,-41.7f,1.2f,1.2f));
            Bicycle(new Vector3(-22.5f,0,-39.5f));
            Block(new Vector3(-34,.15f,-38.5f),new Vector3(4.8f,.30f,2.5f),new Color(.28f,.18f,.13f),"Wood");
            obstacles.Add(new Rect(-36.5f,-39.85f,5,2.7f));
            foreach(float x in new[]{-35.9f,-32.1f}){
                Block(new Vector3(x,.95f,-39.3f),new Vector3(.65f,1.6f,.55f),Ink,"Rubber");
                Rod(new Vector3(x,.85f,-38.99f),new Vector3(x,.85f,-38.96f),.22f,Teal,"CityMetal",12);
            }
            Rod(new Vector3(-34,.3f,-38.15f),new Vector3(-34,2.55f,-38.15f),.035f,Cream,"CityMetal",6);
            Rod(new Vector3(-34,2.55f,-38.15f),new Vector3(-34.28f,2.64f,-38.15f),.09f,Ink,"Rubber",8);
            // The perimeter wall becomes a music mural, facing the courtyard.
            PlaceCityModule(()=>{
                Block(new Vector3(0,2.0f,0),new Vector3(18.8f,3.8f,.16f),new Color(.22f,.13f,.35f),"CityBrick");
                Words("BATALHA DE RIMA",new Vector3(0,2.8f,-.12f),.09f,Cream);
                Words("RESPEITA O CORRE",new Vector3(0,1.9f,-.12f),.048f,new Color(.18f,.82f,.57f));
                for(int side=-1;side<=1;side+=2)for(int i=0;i<3;i++){
                    var a=new Vector3(side*(6.2f+i*.65f),.5f,-.13f);var b=new Vector3(side*(6.4f+i*.65f),3.5f,-.13f);
                    var c=new Vector3(side*(6.7f+i*.65f),3.2f,-.13f);var d=new Vector3(side*(6.5f+i*.65f),.8f,-.13f);var color=i==1?Kaos:new Color(.15f,.63f,.48f);
                    if(side<0)Quad(d,c,b,a,color,"Paper");else Quad(a,b,c,d,color,"Paper");
                }
            },new Vector3(-28.5f,0,-43.95f),true);
        }
    }
}
