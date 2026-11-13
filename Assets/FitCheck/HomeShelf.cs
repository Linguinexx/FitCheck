using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        void FurnishHomeShelf(){
            // Narrow covers, visible page edges and spine bands read as books,
            // rather than the old three unmarked boxes.
            for(int i=0;i<5;i++){
                float x=-4.70f+i*.145f,h=.41f+(i%3)*.055f;
                var cover=i%3==0?new Color(.20f,.29f,.34f):i%3==1?new Color(.46f,.24f,.12f):new Color(.24f,.33f,.20f);
                Block(new Vector3(x,2.76f+h/2,-4.21f),new Vector3(.115f,h,.31f),cover,"Paper",.007f);
                Block(new Vector3(x,2.76f+h/2,-4.042f),new Vector3(.077f,h-.045f,.015f),Cream,"Paper",.002f);
                foreach(float y in new[]{2.82f,2.76f+h-.07f})Block(new Vector3(x,y,-4.026f),new Vector3(.08f,.013f,.009f),cover,"Paper",.001f);
            }
            var record=new Vector3(-3.62f,3.075f,-4.20f);
            Block(record,new Vector3(.57f,.61f,.035f),new Color(.67f,.52f,.33f),"Paper",.012f);
            Rod(record+Vector3.forward*.023f,record+Vector3.forward*.035f,.25f,Ink,"Rubber",24);
            Rod(record+Vector3.forward*.036f,record+Vector3.forward*.041f,.09f,new Color(.10f,.58f,.48f),"Paper",18);
            Rod(record+Vector3.forward*.042f,record+Vector3.forward*.044f,.017f,Cream,"Paper",10);
            Block(new Vector3(-3.62f,2.79f,-4.11f),new Vector3(.60f,.055f,.19f),Ink,"Wood",.009f);
            var speaker=new Vector3(-2.91f,2.99f,-4.21f);
            Block(speaker,new Vector3(.31f,.46f,.30f),Ink,"CityMetal",.025f);
            foreach(float y in new[]{2.88f,3.10f}){
                var cone=new Vector3(speaker.x,y,-4.053f);
                Rod(cone,cone+Vector3.forward*.013f,y<3?.102f:.055f,new Color(.24f,.25f,.24f),"Rubber",16);
                Rod(cone+Vector3.forward*.014f,cone+Vector3.forward*.022f,y<3?.046f:.025f,Ink,"Rubber",12);
            }
        }
    }
}
