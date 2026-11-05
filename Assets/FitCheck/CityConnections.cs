using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        static string NeighbourhoodName(Vector2 p)
        {
            if(Vector2.Distance(p,StreetHomeDoor)<6)return "RESIDENCIAL  /  AP 03";
            if(p.y< -16&&p.x> -65&&p.x< -32)return "PALCO  /  AO VIVO";
            if(p.y>30&&p.x> -107&&p.x< -86)return "GALERIA  /  LADO B";
            if(p.y>30&&p.x> -88&&p.x< -63)return "QUADRA DO ENCONTRO";
            if(p.y>16)return "TRAVESSA DO ENCONTRO";
            if(p.x< -107)return "RUA DAS LAJES";
            if(p.x< -64)return "PASSAGEM DA GALERIA";
            if(p.x< -24)return "PASSAGEM DO COMÉRCIO";
            if(p.y>1&&p.x< -9)return "CAFÉ DO LADO";
            return p.y>0&&p.x>0?"DRIPLAB":"PRAÇA DO ENCONTRO";
        }
        void CityConnections()
        {
            // Two northbound pedestrian passages, linked by a small civic square.
            Ground(-108,18,4.2f,34,-.039f,Cream,"CityStone",4.7f);
            Ground(-76,19,8.2f,34,-.039f,Cream,"CityStone",4.7f);
            Ground(-93,28,35,10,-.038f,Cream,"CityStone",4.7f);
            Ground(-96.5f,33,17,5,-.035f,Cream,"CityStone",4.7f);
            foreach(float x in new[]{-104.5f,-89}){Bench(x,26);Lamp(x,30.5f);}
            StreetPlanter(-100,23,12);StreetPlanter(-84,25,13);
            // Art walls face the cross street behind the new local storefronts.
            foreach(float x in new[]{-103,-91}){
                Block(new Vector3(x,1.2f,17.8f),new Vector3(3.2f,2.4f,.3f),Cream,"CityBrick");
                Words("LADO B",new Vector3(x,1.5f,17.62f),.055f,Kaos);
                obstacles.Add(new Rect(x-1.7f,17.5f,3.4f,.6f));
            }
            Ground(-49,-18,30,6,-.033f,Cream,"CityStone",4.7f);
            for(int i=0;i<7;i++)Ground(-53,-15.7f+i*.65f,4,.35f,-.010f,Cream,"Leather");
            foreach(float x in new[]{-61,-37})Lamp(x,-18.3f);
            StreetPoster(-63,-18,3);StreetPoster(-36,-20,4);
            // Ordinary residential cross street, with enough room to circle the blocks.
            Ground(-102,21,77,4,-.039f,Cream,"CityStone",4.7f);
            Ground(-61,19,6,35,-.039f,Cream,"CityStone",4.7f);
            for(int i=0;i<4;i++){
                float x=-106+i*8;
                Rod(new Vector3(x,6,20),new Vector3(x+8,5.9f,20),.018f,Ink,"CityMetal",5);
                Quad(new Vector3(x,5.9f,20),new Vector3(x,5.2f,20),new Vector3(x+1.4f,5.2f,20),new Vector3(x+1.4f,5.9f,20),i%2==0?Cream:Kaos,"CityBanners");
            }
            // Front/back signs at the passages can be read while walking in either direction.
            foreach(var p in new[]{new Vector3(-108,3.4f,4),new Vector3(-76,3.4f,3)}){
                Block(p,new Vector3(3.8f,.85f,.17f),Ink,"CityMetal");Words("QUADRA",p+Vector3.back*.11f,.06f,Cream);
                Rod(p+new Vector3(-1.8f,-3.4f,0),p+new Vector3(-1.8f,.3f,0),.045f,Ink,"CityMetal");
                obstacles.Add(new Rect(p.x-2,p.z-.2f,.4f,.4f));
            }
        }
        void CloseCityEdges()
        {
            // Physical closures are inside the ground apron; no visible edge drops into nothing.
            Ground(-60,7,230,170,-.26f,new Color(.22f,.24f,.18f),"Plaster",6);
            foreach(float x in new[]{-144.5f,26}){
                RoadBoundary(x);
                obstacles.Add(new Rect(x-.4f,-44.5f,.8f,103));
                for(int i=0;i<9;i++){
                    float z=-38+i*11;if(z>-23&&z<-5)continue;
                    int seed=i;float sideX=x+(x<0?-7:7);int facing=x<0?1:-1;
                    PlaceCityModule(()=>Residence(0,0,3+seed%2,11,9.8f,seed%2==0?Peach:Teal,facing,seed+60),new Vector3(sideX,0,z));
                }
            }
            foreach(float z in new[]{-44.5f,58.5f}){
                Block(new Vector3(-59,3,z),new Vector3(171,6,.7f),Teal,"CityBrick");
                obstacles.Add(new Rect(-144.5f,z-.4f,171,.8f));
                for(int i=0;i<12;i++){
                    float x=-138+i*14;
                    int seed=i;float backZ=z+(z<0?-7:7);
                    PlaceCityModule(()=>Residence(0,0,3+seed%2,12,10,seed%3==0?Peach:Teal,0,seed+80),new Vector3(x,0,backZ),z<0);
                }
            }
            // Tall foliage behind the terminal walls softens the skyline without suggesting exits.
            foreach(float x in new[]{-142f,-119,-55,19}){
                RootedTree(new Vector3(x,0,-52),8.4f,(int)-x);
            }
        }
    }
}
