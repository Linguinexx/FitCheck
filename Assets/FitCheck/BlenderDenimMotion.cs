using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        LegPose denimLeft,denimRight;
        static void DenimLegPoint(Vector3 p,Vector3 n,LegPose leg,BodyPose body,out Vector3 posed,out Vector3 normal)
        {
            float upper=MotionWeight(leg.kneeRest.y-.10f,leg.kneeRest.y+.10f,p.y);
            posed=Vector3.Lerp(leg.knee+leg.lower*(p-leg.kneeRest),leg.hip+leg.upper*(p-leg.hipRest),upper);
            normal=Vector3.Lerp(leg.lower*n,leg.upper*n,upper);
            float cuff=1-MotionWeight(.39f,.62f,p.y);
            posed=Vector3.Lerp(posed,leg.ankle+leg.foot*(p-leg.ankleRest),cuff);normal=Vector3.Lerp(normal,leg.foot*n,cuff);
            float waist=MotionWeight(1.08f,1.28f,p.y);posed=Vector3.Lerp(posed,body.Point(p),waist);normal=Vector3.Lerp(normal,body.Normal(n,p.y),waist);
        }
    }
}
