using System;
using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        public void BuildPhotoLocation(int index)
        {
            interior=true;transform.position=new Vector3(30,0,0);
            Ground(0,2,30,30,-.041f,Cream,"CityStone",4.7f);
            if(index==1||index==6)PlaceCityModule(()=>{StreetBuilding(-96.5f,17,2,new Color(.77f,.70f,.55f),3);GalleryFront();},new Vector3(96.5f,0,-2));
            else if(index==2)PlaceCityModule(NeighbourhoodCourt,new Vector3(76,0,-14));
            else PlaceCityModule(()=>{StreetBuilding(-49,20,2,new Color(.35f,.30f,.30f),4);MusicFront();},new Vector3(49,0,-2));
            GeometryBatches=batches.Count;FinishGeometry();
        }
        // Only static batched geometry and its matching navigation rectangles are moved.
        void PlaceCityModule(Action build,Vector3 offset,bool reverse=false)
        {
            var starts=new Dictionary<string,int>();foreach(var pair in batches)starts[pair.Key]=pair.Value.v.Count;
            int firstObstacle=obstacles.Count;build();
            foreach(var pair in batches){
                int first;starts.TryGetValue(pair.Key,out first);var b=pair.Value;
                for(int i=first;i<b.v.Count;i++){
                    var p=b.v[i];var n=b.n[i];if(reverse){p.x=-p.x;p.z=-p.z;n.x=-n.x;n.z=-n.z;}
                    b.v[i]=p+offset;b.n[i]=n;
                }
            }
            for(int i=firstObstacle;i<obstacles.Count;i++){
                var r=obstacles[i];obstacles[i]=new Rect((reverse?-r.xMax:r.xMin)+offset.x,(reverse?-r.yMax:r.yMin)+offset.z,r.width,r.height);
            }
        }
        void WestNeighbourhood()
        {
            // A continuous level promenade, not a longer invisible corridor or slower player.
            Ground(-84,1.8f,120,24,-.041f,Cream,"CityStone",4.7f);
            Ground(-86,-18.4f,126,4.1f,-.035f,Cream,"CityStone",4.7f);
            Ground(-86,10,150,140,-.22f,new Color(.25f,.29f,.22f),"Plaster",6);
            Block(new Vector3(-86,-.01f,-10.95f),new Vector3(122,.25f,.30f),Cream,"Plaster",.045f);
            for(int i=0;i<24;i++)Ground(-143+i*7.8f,-14.7f,2.4f,.10f,-.01f,new Color(.77f,.65f,.36f),"Leather");
            foreach(float x in new[]{-124.8f,-76,-36})for(int i=0;i<7;i++)Ground(x,-15.7f+i*.65f,3.6f,.35f,-.012f,Cream,"Leather");

            PlaceCityModule(ResidentialEntrance,new Vector3(-120,0,0));
            StreetBuilding(-137,12,2,new Color(.65f,.45f,.35f),1);
            StreetBuilding(-112,13,2,new Color(.58f,.56f,.44f),2);
            Shopfront(-112,12,"MERCEARIA",new Color(.47f,.31f,.23f),false);
            PlaceCityModule(()=>{StreetBuilding(-96.5f,17,2,new Color(.77f,.70f,.55f),3);GalleryFront();},new Vector3(0,0,30));
            PlaceCityModule(()=>{StreetBuilding(-49,20,2,new Color(.35f,.30f,.30f),4);MusicFront();},new Vector3(-98,0,-15),true);
            StreetBuilding(-31.5f,13,2,new Color(.67f,.52f,.42f),5);
            Shopfront(-31.5f,12,"OFICINA",Ink,false);
            PlaceCityModule(NeighbourhoodCourt,new Vector3(0,0,30));

            // South side and receding northern roofs give the street a complete silhouette.
            for(int i=0;i<6;i++){
                int seed=i;float x=-137+i*19;
                if(i<4)PlaceCityModule(()=>Residence(0,0,2+seed%2,16,10,seed%2==0?Peach:Teal,0,seed+30),new Vector3(x,0,-24),true);
                if(i!=2&&i!=3)PlaceCityModule(()=>Residence(0,0,3+seed%2,15,10,seed%2==0?Teal:Cream,0,seed+40),new Vector3(x,0,29+seed%2*3));
            }
            for(int i=0;i<7;i++){
                float x=-134+i*18;Lamp(x,-7.4f);
                // Drainage, curb seams and small utility boxes break up the long paving.
                Block(new Vector3(x+2,-.015f,-10.3f),new Vector3(.8f,.035f,.55f),Ink,"CityMetal",.015f);
                for(int j=0;j<5;j++)Block(new Vector3(x+1.7f+j*.15f,.005f,-10.3f),new Vector3(.045f,.02f,.45f),Teal,"CityMetal",.002f);
                if(i<6)StreetPlanter(x+8,-7.5f,i);
                if(i<6&&i%2==0){Bench(x+5,-4.8f);StreetPoster(x+3.5f,-6.4f,i);}
            }
            // Shaded community seating between the court and music venue.
            foreach(float x in new[]{-63.5f,-59.5f})Rod(new Vector3(x,0,2.8f),new Vector3(x,4.5f,2.8f),.075f,Ink,"CityMetal");
            Quad(new Vector3(-64.5f,4.6f,5.3f),new Vector3(-64.5f,4.3f,1.8f),new Vector3(-58.5f,4.3f,1.8f),new Vector3(-58.5f,4.6f,5.3f),Kaos,"Cloth");
            Bench(-61.5f,4.4f);
            obstacles.Add(new Rect(-63.8f,2.5f,.6f,.6f));obstacles.Add(new Rect(-59.8f,2.5f,.6f,.6f));

            // Diegetic route signs point both ways; no quest marker or shortcut menu.
            DirectionBoard(-120,-5.6f);DirectionBoard(-82,-5.6f);DirectionBoard(-28,-5.6f);
            Block(new Vector3(-144.6f,1.4f,2.5f),new Vector3(.4f,2.8f,24),Teal,"CityBrick");
            obstacles.Add(new Rect(-145, -9.5f,.7f,24));
            CityConnections();CloseCityEdges();
        }
        void StreetBuilding(float x,float width,int floors,Color color,int seed)
        {
            Residence(x,12,floors,width,12,color,0,seed);
            obstacles.Add(new Rect(x-width*.5f,5.8f,width,12.4f));
            // Street-level brick, worn pilasters and pipes keep the walls readable close up.
            foreach(float side in new[]{-1f,1f}){
                Block(new Vector3(x+side*(width*.5f-.2f),2.1f,5.78f),new Vector3(.35f,4.2f,.36f),color*.82f,"CityBrick");
                Rod(new Vector3(x+side*(width*.5f-.65f),.2f,5.6f),new Vector3(x+side*(width*.5f-.65f),8.5f,5.6f),.045f,Ink,"CityMetal",6);
            }
        }
        void Shopfront(float x,float width,string label,Color color,bool light)
        {
            Block(new Vector3(x,4.8f,5.64f),new Vector3(width,1.1f,.3f),color,"Wood");
            Words(label,new Vector3(x,4.8f,5.45f),label.Length>9?.09f:.13f,Cream,light?"CityGlow":"Leather");
            Awning(x,5.6f,3.7f,width,Kaos);
        }
        void GalleryFront()
        {
            Shopfront(-96.5f,15,"GALERIA",Ink,true);
            Block(new Vector3(-96.5f,2.05f,5.67f),new Vector3(14.3f,3.2f,.22f),Ink,"Wood");
            Block(new Vector3(-96.5f,.28f,5.59f),new Vector3(14.3f,.56f,.38f),Ink,"CityBrick");
            for(int i=0;i<3;i++){
                float x=-101+i*4.5f;
                Block(new Vector3(x,2.05f,5.46f),new Vector3(3.4f,2.5f,.15f),Cream,"Paper");
                Quad(new Vector3(x-1.4f,.98f,5.36f),new Vector3(x-.8f,3.1f,5.36f),new Vector3(x+1.4f,2.8f,5.36f),new Vector3(x+.6f,1.25f,5.36f),i==1?Kaos:Ink,"Cloth");
                Words(i==1?"LADO B":"HOJE",new Vector3(x,2.1f,5.32f),.07f,i==1?Cream:Kaos);
                Block(new Vector3(x,3.34f,5.26f),new Vector3(.8f,.08f,.30f),new Color(1,.79f,.46f),"CityGlow");
            }
            Ground(-96.5f,4.55f,15,1.9f,-.025f,Cream,"EntryTile");
            // Folded roof detail gives the gallery an identifiable silhouette in photos.
            for(int i=0;i<4;i++){
                float x=-104+i*4;
                Triangle(new Vector3(x,9,6),new Vector3(x+1.6f,11.3f,10),new Vector3(x+3.8f,9,6),i%2==0?Kaos:Ink,"CityMetal");
            }
        }
        void MusicFront()
        {
            Shopfront(-49,18,"AO VIVO",Ink,true);
            Block(new Vector3(-49,1.9f,5.65f),new Vector3(5,3.5f,.2f),Ink,"CityMetal");
            for(int side=-1;side<=1;side+=2){
                float x=-49+side*5.6f;
                Block(new Vector3(x,2.1f,5.56f),new Vector3(3.5f,3.3f,.12f),Cream,"Paper");
                Words("HOJE",new Vector3(x,2.8f,5.44f),.09f,Kaos);
                Words("SOM",new Vector3(x,1.7f,5.44f),.11f,Ink);
                for(int i=0;i<4;i++)Block(new Vector3(x-1+i*.7f,.9f+i%2*.18f,5.39f),new Vector3(.32f,.55f,.04f),Kaos,"Leather");
            }
            // Off-axis marquee, with two visible supporting brackets.
            Quad(new Vector3(-58.5f,5.4f,5.7f),new Vector3(-58.5f,5.6f,2.5f),new Vector3(-39.5f,5.6f,3.6f),new Vector3(-39.5f,5.4f,5.7f),Kaos,"CityMetal");
            foreach(float x in new[]{-57,-41})Rod(new Vector3(x,4.1f,5.65f),new Vector3(x,5.5f,3.5f),.09f,Ink,"CityMetal");
            for(int i=0;i<9;i++){
                float x=-57+i*2;
                Block(new Vector3(x,9.6f+Mathf.Sin(i*.8f)*.6f,6.1f),new Vector3(.25f,1.7f,.20f),i%2==0?Kaos:Cream,"CityMetal");
            }
        }
        void NeighbourhoodCourt()
        {
            Ground(-76,12,22,16,-.025f,new Color(.55f,.28f,.22f),"CityAsphalt",3);
            // Flat painted lines do not create collision ridges under the player.
            foreach(float x in new[]{-86.5f,-65.5f})Ground(x,12,.09f,15,-.012f,Cream,"Leather");
            foreach(float z in new[]{4.5f,19.5f})Ground(-76,z,21,.09f,-.012f,Cream,"Leather");
            foreach(float x in new[]{-80,-72})Ground(x,15.5f,.09f,8,-.011f,Cream,"Leather");
            Ground(-76,11.5f,8,.09f,-.011f,Cream,"Leather");
            for(int i=0;i<32;i++){
                float a=i*Mathf.PI*2/32,b=(i+1)*Mathf.PI*2/32;
                var p=new Vector3(-76+Mathf.Cos(a)*3.2f,-.009f,8+Mathf.Sin(a)*3.2f);
                var q=new Vector3(-76+Mathf.Cos(b)*3.2f,-.009f,8+Mathf.Sin(b)*3.2f);
                Quad(p,p+Vector3.right*.07f,q+Vector3.right*.07f,q,Cream,"Leather");
            }
            foreach(float x in new[]{-87.2f,-64.8f}){
                for(int i=0;i<6;i++)Rod(new Vector3(x,0,4+i*3.2f),new Vector3(x,3.6f,4+i*3.2f),.045f,Ink,"CityMetal",6);
                foreach(float y in new[]{.3f,1.6f,3.5f})Rod(new Vector3(x,y,4),new Vector3(x,y,20),.025f,Ink,"CityMetal",5);
                for(int i=0;i<16;i++)Rod(new Vector3(x,.3f,4+i),new Vector3(x,3.5f,Mathf.Min(20,6+i)),.012f,Teal,"CityMetal",4);
                obstacles.Add(new Rect(x-.18f,3.9f,.36f,16.3f));
            }
            Block(new Vector3(-76,1.35f,20.25f),new Vector3(22.7f,2.7f,.35f),Cream,"CityBrick");
            Words("QUADRA",new Vector3(-76,2,20.04f),.15f,Ink);
            CourtMural();
            obstacles.Add(new Rect(-87.5f,20,23,.5f));
            Rod(new Vector3(-76,0,18.7f),new Vector3(-76,6.7f,18.7f),.12f,Ink,"CityMetal");
            Block(new Vector3(-76,6.1f,18.1f),new Vector3(3.4f,2.1f,.18f),Cream,"Wood");
            Block(new Vector3(-76,5.8f,17.97f),new Vector3(1.5f,.8f,.04f),Kaos,"Leather");
            for(int i=0;i<16;i++){
                float a=i*Mathf.PI*2/16,b=(i+1)*Mathf.PI*2/16;
                Rod(new Vector3(-76+Mathf.Cos(a)*.65f,5.3f,17.35f+Mathf.Sin(a)*.65f),new Vector3(-76+Mathf.Cos(b)*.65f,5.3f,17.35f+Mathf.Sin(b)*.65f),.035f,Kaos,"CityMetal",5);
                if(i%2==0)Rod(new Vector3(-76+Mathf.Cos(a)*.65f,5.3f,17.35f+Mathf.Sin(a)*.65f),new Vector3(-76+Mathf.Cos(a)*.4f,4.6f,17.35f+Mathf.Sin(a)*.4f),.012f,Cream,"Leather",4);
            }
            obstacles.Add(new Rect(-76.2f,18.5f,.4f,.4f));
            Words("QUADRA",new Vector3(-80.5f,4.4f,4.1f),.09f,Cream);
            Block(new Vector3(-80.5f,4.4f,4.2f),new Vector3(5.3f,.85f,.16f),Ink,"CityMetal");
            foreach(float x in new[]{-83,-78}){Rod(new Vector3(x,0,4.4f),new Vector3(x,4.8f,4.4f),.055f,Ink,"CityMetal");obstacles.Add(new Rect(x-.2f,4.2f,.4f,.4f));}
        }
        void StreetPlanter(float x,float z,int seed)
        {
            Block(new Vector3(x,.38f,z),new Vector3(1.5f,.76f,1.5f),Peach,"CityBrick");
            Foliage(new Vector3(x,1.15f,z),new Vector3(.85f,.85f,.85f),new Color(.28f,.38f,.18f),seed);
            if(seed%2==0)Palm(x+3,z-.5f,seed);
            obstacles.Add(new Rect(x-.8f,z-.8f,1.6f,1.6f));
        }
        void StreetPoster(float x,float z,int seed)
        {
            Block(new Vector3(x,1.2f,z),new Vector3(1.5f,2.4f,.22f),Ink,"Wood");
            Block(new Vector3(x,1.25f,z-.14f),new Vector3(1.22f,1.95f,.045f),Cream,"Paper");
            Words(seed%3==0?"SOM":seed%3==1?"LADO B":"HOJE",new Vector3(x,1.6f,z-.18f),.038f,Kaos);
            Quad(new Vector3(x-.45f,.45f,z-.18f),new Vector3(x-.25f,1.25f,z-.18f),new Vector3(x+.4f,1.15f,z-.18f),new Vector3(x+.25f,.5f,z-.18f),Ink,"Paper");
            obstacles.Add(new Rect(x-.8f,z-.2f,1.6f,.4f));
        }
        void DirectionBoard(float x,float z)
        {
            Rod(new Vector3(x,0,z),new Vector3(x,4.3f,z),.065f,Ink,"CityMetal");
            foreach(float y in new[]{3.1f,3.9f})Block(new Vector3(x,y,z),new Vector3(3.9f,.65f,.14f),Ink,"CityMetal");
            Words("DRIPLAB",new Vector3(x-.2f,3.9f,z-.09f),.055f,Cream);
            Words("AP 03",new Vector3(x+.2f,3.1f,z-.09f),.055f,Cream);
            Triangle(new Vector3(x+1.7f,3.9f,z-.1f),new Vector3(x+1.3f,3.68f,z-.1f),new Vector3(x+1.3f,4.12f,z-.1f),Kaos,"Leather");
            Triangle(new Vector3(x-1.7f,3.1f,z-.1f),new Vector3(x-1.3f,3.32f,z-.1f),new Vector3(x-1.3f,2.88f,z-.1f),Kaos,"Leather");
            obstacles.Add(new Rect(x-.2f,z-.2f,.4f,.4f));
        }
    }
}
