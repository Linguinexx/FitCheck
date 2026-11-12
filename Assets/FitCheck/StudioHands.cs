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
        static readonly HandVolume[] gestureVolumes=new HandVolume[7];
        static readonly float[] HandFingerX={-.061f,-.020f,.022f,.063f},HandFingerJoint={-.199f,-.211f,-.204f,-.183f},HandFingerTip={-.239f,-.253f,-.244f,-.219f};
        static float HandBlend(float a,float b,float radius)
        {float h=Mathf.Clamp01(.5f+.5f*(b-a)/radius);return Mathf.Lerp(b,a,h)-radius*h*(1-h);}
        static float HandCapsule(Vector3 p,Vector3 a,Vector3 b,float radius)
        {var line=b-a;return (p-a-line*Mathf.Clamp01(Vector3.Dot(p-a,line)/line.sqrMagnitude)).magnitude-radius;}
        static float HandDistance(Vector3 p,int gesture=0)
        {
            if(gesture>0)return EmoteHandDistance(p,gesture);
            var q=p-new Vector3(0,-.083f,.003f);var r=gesture==0?new Vector3(.082f,.098f,.063f):new Vector3(.076f,.087f,.052f);
            float k0=new Vector3(q.x/r.x,q.y/r.y,q.z/r.z).magnitude;
            float k1=new Vector3(q.x/(r.x*r.x),q.y/(r.y*r.y),q.z/(r.z*r.z)).magnitude;
            float surface=k1<.00001f?-.063f:k0*(k0-1)/k1;
            surface=HandBlend(surface,HandCapsule(p,new Vector3(0,.003f,0),new Vector3(0,-.045f,0),.061f),.025f);
            for(int finger=0;finger<4;finger++){
                var knuckle=new Vector3(HandFingerX[finger],-.128f,.008f);var joint=new Vector3(HandFingerX[finger],HandFingerJoint[finger],-.006f);
                var end=new Vector3(HandFingerX[finger],HandFingerTip[finger],-.048f);
                if(gesture==1||gesture==4||gesture==5){
                    bool extended=gesture==4&&finger==0||gesture==5&&finger==1;
                    knuckle.y=-.119f;
                    joint=new Vector3(HandFingerX[finger],extended?-.224f:-.157f,extended?.005f:-.025f);
                    end=new Vector3(HandFingerX[finger],extended?-.302f:-.112f,extended?.005f:-.068f);
                }else if(gesture==2||gesture==3||gesture==6){
                    joint.z=.008f;end.z=.008f;
                    if(gesture==6){joint.x*=1.15f;end.x*=1.4f;}
                }
                float digit=HandBlend(HandCapsule(p,knuckle,joint,.024f),HandCapsule(p,joint,end,.019f),.012f);
                surface=HandBlend(surface,digit,gesture==0?.008f:.017f);
            }
            bool thumbOut=gesture==1||gesture==5;
            var thumbJoint=thumbOut?new Vector3(-.112f,-.066f,.004f):new Vector3(-.105f,-.103f,-.023f);
            var thumbTip=thumbOut?new Vector3(-.154f,-.072f,.004f):new Vector3(-.112f,-.151f,-.056f);
            if(gesture==2){thumbJoint=new Vector3(-.085f,-.095f,-.005f);thumbTip=new Vector3(-.088f,-.145f,-.006f);}
            float thumb=HandBlend(HandCapsule(p,new Vector3(-.063f,-.061f,0),thumbJoint,thumbOut?.040f:.032f),HandCapsule(p,thumbJoint,thumbTip,thumbOut?.035f:.025f),.018f);
            return HandBlend(surface,thumb,.020f);
        }
        static float HandEllipsoid(Vector3 p,Vector3 center,Vector3 radius)
        {
            var q=p-center;float k0=new Vector3(q.x/radius.x,q.y/radius.y,q.z/radius.z).magnitude;
            float k1=new Vector3(q.x/(radius.x*radius.x),q.y/(radius.y*radius.y),q.z/(radius.z*radius.z)).magnitude;
            return k1<.00001f?-Mathf.Min(radius.x,Mathf.Min(radius.y,radius.z)):k0*(k0-1)/k1;
        }
        // One rounded anatomical template; only joint positions change by gesture.
        // The thenar mound and curved thumb share a continuous web with the palm.
        static float EmoteHandDistance(Vector3 p,int gesture)
        {
            if(gesture==1)return ThumbsUpDistance(p);
            float surface=HandEllipsoid(p,new Vector3(0,-.092f,0),new Vector3(.083f,.102f,.041f));
            surface=HandBlend(surface,HandCapsule(p,new Vector3(0,.010f,0),new Vector3(0,-.046f,0),.050f),.030f);
            surface=HandBlend(surface,HandEllipsoid(p,new Vector3(-.049f,-.065f,-.005f),new Vector3(.043f,.058f,.041f)),.022f);
            for(int finger=0;finger<4;finger++){
                float x=HandFingerX[finger],length=finger==1?.165f:finger==0?.145f:finger==2?.153f:.113f;
                float spread=gesture==6?1.55f:1.28f,radius=finger==3?.018f:.021f;
                var root=new Vector3(x,-.148f,0);var joint=new Vector3(x*1.12f,-.148f-length*.53f,.002f);var tip=new Vector3(x*spread,-.148f-length,.005f);
                if(gesture==4){joint.z=-.018f;tip.z=-.067f;}
                bool folded=gesture==1||gesture==5&&finger!=1;
                float digit;
                if(folded){
                    joint=new Vector3(x,-.186f,-.012f);var curl=new Vector3(x,-.178f,-.052f);tip=new Vector3(x,-.126f,-.061f);
                    digit=HandBlend(HandCapsule(p,root,joint,radius*1.06f),HandCapsule(p,joint,curl,radius),.012f);
                    digit=HandBlend(digit,HandCapsule(p,curl,tip,radius*.90f),.012f);
                }else{
                    digit=HandBlend(HandCapsule(p,root,joint,radius),HandCapsule(p,joint,tip,radius*.86f),.010f);
                }
                surface=HandBlend(surface,digit,.010f);
            }
            var basePoint=new Vector3(-.059f,-.026f,0);
            var proximal=new Vector3(-.096f,-.069f,-.004f);var distal=new Vector3(-.120f,-.110f,-.005f);var thumbTip=new Vector3(-.124f,-.151f,0);
            if(gesture==1){proximal=new Vector3(-.098f,-.040f,0);distal=new Vector3(-.128f,-.055f,0);thumbTip=new Vector3(-.149f,-.073f,0);}
            if(gesture==5){basePoint=new Vector3(-.059f,-.060f,0);proximal=new Vector3(-.100f,-.066f,0);distal=new Vector3(-.138f,-.074f,0);thumbTip=new Vector3(-.173f,-.082f,0);}
            float thumb=HandBlend(HandCapsule(p,basePoint,proximal,.030f),HandCapsule(p,proximal,distal,.026f),.018f);
            thumb=HandBlend(thumb,HandCapsule(p,distal,thumbTip,.022f),.014f);
            return HandBlend(surface,thumb,.022f);
        }
        static float ThumbsUpDistance(Vector3 p)
        {
            float surface=HandEllipsoid(p,new Vector3(0,-.112f,0),new Vector3(.084f,.081f,.047f));
            surface=HandBlend(surface,HandCapsule(p,new Vector3(0,.010f,0),new Vector3(0,-.060f,0),.050f),.030f);
            // A single pad per folded finger. No return segment peeking through
            // the knuckle row and producing the previous double-finger silhouette.
            for(int finger=0;finger<4;finger++){
                float x=HandFingerX[finger],end=finger==1?-.220f:finger==0?-.213f:finger==2?-.215f:-.196f;
                float radius=finger==3?.0175f:finger==1?.020f:.0195f;
                float pad=HandCapsule(p,new Vector3(x,-.127f,-.047f),new Vector3(x,end,-.047f),radius);
                surface=HandBlend(surface,pad,.006f);
            }
            surface=HandBlend(surface,HandEllipsoid(p,new Vector3(-.048f,-.064f,0),new Vector3(.042f,.055f,.043f)),.022f);
            var a=new Vector3(-.055f,-.060f,0);var b=new Vector3(-.102f,-.060f,0);var c=new Vector3(-.141f,-.061f,0);var d=new Vector3(-.175f,-.064f,0);
            float thumb=HandBlend(HandCapsule(p,a,b,.032f),HandCapsule(p,b,c,.028f),.018f);
            thumb=HandBlend(thumb,HandCapsule(p,c,d,.024f),.014f);
            return HandBlend(surface,thumb,.022f);
        }
        static HandVolume CreateHandVolume(int gesture=0)
        {
            int nx=gesture==0?24:32,ny=gesture==0?29:37,nz=gesture==0?21:23;float step=gesture==0?.015f:.012f;
            var origin=gesture==0?new Vector3(-.179f,-.333f,-.147f):new Vector3(-.205f,-.36f,-.132f);var points=new Vector3[nx*ny*nz];var distances=new float[points.Length];
            for(int z=0;z<nz;z++)for(int y=0;y<ny;y++)for(int x=0;x<nx;x++){
                int id=x+nx*(y+ny*z);points[id]=origin+new Vector3(x,y,z)*step;float distance=HandDistance(points[id],gesture);
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
        public static Mesh CreateMiddleFingerHand(int side)
        {return CreateGestureHand(side,5);}
        public static Mesh CreateGestureHand(int side,int gesture)
        {
            if(gestureVolumes[gesture]==null)gestureVolumes[gesture]=CreateHandVolume(gesture);
            var volume=gestureVolumes[gesture];var v=new Vector3[volume.vertices.Count];var uv=new Vector2[v.Length];
            var triangles=volume.triangles.ToArray();
            for(int i=0;i<v.Length;i++){var p=volume.vertices[i];v[i]=new Vector3(-side*p.x,p.y,p.z);uv[i]=new Vector2((p.x+.18f)/AvatarTileSize,(p.y+.34f)/AvatarTileSize);}
            if(side>0)for(int i=0;i<triangles.Length;i+=3){int swap=triangles[i+1];triangles[i+1]=triangles[i+2];triangles[i+2]=swap;}
            var normals=new Vector3[v.Length];const float h=.002f;
            for(int i=0;i<v.Length;i++){
                var p=volume.vertices[i];var n=new Vector3(HandDistance(p+Vector3.right*h,gesture)-HandDistance(p-Vector3.right*h,gesture),HandDistance(p+Vector3.up*h,gesture)-HandDistance(p-Vector3.up*h,gesture),HandDistance(p+Vector3.forward*h,gesture)-HandDistance(p-Vector3.forward*h,gesture)).normalized;
                normals[i]=new Vector3(-side*n.x,n.y,n.z);
            }
            var mesh=new Mesh{name="Mão — gesto alternativo",vertices=v,triangles=triangles,uv=uv,normals=normals};mesh.RecalculateBounds();return mesh;
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
