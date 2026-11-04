using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        sealed class ResidentSkinCache
        {
            public int[] groups;public Vector4[] weights;public float[] crossLeg;public Matrix4x4[] matrices,normalMatrices;
        }
        static Matrix4x4 BoneMatrix(Vector3 origin,Quaternion rotation,Vector3 rest)
        {return Matrix4x4.TRS(origin-rotation*rest,rotation,Vector3.one);}
        static Matrix4x4 MixSkin(Matrix4x4 a,Matrix4x4 b,float t)
        {for(int row=0;row<3;row++)for(int col=0;col<4;col++){int i=row+col*4;a[i]+=(b[i]-a[i])*t;}return a;}
        void PoseCachedSkin(ResidentPart part,Resident actor,BodyPose body,LegPose leg,Vector3 shoulder,Vector3 elbowRest,Vector3 elbow,Quaternion upperArm,Quaternion forearm)
        {
            if(part.skinCache==null){
                var groups=new int[part.rest.Length];var weights=new List<Vector4>();var cross=new List<float>();var ids=new Dictionary<(Vector4,float),int>();
                for(int i=0;i<groups.Length;i++){
                    var p=part.rest[i];Vector4 w;
                    if(part.arm)w=new Vector4(MotionWeight(1.42f,1.60f,p.y),part.node.name.Contains("Manga")&&i>=part.rest.Length-21?0:1-MotionWeight(1.78f,1.94f,p.y),0,0);
                    else if(part.blenderDenim||part.leg&&!part.foot)w=new Vector4(MotionWeight(leg.kneeRest.y-.10f,leg.kneeRest.y+.10f,p.y),1-MotionWeight(.39f,.62f,p.y),MotionWeight(1.08f,1.28f,p.y),MotionWeight(1.10f,1.60f,p.y));
                    else w=new Vector4(MotionWeight(1.10f,1.60f,p.y),0,0,0);
                    float side=part.blenderDenim?Mathf.SmoothStep(0,1,(p.x+.12f)/.24f):0;var key=(w,side);
                    if(!ids.TryGetValue(key,out int index)){index=weights.Count;ids.Add(key,index);weights.Add(w);cross.Add(side);}groups[i]=index;
                }
                part.skinCache=new ResidentSkinCache{groups=groups,weights=weights.ToArray(),crossLeg=cross.ToArray(),matrices=new Matrix4x4[weights.Count],normalMatrices=part.arm?new Matrix4x4[weights.Count]:null};
            }
            var cache=part.skinCache;
            var pelvis=BoneMatrix(body.PelvisPoint(Vector3.zero),body.pelvis,Vector3.zero);
            var torso=BoneMatrix(body.TorsoPoint(Vector3.zero),body.torso,Vector3.zero);
            var armUpper=BoneMatrix(shoulder,upperArm,shoulder);var armLower=BoneMatrix(elbow,forearm,elbowRest);
            var foot=BoneMatrix(leg.ankle,leg.foot,leg.ankleRest);var limb=new SkinLegFrames(leg);var leftFrames=part.blenderDenim?new SkinLegFrames(denimLeft):limb;var rightFrames=part.blenderDenim?new SkinLegFrames(denimRight):limb;float blend=actor.wallLean?1:actor.blend;
            for(int i=0;i<cache.matrices.Length;i++){
                var w=cache.weights[i];Matrix4x4 matrix;
                if(part.arm){var arm=MixSkin(armLower,armUpper,w.x);matrix=torso*MixSkin(Matrix4x4.identity,arm,w.y);cache.normalMatrices[i]=w.y==0?torso:torso*arm;}
                else if(part.blenderDenim){
                    var waist=MixSkin(pelvis,torso,w.w);
                    var left=SkinLegMatrix(leftFrames,w,waist);var right=SkinLegMatrix(rightFrames,w,waist);
                    matrix=MixSkin(Matrix4x4.identity,MixSkin(left,right,cache.crossLeg[i]),blend);
                }else if(part.leg)matrix=MixSkin(Matrix4x4.identity,part.foot?foot:SkinLegMatrix(limb,w,MixSkin(pelvis,torso,w.w)),blend);
                else matrix=MixSkin(pelvis,torso,w.x);
                cache.matrices[i]=matrix;
            }
            var vertices=part.vertices;var normals=part.posedNormals;
            for(int i=0;i<vertices.Length;i++){int group=cache.groups[i];var matrix=cache.matrices[group];vertices[i]=matrix.MultiplyPoint3x4(part.rest[i]);normals[i]=(cache.normalMatrices==null?matrix:cache.normalMatrices[group]).MultiplyVector(part.normals[i]);}
            part.mesh.vertices=vertices;part.mesh.normals=normals;part.mesh.RecalculateBounds();
        }
        struct SkinLegFrames
        {
            public Matrix4x4 upper,lower,foot;
            public SkinLegFrames(LegPose leg){upper=BoneMatrix(leg.hip,leg.upper,leg.hipRest);lower=BoneMatrix(leg.knee,leg.lower,leg.kneeRest);foot=BoneMatrix(leg.ankle,leg.foot,leg.ankleRest);}
        }
        static Matrix4x4 SkinLegMatrix(SkinLegFrames leg,Vector4 w,Matrix4x4 waist)
        {return MixSkin(MixSkin(MixSkin(leg.lower,leg.upper,w.x),leg.foot,w.y),waist,w.z);}
    }
}
