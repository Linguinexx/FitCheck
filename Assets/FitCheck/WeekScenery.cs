using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        Transform[] weekScenery;
        public int SceneryDay {get;private set;}
        static readonly string[] WeekHeadlines={"A SEMANA E NOSSA","FOTOS DA RUA","RIMA NA PRACA","TREINO ABERTO","PASSAGEM DE SOM","GALERIA ABERTA","HOJE TEM ENCONTRO"};
        static readonly Color[] WeekColors={new Color(.90f,.24f,.13f),new Color(.11f,.66f,.70f),new Color(.65f,.26f,.89f),new Color(.20f,.70f,.32f),new Color(.96f,.61f,.08f),new Color(.89f,.26f,.57f),new Color(.95f,.18f,.22f)};
        static readonly string[] WeekPrints={"A SEMANA E NOSSA","FOTOS DA RUA","RIMA NA PRACA","TREINO ABERTO","PASSAGEM DE SOM","GALERIA ABERTA","HOJE TEM ENCONTRO","DIA 1","DIA 2","DIA 3","DIA 4","DIA 5","DIA 6","DIA 7","CHEGA JUNTO","NO BAIRRO","TODO MUNDO","SOM VIVO","7 NOITES","SETE NOITES. NOSSO BAIRRO."};
        void WeekWords(string text,Vector3 center,float pixel,Color color){
            int index=System.Array.IndexOf(WeekPrints,text);if(index<0)throw new System.ArgumentException("Cartaz da semana ausente: "+text);
            float width=(text.Length*5.4f)*pixel,height=7*pixel;var batch=Get("WeekSign");int start=batch.v.Count;
            batch.v.AddRange(new[]{center+new Vector3(-width/2,-height/2,0),center+new Vector3(-width/2,height/2,0),center+new Vector3(width/2,height/2,0),center+new Vector3(width/2,-height/2,0)});float u=index%4*.25f,v=1-(index/4+1)*.125f;
            batch.uv.AddRange(new[]{new Vector2(u,v),new Vector2(u,v+.125f),new Vector2(u+.25f,v+.125f),new Vector2(u+.25f,v)});for(int i=0;i<4;i++){batch.n.Add(Vector3.back);batch.c.Add(color);}batch.t.AddRange(new[]{start,start+1,start+2,start,start+2,start+3});
        }
        public void ApplyWeekScenery(int day){
            if(IsInterior)return;day=Mathf.Clamp(day,1,7);if(SceneryDay==day)return;
            if(weekScenery==null){
                weekScenery=new Transform[7];
                for(int d=0;d<7;d++){
                    var group=new GameObject("Bairro vivo — dia "+(d+1)).transform;group.SetParent(transform,false);weekScenery[d]=group;
                    int first=transform.childCount;
                    // Existing walls and stage: signs stay clear of walking routes.
                    Block(new Vector3(-97,4.35f,21.62f),new Vector3(6.1f,.95f,.07f),WeekColors[d],"Paper");
                    WeekWords(WeekHeadlines[d],new Vector3(-97,4.35f,21.56f),.058f,Cream);
                    Block(new Vector3(-109,2.1f,5.31f),new Vector3(1.6f,2.0f,.04f),WeekColors[d],"Paper");
                    WeekWords("DIA "+(d+1),new Vector3(-109,2.55f,5.27f),.065f,Cream);
                    WeekWords(d<3?"CHEGA JUNTO":d<6?"NO BAIRRO":"TODO MUNDO",new Vector3(-109,1.95f,5.27f),.026f,Cream);
                    if(d>=2){
                        Rod(new Vector3(-101,5.3f,20.8f),new Vector3(-93,5.3f,20.8f),.018f,Ink,"CityMetal",5);
                        for(int i=0;i<6+d;i++){float x=-100.7f+i*(7.4f/(5+d));Triangle(new Vector3(x,5.27f,20.79f),new Vector3(x+.46f,5.27f,20.79f),new Vector3(x+.23f,4.72f,20.79f),i%2==0?WeekColors[d]:Cream,"Paper");}
                    }
                    if(d>=4){
                        for(int i=0;i<3;i++){var p=new Vector3(-32+i*1.3f,3.5f,-39.2f);Block(p,new Vector3(.8f,1.1f,.045f),i%2==0?WeekColors[d]:Cream,"Paper");WeekWords(d==6?"7 NOITES":"SOM VIVO",p+Vector3.back*.03f,.019f,Ink);}
                    }
                    FinishGeometry();
                    // FinishGeometry adds only render geometry; it never adds obstacles or lights.
                    while(transform.childCount>first)transform.GetChild(first).SetParent(group,false);
                    group.gameObject.SetActive(false);
                }
            }
            for(int i=0;i<weekScenery.Length;i++)weekScenery[i].gameObject.SetActive(i==day-1);SceneryDay=day;
        }
    }
}
