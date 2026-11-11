using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        sealed class SneakerShape
        {
            public float[] z,w,h,y,soleY,soleH;public int style;
        }
        static SneakerShape Sneaker(Garment item)
        {
            if(item.id=="AH_SHOES_01")return new SneakerShape{style=0,
                z=new[]{.28f,.23f,.10f,-.10f,-.34f,-.47f,-.50f},w=new[]{.115f,.17f,.18f,.20f,.19f,.145f,.075f},
                h=new[]{.08f,.14f,.15f,.115f,.075f,.045f,.025f},y=new[]{.30f,.32f,.32f,.285f,.245f,.23f,.23f},
                soleY=new[]{.19f,.17f,.16f,.15f,.145f,.15f,.16f},soleH=new[]{.065f,.07f,.07f,.055f,.05f,.05f,.03f}};
            if(item.id=="SB_SHOES_01")return new SneakerShape{style=1,
                z=new[]{.26f,.21f,.08f,-.12f,-.32f,-.44f,-.47f},w=new[]{.115f,.17f,.185f,.20f,.19f,.15f,.075f},
                h=new[]{.10f,.20f,.17f,.105f,.065f,.04f,.025f},y=new[]{.34f,.37f,.31f,.255f,.235f,.225f,.225f},
                soleY=new[]{.14f,.14f,.14f,.14f,.14f,.14f,.14f},soleH=new[]{.04f,.04f,.04f,.04f,.04f,.04f,.04f}};
            return new SneakerShape{style=2,
                z=new[]{.27f,.22f,.08f,-.12f,-.34f,-.47f,-.51f},w=new[]{.13f,.185f,.20f,.215f,.215f,.18f,.10f},
                h=new[]{.11f,.16f,.15f,.105f,.075f,.05f,.035f},y=new[]{.30f,.32f,.31f,.28f,.255f,.25f,.25f},
                soleY=new[]{.155f,.155f,.15f,.15f,.15f,.155f,.165f},soleH=new[]{.055f,.055f,.05f,.05f,.05f,.05f,.04f}};
        }
        GameObject ShoeVolume(string name,float x,Material material,SneakerShape shape,bool sole,float layerScale=1,float layerDrop=0)
        {
            const int sides=24;var v=new List<Vector3>();var t=new List<int>();var uv=new List<Vector2>();var n=new List<Vector3>();
            var toeTriangles=shape.style==1&&!sole?new List<int>():null;
            for(int r=0;r<shape.z.Length;r++)for(int i=0;i<=sides;i++){
                int lo=Mathf.Max(0,r-1),hi=Mathf.Min(shape.z.Length-1,r+1);float a=i*Mathf.PI*2/sides;
                float h=sole?shape.soleH[r]*layerScale:shape.h[r],y=sole?shape.soleY[r]-shape.soleH[r]*layerDrop:shape.y[r];
                float dh=sole?(shape.soleH[hi]-shape.soleH[lo])*layerScale:shape.h[hi]-shape.h[lo];
                float dy=sole?shape.soleY[hi]-shape.soleY[lo]-(shape.soleH[hi]-shape.soleH[lo])*layerDrop:shape.y[hi]-shape.y[lo];
                var around=new Vector3(Mathf.Cos(a)*shape.w[r],-Mathf.Sin(a)*h,0);
                var along=new Vector3(Mathf.Sin(a)*(shape.w[hi]-shape.w[lo]),dy+Mathf.Cos(a)*dh,shape.z[hi]-shape.z[lo]);
                float sx=Mathf.Sin(a),sy=Mathf.Cos(a);
                if(sole){sx=Mathf.Sign(sx)*Mathf.Pow(Mathf.Abs(sx),.55f)*1.045f;sy=Mathf.Sign(sy)*Mathf.Pow(Mathf.Abs(sy),.55f);}
                // Stitch the upper into the midsole instead of leaving an elliptical gap.
                float upperBottom=shape.soleY[r]+shape.soleH[r]-.012f;
                float height=!sole&&sy<0?y-Mathf.Pow(-sy,.45f)*(y-upperBottom):y+sy*h;
                v.Add(new Vector3(sx*shape.w[r],height,shape.z[r]));n.Add(Vector3.Cross(around,along).normalized);
                uv.Add(new Vector2(i/(float)sides*.75f,(shape.z[0]-shape.z[r])/AvatarTileSize));
            }
            for(int r=0;r<shape.z.Length-1;r++)for(int i=0;i<sides;i++){
                int a=r*(sides+1)+i,b=a+sides+1;
                var destination=toeTriangles!=null&&r>=4?toeTriangles:t;destination.AddRange(new[]{a,a+1,b,a+1,b+1,b});
            }
            for(int cap=0;cap<2;cap++){
                int r=cap==0?0:shape.z.Length-1,start=v.Count;float y=sole?shape.soleY[r]-shape.soleH[r]*layerDrop:shape.y[r];
                v.Add(new Vector3(0,y,shape.z[r]));n.Add(cap==0?Vector3.forward:Vector3.back);uv.Add(new Vector2(.5f,.5f));
                for(int i=0;i<=sides;i++){
                    var point=v[r*(sides+1)+i];v.Add(point);n.Add(cap==0?Vector3.forward:Vector3.back);uv.Add(new Vector2(.5f+point.x/AvatarTileSize,.5f+(point.y-y)/AvatarTileSize));
                }
                var destination=toeTriangles!=null&&cap==1?toeTriangles:t;
                for(int i=0;i<sides;i++)destination.AddRange(cap==0?new[]{start,start+i+2,start+i+1}:new[]{start,start+i+1,start+i+2});
            }
            var go=Surface(name,v,t,uv,n,new Vector3(x,0,0),Vector3.one,material,clothes);
            if(toeTriangles!=null){
                var mesh=go.GetComponent<MeshFilter>().sharedMesh;mesh.subMeshCount=2;mesh.SetTriangles(toeTriangles,1);
                go.GetComponent<MeshRenderer>().sharedMaterials=new[]{material,SurfaceMaterial(new Color(.86f,.84f,.76f),"Rubber")};
            }
            return go;
        }
        Vector3 ShoeSection(SneakerShape shape,float z)
        {
            int r=0;while(r<shape.z.Length-2&&z<shape.z[r+1])r++;float f=Mathf.InverseLerp(shape.z[r],shape.z[r+1],z);
            return new Vector3(Mathf.Lerp(shape.w[r],shape.w[r+1],f),Mathf.Lerp(shape.y[r],shape.y[r+1],f),Mathf.Lerp(shape.h[r],shape.h[r+1],f));
        }
        float ShoeTop(SneakerShape shape,float x,float z)
        {
            var section=ShoeSection(shape,z);return section.y+section.z*Mathf.Sqrt(Mathf.Max(0,1-x*x/(section.x*section.x)));
        }
        void ShoePanel(string name,float x,SneakerShape shape,float back,float front,float angleA,float angleB,Material material)
        {
            const int columns=8,rows=6;var v=new List<Vector3>();var t=new List<int>();var uv=new List<Vector2>();
            for(int r=0;r<=rows;r++)for(int i=0;i<=columns;i++){
                float z=Mathf.Lerp(back,front,r/(float)rows),a=Mathf.Lerp(angleA,angleB,i/(float)columns);var section=ShoeSection(shape,z);
                var normal=new Vector3(Mathf.Sin(a),Mathf.Cos(a),0);
                v.Add(new Vector3(Mathf.Sin(a)*section.x,section.y+Mathf.Cos(a)*section.z,z)+normal*.006f);
                uv.Add(new Vector2(i/(float)columns*.5f,(back-z)/AvatarTileSize));
            }
            for(int r=0;r<rows;r++)for(int i=0;i<columns;i++){
                int a=r*(columns+1)+i,b=a+columns+1;t.AddRange(angleB>angleA?new[]{a,a+1,b,a+1,b+1,b}:new[]{a,b,a+1,a+1,b,b+1});
            }
            Surface(name,v,t,uv,null,new Vector3(x,0,0),Vector3.one,material,clothes);
        }
        void ShoeDiagonalPanel(float x,SneakerShape shape,int side,float back,Color color)
        {
            var v=new List<Vector3>();var t=new List<int>();var uv=new List<Vector2>();
            for(int row=0;row<=8;row++)for(int col=0;col<2;col++){
                float f=row/8f,a=side*Mathf.Lerp(.72f,1.55f,f),z=back-f*.09f-col*.045f;
                var section=ShoeSection(shape,z);var normal=new Vector3(Mathf.Sin(a),Mathf.Cos(a),0);
                v.Add(new Vector3(Mathf.Sin(a)*section.x,section.y+Mathf.Cos(a)*section.z,z)+normal*.010f);uv.Add(new Vector2(col*.1f,f*.3f));
            }
            for(int row=0;row<8;row++){int a=row*2;t.AddRange(side>0?new[]{a,a+2,a+1,a+1,a+2,a+3}:new[]{a,a+1,a+2,a+1,a+3,a+2});}
            Surface("Tênis — faixas diagonais",v,t,uv,null,new Vector3(x,0,0),Vector3.one,SurfaceMaterial(color,"Leather"),clothes);
        }
        internal const string SlideFootMeshName="Pé do slide";
        const string CloudSlideId="AH_SHOES_02";
        void BuildSlideFoot(int side)
        {
            var foot=new SneakerShape{
                z=new[]{.28f,.22f,.12f,-.02f,-.14f,-.29f,-.41f,-.44f},
                w=new[]{.08f,.113f,.113f,.108f,.107f,.117f,.095f,.040f},
                y=new[]{.232f,.27f,.275f,.23f,.224f,.218f,.213f,.205f},
                h=new[]{.04f,.142f,.145f,.066f,.047f,.040f,.031f,.016f},
                soleY=new[]{.184f,.184f,.184f,.184f,.184f,.184f,.184f,.184f},
                soleH=new[]{.014f,.014f,.014f,.014f,.014f,.014f,.014f,.014f}
            };
            ShoeVolume(SlideFootMeshName,side*.24f,skin,foot,false);
        }
        void BuildShoes(Garment item,int side)
        {
            if(BuildStreetwearShoes(item,side)){
                if(item.id==CloudSlideId)BuildSlideFoot(side);
                return;
            }
            var shape=Sneaker(item);float x=side*.24f;var cream=new Color(.86f,.84f,.76f);
            var upper=SurfaceMaterial(item.color,shape.style==2?"Leather":"Cloth");
            upper.SetFloat("_FabricWeight",.20f);
            var soleColor=cream;
            ShoeVolume("Tênis",x,upper,shape,false);ShoeVolume("Sola",x,SurfaceMaterial(soleColor,"Rubber"),shape,true);
            if(shape.style==0){
                ShoeVolume("Sola — borracha de tração",x,SurfaceMaterial(new Color(.12f,.14f,.16f),"Rubber"),shape,true,.22f,.78f);
                for(int lateral=-1;lateral<=1;lateral+=2){
                    for(int rib=0;rib<3;rib++)ShoePanel("Tênis — suporte lateral",x,shape,.12f-rib*.105f,.075f-rib*.105f,lateral*.65f,lateral*1.55f,SurfaceMaterial(new Color(.56f,.59f,.59f),"Leather"));
                    ShoePanel("Tênis — contraforte",x,shape,.23f,.115f,lateral*.55f,lateral*1.65f,SurfaceMaterial(new Color(.25f,.28f,.29f),"Leather"));
                }
                ShoePanel("Tênis — reforço da biqueira",x,shape,-.30f,-.485f,-1.2f,1.2f,SurfaceMaterial(item.color*.81f,"Leather"));
                Tube("Tênis — puxador do calcanhar",new[]{new Vector3(x-.045f,.36f,.28f),new Vector3(x-.045f,.48f,.29f),new Vector3(x+.045f,.48f,.29f),new Vector3(x+.045f,.36f,.28f)},.012f,SurfaceMaterial(new Color(.19f,.23f,.25f),"Leather"),clothes);
            }else if(shape.style==1){
                // Rubber toe is a material region of the upper itself: no
                // intersecting overlay shell or separate bead at the toe tip.
                for(int lateral=-1;lateral<=1;lateral+=2)ShoePanel("Tênis — painel de lona",x,shape,.17f,-.19f,lateral*.65f,lateral*1.55f,SurfaceMaterial(new Color(.27f,.28f,.30f),"Cloth"));
                var border=new List<Vector3>();for(int i=0;i<shape.z.Length;i++)border.Add(new Vector3(x+shape.w[i],.158f,shape.z[i]));
                for(int i=shape.z.Length-1;i>=0;i--)border.Add(new Vector3(x-shape.w[i],.158f,shape.z[i]));
                Tube("Sola — filete skate",border.ToArray(),.0055f,SurfaceMaterial(new Color(.14f,.15f,.16f),"Rubber"),clothes,true);
            }else{
                ShoeVolume("Sola — piso verde escuro",x,SurfaceMaterial(item.color*.30f,"Rubber"),shape,true,.20f,.82f);
                ShoePanel("Tênis — sobreposição da biqueira",x,shape,-.25f,-.475f,-1.25f,1.25f,SurfaceMaterial(item.color*.62f,"Leather"));
                for(int lateral=-1;lateral<=1;lateral+=2){
                    for(int stripe=0;stripe<3;stripe++)ShoeDiagonalPanel(x,shape,lateral,.095f-stripe*.095f,cream);
                    ShoePanel("Tênis — reforço do calcanhar",x,shape,.25f,.145f,lateral*.45f,lateral*1.65f,SurfaceMaterial(item.color*.52f,"Leather"));
                }
            }
            float collarY=shape.style==0?.445f:shape.style==1?.545f:.445f;
            var lining=SurfaceMaterial(item.color*.5f,"Leather");
            Tube("Tênis — colarinho",EllipsePath(new Vector3(x,collarY,.12f),shape.style==1?.13f:.105f,.12f,true),shape.style==1?.028f:.02f,lining,clothes,true);
            ShoePanel("Tênis — língua costurada",x,shape,.15f,-.22f,-.50f,.50f,SurfaceMaterial(item.color*.70f,"Leather"));
            var thread=SurfaceMaterial(cream,"Cloth");int laces=shape.style==0?3:shape.style==1?4:5;
            for(int lace=0;lace<laces;lace++){
                float z=.075f-lace*(shape.style==2?.050f:.060f);int strokes=2;
                for(int stroke=0;stroke<strokes;stroke++){
                    var path=new Vector3[9];for(int i=0;i<path.Length;i++){
                        float f=i/(float)(path.Length-1),across=(f*2-1)*(shape.style==2?.10f:.12f);
                        float depth=z-(stroke==0?f:1-f)*.048f;
                        path[i]=new Vector3(x+across,ShoeTop(shape,across,depth)+.010f,depth);
                    }
                    Tube("Cadarço",path,shape.style==2?.0065f:.009f,thread,clothes);
                }
            }
        }
    }
}
