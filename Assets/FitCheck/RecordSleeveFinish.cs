using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        Texture2D recordSleeveGloss;
        void DrawRecordSleeve(Rect bounds,Texture2D art){
            if(art==null)return;
            if(recordSleeveGloss==null){
                const int size=128;var pixels=new Color[size*size];
                for(int y=0;y<size;y++)for(int x=0;x<size;x++){
                    float u=x/(float)(size-1),v=1-y/(float)(size-1),diagonal=u+v*.64f;
                    float broad=Mathf.Exp(-Mathf.Pow((diagonal-.47f)/.13f,2))*.15f;
                    float edge=Mathf.Exp(-Mathf.Pow((diagonal-.31f)/.012f,2))*.10f;
                    float second=Mathf.Exp(-Mathf.Pow((diagonal-1.06f)/.07f,2))*.065f;
                    float seam=u<.015f||u>.985f||v<.015f||v>.985f?.08f:0;
                    pixels[y*size+x]=new Color(1,1,1,Mathf.Min(.20f,broad+edge+second+seam));
                }
                recordSleeveGloss=new Texture2D(size,size,TextureFormat.RGBA32,false){name="Plástico — reflexo das capas",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};
                recordSleeveGloss.SetPixels(pixels);recordSleeveGloss.Apply(false,true);
            }
            Fill(new Rect(bounds.x-2,bounds.y+3,bounds.width+5,bounds.height+2),new Color(0,0,0,.4f));
            GUI.DrawTexture(bounds,art,ScaleMode.ScaleToFit);
            GUI.DrawTexture(bounds,recordSleeveGloss,ScaleMode.StretchToFill,true);
            RoundedPanel(bounds,new Color(1,1,1,.16f),1,1);
        }
    }
}
