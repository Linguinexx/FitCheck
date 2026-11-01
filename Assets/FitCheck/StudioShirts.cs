using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        const int ShirtSides=32;
        static Vector3 ShirtPoint(int row,int column,bool loose)
        {
            float[] heights={1.37f,1.44f,1.75f,loose?1.84f:1.89f,2.0f,loose?2.12f:2.14f,2.23f};
            float[] widths={1f,1.03f,.95f,1f,.92f,.77f,.38f};
            float a=column*Mathf.PI*2/ShirtSides;
            return new Vector3(Mathf.Sin(a)*(loose?1.07f:.84f)*.5f*widths[row],heights[row],-Mathf.Cos(a)*.285f*widths[row]);
        }
        void ShirtBody(Garment top,Material fabric)
        {
            var v=new List<Vector3>();var uv=new List<Vector2>();var t=new List<int>();
            for(int row=0;row<7;row++)for(int i=0;i<=ShirtSides;i++){
                var p=ShirtPoint(row,i,top.loose);v.Add(p);uv.Add(new Vector2(i/(float)ShirtSides*2,(p.y-1.37f)/AvatarTileSize));
            }
            for(int row=0;row<6;row++)for(int i=0;i<ShirtSides;i++){
                // Side openings are bounded by the same vertices as the sleeves.
                if(row>=3&&row<5&&((i>=4&&i<12)||(i>=20&&i<28)))continue;
                int a=row*(ShirtSides+1)+i,b=a+ShirtSides+1;t.AddRange(new[]{a,b,a+1,a+1,b,b+1});
            }
            Surface("Camiseta com caimento",v,t,uv,null,Vector3.zero,Vector3.one,fabric,clothes);
        }
        List<Vector3> SleeveShoulder(int side,bool loose)
        {
            int start=side>0?4:20,end=start+8;var boundary=new List<Vector3>();
            for(int i=start;i<=end;i++)boundary.Add(ShirtPoint(3,i,loose));
            boundary.Add(ShirtPoint(4,end,loose));boundary.Add(ShirtPoint(5,end,loose));
            for(int i=end-1;i>=start;i--)boundary.Add(ShirtPoint(5,i,loose));
            boundary.Add(ShirtPoint(4,start,loose));
            for(int i=0;i<boundary.Count;i++)boundary[i]=Vector3.Scale(boundary[i],new Vector3(.9f,1,.9f));
            var across=Vector3.down*side;var forward=Vector3.back;var center=Vector3.zero;foreach(var p in boundary)center+=p;center/=boundary.Count;
            boundary.Sort((a,b)=>Mathf.Atan2(Vector3.Dot(a-center,forward),Vector3.Dot(a-center,across)).CompareTo(Mathf.Atan2(Vector3.Dot(b-center,forward),Vector3.Dot(b-center,across))));
            return boundary;
        }
    }
}
