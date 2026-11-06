using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        const int ShirtSides=32;
        void BuildPosterJacket()
        {
            var paper=SurfaceMaterial(new Color(.94f,.84f,.59f),"Cloth");
            var red=SurfaceMaterial(new Color(.84f,.17f,.22f),"Cloth");
            var teal=SurfaceMaterial(new Color(.15f,.59f,.54f),"Cloth");
            for(int side=-1;side<=1;side+=2){
                for(int i=0;i<5;i++){
                    float x=-.30f+i*.15f,y=1.72f+(i%3)*.10f,z=side*(.305f-.045f*Mathf.Abs(x)/.30f);
                    var vertices=new List<Vector3>{
                        new Vector3(x-.09f,y-.12f,z),new Vector3(x-.076f,y+.11f,z),
                        new Vector3(x+.104f,y+.081f,z),new Vector3(x+.069f,y-.12f,z),
                        new Vector3(x+.018f,y-.091f,z)};
                    Surface("BlenderTopo Cartaz rasgado",vertices,
                        side<0?new List<int>{0,1,2,0,2,4,2,3,4}:new List<int>{0,2,1,0,4,2,2,4,3},
                        new List<Vector2>{Vector2.zero,Vector2.up,Vector2.one,Vector2.right,new Vector2(.62f,.12f)},
                        null,Vector3.zero,Vector3.one,i%3==0?paper:i%3==1?red:teal,clothes);
                    Box("BlenderTopo Tipografia do cartaz",new Vector3(x,y+.028f,z+side*.008f),
                        new Vector3(.104f,.026f,.009f),i%3==0?teal:paper,clothes);
                }
            }
        }
        void BuildGarageDjJacket()
        {
            var ink=SurfaceMaterial(new Color(.075f,.066f,.11f),"Knit");
            var lime=SurfaceMaterial(new Color(.68f,.88f,.19f),"Cloth");
            Rings("Gola canelada",new Vector3(0,2.217f,0),new Vector3(.40f,.055f,.33f),ink,clothes,new[]{0f,1f},new[]{1f,1f},12);
            Tube("Zíper lima",new[]{
                new Vector3(0,1.42f,-.302f),new Vector3(0,1.75f,-.285f),
                new Vector3(0,2.0f,-.272f),new Vector3(0,2.22f,-.120f)
            },.014f,lime,clothes);
            float[] levels={.08f,.15f,.25f,.19f,.10f};
            for(int i=0;i<levels.Length;i++){
                float x=(i-2)*.084f,z=.285f-.024f*Mathf.Abs(i-2)/2;
                Box("Equalizador",new Vector3(x,1.85f,z),new Vector3(.047f,levels[i],.027f),lime,clothes);
            }
        }
        void BuildFittingGhostShirt()
        {
            var violet=SurfaceMaterial(new Color(.36f,.24f,.50f),"Cloth");
            var mint=SurfaceMaterial(new Color(.47f,.84f,.70f),"Cloth");
            Rings("Gola violeta",new Vector3(0,2.217f,0),new Vector3(.43f,.052f,.32f),violet,clothes,new[]{0f,1f},new[]{1f,1f},12);
            BuildGhostPatch(-1,.74f,violet,mint);
            BuildGhostPatch(1,1.17f,violet,mint);
        }
        void BuildGhostPatch(int side,float scale,Material violet,Material mint)
        {
            var outline=new[]{
                new Vector2(-.18f,-.17f),new Vector2(-.15f,-.04f),new Vector2(-.15f,.05f),
                new Vector2(-.125f,.125f),new Vector2(-.066f,.176f),new Vector2(0,.19f),
                new Vector2(.066f,.176f),new Vector2(.125f,.125f),new Vector2(.15f,.05f),
                new Vector2(.15f,-.04f),new Vector2(.18f,-.17f),new Vector2(.115f,-.184f),
                new Vector2(.06f,-.125f),new Vector2(0,-.178f),new Vector2(-.06f,-.125f),new Vector2(-.115f,-.184f)
            };
            var vertices=new List<Vector3>{GhostPatchPoint(Vector2.zero,side,scale,.009f)};
            var uv=new List<Vector2>{Vector2.one*.5f};
            var triangles=new List<int>();
            for(int i=0;i<outline.Length;i++){
                vertices.Add(GhostPatchPoint(outline[i],side,scale,.009f));
                uv.Add(outline[i]*2.5f+Vector2.one*.5f);
                int current=i+1,next=(i+1)%outline.Length+1;
                triangles.AddRange(side<0?new[]{0,current,next}:new[]{0,next,current});
            }
            Surface("Bordado — lençol do fantasma",vertices,triangles,uv,null,Vector3.zero,Vector3.one,violet,clothes);
            var seam=new Vector3[outline.Length];
            for(int i=0;i<outline.Length;i++)seam[i]=GhostPatchPoint(outline[i],side,scale,.012f);
            Tube("Contorno do bordado",seam,.005f*scale,mint,clothes,true);
            foreach(float x in new[]{-.05f,.05f})
                Ellipsoid("Olho bordado",GhostPatchPoint(new Vector2(x,.064f),side,scale,.019f),new Vector3(.036f,.062f,.017f)*scale,mint,clothes,10,8);
        }
        static Vector3 GhostPatchPoint(Vector2 point,int side,float scale,float offset)
        {
            float x=point.x*scale,y=1.82f+point.y*scale;
            int row=1;
            while(row<5&&ShirtPoint(row+1,0,true).y<y)row++;
            Vector3 lower=ShirtPoint(row,0,true),upper=ShirtPoint(row+1,0,true);
            float fraction=Mathf.InverseLerp(lower.y,upper.y,y);
            float depth=-Mathf.Lerp(lower.z,upper.z,fraction);
            float width=Mathf.Lerp(ShirtPoint(row,ShirtSides/4,true).x,ShirtPoint(row+1,ShirtSides/4,true).x,fraction);
            float surfaceDepth=depth*Mathf.Sqrt(1-x*x/(width*width));
            return new Vector3(x,y,side*(surfaceDepth+offset));
        }
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
