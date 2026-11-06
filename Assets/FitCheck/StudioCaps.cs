using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        static Vector3 CapPoint(float theta,float fraction,bool structured)
        {
            float ry=structured?.63f:.60f,edgeHeight=.32f-.13f*(1-Mathf.Cos(theta))*.5f;
            // An arched rear opening above the adjustment strap also gives tied
            // hair a real exit; it is part of the crown boundary, not a cut decal.
            float rearAngle=Mathf.Abs(Mathf.DeltaAngle(theta*Mathf.Rad2Deg,180))*Mathf.Deg2Rad;
            float opening=Mathf.SmoothStep(.76f,1,Mathf.Clamp01(rearAngle/.36f));
            float phi=Mathf.Acos(edgeHeight/ry)*fraction*opening;
            return new Vector3(Mathf.Sin(theta)*Mathf.Sin(phi)*.585f,2.8f+Mathf.Cos(phi)*ry,-Mathf.Cos(theta)*Mathf.Sin(phi)*.585f);
        }
        void BuildHeadwear(Garment item)
        {
            if(item==null)return;
            if(item.id=="PLAYER_PINK_GLASSES"){BuildPlayerGlasses();return;}
            if(!item.cap){
                var knit=SurfaceMaterial(item.color,"Knit");
                BuildSoftBeanie(item,knit);return;
            }
            bool structured=item.id=="SB_HEAD_01";
            bool denim=item.id=="PLAYER_BLUE_CAP";
            string surface=denim?"Denim":"Cloth";
            var fabric=SurfaceMaterial(item.color,surface);var edge=SurfaceMaterial(item.color*.78f,surface);
            const int sides=64,rows=12;var v=new List<Vector3>();var t=new List<int>();var uv=new List<Vector2>();
            for(int r=0;r<=rows;r++)for(int i=0;i<=sides;i++){
                float theta=i*Mathf.PI*2/sides-Mathf.PI;v.Add(CapPoint(theta,r/(float)rows,structured)-new Vector3(0,2.8f,0));uv.Add(new Vector2(i/(float)sides*2.4f,r/(float)rows*.6f));
            }
            for(int r=0;r<rows;r++)for(int i=0;i<sides;i++){
                int a=r*(sides+1)+i,b=a+sides+1;if(r>0)t.AddRange(new[]{a,a+1,b});t.AddRange(new[]{b,a+1,b+1});
            }
            Surface("Boné",v,t,uv,null,new Vector3(0,2.8f,0),Vector3.one,fabric,clothes);
            var band=new Vector3[sides];for(int i=0;i<sides;i++)band[i]=CapPoint(i*Mathf.PI*2/sides-Mathf.PI,1,structured);
            Tube("Boné — faixa ajustada",band,.012f,edge,clothes,true,origin:new Vector3(0,2.8f,0));
            for(int panel=0;panel<6;panel++){
                var path=new Vector3[13];float theta=panel*Mathf.PI/3;
                for(int i=0;i<path.Length;i++)path[i]=CapPoint(theta,.08f+i/(float)(path.Length-1)*.91f,structured)+new Vector3(Mathf.Sin(theta)*.003f,.003f,-Mathf.Cos(theta)*.003f);
                Tube("Boné — costura de painel",path,.0035f,edge,clothes,origin:new Vector3(0,2.8f,0));
            }
            BuildBill(structured,denim?fabric:SurfaceMaterial(structured?item.color:new Color(.15f,.32f,.22f),"Cloth"));
            Box("Boné — ajuste traseiro",new Vector3(0,3.005f,.555f),new Vector3(.24f,.047f,.025f),edge,clothes);
        }
        void BuildPlayerGlasses()
        {
            var ink=SurfaceMaterial(new Color(.067f,.061f,.079f),"Leather");
            var hinge=SurfaceMaterial(new Color(.56f,.45f,.28f),"Leather");
            var origin=new Vector3(0,2.8f,0);
            for(int side=-1;side<=1;side+=2){
                var frame=new[]{
                    new Vector3(side*.08f,2.99f,-.632f),new Vector3(side*.12f,3.025f,-.638f),
                    new Vector3(side*.43f,3.025f,-.631f),new Vector3(side*.47f,2.99f,-.627f),
                    new Vector3(side*.44f,2.79f,-.626f),new Vector3(side*.39f,2.755f,-.632f),
                    new Vector3(side*.14f,2.755f,-.638f),new Vector3(side*.10f,2.79f,-.638f)
                };
                Tube("Óculos — armação",frame,.019f,ink,clothes,true,origin:origin);
                var temple=new[]{frame[3],new Vector3(side*.54f,2.985f,-.49f),new Vector3(side*.56f,2.92f,.02f)};
                Tube("Óculos — haste",temple,.015f,ink,clothes,origin:origin);
                Tube("Óculos — dobradiça",new[]{frame[3],Vector3.Lerp(frame[3],temple[1],.29f)},.017f,hinge,clothes,origin:origin);
            }
            Tube("Óculos — ponte",new[]{new Vector3(-.095f,2.955f,-.633f),new Vector3(0,2.983f,-.654f),new Vector3(.095f,2.955f,-.633f)},
                .016f,ink,clothes,origin:origin);
        }
        void BuildSoftBeanie(Garment item,Material knit)
        {
            float[] heights={3.27f,3.37f,3.47f,3.55f,3.60f,3.62f};
            float[] radii={.45f,.43f,.36f,.27f,.14f,0};
            var v=new List<Vector3>();var uv=new List<Vector2>();var t=new List<int>();const int sides=48;
            for(int row=0;row<heights.Length;row++)for(int i=0;i<=sides;i++){
                float a=i*Mathf.PI*2/sides,f=row/(float)(heights.Length-1),slouch=Mathf.SmoothStep(.2f,1,f);
                float rib=1+.006f*Mathf.Cos(a*24);v.Add(new Vector3(Mathf.Sin(a)*radii[row]*rib+.035f*slouch,heights[row],Mathf.Cos(a)*radii[row]*rib+.04f*slouch));
                uv.Add(new Vector2(i/(float)sides*3,(heights[row]-3.27f)/AvatarTileSize));
            }
            for(int row=0;row<heights.Length-1;row++)for(int i=0;i<sides;i++){
                int a=row*(sides+1)+i,b=a+sides+1;t.AddRange(new[]{a,a+1,b,a+1,b+1,b});
            }
            // A compact knitted dome sits above the eyes, without a tilted box top.
            for(int i=0;i<v.Count;i++)v[i]-=new Vector3(0,3.27f,0);
            Surface("Gorro",v,t,uv,null,new Vector3(0,3.27f,0),Vector3.one,knit,clothes);
            RoundedLoft("Dobra do gorro",new Vector3(0,3.24f,0),new Vector3(.96f,.16f,.96f),SurfaceMaterial(item.color*1.22f,"Knit"),clothes,
                new[]{0f,.08f,.20f,.80f,.94f,1f},new[]{.94f,.99f,1f,.96f,.93f,.90f},48);
            if(item.id=="PLAYER_RED_BEANIE"){
                var thread=SurfaceMaterial(new Color(.82f,.80f,.75f),"Knit");
                for(int side=-1;side<=1;side+=2){
                    var path=new[]{new Vector3(side*.29f,3.216f,-.393f),new Vector3(side*.18f,3.263f,-.454f),
                        new Vector3(side*.09f,3.235f,-.482f),new Vector3(0,3.272f,-.482f)};
                    Tube("Gorro — bordado claro",path,.0075f,thread,clothes,origin:new Vector3(0,3.24f,0));
                }
                return;
            }
            BeaniePatch("Etiqueta do gorro",.11f,.07f,.004f,new Color(.82f,.79f,.69f));
            BeaniePatch("Marca do gorro",.055f,.014f,.006f,new Color(.12f,.10f,.17f));
        }
        void BeaniePatch(string name,float width,float height,float offset,Color color)
        {
            var v=new List<Vector3>();var uv=new List<Vector2>();var t=new List<int>();const int columns=6;
            for(int row=0;row<=2;row++)for(int i=0;i<=columns;i++){
                float y=(row/2f-.5f)*height,f=(y+.08f)/.16f;
                float radius=.48f*Mathf.Lerp(1,.96f,Mathf.InverseLerp(.2f,.8f,f))+offset;
                float a=Mathf.Asin(.13f/.47f)+(i/(float)columns-.5f)*width/.47f;
                v.Add(new Vector3(Mathf.Sin(a)*radius,y,-Mathf.Cos(a)*radius));uv.Add(new Vector2(i/(float)columns,row/2f));
            }
            for(int row=0;row<2;row++)for(int i=0;i<columns;i++){int a=row*(columns+1)+i,b=a+columns+1;t.AddRange(new[]{a,b,a+1,a+1,b,b+1});}
            Surface(name,v,t,uv,null,new Vector3(0,3.24f,0),Vector3.one,SurfaceMaterial(color,"Knit"),clothes);
        }
        void BuildBill(bool flat,Material material)
        {
            const int across=20,depth=7;var v=new List<Vector3>();var uv=new List<Vector2>();var t=new List<int>();
            int layer=(across+1)*(depth+1);
            for(int side=0;side<2;side++)for(int r=0;r<=depth;r++)for(int i=0;i<=across;i++){
                float q=i/(float)across*2-1,f=r/(float)depth,x=q*(flat?.45f:.40f);
                float back=-Mathf.Sqrt(Mathf.Max(.01f,.50f*.50f-x*x));
                float front=(flat?-1.02f:-.96f)+.15f*Mathf.Pow(Mathf.Abs(q),4);
                float y=flat?-.012f*f-.012f*q*q:-.052f*f*f-.070f*q*q;
                v.Add(new Vector3(x,y+(side==0?.009f:-.014f),Mathf.Lerp(back,front,f)));uv.Add(new Vector2((x+.5f)/AvatarTileSize,f*.48f));
            }
            for(int side=0;side<2;side++)for(int r=0;r<depth;r++)for(int i=0;i<across;i++){
                int a=side*layer+r*(across+1)+i,b=a+across+1;
                t.AddRange(side==0?new[]{a,a+1,b,a+1,b+1,b}:new[]{a,b,a+1,a+1,b,b+1});
            }
            var border=new List<int>();for(int i=0;i<=across;i++)border.Add(i);
            for(int r=1;r<=depth;r++)border.Add(r*(across+1)+across);
            for(int i=across-1;i>=0;i--)border.Add(depth*(across+1)+i);
            for(int r=depth-1;r>0;r--)border.Add(r*(across+1));
            for(int i=0;i<border.Count;i++){int a=border[i],b=border[(i+1)%border.Count];t.AddRange(new[]{a,a+layer,b,b,a+layer,b+layer});}
            Surface("Aba do boné",v,t,uv,null,new Vector3(0,3.12f,0),Vector3.one,material,clothes);
        }
    }
}
