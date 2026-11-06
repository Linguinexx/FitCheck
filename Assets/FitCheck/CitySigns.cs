using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        static readonly string[] SignLabels={"DRIPLAB","CAFE DO LADO","RADIO","ABERTO","DROP 01","DROP","D","SOM","HOJE","PRACA","ENCONTRO","PROVADOR","SIDE B","AFTER HOURS","COMMON GROUND","FIT CHECK","CASA","AP 03","GALERIA","LADO B","QUADRA","AO VIVO","MERCEARIA","OFICINA","BATALHA DE RIMA","CHINELO NO FIO","FIADO SO AMANHA","FUBA DONO DA RUA","SEM PIX SEM FIT","RESPEITA O CORRE","PASSINHO LIBERADO","SO MAIS UM DROP"};
        void FacadeSign(string label,Vector3 center,float width,float height,float letterSize,Color finish,Color ink,bool glow=false)
        {
            Block(center,new Vector3(width,height,.18f),finish*.58f,"CityMetal",.025f);
            Block(center-Vector3.forward*.095f,new Vector3(width-.13f,height-.10f,.08f),finish,"CityMetal",.018f);
            foreach(float side in new[]{-1f,1f}){
                var fixing=center+new Vector3(side*(width*.5f-.18f),0,-.142f);
                Block(fixing,new Vector3(.063f,.063f,.027f),finish*.57f,"CityMetal",.012f);
            }
            PrintedWords(label,center-Vector3.forward*.143f,letterSize,ink,glow);
        }
        void PrintedWords(string text,Vector3 center,float pixel,Color color,bool glow)
        {
            int sign=System.Array.IndexOf(SignLabels,text);if(sign<0)return;
            float width=0;foreach(char c in text)width+=(c=='I'||c==' '?4:6)*pixel;width-=pixel;
            float height=pixel*7;var batch=Get(glow?"CitySignGlow":"CitySign");int start=batch.v.Count;
            batch.v.AddRange(new[]{center+new Vector3(-width/2,-height/2,0),center+new Vector3(-width/2,height/2,0),center+new Vector3(width/2,height/2,0),center+new Vector3(width/2,-height/2,0)});
            float heightUV=1f/Mathf.CeilToInt(SignLabels.Length/4f),u=sign%4*.25f,v=1-(sign/4+1)*heightUV;
            batch.uv.AddRange(new[]{new Vector2(u,v),new Vector2(u,v+heightUV),new Vector2(u+.25f,v+heightUV),new Vector2(u+.25f,v)});
            for(int i=0;i<4;i++){batch.n.Add(Vector3.back);batch.c.Add(color);}
            batch.t.AddRange(new[]{start,start+1,start+2,start,start+2,start+3});
        }
    }
}
