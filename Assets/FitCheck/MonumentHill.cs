using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        void MonumentHill()
        {
            const int sides=28;float[] radius={1,.83f,.57f,.28f,.02f},height={-.20f,2.4f,6.2f,9.6f,11};
            var rings=new Vector3[radius.Length,sides];
            for(int r=0;r<radius.Length;r++)for(int i=0;i<sides;i++){
                float a=i*Mathf.PI*2/sides,wobble=1+Mathf.Sin(i*1.7f+r*.4f)*.065f;
                rings[r,i]=new Vector3(-37+Mathf.Cos(a)*30*radius[r]*wobble,height[r],67+Mathf.Sin(a)*19*radius[r]*wobble);
            }
            for(int r=0;r<radius.Length-1;r++)for(int i=0;i<sides;i++){
                int next=(i+1)%sides;var green=(i+r)%5==0?new Color(.35f,.27f,.15f):new Color(.13f+.02f*(i%3),.34f+.025f*(i%2),.10f);
                Quad(rings[r,i],rings[r+1,i],rings[r+1,next],rings[r,next],green,"Plaster");
                Quad(rings[r,next],rings[r+1,next],rings[r+1,i],rings[r,i],green,"Plaster");
            }
            Ground(-37,67,7,5,11.02f,new Color(.38f,.32f,.19f),"CityStone",2);
            foreach(var p in new[]{new Vector3(-58,4,62),new Vector3(-17,4,64),new Vector3(-51,7,68)})Foliage(p,new Vector3(2.6f,2.2f,2),new Color(.10f,.36f,.13f),4);
        }
    }
}
