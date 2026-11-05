using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public int CommerceInfillTriangles {get;private set;}
        public int CommerceInfillBatches {get;private set;}
        void FillCommerceGap()
        {
            int before=0,count=batches.Count;foreach(var b in batches.Values)before+=b.t.Count/3;
            LocalCommerce(-98,14,0,new Color(.64f,.27f,.18f),new Color(.10f,.47f,.44f));
            LocalCommerce(-85,10,1,new Color(.28f,.43f,.52f),new Color(.86f,.58f,.08f));
            LocalCommerce(-49,18,2,new Color(.62f,.49f,.25f),new Color(.19f,.40f,.30f));
            // A small planted pause between the two northbound pedestrian passages.
            Bench(-66.7f,12.8f);RootedTree(new Vector3(-66.7f,0,16.2f),5.8f,143);
            StreetPlanter(-69.5f,8.5f,145);
            foreach(float x in new[]{-104.8f,-91.3f,-57.5f,-40.3f}){
                Block(new Vector3(x,.35f,6.2f),new Vector3(.9f,.7f,.9f),Peach,"CityBrick");
                Foliage(new Vector3(x,.95f,6.2f),new Vector3(.48f,.70f,.48f),LeafGreen,146);
                obstacles.Add(new Rect(x-.5f,5.7f,1,1));
            }
            int after=0;foreach(var b in batches.Values)after+=b.t.Count/3;CommerceInfillTriangles=after-before;CommerceInfillBatches=batches.Count-count;
        }
        void LocalCommerce(float x,float width,int kind,Color wall,Color accent)
        {
            const float z=12,front=6.95f;
            Residence(x,z,kind==2?3:2,width,10,wall,0,160+kind,true);
            obstacles.Add(new Rect(x-width*.5f,6.8f,width,10.4f));
            Block(new Vector3(x,1.9f,front),new Vector3(width-.5f,3.8f,.20f),Ink,"CityMetal");
            Block(new Vector3(x,4.25f,front-.1f),new Vector3(width-.2f,1.1f,.30f),accent,"Wood");
            CommerceSign(kind,new Vector3(x,4.25f,front-.27f),8,1,Cream);
            float door=x+width*.5f-1.7f;ResidenceDoor(door,front-.2f);
            float display=x-1.3f,displayWidth=width-4.5f;
            Block(new Vector3(display,2.05f,front-.20f),new Vector3(displayWidth,2.75f,.14f),new Color(.24f,.24f,.20f),"CityWindow");
            foreach(float side in new[]{-1f,1f})Block(new Vector3(display+side*displayWidth*.5f,2.02f,front-.31f),new Vector3(.14f,2.9f,.18f),Cream,"Wood");
            Block(new Vector3(display,.60f,front-.33f),new Vector3(displayWidth+.3f,.20f,.42f),accent,"CityBrick");
            Block(new Vector3(display,3.54f,front-.32f),new Vector3(displayWidth+.3f,.13f,.34f),Cream,"Wood");
            Awning(x,front-.30f,3.68f,width-.8f,accent);
            Block(new Vector3(x,3.68f,front-.70f),new Vector3(width-.9f,.065f,.16f),new Color(1,.81f,.48f),"CityGlow");
            AddNightSource(new Vector3(x,3.55f,front-.85f),kind==1?new Color(.27f,.77f,1):new Color(1,.68f,.34f),8,.40f,.24f);
            if(kind==0){
                // Two chairs and mirrors behind the glazed frontage, plus a barber pole.
                foreach(float chair in new[]{display-2.2f,display+1.3f}){
                    Block(new Vector3(chair,2.3f,front-.31f),new Vector3(1.45f,1.65f,.08f),Cream,"CityMetal");
                    Block(new Vector3(chair,2.3f,front-.36f),new Vector3(1.20f,1.4f,.03f),new Color(.38f,.45f,.43f),"CityWindow");
                    Block(new Vector3(chair,1.0f,front-.43f),new Vector3(1.4f,.45f,.16f),Ink,"Leather");
                    Block(new Vector3(chair,1.45f,front-.46f),new Vector3(1.05f,.6f,.10f),new Color(.55f,.10f,.12f),"Leather");
                }
                float pole=x-width*.5f+.45f;
                for(int i=0;i<10;i++)Rod(new Vector3(pole,1.4f+i*.16f,front-.52f),new Vector3(pole,1.56f+i*.16f,front-.52f),.15f,i%3==0?Kaos:i%3==1?Cream:new Color(.10f,.28f,.56f),"CityMetal",8);
                foreach(float y in new[]{1.33f,3.05f})Block(new Vector3(pole,y,front-.52f),new Vector3(.42f,.12f,.42f),Cream,"CityMetal");
            }else if(kind==1){
                for(int i=0;i<3;i++){
                    float machine=display-1.65f+i*1.65f;
                    Block(new Vector3(machine,1.55f,front-.36f),new Vector3(1.45f,1.8f,.10f),Cream,"CityMetal");
                    Rod(new Vector3(machine,1.48f,front-.42f),new Vector3(machine,1.48f,front-.49f),.53f,Ink,"Rubber",16);
                    Rod(new Vector3(machine,1.48f,front-.50f),new Vector3(machine,1.48f,front-.52f),.39f,new Color(.20f,.38f,.43f),"CityMetal",16);
                    Block(new Vector3(machine,2.18f,front-.44f),new Vector3(.75f,.13f,.04f),accent,"CityMetal");
                }
            }else{
                // Repair counter, pegboard and hung tools give the eastern facade a purpose.
                Block(new Vector3(display,1.02f,front-.40f),new Vector3(displayWidth-.5f,.65f,.12f),new Color(.55f,.29f,.12f),"Wood");
                for(int i=0;i<8;i++){
                    float tool=display-displayWidth*.40f+i*displayWidth*.11f;
                    Block(new Vector3(tool,2.05f,front-.37f),new Vector3(.12f,.6f+(i%3)*.14f,.06f),Cream,"CityMetal");
                    Block(new Vector3(tool,2.44f,front-.39f),new Vector3(.38f,.12f,.08f),accent,"CityMetal");
                }
                CommerceSign(3,new Vector3(display,2.96f,front-.42f),4,.5f,Cream);
            }
            foreach(float px in new[]{x-width*.5f+.22f,x+width*.5f-.22f})Rod(new Vector3(px,.1f,front-.1f),new Vector3(px,8.7f,front-.1f),.045f,Ink,"CityMetal",6);
        }
        void CommerceSign(int index,Vector3 center,float width,float height,Color color)
        {
            var b=Get("CommerceSign");int first=b.v.Count;float v=1-(index+1)*.25f;
            b.v.AddRange(new[]{center+new Vector3(-width/2,-height/2,0),center+new Vector3(-width/2,height/2,0),center+new Vector3(width/2,height/2,0),center+new Vector3(width/2,-height/2,0)});
            b.uv.AddRange(new[]{new Vector2(0,v),new Vector2(0,v+.25f),new Vector2(1,v+.25f),new Vector2(1,v)});
            for(int i=0;i<4;i++){b.n.Add(Vector3.back);b.c.Add(color);}b.t.AddRange(new[]{first,first+1,first+2,first,first+2,first+3});
        }
    }
}
