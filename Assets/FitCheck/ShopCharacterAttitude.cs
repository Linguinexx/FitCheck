using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        public void SetShopAttitude()
        {
            SetExpression(AvatarExpression.Skeptical);
            mouthLineMesh.vertices=ArcVertices(new Vector3(0,2.50f,0),.21f,.030f,.020f,.030f);
            mouthLineMesh.RecalculateNormals();mouthLineMesh.RecalculateBounds();
            for(int i=0;i<2;i++){
                UpdatePupil(i,.070f,.085f,-.025f,0);
                upperCover[i]=i==0?.42f:.28f;UpdateLid(i,false,upperCover[i]);
            }
        }
    }
    public partial class CityView
    {
        const float ShopLeanWall=8.55f;
        static readonly Quaternion ShopLeanFoot=Quaternion.Euler(-78,0,0);
        void ConfigureShopLean(Resident actor)
        {
            actor.wallLean=true;
            var ankleRest=new Vector3(.225f,.265f,.020f);float furthest=float.NegativeInfinity;
            foreach(var part in actor.parts)if(part.foot&&part.side>0)
                foreach(var point in part.rest)furthest=Mathf.Max(furthest,(ShopLeanFoot*(point-ankleRest)).z);
            actor.leanAnkle=new Vector3(.27f,.90f,ShopLeanWall-.005f-actor.home.z-furthest);
            PoseResident(actor,0,0);
        }
        BodyPose ShopLeanBody(Resident actor)
        {
            float breath=Mathf.Sin(actor.cycle*.9f)*.0015f;
            return new BodyPose{
                pelvis=Quaternion.Euler(0,0,-3),torso=Quaternion.Euler(6,0,-5),
                head=Quaternion.Euler(-12,-8,4),
                pelvisShift=new Vector3(-.035f,0,.08f),torsoShift=new Vector3(-.10f,breath,.23f)
            };
        }
        public void ValidateShopLean()
        {
            var actor=residents[0];if(!actor.wallLean)throw new System.Exception("Cliente sem apoio na parede");
            float wall=float.NegativeInfinity,raised=float.PositiveInfinity,stance=float.PositiveInfinity;
            foreach(var part in actor.parts)if(part.foot)foreach(var point in part.vertices){
                if(part.side>0){wall=Mathf.Max(wall,point.z+actor.root.localPosition.z);raised=Mathf.Min(raised,point.y);}
                else stance=Mathf.Min(stance,point.y);
            }
            if(Mathf.Abs(wall-(ShopLeanWall-.005f))>.015f||raised-stance<.25f)throw new System.Exception("Pé não está apoiado elevado na parede: "+wall+" / "+(raised-stance));
            CheckResidents();
        }
    }
}
