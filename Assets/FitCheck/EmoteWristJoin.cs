using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        readonly ResidentPart[] wristArms=new ResidentPart[2];
        void PoseWristJoin(int hand,Vector3 wrist,Quaternion lower,Quaternion handRotation,BodyPose body,Vector3 elbowRest,float weight){
            var part=wristArms[hand];
            if(part==null){foreach(var candidate in playerBody.parts)if(candidate.node.name=="Braço contínuo"&&(candidate.side>0)==(hand==1)){part=candidate;break;}wristArms[hand]=part;}
            if(part==null)return;
            var restWrist=originalPalms[hand].handRestMatrix.MultiplyPoint3x4(Vector3.zero);var axis=(restWrist-elbowRest).normalized;
            // Only the terminal 14.5 cm: taper the existing forearm rings into
            // the hand's existing wrist. No additional shell, cap or cuff.
            var handAxis=-(handRotation*Vector3.up);var turn=Quaternion.FromToRotation(lower*axis,handAxis);
            for(int i=0;i<part.rest.Length;i++){
                var relative=part.rest[i]-restWrist;float along=Vector3.Dot(relative,axis);
                if(along<-.145f||along>.085f)continue;
                float blend=Mathf.SmoothStep(0,1,Mathf.InverseLerp(-.145f,-.015f,along))*weight;
                var radial=relative-axis*along;var target=wrist+handAxis*(along+.035f)+turn*(lower*radial)*.66f;
                part.vertices[i]=Vector3.Lerp(part.vertices[i],body.TorsoPoint(target),blend);
                part.posedNormals[i]=Vector3.Slerp(part.posedNormals[i],body.torso*turn*lower*part.normals[i],blend).normalized;
            }
            part.mesh.vertices=part.vertices;part.mesh.normals=part.posedNormals;part.mesh.RecalculateBounds();
        }
    }
}
