using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        void StreetCulture()
        {
            // Deliberate clusters: a rhyme pocket, a record crate and one bicycle.
            Block(new Vector3(-97,.16f,20),new Vector3(5.2f,.32f,2.6f),Ink,"Wood");obstacles.Add(new Rect(-99.7f,18.6f,5.4f,2.8f));
            Block(new Vector3(-97,3.0f,21.7f),new Vector3(6.2f,1.0f,.17f),new Color(.43f,.10f,.72f),"CityMetal");
            Words("BATALHA DE RIMA",new Vector3(-97,3.0f,21.59f),.065f,Cream);
            foreach(float x in new[]{-99.8f,-94.2f}){
                Rod(new Vector3(x,0,21.8f),new Vector3(x,3.6f,21.8f),.06f,Ink,"CityMetal");
                Block(new Vector3(x,.75f,20),new Vector3(.65f,1.5f,.6f),Ink,"Rubber");
                Rod(new Vector3(x,.65f,19.67f),new Vector3(x,.65f,19.63f),.22f,Teal,"CityMetal",12);
            }
            Rod(new Vector3(-97,.32f,20),new Vector3(-97,2.5f,20),.035f,Ink,"CityMetal");Rod(new Vector3(-97,2.5f,20),new Vector3(-96.75f,2.58f,20),.09f,Ink,"Rubber");
            Block(new Vector3(-93,.36f,9.5f),new Vector3(1.7f,.72f,.75f),new Color(.47f,.24f,.10f),"Wood");
            for(int i=0;i<3;i++){
                float x=-93.5f+i*.5f;
                Rod(new Vector3(x,.87f,9.54f),new Vector3(x,.87f,9.45f),.36f,Ink,"Rubber",16);
                Rod(new Vector3(x,.87f,9.44f),new Vector3(x,.87f,9.42f),.105f,i==1?Kaos:new Color(.98f,.65f,.06f),"Paper",12);
            }
            Words("SO MAIS UM DROP",new Vector3(-93,.38f,9.10f),.023f,Cream);obstacles.Add(new Rect(-93.9f,9,1.8f,1));
            Block(new Vector3(-112,1.45f,5.34f),new Vector3(3.4f,.70f,.045f),new Color(.99f,.68f,.05f),"Paper");
            Words("FIADO SO AMANHA",new Vector3(-112,1.45f,5.30f),.037f,Ink);
            Words("FUBA DONO DA RUA",new Vector3(-4.8f,1.4f,11.60f),.036f,Cream);
            // Shoes hanging from a wire, in the scale of the pedestrians rather than the storefront display.
            Rod(new Vector3(-24,7,-1),new Vector3(-12,6.8f,-1),.014f,Ink,"CityMetal",5);
            for(int i=0;i<2;i++){
                float x=-20+i*.45f,y=5.9f-i*.22f;
                Rod(new Vector3(x,6.92f,-1),new Vector3(x,y+.2f,-1),.012f,Cream,"Cloth",5);
                Block(new Vector3(x,y,-1),new Vector3(.30f,.20f,.70f),i==0?new Color(.18f,.30f,.87f):Kaos,"Cloth",.07f);
                Block(new Vector3(x,y-.12f,-1),new Vector3(.33f,.065f,.75f),Cream,"Rubber",.035f);
            }
            Words("CHINELO NO FIO",new Vector3(-20,1.1f,5.28f),.031f,Cream);
            Bicycle(new Vector3(-61,0,15));
            // Farther stretches receive small printed posters, keeping the paving open.
            foreach(var p in new[]{new Vector3(-130,1.5f,5.8f),new Vector3(-33,1.5f,5.6f)}){
                Block(p,new Vector3(2,1.3f,.055f),new Color(.05f,.67f,.48f),"Paper");Words("RESPEITA O CORRE",p+Vector3.back*.04f,.023f,Ink);
            }
        }
        void CourtMural()
        {
            foreach(int side in new[]{-1,1}){
                float x=-76+side*7.2f;var c=side<0?new Color(.48f,.11f,.85f):new Color(.02f,.68f,.52f);
                Quad(new Vector3(x-2.8f,.25f,20.03f),new Vector3(x-2.4f,2.55f,20.03f),new Vector3(x+2.8f,2.3f,20.03f),new Vector3(x+2.5f,.3f,20.03f),c,"Paper");
                Words(side<0?"PASSINHO LIBERADO":"RESPEITA O CORRE",new Vector3(x,1.45f,19.99f),.037f,Cream);
            }
        }
        void Bicycle(Vector3 p)
        {
            foreach(float x in new[]{-1f,1f})for(int i=0;i<20;i++){
                float a=i*Mathf.PI*2/20,b=(i+1)*Mathf.PI*2/20;
                var u=p+new Vector3(x+Mathf.Cos(a)*.65f,.7f+Mathf.Sin(a)*.65f,0);var v=p+new Vector3(x+Mathf.Cos(b)*.65f,.7f+Mathf.Sin(b)*.65f,0);
                Rod(u,v,.035f,Ink,"Rubber",5);if(i%4==0)Rod(p+new Vector3(x,.7f,0),u,.009f,Cream,"CityMetal",4);
            }
            var a0=p+new Vector3(-1,.7f,0);var b0=p+new Vector3(-.55f,1.6f,0);var c0=p+new Vector3(.25f,.7f,0);var d0=p+new Vector3(.7f,1.6f,0);var e0=p+new Vector3(1,.7f,0);
            foreach(var pair in new[]{new[]{a0,b0},new[]{b0,c0},new[]{c0,a0},new[]{b0,d0},new[]{d0,c0},new[]{d0,e0}})Rod(pair[0],pair[1],.045f,Kaos,"CityMetal",6);
            Block(b0+Vector3.up*.15f,new Vector3(.4f,.10f,.25f),Ink,"Leather");Rod(d0,d0+new Vector3(0,.3f,0),.035f,Ink,"CityMetal");Rod(d0+new Vector3(0,.3f,-.32f),d0+new Vector3(0,.3f,.32f),.035f,Ink,"CityMetal");
            obstacles.Add(new Rect(p.x-1.8f,p.z-.4f,3.6f,.8f));
        }
    }
}
