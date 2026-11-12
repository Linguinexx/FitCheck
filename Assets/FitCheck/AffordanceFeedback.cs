using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        const float AffordanceTapDuration=.30f;
        float affordanceTapAge=1;
        WorldHint affordanceTapped;
        Rect affordanceLastRect;
        int affordanceDrawFrame=-1;
        void ConfirmAffordance(WorldHint hint)
        {
            if(!hint.valid)return;
            affordanceTapped=hint;affordanceTapAge=0;
        }
        void DrawAffordancePanel(Rect rect,WorldHint hint)
        {
            affordanceLastRect=rect;
            if(Event.current.type==EventType.Repaint)affordanceDrawFrame=Time.frameCount;
            bool tapped=affordanceTapAge<AffordanceTapDuration&&hint.key==affordanceTapped.key&&hint.label==affordanceTapped.label;
            float t=tapped?Mathf.Clamp01(affordanceTapAge/AffordanceTapDuration):1;
            float flash=tapped?1-t:0;
            float press=tapped&&!reducedMotion?3*Mathf.Sin(Mathf.PI*Mathf.Clamp01(t*2)):0;
            if(tapped&&!reducedMotion){
                float spread=2+t*10;
                RoundedPanel(new Rect(rect.x-spread,rect.y-spread,rect.width+spread*2,rect.height+spread*2),new Color(1,.18f,.20f,flash*.55f),10,1.5f);
            }
            RoundedPanel(new Rect(rect.x+2,rect.y+4,rect.width,rect.height),new Color(0,0,0,.35f));
            RoundedPanel(rect,Color.Lerp(new Color(.035f,.03f,.035f,.96f),new Color(.22f,.025f,.035f,.98f),flash));
            RoundedPanel(rect,Color.Lerp(CreatorRed,paper,flash*.85f),7,1.2f);
            var key=new Rect(rect.x+8,rect.y+7+press,36,31);
            RoundedPanel(new Rect(key.x,key.y+4-press,key.width,key.height),new Color(.25f,0,0),9);
            RoundedPanel(key,Color.Lerp(CreatorRed,paper,flash),9);
            Txt(key,hint.key,action,Color.Lerp(paper,CreatorRed,flash));
            Txt(new Rect(rect.x+56,rect.y+8,rect.width-66,34),hint.label,action,paper);
        }
        void DrawAffordanceResponse()
        {
            // Keep the short key response visible when the action opens a panel.
            // Loading already has its own transition and must cover world hints.
            if(affordanceTapAge>=AffordanceTapDuration||!affordanceTapped.valid||
                paused||loadingActive||affordanceDrawFrame==Time.frameCount)return;
            DrawAffordancePanel(affordanceLastRect,affordanceTapped);
        }
    }
}
