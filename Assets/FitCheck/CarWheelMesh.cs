using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        void BuildCarWheelAssets(out Mesh mesh,out Material[] materials)
        {
            const int segments=20;
            var vertices=new List<Vector3>();var colors=new List<Color>();var uv=new List<Vector2>();
            var tire=new List<int>();var metal=new List<int>();
            Vector3 Point(float radius,float depth,float angle)=>new Vector3(Mathf.Cos(angle)*radius,depth,Mathf.Sin(angle)*radius);
            void Face(Vector3 a,Vector3 b,Vector3 c,Vector3 d,Color color,List<int> triangles,bool reverse=false){
                int first=vertices.Count;vertices.AddRange(reverse?new[]{d,c,b,a}:new[]{a,b,c,d});uv.AddRange(new[]{Vector2.zero,Vector2.up,Vector2.one,Vector2.right});
                for(int i=0;i<4;i++)colors.Add(color);triangles.AddRange(new[]{first,first+1,first+2,first,first+2,first+3});
            }
            void Disc(float radius,float depth,Color color){
                for(int i=0;i<segments;i++){float a=i*Mathf.PI*2/segments,b=(i+1)*Mathf.PI*2/segments;
                    int first=vertices.Count;var p=Point(radius,depth,a);var q=Point(radius,depth,b);
                    vertices.AddRange(new[]{new Vector3(0,depth,0),depth>0?q:p,depth>0?p:q});
                    uv.AddRange(new[]{new Vector2(.5f,.5f),Vector2.zero,Vector2.one});for(int j=0;j<3;j++)colors.Add(color);metal.AddRange(new[]{first,first+1,first+2});
                }
            }
            // Round shoulder, broad tread and inset bead; axis matches existing spin.
            var profile=new[]{new Vector2(.245f,-.13f),new Vector2(.335f,-.13f),new Vector2(.383f,-.095f),new Vector2(.39f,-.065f),new Vector2(.39f,.065f),new Vector2(.383f,.095f),new Vector2(.335f,.13f),new Vector2(.245f,.13f)};
            for(int ring=0;ring<profile.Length;ring++)for(int i=0;i<segments;i++){
                var p=profile[ring];var q=profile[(ring+1)%profile.Length];float a=i*Mathf.PI*2/segments,b=(i+1)*Mathf.PI*2/segments;
                var shade=ring==3?new Color(.075f,.078f,.082f):new Color(.105f,.11f,.115f);
                Face(Point(p.x,p.y,a),Point(q.x,q.y,a),Point(q.x,q.y,b),Point(p.x,p.y,b),shade,tire);
            }
            for(int i=0;i<segments;i++){
                float a=i*Mathf.PI*2/segments;
                Face(Point(.391f,-.065f,a),Point(.391f,.065f,a+.05f),Point(.391f,.065f,a+.075f),Point(.391f,-.065f,a+.025f),new Color(.038f,.04f,.044f),tire);
            }
            foreach(float side in new[]{-1f,1f}){
                Disc(.235f,side*.116f,new Color(.065f,.07f,.075f));
                // Outer lip is chamfered, with five raised swept spokes over a dark recess.
                for(int i=0;i<segments;i++){
                    float a=i*Mathf.PI*2/segments,b=(i+1)*Mathf.PI*2/segments;
                    Face(Point(.245f,side*.132f,a),Point(.225f,side*.153f,a),Point(.225f,side*.153f,b),Point(.245f,side*.132f,b),new Color(.72f,.74f,.75f),metal,side<0);
                    Face(Point(.225f,side*.153f,a),Point(.205f,side*.135f,a),Point(.205f,side*.135f,b),Point(.225f,side*.153f,b),new Color(.43f,.46f,.49f),metal,side<0);
                }
                for(int spoke=0;spoke<5;spoke++){
                    float a=spoke*Mathf.PI*2/5;
                    var points=new[]{Point(.06f,side*.154f,a-.28f),Point(.217f,side*.145f,a+.02f),Point(.217f,side*.145f,a+.22f),Point(.06f,side*.154f,a+.28f)};
                    Face(points[0],points[1],points[2],points[3],new Color(.64f,.67f,.70f),metal,side>0);
                    for(int j=0;j<4;j++){var u=points[j];var v=points[(j+1)%4];Face(u,u-Vector3.up*side*.025f,v-Vector3.up*side*.025f,v,new Color(.32f,.35f,.38f),metal,side>0);}
                    float bolt=a+.15f;var center=Point(.039f,side*.168f,bolt);
                    Face(center+new Vector3(-.009f,0,-.009f),center+new Vector3(-.009f,0,.009f),center+new Vector3(.009f,0,.009f),center+new Vector3(.009f,0,-.009f),new Color(.20f,.22f,.24f),metal,side<0);
                }
                Disc(.064f,side*.16f,new Color(.50f,.53f,.56f));
            }
            mesh=new Mesh{name="Roda compartilhada — pneu chanfrado e aro 5 raios"};mesh.SetVertices(vertices);mesh.SetColors(colors);mesh.SetUVs(0,uv);
            mesh.subMeshCount=2;mesh.SetTriangles(tire,0);mesh.SetTriangles(metal,1);mesh.RecalculateNormals();mesh.RecalculateBounds();assets.Add(mesh);
            var rubber=CityMaterial("Rubber");var alloy=CityMaterial("CityMetal");rubber.SetFloat("_Cull",0);alloy.SetFloat("_Cull",0);alloy.SetFloat("_TextureWeight",.22f);
            assets.Add(rubber);assets.Add(alloy);materials=new[]{rubber,alloy};
        }
    }
}
