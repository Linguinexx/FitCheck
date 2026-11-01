using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        [field:SerializeField] public Material SkyMaterial {get;private set;}
        float atmosphereTime;Material bannerMaterial;
        void BuildAtmosphere()
        {
            SkyMaterial=new Material(Resources.Load<Shader>("CitySky")){name="Céu noturno — noite DripLab"};assets.Add(SkyMaterial);
        }
        void AnimateAtmosphere(float delta)
        {
            atmosphereTime+=delta;SkyMaterial.SetFloat("_CityTime",atmosphereTime);
            if(bannerMaterial!=null)bannerMaterial.SetFloat("_CityTime",atmosphereTime);
            AnimateNight(delta);
        }
        void CityIdentity()
        {
            // Offset packaging volumes: an unmistakable silhouette above the existing shop.
            Block(new Vector3(7.8f,9.6f,8.5f),new Vector3(16.8f,.65f,8.6f),Ink,"Rubber",.18f);
            Block(new Vector3(9.8f,11.2f,9.2f),new Vector3(11.4f,2.65f,6.6f),Cream,"Plaster",.23f);
            Block(new Vector3(9.8f,12.63f,9.2f),new Vector3(12.1f,.28f,7.1f),Kaos,"Leather",.1f);
            for(int i=0;i<5;i++)Window(5.4f+i*2.15f,11.1f,5.84f,1.35f,1.4f,Ink);
            // Overhanging cap-like terrace visor: curved mesh, stitched lip, open underneath.
            for(int i=0;i<24;i++){
                float a=i*Mathf.PI/24,b=(i+1)*Mathf.PI/24;
                var p=new Vector3(8+Mathf.Cos(a)*9.3f,14.2f+Mathf.Sin(a)*1.0f,9.5f-Mathf.Sin(a)*7.5f);
                var q=new Vector3(8+Mathf.Cos(b)*9.3f,14.2f+Mathf.Sin(b)*1.0f,9.5f-Mathf.Sin(b)*7.5f);
                Triangle(new Vector3(8,14.9f,10),q,p,Kaos,"Cloth");
                Triangle(new Vector3(8,14.82f,10),p-Vector3.up*.1f,q-Vector3.up*.1f,Ink,"Leather");
                Quad(p-Vector3.up*.22f,p,q,q-Vector3.up*.22f,Ink,"Leather");
                Rod(p+Vector3.up*.025f,Vector3.Lerp(p,q,.55f)+Vector3.up*.025f,.035f,Cream,"Leather",5);
            }
            foreach(float x in new[]{.4f,15.6f})Rod(new Vector3(x,9.7f,8),new Vector3(x,14.3f,8),.13f,Ink,"Leather");
            // A real hanging tag, with an eyelet and lacing, instead of a tiny generic board.
            Block(new Vector3(17.1f,8.7f,3.4f),new Vector3(2.65f,5.1f,.25f),Kaos,"Leather",.3f);
            Rod(new Vector3(17.1f,10.65f,3.20f),new Vector3(17.1f,10.65f,3.1f),.25f,Ink,"Leather",12);
            Rod(new Vector3(17.1f,10.65f,3.05f),new Vector3(16.3f,13.1f,6),.045f,Cream,"Cloth");
            Words("D",new Vector3(17.1f,8.95f,3.25f),.28f,Cream);
            Words("DROP",new Vector3(17.1f,7.6f,3.25f),.065f,Cream);
            for(int i=0;i<9;i++)Block(new Vector3(16.25f+i*.20f,6.65f,3.25f),new Vector3(i%3==0?.1f:.045f,.48f,.03f),Cream,"Leather",.001f);

            // Café has a split-level planted terrace, a curved red parapet and an inhabited display.
            Block(new Vector3(-19,9.85f,10),new Vector3(8.1f,1.3f,6),Ink,"Plaster",.15f);
            for(int i=0;i<24;i++){
                float a=i*Mathf.PI/24,b=(i+1)*Mathf.PI/24;
                var p=new Vector3(-17+Mathf.Cos(a)*6.3f,9.25f,9-Mathf.Sin(a)*4);
                var q=new Vector3(-17+Mathf.Cos(b)*6.3f,9.25f,9-Mathf.Sin(b)*4);
                Triangle(new Vector3(-17,9.25f,9),p,q,Cream);
                Quad(p,p+Vector3.up*.7f,q+Vector3.up*.7f,q,Kaos,"Plaster");
                Rod(p+Vector3.up*.72f,q+Vector3.up*.72f,.07f,Cream,"Leather",6);
            }
            for(int i=0;i<5;i++){
                float x=-22+i*2.5f;
                Block(new Vector3(x,9.65f,6.5f),new Vector3(1.7f,.6f,1.0f),Cream);
                Foliage(new Vector3(x,10.25f,6.5f),new Vector3(.9f,.6f,.65f),Teal,i);
            }
            for(int i=0;i<3;i++){
                float x=-19.5f+i*2.7f;
                Block(new Vector3(x,1.8f,5.24f),new Vector3(2.1f,2.6f,.06f),new Color(.57f,.43f,.29f),"Wood");
                Block(new Vector3(x,1.1f,4.94f),new Vector3(2.2f,.18f,.5f),Cream);
                Rod(new Vector3(x,2.95f,4.95f),new Vector3(x,3.35f,4.95f),.025f,Ink,"Leather");
                Rod(new Vector3(x,2.9f,4.95f),new Vector3(x,2.8f,4.95f),.22f,Kaos,"Leather",12);
                for(int j=0;j<3;j++)Rod(new Vector3(x-.6f+j*.55f,1.2f,4.8f),new Vector3(x-.6f+j*.55f,1.47f,4.8f),.11f,Cream,"Plaster");
            }
            // Studio on the right: tall sawtooth silhouette and oversized sound-wave facade.
            Block(new Vector3(23.1f,12.0f,10),new Vector3(6,5.7f,6),Ink,"Plaster",.16f);
            for(int i=0;i<7;i++)Block(new Vector3(20.5f+i*.8f,12.3f+Mathf.Sin(i*.8f)*1.2f,6.9f),new Vector3(.22f,3.5f,.3f),i%2==0?Kaos:Cream,"Leather");
            for(int i=0;i<3;i++){
                float x=20+i*2.0f;
                Triangle(new Vector3(x,14.85f,7),new Vector3(x+1,16.9f,10),new Vector3(x+2,14.85f,7),Kaos,"Leather");
            }
            // Neighbourhood pennants: soft continuous wind, black/white/red identity.
            for(int i=0;i<18;i++){
                float x=-23+i*2.7f,y=10.7f-Mathf.Sin(i/17f*Mathf.PI)*2;
                Rod(new Vector3(x,y,2),new Vector3(x+2.7f,y-.1f,2),.012f,Ink,"Leather",5);
                Quad(new Vector3(x,y-1.1f,2),new Vector3(x,y,2),new Vector3(x+1.5f,y,2),new Vector3(x+1.5f,y-1.1f,2),i%3==0?Kaos:i%3==1?Cream:Ink,"CityBanners");
            }
            BackgroundDistrict();Monument();
        }
        void Monument()
        {
            // Original distant landmark: a woven ribbon spirals into a tilted open halo.
            Foliage(new Vector3(-37,-1,67),new Vector3(31,12,19),new Color(.17f,.29f,.29f),7);
            Block(new Vector3(-37,11,67),new Vector3(9,3,8),Ink,"Plaster",.4f);
            for(int i=0;i<26;i++){
                float a=i/26f,b=(i+1)/26f;
                var p=new Vector3(-37+Mathf.Sin(a*5)*2.8f,12+a*21,67);
                var q=new Vector3(-37+Mathf.Sin(b*5)*2.8f,12+b*21,67);
                Quad(p-Vector3.right*1.4f,q-Vector3.right*1.4f,q+Vector3.right*1.4f,p+Vector3.right*1.4f,Cream,"Plaster");
                Quad(p+Vector3.right*1.4f,q+Vector3.right*1.4f,q-Vector3.right*1.4f,p-Vector3.right*1.4f,Kaos,"Leather");
            }
            for(int i=0;i<32;i++){
                float a=i*Mathf.PI*2/32,b=(i+1)*Mathf.PI*2/32;
                Rod(new Vector3(-39.5f+Mathf.Cos(a)*6,33+Mathf.Sin(a)*6,67+Mathf.Cos(a)*1.4f),new Vector3(-39.5f+Mathf.Cos(b)*6,33+Mathf.Sin(b)*6,67+Mathf.Cos(b)*1.4f),.65f,Kaos,"Plaster",6);
            }
        }
    }
}
