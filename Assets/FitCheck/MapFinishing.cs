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
            // The open leaf remains attached to its hinges inside a finished vestibule.
            var a=new Vector3(-5.74f,.04f,8.80f);var b=a+new Vector3(.22f,0,1.55f);
            Rod(a,a+Vector3.up*3.65f,.045f,Cream,"CityMetal");Rod(b,b+Vector3.up*3.65f,.045f,Cream,"CityMetal");
            Rod(a+Vector3.up*3.65f,b+Vector3.up*3.65f,.045f,Cream,"CityMetal");Rod(a,b,.045f,Cream,"CityMetal");
            foreach(float x in new[]{-7.1f,-2.5f}){Block(new Vector3(x,.35f,8.40f),new Vector3(1.1f,.7f,.55f),Ink,"CityBrick");Foliage(new Vector3(x,.95f,8.40f),new Vector3(.45f,.38f,.22f),new Color(.18f,.40f,.16f),4);obstacles.Add(new Rect(x-.6f,8.1f,1.2f,.7f));}
        }
        void RoadBoundary(float x)
        {
            foreach(var range in new[]{new Vector2(-44.5f,-17.4f),new Vector2(-10.2f,58.5f)})Block(new Vector3(x,3.2f,(range.x+range.y)*.5f),new Vector3(.7f,6.4f,range.y-range.x),Teal,"CityBrick");
            Block(new Vector3(x,5.8f,-13.8f),new Vector3(.75f,1.2f,7.2f),Teal,"CityBrick");
            float side=x>0?1:-1,entry=x-side*.42f,far=entry+side*19;
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
