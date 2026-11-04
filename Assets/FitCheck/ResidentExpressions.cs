using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        void AnimateResidentExpression(Resident actor,int index)
        {
            float phase=Mathf.Repeat(actor.cycle+index*.83f,8);
            bool reaction=actor.expression==AvatarExpression.Surprised&&phase>.05f&&phase<.75f;
            UpdateBlinkVisibility(actor,index);
            foreach(var part in actor.parts)if(part.mouthKind>0&&part.node!=null){
                bool visible=part.mouthKind==1?actor.expression!=AvatarExpression.Happy&&!reaction:part.mouthKind==3?reaction:(part.mouthKind==2||part.mouthKind==4)&&actor.expression==AvatarExpression.Happy;
                part.faceVisible=visible;part.node.SetActive(visible);
            }
        }
        void UpdateBlinkVisibility(Resident actor,int index)
        {foreach(var part in actor.parts)if(part.node!=null&&part.node.name=="Pupila")part.node.SetActive(true);}
        bool PlayerFaceVisible(GameObject piece)
        {foreach(var part in playerBody.parts)if(part.node==piece&&part.mouthKind>0)return part.faceVisible;return true;}
    }
}
