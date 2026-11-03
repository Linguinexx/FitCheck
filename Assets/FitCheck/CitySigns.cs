using UnityEngine;

namespace FitCheck
{
    public partial class CityView
    {
        static readonly string[] SignLabels={"DRIPLAB","CAFE DO LADO","RADIO","ABERTO","DROP 01","DROP","D","SOM","HOJE","PRACA","ENCONTRO","PROVADOR","SIDE B","AFTER HOURS","COMMON GROUND","FIT CHECK","CASA","AP 03","GALERIA","LADO B","QUADRA","AO VIVO","MERCEARIA","OFICINA"};
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
