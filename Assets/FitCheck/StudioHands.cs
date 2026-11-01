using System;
using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        sealed class HandVolume
        {
            public readonly List<Vector3> vertices=new List<Vector3>();
            public readonly List<int> triangles=new List<int>();
            readonly Dictionary<long,int> intersections=new Dictionary<long,int>();
            readonly Vector3[] points;readonly float[] distances;
            Vector3 outward;
            public HandVolume(Vector3[] points,float[] distances){this.points=points;this.distances=distances;}
            int Edge(int a,int b)
            {
                long key=((long)Mathf.Min(a,b)<<32)|(uint)Mathf.Max(a,b);int index;
                if(intersections.TryGetValue(key,out index))return index;
                float f=distances[a]/(distances[a]-distances[b]);index=vertices.Count;
                vertices.Add(Vector3.Lerp(points[a],points[b],f));intersections[key]=index;return index;
            }
            void Triangle(int a,int b,int c)
            {
                if(Vector3.Dot(Vector3.Cross(vertices[b]-vertices[a],vertices[c]-vertices[a]),outward)<0){int swap=b;b=c;c=swap;}
                triangles.AddRange(new[]{a,b,c});
            }
            public void Tetrahedron(int a,int b,int c,int d)
            {
                int[] ids={a,b,c,d};var inside=new List<int>(4);var outside=new List<int>(4);
                foreach(int id in ids)(distances[id]<0?inside:outside).Add(id);
                if(inside.Count==0||inside.Count==4)return;
                var inner=Vector3.zero;var outer=Vector3.zero;foreach(int id in inside)inner+=points[id];foreach(int id in outside)outer+=points[id];
                outward=outer/outside.Count-inner/inside.Count;
                if(inside.Count==1){Triangle(Edge(inside[0],outside[0]),Edge(inside[0],outside[1]),Edge(inside[0],outside[2]));return;}
                if(inside.Count==3){Triangle(Edge(outside[0],inside[0]),Edge(outside[0],inside[1]),Edge(outside[0],inside[2]));return;}
                int p=Edge(inside[0],outside[0]),q=Edge(inside[0],outside[1]),r=Edge(inside[1],outside[0]),s=Edge(inside[1],outside[1]);
                Triangle(p,q,r);Triangle(q,s,r);
            }
        }
        static HandVolume handVolume;
        static readonly float[] HandFingerX={-.061f,-.020f,.022f,.063f},HandFingerJoint={-.199f,-.211f,-.204f,-.183f},HandFingerTip={-.239f,-.253f,-.244f,-.219f};
        static float HandBlend(float a,float b,float radius)
        {float h=Mathf.Clamp01(.5f+.5f*(b-a)/radius);return Mathf.Lerp(b,a,h)-radius*h*(1-h);}
        static float HandCapsule(Vector3 p,Vector3 a,Vector3 b,float radius)
        {var line=b-a;return (p-a-line*Mathf.Clamp01(Vector3.Dot(p-a,line)/line.sqrMagnitude)).magnitude-radius;}
        static float HandDistance(Vector3 p)
        {
            var q=p-new Vector3(0,-.083f,.003f);var r=new Vector3(.082f,.098f,.063f);
            float k0=new Vector3(q.x/r.x,q.y/r.y,q.z/r.z).magnitude;
            float k1=new Vector3(q.x/(r.x*r.x),q.y/(r.y*r.y),q.z/(r.z*r.z)).magnitude;
            float surface=k1<.00001f?-.063f:k0*(k0-1)/k1;
            surface=HandBlend(surface,HandCapsule(p,new Vector3(0,.003f,0),new Vector3(0,-.045f,0),.061f),.025f);
            for(int finger=0;finger<4;finger++){
                var knuckle=new Vector3(HandFingerX[finger],-.128f,.008f);var joint=new Vector3(HandFingerX[finger],HandFingerJoint[finger],-.006f);
                var end=new Vector3(HandFingerX[finger],HandFingerTip[finger],-.048f);
                float digit=HandBlend(HandCapsule(p,knuckle,joint,.024f),HandCapsule(p,joint,end,.019f),.012f);
                surface=HandBlend(surface,digit,.008f);
            }
            float thumb=HandBlend(HandCapsule(p,new Vector3(-.063f,-.061f,0),new Vector3(-.105f,-.103f,-.023f),.032f),
                HandCapsule(p,new Vector3(-.105f,-.103f,-.023f),new Vector3(-.112f,-.151f,-.056f),.025f),.018f);
            return HandBlend(surface,thumb,.020f);
        }
        static HandVolume CreateHandVolume()
        {
            const int nx=24,ny=29,nz=21;const float step=.015f;
            var origin=new Vector3(-.179f,-.333f,-.147f);var points=new Vector3[nx*ny*nz];var distances=new float[points.Length];
            for(int z=0;z<nz;z++)for(int y=0;y<ny;y++)for(int x=0;x<nx;x++){
                int id=x+nx*(y+ny*z);points[id]=origin+new Vector3(x,y,z)*step;float distance=HandDistance(points[id]);
                distances[id]=Mathf.Abs(distance)<.00001f?(distance<0?-.00001f:.00001f):distance;
            }
            var volume=new HandVolume(points,distances);
            for(int z=0;z<nz-1;z++)for(int y=0;y<ny-1;y++)for(int x=0;x<nx-1;x++){
                int a=x+nx*(y+ny*z),b=a+1,c=b+nx,d=a+nx,e=a+nx*ny,f=e+1,g=f+nx,h=e+nx;
                volume.Tetrahedron(a,f,b,g);volume.Tetrahedron(a,b,c,g);volume.Tetrahedron(a,c,d,g);
                volume.Tetrahedron(a,d,h,g);volume.Tetrahedron(a,h,e,g);volume.Tetrahedron(a,e,f,g);
            }
            return volume;
        }
        void BuildRelaxedHand(int side,Transform parent)
        {
            if(handVolume==null)handVolume=CreateHandVolume();
            var vertices=new List<Vector3>();var uv=new List<Vector2>();var triangles=new List<int>(handVolume.triangles);
            foreach(var point in handVolume.vertices){vertices.Add(new Vector3(-side*point.x,point.y,point.z));uv.Add(new Vector2((point.x+.18f)/AvatarTileSize,(point.y+.34f)/AvatarTileSize));}
            if(side>0)for(int i=0;i<triangles.Count;i+=3){int swap=triangles[i+1];triangles[i+1]=triangles[i+2];triangles[i+2]=swap;}
            var material=Instantiate(skin);material.SetFloat("_Outline",0);Own(material);
            Surface("Mão",vertices,triangles,uv,null,Vector3.zero,Vector3.one,material,parent);
        }
        void ValidateHandMeshes()
        {
            foreach(var hand in handPivots){
                var filters=hand.GetComponentsInChildren<MeshFilter>();
                if(filters.Length!=1||filters[0].name!="Mão")throw new InvalidOperationException("Mão dividida em primitivas sobrepostas");
                var mesh=filters[0].sharedMesh;var vertices=mesh.vertices;var indices=mesh.triangles;
                var edges=new Dictionary<long,int>();var directions=new Dictionary<long,int>();
                for(int i=0;i<indices.Length;i+=3){
                    var a=vertices[indices[i]];var b=vertices[indices[i+1]];var c=vertices[indices[i+2]];
                    if(Vector3.Cross(b-a,c-a).sqrMagnitude<1e-22f)throw new InvalidOperationException("Triângulo degenerado na mão");
                    for(int edge=0;edge<3;edge++){
                        int from=indices[i+edge],to=indices[i+(edge+1)%3];long key=((long)Mathf.Min(from,to)<<32)|(uint)Mathf.Max(from,to);
                        if(!edges.ContainsKey(key)){edges[key]=0;directions[key]=0;}
                        edges[key]++;directions[key]+=from<to?1:-1;
                    }
                }
                foreach(var edge in edges)if(edge.Value!=2||directions[edge.Key]!=0)throw new InvalidOperationException("Mão com abertura ou face invertida: "+edge.Value+" faces / direção "+directions[edge.Key]);
            }
        }
    }
}
