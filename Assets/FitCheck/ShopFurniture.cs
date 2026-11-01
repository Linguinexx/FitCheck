using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        void ShopRoundedSolid(Vector3 center,Vector3 size,float radius,Color color,string texture="CityMetal")
        {
            const int corners=5;int count=corners*4;var points=new Vector3[4,count];
            float bevel=Mathf.Min(.045f,size.y*.18f);
            for(int row=0;row<4;row++){
                float inset=row==0||row==3?bevel:0;
                float hx=size.x*.5f-inset,hz=size.z*.5f-inset,r=Mathf.Min(radius,Mathf.Min(hx,hz));
                float y=row==0?-size.y*.5f:row==1?-size.y*.5f+bevel:row==2?size.y*.5f-bevel:size.y*.5f;
                for(int corner=0;corner<4;corner++)for(int i=0;i<corners;i++){
                    float angle=(corner*90+i*90f/(corners-1))*Mathf.Deg2Rad;
                    var c=new Vector3(corner==0||corner==3?hx-r:-hx+r,y,corner<2?hz-r:-hz+r);
                    points[row,corner*corners+i]=center+c+new Vector3(Mathf.Cos(angle)*r,0,Mathf.Sin(angle)*r);
                }
            }
            for(int i=0;i<count;i++){
                int next=(i+1)%count;
                for(int row=0;row<3;row++)Quad(points[row,i],points[row+1,i],points[row+1,next],points[row,next],color,texture);
                Triangle(center+Vector3.up*size.y*.5f,points[3,next],points[3,i],color,texture);
                Triangle(center-Vector3.up*size.y*.5f,points[0,i],points[0,next],color*.8f,texture);
            }
        }
        void ShopRing(Vector3 center,Vector3 right,Vector3 up,float radius,Color color,string texture="CityMetal")
        {
            for(int i=0;i<12;i++){
                float a=i*Mathf.PI/6,b=(i+1)*Mathf.PI/6;
                Rod(center+(right*Mathf.Cos(a)+up*Mathf.Sin(a)),center+(right*Mathf.Cos(b)+up*Mathf.Sin(b)),radius,color,texture);
            }
        }
        void ShopDetailedCounter()
        {
            ShopRoundedSolid(new Vector3(4.4f,.76f,6.2f),new Vector3(5.65f,1.48f,1.25f),.26f,Ink);
            ShopRoundedSolid(new Vector3(4.4f,.10f,6.2f),new Vector3(5.42f,.2f,1.1f),.25f,Ink*.65f);
            ShopRoundedSolid(new Vector3(4.4f,1.55f,6.2f),new Vector3(5.95f,.17f,1.52f),.32f,new Color(.76f,.65f,.51f),"Wood");
            ShopRoundedSolid(new Vector3(4.4f,1.45f,6.2f),new Vector3(5.8f,.055f,1.4f),.28f,Kaos);
            for(int i=0;i<27;i++){
                float x=1.88f+i*.194f;
                Block(new Vector3(x,.78f,5.54f),new Vector3(.095f,1.12f,.095f),i%3==0?Kaos*.70f:Kaos,"Wood",.025f);
            }
            ShopRoundedSolid(new Vector3(4.4f,.78f,5.42f),new Vector3(3.5f,.68f,.10f),.035f,Ink);
            Words("DRIPLAB",new Vector3(4.4f,.78f,5.355f),.072f,Cream);
            for(int drawer=0;drawer<4;drawer++){
                float x=2.38f+drawer*1.28f;
                Block(new Vector3(x,.85f,6.84f),new Vector3(1.18f,.8f,.025f),Ink*.75f);
                Rod(new Vector3(x-.23f,1.03f,6.89f),new Vector3(x+.23f,1.03f,6.89f),.026f,Cream,"CityMetal");
            }
            // Screen, stand, keyboard, scanner and receipt printer share the same physical counter.
            var equipmentStart=new System.Collections.Generic.Dictionary<Batch,int>();
            foreach(var batch in batches.Values)equipmentStart[batch]=batch.v.Count;
            ShopRoundedSolid(new Vector3(5.8f,1.68f,6.21f),new Vector3(.70f,.10f,.48f),.11f,Ink);
            Rod(new Vector3(5.8f,1.68f,6.25f),new Vector3(5.8f,2.04f,6.28f),.065f,Ink,"CityMetal");
            var screenCenter=new Vector3(5.8f,2.15f,6.19f);
            var screenX=Vector3.right*.475f;var screenY=new Vector3(0,.955f,.296f)*.335f;
            var screenDepth=new Vector3(0,-.296f,.955f)*.12f;
            var corners=new[]{screenCenter-screenX-screenY,screenCenter-screenX+screenY,screenCenter+screenX+screenY,screenCenter+screenX-screenY};
            for(int edge=0;edge<4;edge++){var a=corners[edge];var b=corners[(edge+1)%4];Quad(a,a+screenDepth,b+screenDepth,b,Ink,"CityMetal");}
            Quad(corners[3]+screenDepth,corners[2]+screenDepth,corners[1]+screenDepth,corners[0]+screenDepth,Ink,"CityMetal");
            ShopTiltPanel(new Vector3(5.8f,2.15f,6.19f),.95f,.67f,Ink);
            ShopTiltPanel(new Vector3(5.8f,2.15f,6.16f),.79f,.51f,new Color(.16f,.25f,.19f),"CityWindow");
            ShopTiltPanel(new Vector3(5.8f,2.33f,6.18f),.73f,.05f,Kaos);
            for(int row=0;row<3;row++)ShopTiltPanel(new Vector3(5.68f,2.21f-row*.10f,6.13f-row*.031f),.40f,.025f,Cream);
            ShopRoundedSolid(new Vector3(5.8f,1.68f,5.79f),new Vector3(.85f,.07f,.27f),.06f,Ink);
            for(int row=0;row<3;row++)for(int key=0;key<10;key++)Block(new Vector3(5.47f+key*.071f,1.725f,5.71f+row*.069f),new Vector3(.051f,.014f,.045f),row==2&&key==9?Kaos:Cream,"CityMetal",.004f);
            ShopRoundedSolid(new Vector3(6.60f,1.72f,6.08f),new Vector3(.44f,.19f,.38f),.08f,Ink);
            Rod(new Vector3(6.6f,1.79f,6.08f),new Vector3(6.52f,2.04f,6.02f),.048f,Ink,"CityMetal");
            ShopRoundedSolid(new Vector3(6.52f,2.05f,5.99f),new Vector3(.26f,.11f,.21f),.035f,Ink);
            Block(new Vector3(6.52f,2.04f,5.875f),new Vector3(.18f,.045f,.018f),Kaos,"CityGlow");
            ShopRoundedSolid(new Vector3(4.83f,1.80f,6.23f),new Vector3(.44f,.34f,.44f),.07f,Cream);
            Block(new Vector3(4.83f,1.83f,5.99f),new Vector3(.30f,.055f,.025f),Ink);
            Block(new Vector3(4.83f,1.72f,5.95f),new Vector3(.21f,.20f,.012f),Cream,"Paper");
            for(int line=0;line<4;line++)Block(new Vector3(4.83f,1.77f-line*.033f,5.94f),new Vector3(line%2==0?.14f:.10f,.01f,.008f),Ink,"Leather",.002f);
            // Controls face the employee aisle behind the counter.
            var pivot=new Vector3(5.8f,0,6.2f);var turn=Quaternion.Euler(0,180,0);
            foreach(var batch in batches.Values){
                int first;equipmentStart.TryGetValue(batch,out first);
                for(int i=first;i<batch.v.Count;i++){batch.v[i]=pivot+turn*(batch.v[i]-pivot);batch.n[i]=turn*batch.n[i];}
            }
            ShopPaperBag(new Vector3(3.35f,1.64f,6.10f),.75f);
            ShopPaperBag(new Vector3(2.42f,1.64f,6.31f),.57f);
        }
        void ShopTiltPanel(Vector3 center,float width,float height,Color color,string texture="CityMetal")
        {
            var x=Vector3.right*width*.5f;var y=new Vector3(0,.955f,.296f)*height*.5f;
            Quad(center-x-y,center-x+y,center+x+y,center+x-y,color,texture);
        }
        void ShopPaperBag(Vector3 bottom,float width)
        {
            float height=width*.9f,depth=width*.45f;
            var p=bottom+Vector3.up*height*.5f;
            ShopRoundedSolid(p,new Vector3(width,height,depth),.025f,Cream,"Paper");
            for(int side=-1;side<=1;side+=2){
                Rod(bottom+new Vector3(side*width*.41f,.03f,-depth*.53f),bottom+new Vector3(side*width*.41f,height,-depth*.53f),.009f,new Color(.53f,.43f,.32f),"Paper");
                ShopRing(bottom+new Vector3(0,height+.09f,side*depth*.28f),Vector3.right*width*.22f,Vector3.up*.15f,.014f,Ink);
            }
            Words("D",bottom+new Vector3(0,height*.49f,-depth*.52f),width*.045f,Ink);
        }
        void ShopDetailedTable()
        {
            var wood=new Color(.64f,.46f,.30f);
            ShopRoundedSolid(new Vector3(-1.6f,1.26f,.3f),new Vector3(3.25f,.18f,2.0f),.20f,wood,"Wood");
            ShopRoundedSolid(new Vector3(-1.6f,1.15f,.3f),new Vector3(3.04f,.075f,1.78f),.16f,Ink);
            for(int side=-1;side<=1;side+=2){
                float x=-1.6f+side*1.34f;
                for(int end=-1;end<=1;end+=2){
                    Rod(new Vector3(x,1.16f,.3f+end*.70f),new Vector3(x+side*.06f,.08f,.3f+end*.75f),.065f,Ink,"CityMetal");
                    ShopRoundedSolid(new Vector3(x+side*.06f,.07f,.3f+end*.75f),new Vector3(.26f,.10f,.23f),.05f,Ink);
                }
                Rod(new Vector3(x,.27f,-.4f),new Vector3(x,.27f,1),.035f,Ink,"CityMetal");
            }
            ShopRoundedSolid(new Vector3(-1.6f,.30f,.3f),new Vector3(2.95f,.09f,1.72f),.12f,wood,"Wood");
            for(int i=0;i<4;i++)ShopShoeBox(new Vector3(-2.62f+i*.67f,.36f,.25f),i%2==0?Kaos:Ink);
            for(int stack=0;stack<3;stack++)for(int layer=0;layer<3;layer++){
                var color=stack==0?Cream:stack==1?Kaos:new Color(.23f,.29f,.22f);
                var p=new Vector3(-2.65f+stack*1.02f,1.40f+layer*.10f,.25f);
                ShopRoundedSolid(p,new Vector3(.78f,.085f,.97f),.085f,color,"Cloth");
                // Folded sleeve edges and a real collar replace the flat black rectangle.
                Rod(p+new Vector3(-.32f,.04f,-.26f),p+new Vector3(-.32f,.04f,.29f),.012f,color*.75f,"Cloth");
                Rod(p+new Vector3(.32f,.04f,-.26f),p+new Vector3(.32f,.04f,.29f),.012f,color*.75f,"Cloth");
                if(layer==2){
                    ShopRing(p+new Vector3(0,.047f,.26f),Vector3.right*.10f,Vector3.forward*.085f,.021f,color*.55f,"Cloth");
                    Ground(p.x,p.z+.26f,.14f,.12f,p.y+.046f,Ink,"Cloth");
                    Block(p+new Vector3(.15f,.048f,-.06f),new Vector3(.11f,.008f,.075f),stack==1?Cream:Ink,"Cloth",.005f);
                }
            }
        }
        void ShopShoeBox(Vector3 bottom,Color color)
        {
            ShopRoundedSolid(bottom+Vector3.up*.17f,new Vector3(.60f,.34f,.75f),.025f,color,"Paper");
            ShopRoundedSolid(bottom+Vector3.up*.35f,new Vector3(.63f,.055f,.78f),.025f,color*.82f,"Paper");
            Block(bottom+new Vector3(.08f,.17f,-.381f),new Vector3(.28f,.12f,.01f),Cream,"Paper",.002f);
            for(int i=0;i<6;i++)Block(bottom+new Vector3(-.02f+i*.031f,.17f,-.39f),new Vector3(i%2==0?.014f:.007f,.065f,.008f),Ink,"Leather",.001f);
        }
        void ShopFixtureDetails()
        {
            for(int z=-2;z<=4;z+=3)for(int end=-1;end<=1;end+=2){
                float pz=z+end*1.1f;
                Rod(new Vector3(-7.2f,.10f,pz),new Vector3(-6.2f,.10f,pz),.045f,Ink,"CityMetal");
                Rod(new Vector3(-7.2f,.25f,pz),new Vector3(-6.55f,.10f,pz),.032f,Ink,"CityMetal");
                ShopRing(new Vector3(-6.22f,.07f,pz),Vector3.up*.06f,Vector3.forward*.06f,.025f,Ink);
                for(float y=.45f;y<3;y+=.40f)Block(new Vector3(-7.15f,y,pz),new Vector3(.012f,.025f,.035f),Cream);
            }
            for(int support=0;support<4;support++){
                float z=-2.3f+support*2.2f;
                Block(new Vector3(7.65f,1.65f,z),new Vector3(.08f,3.3f,.075f),new Color(.34f,.32f,.29f));
                for(int level=0;level<3;level++){
                    float y=.75f+level*1.05f;
                    Rod(new Vector3(7.65f,y-.36f,z),new Vector3(6.72f,y-.1f,z),.035f,Ink,"CityMetal");
                    Rod(new Vector3(6.72f,y-.1f,z),new Vector3(7.65f,y-.1f,z),.035f,Ink,"CityMetal");
                }
            }
            for(int level=0;level<3;level++){
                float y=.75f+level*1.05f;
                Block(new Vector3(6.59f,y-.03f,1.1f),new Vector3(.025f,.055f,7.7f),Kaos);
                for(int pair=0;pair<3;pair++){
                    float z=-1.5f+pair*2.6f;
                    Block(new Vector3(6.57f,y-.05f,z),new Vector3(.012f,.09f,.32f),Cream,"Paper",.002f);
                    for(int mark=0;mark<3;mark++)Block(new Vector3(6.555f,y-.05f,z-.09f+mark*.08f),new Vector3(.008f,.04f,.035f),Ink,"Leather",.001f);
                }
            }
            for(int side=-1;side<=1;side+=2){
                float x=4.8f+side*.92f;
                Rod(new Vector3(x,.55f,-3.55f),new Vector3(x+side*.10f,.08f,-3.64f),.055f,Ink,"CityMetal");
                Rod(new Vector3(x,.55f,-2.85f),new Vector3(x+side*.10f,.08f,-2.76f),.055f,Ink,"CityMetal");
            }
            for(int seat=0;seat<3;seat++){
                var p=new Vector3(4.0f+seat*.80f,.66f,-3.2f);
                ShopRoundedSolid(p,new Vector3(.76f,.24f,1),.16f,Kaos,"Cloth");
                Rod(p+new Vector3(-.25f,.09f,-.37f),p+new Vector3(.25f,.09f,-.37f),.009f,Kaos*.6f,"Cloth");
            }
        }
        void ShopFabricCurtain(float x)
        {
            const int columns=24,rows=12;var points=new Vector3[rows+1,columns+1];
            for(int row=0;row<=rows;row++)for(int col=0;col<=columns;col++){
                float u=col/(float)columns,v=row/(float)rows;
                points[row,col]=new Vector3(x+(u-.5f)*.80f,.12f+v*3.38f,5.15f+Mathf.Cos(u*Mathf.PI*12)*(.05f+(1-v)*.045f));
            }
            for(int row=0;row<rows;row++)for(int col=0;col<columns;col++)
                Quad(points[row,col],points[row+1,col],points[row+1,col+1],points[row,col+1],FittingPurple,"Cloth");
            for(int i=0;i<6;i++)ShopRing(new Vector3(x-.34f+i*.135f,3.58f,5.15f),Vector3.up*.095f,Vector3.forward*.095f,.014f,Cream);
            for(int col=0;col<columns;col++)Rod(points[0,col],points[0,col+1],.018f,FittingPurple*.7f,"Cloth");
        }
    }
}
