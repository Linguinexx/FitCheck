using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        static Vector3 BrowOrigin(int eye){return new Vector3(eye==0?-.23f:.23f,3.085f,0);}
        void BrowGeometry(int eye,AvatarExpression value,out Vector3[] vertices,out int[] triangles)
        {
            const int steps=16;var v=new List<Vector3>();var t=new List<int>();var origin=BrowOrigin(eye);
            float raise=value==AvatarExpression.Surprised?.017f:value==AvatarExpression.Curious?(eye==0?.012f:.004f):value==AvatarExpression.Sleepy?-.008f:0;
            float arch=value==AvatarExpression.Happy?.012f:value==AvatarExpression.Surprised?.015f:.008f;
            float slope=value==AvatarExpression.Skeptical?(eye==0?-.023f:.020f):value==AvatarExpression.Curious?(eye==0?-.008f:.008f):0;
            float width=value==AvatarExpression.Surprised?.20f:.21f;
            if(feminineAppearance){arch+=.010f;width*=.93f;}
            for(int i=0;i<=steps;i++){
                float u=i/(float)steps,q=u*2-1,x=q*width*.5f;
                float y=raise+arch*(1-q*q)+q*slope;
                float thickness=.009f+.030f*Mathf.Pow(Mathf.Max(0,Mathf.Sin(u*Mathf.PI)),.65f);
                if(feminineAppearance)thickness*=.68f;
                for(int layer=0;layer<2;layer++)for(int edge=0;edge<2;edge++){
                    float py=y+(edge==0?-.5f:.5f)*thickness;
                    v.Add(new Vector3(x,py,FaceZ(origin.x+x,origin.y+py)+(layer==0?-.004f:.006f)));
                }
            }
            for(int i=0;i<steps;i++){
                int a=i*4,b=a+4;
                t.AddRange(new[]{a,a+1,b,a+1,b+1,b,a+2,b+2,a+3,a+3,b+2,b+3});
                t.AddRange(new[]{a,a+4,a+2,a+2,a+4,a+6,a+1,a+3,a+5,a+3,a+7,a+5});
            }
            t.AddRange(new[]{0,2,1,1,2,3});int end=steps*4;t.AddRange(new[]{end,end+1,end+2,end+1,end+3,end+2});
            vertices=v.ToArray();triangles=t.ToArray();
        }
        Transform BuildBrow(int eye,Material material)
        {
            Vector3[] v;int[] t;BrowGeometry(eye,AvatarExpression.Neutral,out v,out t);
            return FaceMesh("Sobrancelha",v,t,BrowOrigin(eye),material).transform;
        }
        void BuildOuterLashes(int eye,Material material)
        {
            float side=eye==0?-1:1;
            var vertices=new List<Vector3>{
                new Vector3(side*.145f,.025f,-.055f),new Vector3(side*.201f,.062f,-.040f),new Vector3(side*.160f,.043f,-.045f),
                new Vector3(side*.136f,.055f,-.052f),new Vector3(side*.184f,.100f,-.038f),new Vector3(side*.151f,.070f,-.041f)
            };
            Surface("Cílios externos",vertices,new List<int>{0,1,2,2,1,0,3,4,5,5,4,3},
                new List<Vector2>{Vector2.zero,Vector2.up,Vector2.one,Vector2.zero,Vector2.up,Vector2.one},
                null,Vector3.zero,Vector3.one,material,eyes[eye]);
        }
        void UpdateBrow(int eye,AvatarExpression value)
        {
            Vector3[] v;int[] t;BrowGeometry(eye,value,out v,out t);
            brows[eye].localPosition=BrowOrigin(eye)-new Vector3(0,2.8f,0);brows[eye].localRotation=Quaternion.identity;
            var mesh=brows[eye].GetComponent<MeshFilter>().sharedMesh;mesh.vertices=v;mesh.triangles=t;mesh.uv=new Vector2[v.Length];mesh.RecalculateNormals();mesh.RecalculateBounds();
        }
    }
}
