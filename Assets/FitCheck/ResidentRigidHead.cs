using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        void PoseRigidHead(ResidentPart part,Resident actor,int index,BodyPose body)
        {
            if(part.closedLid!=null){
                float blink=ResidentBlink(actor.blinkClock,index),cover=Mathf.Max(Mathf.Max(.006f,part.lidBaseCover),blink);
                if(part.headLocalVertices==null){part.headLocalVertices=new Vector3[part.rest.Length];part.headLocalNormals=new Vector3[part.rest.Length];part.lastLidCover=-1;}
                if(cover!=part.lastLidCover){
                    for(int i=0;i<part.rest.Length;i++)EvaluateResidentLid(part,i,blink,out part.headLocalVertices[i],out part.headLocalNormals[i]);
                    part.mesh.vertices=part.headLocalVertices;part.mesh.normals=part.headLocalNormals;part.mesh.RecalculateBounds();part.lastLidCover=cover;
                }
            }
            var shift=body.HeadPoint(Vector3.zero);part.node.transform.localPosition=shift;part.node.transform.localRotation=body.head;
            part.rigidHead=true;part.rigidVerticesDirty=true;part.rigidHeadMatrix=Matrix4x4.TRS(shift,body.head,Vector3.one);
        }
    }
}
