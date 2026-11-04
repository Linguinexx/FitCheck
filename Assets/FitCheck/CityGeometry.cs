using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class CityView : MonoBehaviour
    {
        sealed class Batch
        {
            public readonly List<Vector3> v=new List<Vector3>(),n=new List<Vector3>();
            public readonly List<Vector2> uv=new List<Vector2>();public readonly List<Color> c=new List<Color>();public readonly List<int> t=new List<int>();
        }
        readonly Dictionary<string,Batch> batches=new Dictionary<string,Batch>();
        [SerializeField] List<Object> assets=new List<Object>();
        static readonly Color Kaos=new Color(227/255f,6/255f,19/255f),Ink=new Color(.075f,.085f,.095f),Cream=new Color(.94f,.91f,.82f),Peach=new Color(.66f,.23f,.19f),Teal=new Color(.38f,.35f,.30f),Violet=Kaos;
        Batch Get(string texture){if(texture=="Plaster")texture="CityStucco";Batch b;if(!batches.TryGetValue(texture,out b)){b=new Batch();batches[texture]=b;}return b;}
        void Quad(Vector3 a,Vector3 b,Vector3 c,Vector3 d,Color color,string texture="Plaster",float repeat=1)
        {
            var batch=Get(texture);int index=batch.v.Count;var normal=Vector3.Cross(b-a,c-a).normalized;
            batch.v.AddRange(new[]{a,b,c,d});for(int i=0;i<4;i++){batch.n.Add(normal);batch.c.Add(color);}
            float width=Vector3.Distance(a,d)/repeat,height=Vector3.Distance(a,b)/repeat;
            if((texture=="Wood"||texture=="ShopFloor"||texture=="HomeFloor"||texture=="EntryTile"||texture=="CityStone"||texture=="CityAsphalt")&&Mathf.Abs(normal.y)>.9f)
                batch.uv.AddRange(new[]{ProjectedUV(a,normal)/repeat,ProjectedUV(b,normal)/repeat,ProjectedUV(c,normal)/repeat,ProjectedUV(d,normal)/repeat});
            else batch.uv.AddRange(new[]{Vector2.zero,new Vector2(0,height),new Vector2(width,height),new Vector2(width,0)});
            batch.t.AddRange(new[]{index,index+1,index+2,index,index+2,index+3});
        }
        void Triangle(Vector3 a,Vector3 b,Vector3 c,Color color,string texture="Plaster")
        {
            var batch=Get(texture);int index=batch.v.Count;var normal=Vector3.Cross(b-a,c-a).normalized;
            batch.v.AddRange(new[]{a,b,c});for(int i=0;i<3;i++){batch.n.Add(normal);batch.c.Add(color);}batch.uv.AddRange(new[]{ProjectedUV(a,normal),ProjectedUV(b,normal),ProjectedUV(c,normal)});batch.t.AddRange(new[]{index,index+1,index+2});
        }
        static Vector2 ProjectedUV(Vector3 p,Vector3 normal)
        {
            var n=new Vector3(Mathf.Abs(normal.x),Mathf.Abs(normal.y),Mathf.Abs(normal.z));
            return n.y>=n.x&&n.y>=n.z?new Vector2(p.x,p.z):n.x>=n.z?new Vector2(p.z,p.y):new Vector2(p.x,p.y);
        }
        void Block(Vector3 p,Vector3 size,Color color,string texture="Plaster",float bevel=.05f)
        {
            float hx=size.x*.5f,hz=size.z*.5f,b=Mathf.Min(bevel,Mathf.Min(hx,hz)*.3f),y0=p.y-size.y*.5f,y1=p.y+size.y*.5f;
            float edge=Mathf.Min(b,size.y*.22f);var rings=new Vector3[4][];
            for(int r=0;r<4;r++){
                float inset=r==0||r==3?edge:0,x=hx-inset,z=hz-inset,corner=Mathf.Min(b,Mathf.Min(x,z)*.4f),y=r==0?y0:r==1?y0+edge:r==2?y1-edge:y1;
                rings[r]=new[]{new Vector3(-x+corner,y,-z),new Vector3(x-corner,y,-z),new Vector3(x,y,-z+corner),new Vector3(x,y,z-corner),new Vector3(x-corner,y,z),new Vector3(-x+corner,y,z),new Vector3(-x,y,z-corner),new Vector3(-x,y,-z+corner)};
                for(int i=0;i<8;i++)rings[r][i]+=new Vector3(p.x,0,p.z);
            }
            for(int r=0;r<3;r++)for(int i=0;i<8;i++){int next=(i+1)%8;Quad(rings[r][i],rings[r+1][i],rings[r+1][next],rings[r][next],color*(r==0?.86f:1),texture);}
            for(int i=0;i<8;i++){int next=(i+1)%8;Triangle(new Vector3(p.x,y1,p.z),rings[3][next],rings[3][i],color,texture);Triangle(new Vector3(p.x,y0,p.z),rings[0][i],rings[0][next],color,texture);}
        }
        void Ground(float x,float z,float width,float depth,float y,Color color,string texture="Plaster",float repeat=1)
        {
            if(unifiedStreetPavement&&!IsInterior&&texture=="CityStone"&&y<.1f)return;
            Quad(new Vector3(x-width*.5f,y,z-depth*.5f),new Vector3(x-width*.5f,y,z+depth*.5f),new Vector3(x+width*.5f,y,z+depth*.5f),new Vector3(x+width*.5f,y,z-depth*.5f),color,texture,repeat);
        }
        void Rod(Vector3 a,Vector3 b,float radius,Color color,string texture="Wood",int sides=8)
        {
            var direction=(b-a).normalized;var x=Vector3.Cross(direction,Mathf.Abs(direction.y)>.9f?Vector3.forward:Vector3.up).normalized;var z=Vector3.Cross(direction,x).normalized;
            for(int i=0;i<sides;i++){
                float u=i*Mathf.PI*2/sides,v=(i+1)*Mathf.PI*2/sides;var r=(x*Mathf.Cos(u)+z*Mathf.Sin(u))*radius;var s=(x*Mathf.Cos(v)+z*Mathf.Sin(v))*radius;
                Quad(a+r,a+s,b+s,b+r,color,texture);Triangle(b,b+r,b+s,color,texture);Triangle(a,a+s,a+r,color,texture);
            }
        }
        void Foliage(Vector3 p,Vector3 scale,Color color,int seed)
        {
            const int sides=10,rows=6;var points=new Vector3[rows+1,sides];
            for(int r=0;r<=rows;r++)for(int i=0;i<sides;i++){
                float phi=r*Mathf.PI/rows,a=i*Mathf.PI*2/sides,wobble=1+.12f*Mathf.Sin(i*2.7f+r*1.8f+seed);
                points[r,i]=p+Vector3.Scale(new Vector3(Mathf.Cos(a)*Mathf.Sin(phi)*wobble,Mathf.Cos(phi),Mathf.Sin(a)*Mathf.Sin(phi)*wobble),scale);
            }
            for(int r=0;r<rows;r++)for(int i=0;i<sides;i++){
                int next=(i+1)%sides;float tint=.90f+.10f*((i+r+seed)%3);
                if(r==0)Triangle(points[r,i],points[r+1,next],points[r+1,i],color*tint,"CityLeaves");
                else if(r==rows-1)Triangle(points[r,i],points[r,next],points[r+1,i],color*tint,"CityLeaves");
                else Quad(points[r,i],points[r,next],points[r+1,next],points[r+1,i],color*tint,"CityLeaves");
            }
        }
        void FinishGeometry()
        {
            foreach(var pair in batches){
                var b=pair.Value;var mesh=new Mesh{name="Bairro — "+pair.Key};mesh.indexFormat=UnityEngine.Rendering.IndexFormat.UInt32;
                mesh.SetVertices(b.v);mesh.SetNormals(b.n);mesh.SetColors(b.c);mesh.SetUVs(0,b.uv);mesh.SetTriangles(b.t,0);mesh.RecalculateBounds();assets.Add(mesh);
                var material=CityMaterial(pair.Key);assets.Add(material);
                var child=new GameObject("Arquitetura — "+pair.Key);child.transform.SetParent(transform,false);child.AddComponent<MeshFilter>().sharedMesh=mesh;child.AddComponent<MeshRenderer>().sharedMaterial=material;
            }
            batches.Clear();
        }
        public void PrepareRuntimeDisposal()
        {
            if(assets.Count>0)return;
            if(SkyMaterial!=null)assets.Add(SkyMaterial);
            foreach(var f in GetComponentsInChildren<MeshFilter>(true))if(f.sharedMesh!=null&&!assets.Contains(f.sharedMesh))assets.Add(f.sharedMesh);
            foreach(var r in GetComponentsInChildren<Renderer>(true))foreach(var m in r.sharedMaterials)if(m!=null&&!assets.Contains(m))assets.Add(m);
        }
        void OnDestroy(){foreach(var asset in assets)if(asset!=null)Destroy(asset);}
    }
}
