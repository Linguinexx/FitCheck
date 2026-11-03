using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        // Approved shop: a 4.7-unit shopfront and a door almost 4 units high.
        // Residents are about 3.2 units tall. All residential modules share this scale.
        const float DistrictGroundFloor=4.7f,DistrictUpperFloor=4.1f;
        void BackgroundDistrict()
        {
            Ground(0,24.2f,210,7,-.04f,Color.white,"CityAsphalt",4);
            Ground(0,29.4f,210,3.5f,-.035f,Cream,"CityStone",4.7f);
            for(int i=0;i<13;i++){
                float x=-84+i*14,z=36+(i%3)*9;
                var color=i%3==0?new Color(.38f,.34f,.30f):i%3==1?new Color(.58f,.39f,.33f):new Color(.65f,.59f,.50f);
                Residence(x,z,3+i%3,12.4f+(i%2)*.8f,10.5f,color,0,i);
            }
            for(int side=1;side<=1;side+=2){
                Ground(side*31.2f,1,4.4f,83,-.035f,Cream,"CityStone",4.7f);
                for(int i=0;i<6;i++)Residence(side*(38+i%2*6),-29+i*11,2+i%2,12.8f,10.5f,i%2==0?Teal:Peach,-side,i+7);
            }
        }
        void Residence(float x,float z,int floors,float width,float depth,Color color,int sideFacing,int seed)
        {
            float height=DistrictGroundFloor+(floors-1)*DistrictUpperFloor,front=z-depth*.5f;
            Block(new Vector3(x,height*.5f,z),new Vector3(width,height,depth),color,"Plaster",.17f);
            Block(new Vector3(x,.20f,z),new Vector3(width+.12f,.4f,depth+.12f),Ink,"Plaster",.04f);
            Block(new Vector3(x,height+.16f,z),new Vector3(width+.6f,.32f,depth+.6f),Cream);
            for(int floor=1;floor<floors;floor++){
                float y=DistrictGroundFloor+(floor-1)*DistrictUpperFloor;
                Block(new Vector3(x,y,front-.08f),new Vector3(width,.15f,.24f),Cream,"Plaster",.02f);
                if(sideFacing!=0)Block(new Vector3(x+sideFacing*(width*.5f+.08f),y,z),new Vector3(.24f,.15f,depth),Cream,"Plaster",.02f);
            }
            for(int floor=0;floor<floors;floor++){
                float y=floor==0?2.30f:DistrictGroundFloor+(floor-1)*DistrictUpperFloor+DistrictUpperFloor*.5f;
                for(int bay=0;bay<3;bay++){
                    float px=x+(bay-1)*3.8f;
                    if(floor==0&&bay==1)ResidenceDoor(px,front-.15f);
                    else{
                        Window(px,y,front-.08f,2.05f,2.1f,Cream);
                        if(floor>0&&(bay+seed)%3==0)FrontBalcony(px,y-1.23f,front-.42f);
                    }
                    if(sideFacing!=0){
                        float face=x+sideFacing*(width*.5f+.04f),pz=z+(bay-1)*3.4f;
                        SideResidenceWindow(face,y,pz,2.05f,2.1f,sideFacing,(floor+bay+seed)%4==0);
                    }
                }
            }
            // Compact roof access and tank use human-scale dimensions as well.
            Block(new Vector3(x+.8f,height+1.90f,z+1),new Vector3(3.4f,3.8f,3.4f),color*.85f);
            Block(new Vector3(x+.8f,height+3.88f,z+1),new Vector3(3.65f,.16f,3.65f),Cream);
            if(seed%3==0){
                Rod(new Vector3(x-3.1f,height+.4f,z+1),new Vector3(x-3.1f,height+2.1f,z+1),.85f,Teal,"Plaster",12);
                Rod(new Vector3(x-3.1f,height+2.1f,z+1),new Vector3(x-3.1f,height+2.24f,z+1),.92f,Ink,"Leather",12);
            }
        }
        void ResidenceDoor(float x,float z)
        {
            Block(new Vector3(x,1.98f,z),new Vector3(2.2f,3.96f,.22f),Cream,"Wood");
            Block(new Vector3(x,1.90f,z-.13f),new Vector3(1.86f,3.70f,.08f),Ink,"CityMetal");
            Block(new Vector3(x,2.62f,z-.18f),new Vector3(1.5f,1.42f,.035f),new Color(.34f,.28f,.20f),"CityWindow");
            Block(new Vector3(x+.62f,1.72f,z-.23f),new Vector3(.065f,.45f,.07f),Cream,"Leather");
        }
        void SideResidenceWindow(float x,float y,float z,float width,float height,int facing,bool lit)
        {
            Block(new Vector3(x,y,z),new Vector3(.18f,height+.25f,width+.25f),Cream,"Wood");
            float face=x+facing*.11f;
            var a=new Vector3(face,y-height*.5f,z-width*.5f);var b=new Vector3(face,y+height*.5f,z-width*.5f);
            var c=new Vector3(face,y+height*.5f,z+width*.5f);var d=new Vector3(face,y-height*.5f,z+width*.5f);
            Color glass=lit?new Color(.83f,.58f,.30f):new Color(.16f,.14f,.12f);
            if(facing>0)Quad(a,b,c,d,glass,"CityWindow");else Quad(d,c,b,a,glass,"CityWindow");
            Block(new Vector3(face,y,z),new Vector3(.05f,.07f,width),Cream,"Wood",.01f);
            Block(new Vector3(face,y,z),new Vector3(.05f,height,.07f),Cream,"Wood",.01f);
            Block(new Vector3(x+facing*.18f,y-height*.5f-.13f,z),new Vector3(.5f,.16f,width+.4f),Cream);
        }
        void FrontBalcony(float x,float y,float z)
        {
            Block(new Vector3(x,y,z),new Vector3(2.7f,.18f,.75f),Cream);
            for(int post=0;post<5;post++)Rod(new Vector3(x-1.2f+post*.6f,y+.1f,z-.34f),new Vector3(x-1.2f+post*.6f,y+.8f,z-.34f),.022f,Ink,"Leather",5);
            Rod(new Vector3(x-1.2f,y+.8f,z-.34f),new Vector3(x+1.2f,y+.8f,z-.34f),.03f,Ink,"Leather",5);
        }
    }
}
