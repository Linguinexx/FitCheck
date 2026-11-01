using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        void Bench(float x,float z)
        {
            for(int side=-1;side<=1;side+=2){Block(new Vector3(x+side*1.75f,.43f,z),new Vector3(.12f,.86f,.75f),Ink,"Leather");Block(new Vector3(x+side*1.75f,.9f,z+.33f),new Vector3(.12f,1.7f,.12f),Ink,"Leather");}
            for(int i=0;i<4;i++)Block(new Vector3(x,.83f,z-.32f+i*.21f),new Vector3(4.5f,.12f,.17f),new Color(.63f,.38f,.22f),"Wood");
            for(int i=0;i<3;i++)Block(new Vector3(x,1.16f+i*.22f,z+.35f),new Vector3(4.5f,.16f,.12f),new Color(.63f,.38f,.22f),"Wood");
            obstacles.Add(new Rect(x-2.35f,z-.48f,4.7f,1.0f));
        }
        void Palm(float x,float z,int seed)
        {
            var basePoint=new Vector3(x,.25f,z);var top=new Vector3(x+.45f,7.7f,z+.20f);
            for(int i=0;i<7;i++){
                var a=Vector3.Lerp(basePoint,top,i/7f);var b=Vector3.Lerp(basePoint,top,(i+1)/7f);Rod(a,b,.17f-i*.009f,new Color(.41f,.30f,.20f),"Wood");
            }
            for(int leaf=0;leaf<9;leaf++){
                float angle=leaf*Mathf.PI*2/9+seed;var direction=new Vector3(Mathf.Sin(angle),0,Mathf.Cos(angle));var right=Vector3.Cross(Vector3.up,direction);
                for(int i=0;i<5;i++){
                    float a=i/5f,b=(i+1)/5f;var p=top+direction*(a*3.5f)+Vector3.up*(Mathf.Sin(a*Mathf.PI)*.75f-a*.8f);var q=top+direction*(b*3.5f)+Vector3.up*(Mathf.Sin(b*Mathf.PI)*.75f-b*.8f);
                    float wa=Mathf.Sin(a*Mathf.PI)*.40f+.015f,wb=Mathf.Sin(b*Mathf.PI)*.40f+.015f;
                    Quad(p-right*wa,q-right*wb,q+right*wb,p+right*wa,(leaf%2==0?new Color(.18f,.37f,.24f):new Color(.28f,.46f,.25f)),"CityLeaves");
                }
            }
            Block(new Vector3(x,.18f,z),new Vector3(1.3f,.3f,1.3f),Cream);obstacles.Add(new Rect(x-.70f,z-.70f,1.4f,1.4f));
        }
        void Lamp(float x,float z)
        {
            Rod(new Vector3(x,0,z),new Vector3(x,5.7f,z),.07f,Ink,"Leather");
            Rod(new Vector3(x,5.7f,z),new Vector3(x+.75f,5.75f,z),.065f,Ink,"Leather");
            Block(new Vector3(x+.75f,5.68f,z),new Vector3(.88f,.19f,.40f),Ink,"Leather");
            Block(new Vector3(x+.75f,5.57f,z),new Vector3(.68f,.045f,.28f),new Color(1,.86f,.56f),"CityGlow");
            Block(new Vector3(x,.12f,z),new Vector3(.36f,.24f,.36f),Ink);
            obstacles.Add(new Rect(x-.25f,z-.25f,.5f,.5f));
        }
        void Landscape()
        {
            Ground(0,10,150,110,-.21f,new Color(.31f,.39f,.32f),"Plaster",8);
            Pavement();
            Ground(0,-13.7f,150,5.2f,-.018f,Color.white,"CityAsphalt",4);
            Block(new Vector3(0,-.01f,-10.95f),new Vector3(56,.25f,.30f),Cream,"Plaster",.045f);
            for(int i=0;i<7;i++)Ground(-18,-15.7f+i*.65f,4.0f,.35f,-.012f,Cream,"Leather");
            for(int i=-5;i<=5;i++)Ground(i*5.2f,-14.7f,2.4f,.10f,-.010f,new Color(.77f,.65f,.36f),"Leather");
            Block(new Vector3(-10.3f,.30f,-.7f),new Vector3(7.8f,.60f,5.1f),Cream,"Plaster",.4f);
            Ground(-10.3f,-.7f,7.25f,4.55f,.62f,new Color(.27f,.30f,.19f),"Plaster",2);
            obstacles.Add(new Rect(-14.2f,-3.25f,7.8f,5.1f));
            Rod(new Vector3(-10.8f,.60f,-.4f),new Vector3(-10.3f,7.9f,-.1f),.29f,new Color(.36f,.24f,.17f));
            Rod(new Vector3(-10.6f,4.0f,-.3f),new Vector3(-12.0f,8.8f,-.1f),.15f,new Color(.36f,.24f,.17f));
            Rod(new Vector3(-10.5f,5.1f,-.2f),new Vector3(-8.6f,8.8f,.8f),.15f,new Color(.36f,.24f,.17f));
            Foliage(new Vector3(-10.2f,9.1f,0),new Vector3(3.2f,2.0f,2.8f),new Color(.27f,.45f,.27f),2);
            Foliage(new Vector3(-12.4f,8.4f,-.1f),new Vector3(2.0f,1.5f,1.9f),new Color(.33f,.49f,.28f),5);
            Bench(-10.3f,-4.5f);Bench(-18.7f,1.4f);
            Palm(-22.3f,-4.6f,1);Palm(17.8f,2.1f,2);Lamp(-2.5f,-4.0f);Lamp(17.5f,-7.0f);
            foreach(float x in new[]{-1.0f,17.1f}){
                Block(new Vector3(x,.38f,2.2f),new Vector3(1.4f,.76f,1.7f),Peach);
                Foliage(new Vector3(x,1.10f,2.2f),new Vector3(.70f,.65f,.78f),Teal,3);obstacles.Add(new Rect(x-.75f,1.3f,1.5f,1.8f));
            }
            // Park name on a low ceramic marker, not a floating mission marker.
            Block(new Vector3(-3.8f,.72f,-.45f),new Vector3(2.4f,1.4f,.30f),Teal);
            Words("PRACA",new Vector3(-3.8f,.97f,-.62f),.055f,Cream);Words("ENCONTRO",new Vector3(-3.8f,.45f,-.62f),.038f,Cream);
            obstacles.Add(new Rect(-5.1f,-.75f,2.6f,.6f));
            for(int i=0;i<10;i++){
                float a=i/10f,b=(i+1)/10f;
                var p=new Vector3(Mathf.Lerp(-24,26,a),9.7f-Mathf.Sin(a*Mathf.PI)*2,6.0f);var q=new Vector3(Mathf.Lerp(-24,26,b),9.7f-Mathf.Sin(b*Mathf.PI)*2,6.0f);Rod(p,q,.015f,Ink,"Leather",5);
            }
        }
        void Pavement()
        {
            var batch=Get("CityStone");
            for(int x=-29;x<29;x++)for(int z=-17;z<16;z++){
                int index=batch.v.Count;var points=new[]{new Vector3(x,-.04f,z),new Vector3(x,-.04f,z+1),new Vector3(x+1,-.04f,z+1),new Vector3(x+1,-.04f,z)};
                foreach(var p in points){
                    float tree=new Vector2((p.x+11)/4.2f,(p.z+.7f)/3.1f).magnitude;
                    float canopy=Mathf.Clamp01((3.8f-p.z)/2.0f)*Mathf.Clamp01((p.z-.6f)/1.5f)*Mathf.Clamp01((8.7f-Mathf.Abs(p.x-7.5f))/1.0f);
                    float shade=1-.20f*(1-Mathf.SmoothStep(.2f,1,tree))-.15f*canopy;
                    batch.v.Add(p);batch.n.Add(Vector3.up);batch.c.Add(Cream*shade);batch.uv.Add(new Vector2(p.x/4.7f,p.z/4.7f));
                }
                batch.t.AddRange(new[]{index,index+1,index+2,index,index+2,index+3});
            }
        }
    }
}
