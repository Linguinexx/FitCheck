using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        void Words(string text,Vector3 center,float pixel,Color color,string texture="Leather")
        {PrintedWords(text.ToUpperInvariant(),center,pixel,color,texture=="CityGlow");}
        void Window(float x,float y,float z,float width,float height,Color frame,bool shutters=false)
        {
            Block(new Vector3(x,y,z),new Vector3(width+.22f,height+.22f,.22f),frame,"Wood");
            Block(new Vector3(x,y,z-.14f),new Vector3(width,height,.06f),new Color(.75f,.49f,.27f),"CityWindow",.015f);
            Block(new Vector3(x,y,z-.20f),new Vector3(.055f,height,.05f),frame,"Wood",.005f);
            Block(new Vector3(x,y,z-.20f),new Vector3(width,.055f,.05f),frame,"Wood",.005f);
            Block(new Vector3(x,y-height*.5f-.17f,z-.22f),new Vector3(width+.45f,.18f,.45f),Cream);
            if(shutters)for(int side=-1;side<=1;side+=2){
                Block(new Vector3(x+side*(width*.5f+.36f),y,z-.10f),new Vector3(.55f,height,.16f),Teal,"Wood");
                for(int i=0;i<7;i++)Block(new Vector3(x+side*(width*.5f+.36f),y-height*.38f+i*height*.125f,z-.20f),new Vector3(.46f,.035f,.06f),Teal*.72f,"Wood",.005f);
            }
        }
        void Awning(float x,float z,float y,float width,Color color)
        {
            var profile=new[]{new Vector2(z,y+.35f),new Vector2(z-.55f,y+.32f),new Vector2(z-1.15f,y+.12f),new Vector2(z-1.5f,y-.15f)};
            int stripes=Mathf.CeilToInt(width/.6f);
            for(int i=0;i<stripes;i++){
                float x0=x-width*.5f+width*i/stripes,x1=x-width*.5f+width*(i+1)/stripes;Color shade=i%2==0?color:color*.83f;
                for(int j=0;j<profile.Length-1;j++){
                    var a=profile[j];var b=profile[j+1];Quad(new Vector3(x0,a.y,a.x),new Vector3(x1,a.y,a.x),new Vector3(x1,b.y,b.x),new Vector3(x0,b.y,b.x),shade,"Cloth");
                }
                var front=profile[profile.Length-1];Quad(new Vector3(x0,front.y-.27f,front.x),new Vector3(x0,front.y,front.x),new Vector3(x1,front.y,front.x),new Vector3(x1,front.y-.27f,front.x),shade,"Cloth");
            }
            for(int side=-1;side<=1;side+=2)Rod(new Vector3(x+side*width*.45f,y-.3f,z+.2f),new Vector3(x+side*width*.45f,y-.18f,z-1.45f),.035f,Ink,"Leather");
        }
        void Tiles(float x,float y,float z,int columns,int rows,float size,Color color)
        {
            for(int row=0;row<rows;row++)for(int col=0;col<columns;col++){
                float px=x+(col-(columns-1)*.5f)*size,py=y+row*size;
                Block(new Vector3(px,py,z),new Vector3(size*.94f,size*.94f,.035f),Cream,"Plaster",.008f);
                float h=size*.30f;Quad(new Vector3(px,py-h,z-.025f),new Vector3(px-h,py,z-.025f),new Vector3(px,py+h,z-.025f),new Vector3(px+h,py,z-.025f),(row+col)%2==0?color:color*.75f,"Leather");
            }
        }
        void ShoeDisplay(Vector3 p)
        {
            Block(p+new Vector3(0,-.22f,0),new Vector3(2.65f,.23f,.92f),Cream,"Plaster");
            Block(p+new Vector3(0,-.035f,0),new Vector3(2.25f,.18f,.78f),Cream,"Rubber",.12f);
            Block(p+new Vector3(.67f,.18f,0),new Vector3(.95f,.34f,.73f),Kaos,"Leather",.15f);
            Block(p+new Vector3(-.20f,.27f,0),new Vector3(1.15f,.51f,.68f),Kaos,"Cloth",.12f);
            Block(p+new Vector3(-.83f,.29f,0),new Vector3(.52f,.56f,.65f),Ink,"Leather",.10f);
            Block(p+new Vector3(-.60f,.57f,0),new Vector3(.47f,.045f,.43f),Ink,"Rubber",.06f);
            Block(p+new Vector3(-.13f,.51f,0),new Vector3(.55f,.10f,.40f),Cream,"Cloth",.04f);
            foreach(float side in new[]{-1f,1f}){
                float z=side*.355f;
                Quad(p+new Vector3(-.55f,.12f,z),p+new Vector3(-.43f,.40f,z),p+new Vector3(.28f,.28f,z),p+new Vector3(.50f,.13f,z),Cream,"Leather");
                Quad(p+new Vector3(.50f,.13f,z),p+new Vector3(.28f,.28f,z),p+new Vector3(-.43f,.40f,z),p+new Vector3(-.55f,.12f,z),Cream,"Leather");
            }
            for(int i=0;i<4;i++){
                float x=-.28f+i*.15f,y=.59f-i*.045f;
                Rod(p+new Vector3(x,y,-.22f),p+new Vector3(x+.12f,y-.025f,.22f),.022f,Ink,"Cloth",6);
                Rod(p+new Vector3(x,y,.22f),p+new Vector3(x+.12f,y-.025f,-.22f),.022f,Ink,"Cloth",6);
            }
            for(int i=0;i<7;i++)Block(p+new Vector3(-.93f+i*.30f,-.02f,-.397f),new Vector3(.02f,.065f,.02f),Teal,"Rubber",.002f);
        }
        void DripLabFacade()
        {
            Block(new Vector3(7.5f,2.35f,8.0f),new Vector3(16.3f,4.7f,8.6f),Cream,"Plaster",.14f);
            Block(new Vector3(8.3f,7.1f,8.4f),new Vector3(14.4f,3.4f,7.7f),Ink,"Plaster",.15f);
            Block(new Vector3(8.3f,8.92f,8.4f),new Vector3(15,.25f,8.2f),Cream,"Plaster",.08f);
            Block(new Vector3(7.5f,5.05f,4.15f),new Vector3(17.3f,1.35f,1.0f),Ink,"Plaster",.12f);
            Tiles(7.5f,.35f,3.67f,32,2,.50f,Teal);
            foreach(float x in new[]{3.1f,7.0f,13.4f}){
                Block(new Vector3(x,2.55f,3.52f),new Vector3(3.25f,3.15f,.10f),new Color(.14f,.12f,.11f),"Leather");
                for(int side=-1;side<=1;side+=2)Block(new Vector3(x+side*1.66f,2.55f,3.36f),new Vector3(.14f,3.4f,.28f),Teal,"Wood");
                Block(new Vector3(x,.98f,3.30f),new Vector3(3.5f,.16f,.45f),Teal);
                Block(new Vector3(x,4.13f,3.36f),new Vector3(3.5f,.16f,.28f),Teal);
            }
            ShoeDisplay(new Vector3(3.1f,1.6f,3.09f));
            Block(new Vector3(7,2.35f,3.18f),new Vector3(1.15f,1.35f,.15f),new Color(.77f,.73f,.61f),"Cloth");
            for(int side=-1;side<=1;side+=2)Block(new Vector3(7+side*.73f,2.78f,3.18f),new Vector3(.48f,.55f,.17f),new Color(.77f,.73f,.61f),"Cloth");
            Words("DROP 01",new Vector3(13.4f,2.4f,3.35f),.065f,Cream);
            Block(new Vector3(10.4f,2.0f,3.47f),new Vector3(2.05f,3.95f,.18f),Ink,"Leather");
            Block(new Vector3(10.4f,2.55f,3.35f),new Vector3(1.72f,2.48f,.06f),new Color(.21f,.17f,.13f),"Leather");
            Block(new Vector3(11.02f,1.83f,3.24f),new Vector3(.07f,.65f,.12f),Cream,"Leather");
            Ground(10.4f,2.5f,2.6f,1.2f,.025f,Ink,"Rubber");
            Awning(7.5f,3.5f,4.05f,16.5f,Kaos);
            for(int i=0;i<3;i++)Window(3.5f+i*4.5f,7.15f,4.5f,2.35f,1.65f,Cream);
            Block(new Vector3(15.9f,3.15f,2.5f),new Vector3(1.05f,2.35f,.18f),new Color(.78f,.61f,.30f),"Cloth",.13f);
            Words("D",new Vector3(15.9f,3.2f,2.39f),.12f,Ink);
            Rod(new Vector3(15.9f,4.45f,2.5f),new Vector3(15.9f,5.4f,3.4f),.035f,Ink,"Leather");
            // Laje details break the empty roof silhouette at the isometric angle.
            Block(new Vector3(5.0f,9.66f,10.3f),new Vector3(2.5f,1.35f,2.4f),Teal);
            Block(new Vector3(5.0f,10.38f,10.3f),new Vector3(2.8f,.15f,2.7f),Cream);
            Block(new Vector3(5.0f,9.5f,9.04f),new Vector3(.85f,1.02f,.08f),Ink,"Wood");
            Rod(new Vector3(12.6f,9.15f,9.4f),new Vector3(12.6f,10.45f,9.4f),.66f,new Color(.23f,.22f,.20f),"Plaster",12);
            Rod(new Vector3(12.6f,10.45f,9.4f),new Vector3(12.6f,10.58f,9.4f),.72f,Ink,"Leather",12);
            for(int i=0;i<7;i++)Rod(new Vector3(1.6f+i*2.2f,9.10f,5.0f),new Vector3(1.6f+i*2.2f,9.62f,5.0f),.025f,Teal,"Leather",6);
            Rod(new Vector3(1.6f,9.62f,5.0f),new Vector3(14.8f,9.62f,5.0f),.03f,Teal,"Leather",6);
            obstacles.Add(new Rect(-.8f,3.3f,16.8f,9.5f));
        }
        void Neighbours()
        {
            Block(new Vector3(-16.8f,4.45f,9.4f),new Vector3(13.6f,8.9f,7.7f),new Color(.63f,.55f,.43f),"Plaster",.12f);
            Block(new Vector3(-16.8f,9.05f,9.4f),new Vector3(14.1f,.27f,8.1f),Cream);
            Tiles(-16.8f,.38f,5.51f,26,3,.5f,Teal);
            for(int i=0;i<3;i++)Window(-21.2f+i*4.4f,6.8f,5.42f,1.95f,2.15f,Cream,true);
            Block(new Vector3(-16.8f,2.05f,5.4f),new Vector3(8.4f,3.9f,.2f),Ink,"Leather");
            Awning(-16.8f,5.2f,4.0f,11.2f,new Color(.70f,.37f,.25f));
            Block(new Vector3(-16.8f,3.76f,3.56f),new Vector3(10.6f,.55f,.15f),Cream);
            Words("CAFE DO LADO",new Vector3(-16.8f,3.72f,3.46f),.070f,Teal);
            obstacles.Add(new Rect(-23.8f,5,14,8.5f));
            Block(new Vector3(22.1f,4.45f,9.5f),new Vector3(7.6f,8.9f,8.0f),new Color(.40f,.36f,.32f),"Plaster",.12f);
            Block(new Vector3(22.1f,9.05f,9.5f),new Vector3(8.1f,.25f,8.5f),Cream);
            Window(22.0f,6.8f,5.44f,2.05f,2.15f,Cream,true);
            Block(new Vector3(21.8f,2.0f,5.4f),new Vector3(2.25f,3.8f,.14f),Teal,"Wood");
            for(int i=0;i<9;i++)Block(new Vector3(21.8f,1.0f+i*.3f,5.28f),new Vector3(2.0f,.035f,.08f),Teal*.8f,"Wood",.005f);
            obstacles.Add(new Rect(18.2f,5,8,8.6f));
            // The former apartment gap is now a pocket courtyard, without a home door.
            Block(new Vector3(-4.8f,2,11.8f),new Vector3(7.2f,4,.3f),Cream,"CityBrick");
            obstacles.Add(new Rect(-8.5f,11.5f,7.4f,.6f));
            Bench(-4.8f,9.6f);Palm(-7.8f,7.6f,4);
        }
        void ResidentialEntrance()
        {
            var warm=new Color(.74f,.66f,.52f);
            // Ground floor is built around a genuine doorway, rather than painting a door on a solid block.
            foreach(float x in new[]{-7.1f,-2.5f})Block(new Vector3(x,2,8.95f),new Vector3(2.55f,4,.34f),warm,"CityBrick");
            foreach(float x in new[]{-8.35f,-1.25f})Block(new Vector3(x,2,11.2f),new Vector3(.22f,4,4.8f),warm,"Plaster");
            Block(new Vector3(-4.8f,2,10.85f),new Vector3(2.2f,4,.20f),Ink,"Plaster");
            Ground(-4.8f,11.15f,7.1f,4.65f,-.025f,Cream,"EntryTile");
            ResidentialFinish();
            Ground(-4.8f,8.2f,2.25f,1.2f,-.015f,Ink,"Rubber");
            Block(new Vector3(-4.8f,6.3f,11.2f),new Vector3(7.3f,4.6f,4.8f),warm,"Plaster",.09f);
            Block(new Vector3(-4.8f,8.7f,11.2f),new Vector3(7.7f,.24f,5.1f),Cream,"Plaster");
            foreach(float x in new[]{-7.1f,-2.5f})Window(x,6.3f,8.68f,1.6f,2.05f,Ink,true);
            foreach(float x in new[]{-5.86f,-3.74f})Block(new Vector3(x,1.95f,8.69f),new Vector3(.18f,3.9f,.38f),Ink,"Wood");
            Block(new Vector3(-4.8f,4.03f,8.66f),new Vector3(2.55f,.22f,.55f),Ink,"Wood");
            Block(new Vector3(-4.8f,4.40f,8.72f),new Vector3(1.75f,.52f,.08f),Cream,"Paper");
            Words("AP 03",new Vector3(-4.8f,4.39f,8.65f),.052f,Ink);
            Block(new Vector3(-4.8f,3.82f,9.7f),new Vector3(.65f,.05f,.45f),new Color(1,.8f,.48f),"CityGlow");
            // Open leaf and small lobby details remain visible when leaving the apartment.
            var hinge=new Vector3(-5.74f,.04f,8.80f);var leaf=hinge+new Vector3(.22f,0,1.55f);
            Quad(hinge,hinge+Vector3.up*3.65f,leaf+Vector3.up*3.65f,leaf,new Color(.44f,.26f,.14f),"Wood");
            Quad(leaf,leaf+Vector3.up*3.65f,hinge+Vector3.up*3.65f,hinge,new Color(.44f,.26f,.14f),"Wood");
            Rod(leaf+new Vector3(.04f,1.6f,-.10f),leaf+new Vector3(.04f,2.0f,-.10f),.035f,Cream,"CityMetal");
            for(int i=0;i<3;i++)Block(new Vector3(-4.8f+(i-1)*.53f,1.6f,10.70f),new Vector3(.45f,.55f,.10f),Cream,"CityMetal");
            obstacles.Add(new Rect(-8.5f,8.7f,2.65f,5.4f));obstacles.Add(new Rect(-3.75f,8.7f,2.65f,5.4f));obstacles.Add(new Rect(-5.85f,10.6f,2.1f,3.5f));
        }
    }
}
