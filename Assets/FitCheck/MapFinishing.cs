using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        void ResidentialFinish()
        {
            var wall=new Color(.66f,.55f,.42f);
            Block(new Vector3(-4.8f,2.02f,13.57f),new Vector3(7.3f,4.04f,.28f),wall,"Plaster");
            Block(new Vector3(-4.8f,3.98f,11.2f),new Vector3(7.3f,.16f,4.8f),Cream,"Plaster");
            Block(new Vector3(-4.8f,.16f,13.76f),new Vector3(7.35f,.32f,.12f),Ink,"CityBrick");
            obstacles.Add(new Rect(-8.5f,13.4f,7.4f,.4f));
            foreach(float x in new[]{-7f,-2.6f}){
                Block(new Vector3(x,2.25f,13.77f),new Vector3(1.45f,1.55f,.12f),Cream,"CityMetal");
                Block(new Vector3(x,2.25f,13.85f),new Vector3(1.25f,1.35f,.04f),new Color(.17f,.20f,.18f),"CityWindow");
                Block(new Vector3(x,2.25f,13.89f),new Vector3(.055f,1.4f,.06f),Cream,"CityMetal");
            }
            foreach(float x in new[]{-8.35f,-1.25f})Block(new Vector3(x,1.95f,11.2f),new Vector3(.28f,3.9f,4.8f),wall,"Plaster");
            BuildResidentialLobby();
            // The open leaf remains attached to its hinges inside a finished vestibule.
            var a=new Vector3(-5.74f,.04f,8.80f);var b=a+new Vector3(.22f,0,1.55f);
            Rod(a,a+Vector3.up*3.65f,.045f,Cream,"CityMetal");Rod(b,b+Vector3.up*3.65f,.045f,Cream,"CityMetal");
            Rod(a+Vector3.up*3.65f,b+Vector3.up*3.65f,.045f,Cream,"CityMetal");Rod(a,b,.045f,Cream,"CityMetal");
            foreach(float x in new[]{-7.1f,-2.5f}){Block(new Vector3(x,.35f,8.40f),new Vector3(1.1f,.7f,.55f),Ink,"CityBrick");Foliage(new Vector3(x,.95f,8.40f),new Vector3(.45f,.38f,.22f),new Color(.18f,.40f,.16f),4);obstacles.Add(new Rect(x-.6f,8.1f,1.2f,.7f));}
        }
        void BuildResidentialLobby()
        {
            var green=new Color(.20f,.43f,.37f);
            var plaster=new Color(.84f,.72f,.54f);
            var brass=new Color(.75f,.55f,.28f);
            const float hallX=-4.8f;
            // The return wall shares the boundary of the existing playable threshold.
            Block(new Vector3(hallX,1.98f,10.86f),new Vector3(6.90f,3.90f,.30f),plaster,"Plaster");
            Block(new Vector3(hallX,.59f,10.68f),new Vector3(6.90f,1.17f,.075f),green,"EntryTile");
            Block(new Vector3(hallX,1.18f,10.625f),new Vector3(6.90f,.09f,.075f),green*.77f,"CityMetal");
            Ground(hallX,9.78f,6.9f,2.04f,-.024f,Cream,"EntryTile");
            Ground(hallX,9.72f,1.76f,1.76f,-.017f,green,"EntryTile");
            Ground(hallX,9.72f,1.48f,1.48f,-.010f,new Color(.31f,.24f,.19f),"Rubber");
            Block(new Vector3(hallX,1.61f,10.60f),new Vector3(1.76f,3.20f,.17f),Ink,"Wood");
            Block(new Vector3(hallX,1.55f,10.495f),new Vector3(1.54f,3.02f,.055f),new Color(.37f,.22f,.13f),"Wood");
            foreach(float y in new[]{.70f,2.28f})
                Block(new Vector3(hallX,y,10.457f),new Vector3(1.20f,.94f,.034f),new Color(.30f,.18f,.105f),"Wood");
            Rod(new Vector3(-4.22f,1.38f,10.40f),new Vector3(-4.22f,1.70f,10.40f),.037f,brass,"CityMetal");
            Block(new Vector3(hallX,3.52f,10.655f),new Vector3(1.25f,.35f,.065f),green,"CityMetal");
            Words("AP 03",new Vector3(hallX,3.52f,10.606f),.035f,Cream);
            for(int i=0;i<3;i++){
                float x=-3.52f+i*.40f;
                Block(new Vector3(x,1.62f,10.59f),new Vector3(.35f,.40f,.17f),green*.81f,"CityMetal");
                Block(new Vector3(x,1.70f,10.495f),new Vector3(.22f,.028f,.02f),Ink,"CityMetal");
                Block(new Vector3(x,1.55f,10.495f),new Vector3(.11f,.065f,.02f),Cream,"Paper");
            }
            Block(new Vector3(-6.37f,1.93f,10.58f),new Vector3(.78f,1.02f,.13f),new Color(.41f,.27f,.15f),"Wood");
            Block(new Vector3(-6.37f,1.93f,10.50f),new Vector3(.67f,.91f,.035f),Cream,"Paper");
            for(int i=0;i<3;i++)
                Block(new Vector3(-6.37f,2.17f-i*.24f,10.473f),new Vector3(.39f,.14f,.018f),i==1?Peach:green,"Paper");
            Block(new Vector3(hallX,3.80f,9.76f),new Vector3(1.25f,.23f,.38f),Ink,"CityMetal");
            Block(new Vector3(hallX,3.67f,9.76f),new Vector3(1.12f,.047f,.28f),new Color(1,.77f,.43f),"CityGlow");
        }
        void RoadBoundary(float x)
        {
            foreach(var range in new[]{new Vector2(-44.5f,-17.4f),new Vector2(-10.2f,58.5f)})Block(new Vector3(x,3.2f,(range.x+range.y)*.5f),new Vector3(.7f,6.4f,range.y-range.x),Teal,"CityBrick");
            Block(new Vector3(x,5.8f,-13.8f),new Vector3(.75f,1.2f,7.2f),Teal,"CityBrick");
            float side=x>0?1:-1,entry=x-side*.42f,far=entry+side*19;
            // Bridge the offset between the arch face and the perimeter wall.
            foreach(float edge in new[]{-1f,1f}){
                Block(new Vector3(entry+side*.35f,3.2f,-13.8f+edge*3.72f),
                    new Vector3(.92f,6.4f,.84f),Teal,"CityBrick",0);
            }
            const int segments=14;
            for(int i=0;i<segments;i++){
                float a=i*Mathf.PI/segments,b=(i+1)*Mathf.PI/segments;
                var p=new Vector3(entry,3+Mathf.Sin(a)*1.8f,-13.8f+Mathf.Cos(a)*3.3f);var q=new Vector3(entry,3+Mathf.Sin(b)*1.8f,-13.8f+Mathf.Cos(b)*3.3f);
                var outerP=new Vector3(entry-.02f*side,3+Mathf.Sin(a)*2.15f,-13.8f+Mathf.Cos(a)*3.65f);var outerQ=new Vector3(entry-.02f*side,3+Mathf.Sin(b)*2.15f,-13.8f+Mathf.Cos(b)*3.65f);
                Quad(p,q,outerQ,outerP,Cream,"Plaster");Quad(outerP,outerQ,q,p,Cream,"Plaster");
                var topP=new Vector3(outerP.x,6.4f,outerP.z);var topQ=new Vector3(outerQ.x,6.4f,outerQ.z);
                Quad(outerP,outerQ,topQ,topP,Teal,"CityBrick");Quad(topP,topQ,outerQ,outerP,Teal,"CityBrick");
                var extension=Vector3.right*(far-entry);Quad(p,p+extension,q+extension,q,Ink,"Plaster");Quad(q,q+extension,p+extension,p,Ink,"Plaster");
            }
            foreach(float z in new[]{-17.1f,-10.5f}){
                Block(new Vector3((entry+far)*.5f,1.5f,z),new Vector3(19,3,.25f),Ink,"CityBrick");
                Block(new Vector3(entry,1.6f,z),new Vector3(.85f,3.2f,.40f),Cream,"Plaster");
                for(int i=0;i<5;i++)Block(new Vector3(entry-side*.46f,.45f+i*.48f,z),new Vector3(.03f,.24f,.28f),i%2==0?Ink:new Color(.95f,.64f,.05f),"CityMetal");
            }
            Block(new Vector3(far,2.8f,-13.8f),new Vector3(.3f,5.6f,6.7f),Color.black,"Rubber");
            Ground((entry+far)*.5f,-13.8f,19,6.6f,-.019f,Color.white,"CityAsphalt",4);
            foreach(float distance in new[]{2f,7f,12f})Block(new Vector3(entry+side*distance,4.45f,-13.8f),new Vector3(.6f,.06f,1.1f),new Color(.95f,.68f,.3f),"CityGlow");
        }
        void RootedTree(Vector3 p,float crownHeight,int seed)
        {
            Rod(p,p+Vector3.up*(crownHeight+.4f),.19f,new Color(.36f,.23f,.12f),"Wood");
            Foliage(p+Vector3.up*(crownHeight+1.5f),new Vector3(3.2f,2.4f,2.8f),new Color(.16f,.37f,.15f),seed);
            Foliage(p+new Vector3(1.5f,crownHeight+.9f,.4f),new Vector3(1.8f,1.7f,1.8f),new Color(.25f,.43f,.16f),seed+1);
        }
        void BuildNeighbourhoodCourtyards()
        {
            var teal=new Color(.16f,.51f,.43f);
            var brick=new Color(.65f,.30f,.19f);
            // Pocket seating and shutters along the service side behind DripLab.
            float rearZ=25;
            Block(new Vector3(23.65f,1.74f,rearZ),new Vector3(3.3f,3.48f,11),brick,"CityBrick");
            Block(new Vector3(23.34f,3.58f,rearZ),new Vector3(4.3f,.23f,11.6f),Ink,"CityMetal");
            for(int bay=-1;bay<=1;bay++){
                float z=rearZ+bay*3.25f;
                Block(new Vector3(21.94f,1.53f,z),new Vector3(.17f,2.65f,2.75f),bay==0?teal:Ink,"CityMetal");
                for(int slat=0;slat<8;slat++)
                    Block(new Vector3(21.83f,.45f+slat*.29f,z),new Vector3(.08f,.043f,2.63f),bay==0?teal*.65f:Teal,"CityMetal");
                Block(new Vector3(21.52f,3.38f,z),new Vector3(.52f,.055f,1.76f),new Color(1,.72f,.34f),"CityGlow");
            }
            obstacles.Add(new Rect(21.8f,19.45f,3.6f,11.1f));
            Bench(17.2f,22.1f);
            CourtyardPlanter(18.1f,29,brick,27);
            AddNightSource(new Vector3(20.8f,3.1f,25),new Color(1,.60f,.31f),10,.54f,.30f);

            // The open north corner becomes a small listening plaza.
            float plazaX=15,wallZ=55.3f;
            Block(new Vector3(plazaX,1.9f,wallZ),new Vector3(14,3.8f,.4f),teal,"CityBrick");
            Block(new Vector3(plazaX,3.84f,wallZ),new Vector3(14.5f,.2f,.63f),Cream,"Plaster");
            obstacles.Add(new Rect(8,55.05f,14,.55f));
            Words("SOM",new Vector3(plazaX,2.42f,wallZ-.23f),.23f,Cream);
            for(int i=-4;i<=4;i++){
                float height=.44f+(4-Mathf.Abs(i)%5)*.32f;
                float x=plazaX+i*.67f;
                Block(new Vector3(x,.43f+height*.5f,wallZ-.23f),new Vector3(.40f,height,.06f),i%2==0?Cream:Peach,"CityMetal");
            }
            Bench(13,50.1f);Bench(19,50.1f);
            CourtyardPlanter(9.5f,51.5f,brick,41);
            CourtyardPlanter(22.2f,52.4f,brick,42);
            Lamp(10.2f,46.5f);
            AddNightSource(new Vector3(plazaX,3.65f,54.7f),new Color(.28f,.85f,.66f),12,.61f,.38f);

            // Each standalone deck has its own cabinet and matching navigation footprint.
            for(int i=1;i<GarageDeckTargets.Length;i++){
                Vector3 deck=GarageDeckTargets[i];
                BuildGarageDeck(i);
                obstacles.Add(new Rect(deck.x-.59f,deck.z-.34f,1.18f,.68f));
                AddNightSource(deck+Vector3.up*.17f,i==1?new Color(.23f,.80f,.61f):new Color(1,.64f,.33f),4,.28f,.13f);
            }
            // Low amber markers light the public approach to the hill.
            foreach(float x in new[]{-42f,-32f}){
                var light=new Vector3(x,.42f,53.4f);
                Block(light,new Vector3(.48f,.84f,.48f),Ink,"CityMetal");
                Block(light+Vector3.up*.28f,new Vector3(.43f,.13f,.43f),new Color(1,.55f,.19f),"CityGlow");
                obstacles.Add(new Rect(x-.25f,53.15f,.5f,.5f));
                AddNightSource(light+Vector3.up*.28f,new Color(1,.55f,.19f),8,.52f,.18f);
            }
            BuildBacklotSkatePocket();
            BuildLajesCourtyard();
            Bench(-45.2f,51.7f);
            Block(new Vector3(-37,.73f,54.2f),new Vector3(3.1f,1.46f,.38f),Ink,"CityMetal");
            Words("ENCONTRO",new Vector3(-37,.88f,53.99f),.064f,Cream,"CityGlow");
            obstacles.Add(new Rect(-38.6f,54,3.2f,.4f));
        }
        void BuildBacklotSkatePocket()
        {
            const float x=5.8f,z=27.8f;
            var orange=new Color(.75f,.34f,.17f);
            var concrete=new Color(.61f,.56f,.49f);
            Ground(x,z+3.4f,13,14,-.029f,new Color(.39f,.38f,.37f),"ShopFloor",2.8f);
            Block(new Vector3(x,1.34f,z+9.95f),new Vector3(12,2.68f,.38f),Ink,"CityBrick");
            Words("PASSINHO LIBERADO",new Vector3(x,1.86f,z+9.71f),.070f,Cream);
            obstacles.Add(new Rect(x-6,z+9.72f,12,.45f));
            for(int side=-1;side<=1;side+=2){
                float px=x+side*3.75f;
                var lip=new Vector3(px,.98f,z+6.7f);
                var toe=new Vector3(px,.035f,z+3.75f);
                var half=Vector3.right*1.38f;
                Quad(toe-half,lip-half,lip+half,toe+half,concrete,"Plaster");
                Triangle(toe-half,lip-half-Vector3.up*.945f,lip-half,orange,"CityBrick");
                Triangle(toe+half,lip+half,lip+half-Vector3.up*.945f,orange,"CityBrick");
                Block(lip-Vector3.up*.48f+Vector3.forward*.17f,new Vector3(2.76f,.98f,.34f),orange,"CityBrick");
                Rod(lip-half,lip+half,.052f,Ink,"CityMetal");
                obstacles.Add(new Rect(px-1.38f,z+3.75f,2.76f,3.28f));
            }
            for(int i=0;i<2;i++){
                float px=x-1+i*2;
                Rod(new Vector3(px,0,z+2),new Vector3(px,.58f,z+2),.048f,Ink,"CityMetal");
            }
            Rod(new Vector3(x-1.4f,.58f,z+2),new Vector3(x+1.4f,.58f,z+2),.058f,orange,"CityMetal");
            obstacles.Add(new Rect(x-1.45f,z+1.9f,2.9f,.2f));
            Bench(x-3.7f,z+.35f);
            CourtyardPlanter(x+4.65f,z+.3f,orange,73);
            Lamp(x-5.8f,z+7.7f);
            AddNightSource(new Vector3(x+4.8f,2.7f,z+9.5f),new Color(.34f,.75f,.67f),10,.48f,.27f);
        }
        void BuildLajesCourtyard()
        {
            var brick=new Color(.64f,.34f,.24f);
            var blue=new Color(.20f,.43f,.56f);
            const float x=-130,z=45;
            // Residential seating and garden sit beyond the existing through-passage.
            CourtyardPlanter(x-7,z+2,brick,81);
            CourtyardPlanter(x+6,z+5,brick,82);
            Bench(x-4.5f,z+5.2f);
            Bench(x+3,z-.35f);
            Lamp(x-6,z-.7f);
            Block(new Vector3(x-8.1f,1.22f,z+7.9f),new Vector3(5.4f,2.44f,.38f),blue,"CityBrick");
            Words("FIADO SO AMANHA",new Vector3(x-8.1f,1.65f,z+7.66f),.042f,Cream);
            obstacles.Add(new Rect(x-10.85f,z+7.65f,5.5f,.5f));
            for(int bed=0;bed<2;bed++){
                float px=x-2.8f+bed*3.4f;
                Block(new Vector3(px,.31f,z+7.4f),new Vector3(2.75f,.62f,1.65f),brick,"CityBrick");
                Ground(px,z+7.4f,2.5f,1.4f,.624f,new Color(.27f,.19f,.12f),"Plaster");
                for(int i=-2;i<=2;i++)
                    Foliage(new Vector3(px+i*.43f,.90f,z+7.4f+(i%2)*.12f),new Vector3(.27f,.35f,.32f),i%2==0?LeafGreen:new Color(.30f,.49f,.15f),87+i);
                obstacles.Add(new Rect(px-1.4f,z+6.55f,2.8f,1.7f));
            }
            for(int i=0;i<3;i++){
                float px=x-4.4f+i*3.15f;
                var a=new Vector3(px,4.25f,z+9.7f);
                var b=a+Vector3.right*3.15f;
                Rod(a,b,.017f,Ink,"CityMetal",5);
                var tint=i==0?Cream:i==1?blue:new Color(.74f,.25f,.26f);
                Vector3 p=a+Vector3.right*.55f,q=p+Vector3.right*1.2f;
                Vector3 drop=new Vector3(.1f,-.98f,.15f);
                Quad(p+drop,p,q,q+drop,tint,"Cloth");
                Quad(q+drop,q,p,p+drop,tint,"Cloth");
            }
            foreach(float px in new[]{x-4.4f,x+5.05f}){
                Rod(new Vector3(px,0,z+9.7f),new Vector3(px,4.38f,z+9.7f),.065f,Ink,"CityMetal");
                obstacles.Add(new Rect(px-.12f,z+9.58f,.24f,.24f));
            }
            Lamp(x+7.8f,z+8.8f);
        }
        void CourtyardPlanter(float x,float z,Color brick,int seed)
        {
            Block(new Vector3(x,.36f,z),new Vector3(2.8f,.72f,2.4f),brick,"CityBrick");
            Ground(x,z,2.56f,2.16f,.725f,new Color(.27f,.19f,.12f),"Plaster");
            RootedTree(new Vector3(x,.73f,z),3.15f,seed);
            obstacles.Add(new Rect(x-1.42f,z-1.22f,2.84f,2.44f));
        }
        void NeighbourhoodPlanting()
        {
            foreach(var p in new[]{new Vector3(-115,0,23.7f),new Vector3(-68,0,23.8f),new Vector3(-37,0,24),new Vector3(20,0,18),new Vector3(-133,0,-36),new Vector3(-68,0,-35)}){
                var footprint=new Rect(p.x-1.8f,p.z-1.7f,3.6f,3.4f);bool occupied=false;foreach(var obstacle in obstacles)if(obstacle.Overlaps(footprint)){occupied=true;break;}if(occupied)continue;
                Block(p+Vector3.up*.2f,new Vector3(3.0f,.40f,2.8f),Cream,"CityBrick");Ground(p.x,p.z,2.7f,2.5f,.405f,new Color(.20f,.15f,.08f),"Plaster");
                RootedTree(p+Vector3.up*.42f,4.8f,4);obstacles.Add(new Rect(p.x-1.6f,p.z-1.5f,3.2f,3));
                Foliage(p+new Vector3(-.7f,.8f,.5f),new Vector3(.7f,.45f,.7f),new Color(.24f,.40f,.13f),3);
            }
        }
    }
}
