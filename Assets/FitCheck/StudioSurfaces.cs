using System;
using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        const float AvatarTileSize=1.4f;
        GameObject Surface(string name,List<Vector3> v,List<int> t,List<Vector2> uv,List<Vector3> normals,Vector3 p,Vector3 scale,Material material,Transform parent)
        {
            var mesh=new Mesh{name=name};mesh.SetVertices(v);mesh.SetTriangles(t,0);mesh.SetUVs(0,uv);
            if(normals!=null)mesh.SetNormals(normals);else mesh.RecalculateNormals();mesh.RecalculateBounds();Own(mesh);
            var go=new GameObject(name);go.transform.SetParent(parent,false);go.transform.localPosition=p;go.transform.localScale=scale;
            go.AddComponent<MeshFilter>().sharedMesh=mesh;go.AddComponent<MeshRenderer>().sharedMaterial=material;return go;
        }
        Material SurfaceMaterial(Color color,string texture="Cloth",bool moving=false)
        {
            var m=Material(color);m.mainTexture=Resources.Load<Texture2D>("Painted/"+texture);
            if(texture!="Cloth")AmbientSurfaces.ClothDetail(m,false);
            m.SetFloat("_Motion",moving?1:0);m.SetFloat("_SmoothLight",1);m.SetFloat("_Outline",.002f);return m;
        }
        GameObject Ellipsoid(string name,Vector3 p,Vector3 scale,Material material,Transform parent,int sides=24,int rows=12)
        {
            var v=new List<Vector3>();var t=new List<int>();var uv=new List<Vector2>();var n=new List<Vector3>();
            float repeat=Mathf.PI*(scale.x+scale.z)*.5f/AvatarTileSize;
            for(int r=0;r<=rows;r++)for(int i=0;i<=sides;i++){
                float phi=r*Mathf.PI/rows,theta=i*Mathf.PI*2/sides-Mathf.PI;
                var direction=new Vector3(Mathf.Sin(theta)*Mathf.Sin(phi),Mathf.Cos(phi),-Mathf.Cos(theta)*Mathf.Sin(phi));
                v.Add(direction*.5f);n.Add(direction);uv.Add(new Vector2(i/(float)sides*repeat,(1-r/(float)rows)*scale.y/AvatarTileSize));
            }
            for(int r=0;r<rows;r++)for(int i=0;i<sides;i++){
                int a=r*(sides+1)+i,b=a+sides+1;
                if(r>0)t.AddRange(new[]{a,a+1,b});if(r<rows-1)t.AddRange(new[]{b,a+1,b+1});
            }
            return Surface(name,v,t,uv,n,p,scale,material,parent);
        }
        GameObject RoundedLoft(string name,Vector3 p,Vector3 scale,Material material,Transform parent,float[] ys,float[] widths,int sides=24)
        {
            var v=new List<Vector3>();var t=new List<int>();var uv=new List<Vector2>();var n=new List<Vector3>();
            float repeat=Mathf.PI*(scale.x+scale.z)*.5f/AvatarTileSize;
            for(int r=0;r<ys.Length;r++)for(int i=0;i<=sides;i++){
                float a=i*Mathf.PI*2/sides-Mathf.PI;
                int lo=Mathf.Max(0,r-1),hi=Mathf.Min(ys.Length-1,r+1);
                float slope=(widths[hi]-widths[lo])*.5f/Mathf.Max(.001f,ys[hi]-ys[lo]);
                v.Add(new Vector3(Mathf.Sin(a)*.5f*widths[r],ys[r]-.5f,-Mathf.Cos(a)*.5f*widths[r]));
                n.Add(new Vector3(Mathf.Sin(a),-slope,-Mathf.Cos(a)).normalized);
                uv.Add(new Vector2(i/(float)sides*repeat,ys[r]*scale.y/AvatarTileSize));
            }
            for(int r=0;r<ys.Length-1;r++)for(int i=0;i<sides;i++){
                int a=r*(sides+1)+i,b=a+sides+1;t.AddRange(new[]{a,b,a+1,a+1,b,b+1});
            }
            for(int cap=0;cap<2;cap++){
                int r=cap==0?0:ys.Length-1,start=v.Count;float y=ys[r]-.5f;
                v.Add(new Vector3(0,y,0));n.Add(cap==0?Vector3.down:Vector3.up);uv.Add(new Vector2(.5f,.5f));
                for(int i=0;i<=sides;i++){
                    float a=i*Mathf.PI*2/sides-Mathf.PI;float x=Mathf.Sin(a)*.5f*widths[r],z=-Mathf.Cos(a)*.5f*widths[r];
                    v.Add(new Vector3(x,y,z));n.Add(cap==0?Vector3.down:Vector3.up);uv.Add(new Vector2(.5f+x*scale.x/AvatarTileSize,.5f+z*scale.z/AvatarTileSize));
                }
                for(int i=0;i<sides;i++)t.AddRange(cap==0?new[]{start,start+i+1,start+i+2}:new[]{start,start+i+2,start+i+1});
            }
            return Surface(name,v,t,uv,n,p,scale,material,parent);
        }
        static float HairEdge(int profile,float theta)
        {
            float back=(1-Mathf.Cos(theta))*.5f,angle=Mathf.Abs(theta);
            if(profile>=PlayerAppearance.BlackPowerHair){
                float afroFront=1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(.80f,1.28f,angle));
                float afroEdge=profile==PlayerAppearance.BlackPowerHair?1.40f+back*.57f:1.15f+back*.78f;
                return Mathf.Lerp(afroEdge,.98f+.018f*Mathf.Cos(theta*9),afroFront);
            }
            float edge=profile==1?.96f+back*1.15f:1.09f+back*.84f;
            float temple=(angle-1.16f)/.20f;edge+=Mathf.Exp(-temple*temple)*.15f;
            if(profile==0)edge+=.045f*Mathf.Sin(theta*2+.3f)*(1-back);
            if(profile==0)edge+=.095f*Mathf.Cos(theta*9);
            if(profile==3)edge=.94f+back*.95f+.18f*Mathf.SmoothStep(0,1,Mathf.InverseLerp(-.55f,.55f,theta))*(1-back);
            if(profile==4)edge-=.24f*(1-back);
            if(profile==5)edge+=(.055f-.29f*Mathf.SmoothStep(0,1,Mathf.InverseLerp(-.6f,.6f,theta)))*Mathf.Pow(Mathf.Max(0,Mathf.Cos(theta)),3);
            float front=1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(.90f,1.25f,angle));
            float hairline=profile==0?.86f+.030f*Mathf.Cos(theta*10):profile==1?.86f+.015f*Mathf.Sin(theta*2):profile==3?.865f+.020f*Mathf.Cos(theta*6)+.005f*Mathf.Cos(theta*12):profile==4?.82f+.02f*Mathf.Cos(theta*4):.81f+.08f*Mathf.SmoothStep(0,1,Mathf.InverseLerp(-.65f,.65f,theta));
            if(profile==3)hairline=1.18f+.055f*Mathf.Cos(theta*5)+.035f*Mathf.Sin(theta*2);
            if(profile==6)hairline=1.01f+.07f*Mathf.Sin(theta*2+.4f);
            return Mathf.Lerp(edge,hairline,front);

        }
        static float HairStart(int profile,float theta,Garment headwear,HairFit fit)
        {
            if(fit!=HairFit.Cap&&fit!=HairFit.Knit)return 0;
            if(headwear.cap){
                var edge=CapPoint(theta,1,headwear.id=="SB_HEAD_01")-new Vector3(0,2.8f,0);
                return Mathf.Min(Mathf.Atan2(new Vector2(edge.x,edge.z).magnitude,edge.y)-.055f,HairEdge(profile,theta)-.16f);
            }
            // A small underlap into the rolled cuff hides the upper boundary.
            return Mathf.Min(Mathf.Acos((3.19f-2.8f)/.576f)-.06f,HairEdge(profile,theta)-.16f);
        }
        GameObject HairShell(int profile,Material material,Garment headwear,HairFit fit)
        {
            const int sides=72,rows=24;var v=new List<Vector3>();var t=new List<int>();var uv=new List<Vector2>();var n=new List<Vector3>();
            bool coveredCrown=fit==HairFit.Cap||fit==HairFit.Knit;
            for(int r=0;r<=rows;r++)for(int i=0;i<=sides;i++){
                float theta=i*Mathf.PI*2/sides-Mathf.PI,start=HairStart(profile,theta,headwear,fit),edge=HairEdge(profile,theta);
                float f=r/(float)rows,phi=Mathf.Lerp(start,edge,f);
                float taper=1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(.78f,1,f)),thickness=Mathf.Lerp(.004f,.026f,taper);
                if(!coveredCrown){
                    // Broad connected forms, never raised tubes sitting above a cap.
                    float baseThickness=thickness;
                    float crown=Mathf.Pow(1-f,1.25f),wave=Mathf.Sin(phi);
                    if(profile>=PlayerAppearance.BlackPowerHair){
                        float volume=profile==PlayerAppearance.BlackPowerHair?.245f:.078f;
                        float curl=Mathf.Cos(theta*19+Mathf.Sin(phi*11)*.55f)*Mathf.Sin(phi*23);
                        float fitScale=fit==HairFit.Headphones?.78f:1;
                        thickness+=(volume*(.86f+.14f*Mathf.Sin(phi))+.024f*curl*Mathf.Sin(phi))*taper*fitScale;
                    }else if(profile==0){
                        float curl=.5f+.5f*Mathf.Cos(theta*11+Mathf.Sin(phi*8)*.65f)*Mathf.Cos(phi*12);
                        thickness+=(.065f*crown+.105f*wave*curl*curl)*taper;
                    }else if(profile==1)thickness+=(.020f*crown+.006f*wave*Mathf.Cos(theta*9-phi*3))*taper;
                    else if(profile==3||profile==5){
                        float sweep=profile==5?theta+.65f:theta-.25f;
                        float crest=Mathf.Pow(Mathf.Max(0,Mathf.Cos(sweep)),3)*wave;
                        thickness+=(.030f*crown+.080f*crest+.005f*wave*Mathf.Cos(theta*6+phi*2))*taper;
                    }else thickness+=(.022f*crown+.005f*wave*Mathf.Cos(theta*8-phi*2))*taper;
                    if(fit==HairFit.Hood&&profile>=PlayerAppearance.BlackPowerHair)thickness=baseThickness+.006f*taper;
                    else if(fit==HairFit.Hood)
                        thickness=Mathf.Lerp(baseThickness,thickness,Mathf.Lerp(.22f,.78f,Mathf.Max(0,Mathf.Cos(theta))));
                }else thickness=Mathf.Min(thickness,.016f);
                var direction=new Vector3(Mathf.Sin(theta)*Mathf.Sin(phi),Mathf.Cos(phi),-Mathf.Cos(theta)*Mathf.Sin(phi));
                var point=direction*(HeadRadius+thickness);
                if(!coveredCrown){
                    float raised=Mathf.Pow(1-f,1.4f)*(fit==HairFit.Hood?.45f:1);
                    if(profile==3)point+=new Vector3(-.035f,.035f,-.025f)*raised;
                    else if(profile==5)point+=new Vector3(.065f,.040f,-.015f)*raised;
                    else if(profile==0)point.y+=.025f*raised;
                }
                if(point.sqrMagnitude<.564f*.564f)point=point.normalized*.564f;
                if(profile==PlayerAppearance.BlackPowerHair&&!coveredCrown&&fit!=HairFit.Hood){
                    point.y+=(fit==HairFit.Headphones?.07f:.14f)*Mathf.Sqrt(taper);
                    point.x*=1-.04f*taper;
                }
                if(profile==6&&fit!=HairFit.Hood){
                    float longF=coveredCrown?Mathf.Lerp(fit==HairFit.Cap?.40f:.31f,1,f):f,u=1-longF;
                    float y=u*u*u*3.43f+3*u*u*longF*3.40f+3*u*longF*longF*2.50f+longF*longF*longF*2.07f;
                    float rho=3*u*u*longF*.72f+3*u*longF*longF*.60f+longF*longF*longF*.66f;
                    rho+=Mathf.Sin(theta*7-longF*5)*.018f*Mathf.Sin(longF*Mathf.PI);
                    if(longF>0)rho=Mathf.Max(rho,Mathf.Sqrt(Mathf.Max(0,.605f*.605f-(y-2.8f)*(y-2.8f)))+.012f);
                    var continuous=new Vector3(Mathf.Sin(theta)*rho,y-2.8f,-Mathf.Cos(theta)*rho*.97f);
                    float sideBlend=Mathf.SmoothStep(0,1,Mathf.InverseLerp(.52f,1.05f,Mathf.Abs(theta)));
                    point=Vector3.Lerp(point,continuous,sideBlend);
                    if(point.sqrMagnitude<.578f*.578f)point=point.normalized*.578f;
                    if(fit==HairFit.Glasses){
                        point.x*=1+.045f*sideBlend*Mathf.Sin(Mathf.PI*longF);
                    }
                    if(!coveredCrown&&r==0)point=new Vector3(0,.63f,0);
                }
                v.Add(point);n.Add(direction);uv.Add(new Vector2(i/(float)sides*2.6f,profile==6?(point.y+2.8f)*.8f/AvatarTileSize:(Mathf.PI-phi)*.58f/AvatarTileSize));
            }
            for(int r=0;r<rows;r++)for(int i=0;i<sides;i++){
                int a=r*(sides+1)+i,b=a+sides+1;
                if(r>0||coveredCrown)t.AddRange(new[]{a,a+1,b});
                t.AddRange(new[]{b,a+1,b+1});
            }
            var go=Surface("Cabelo ajustado à cabeça",v,t,uv,n,new Vector3(0,2.80f,0),Vector3.one,material,hair);
            var hairMesh=go.GetComponent<MeshFilter>().sharedMesh;
            if(!coveredCrown){
                hairMesh.RecalculateNormals();var normals=hairMesh.normals;
                for(int row=0;row<=rows;row++){
                    int a=row*(sides+1),b=a+sides;var average=(normals[a]+normals[b]).normalized;normals[a]=average;normals[b]=average;
                }
                var pole=Vector3.zero;for(int i=0;i<=sides;i++)pole+=normals[i];pole.Normalize();for(int i=0;i<=sides;i++)normals[i]=pole;
                hairMesh.normals=normals;
            }
            return go;
        }
        GameObject Tube(string name,Vector3[] path,float radius,Material material,Transform parent,bool closed=false,int sides=8,Vector3? origin=null)
        {
            var v=new List<Vector3>();var t=new List<int>();var uv=new List<Vector2>();var n=new List<Vector3>();
            int count=path.Length+(closed?1:0);float distance=0;
            for(int i=0;i<count;i++){
                int current=i%path.Length,previous=(current-1+path.Length)%path.Length,next=(current+1)%path.Length;
                Vector3 tangent=closed?(path[next]-path[previous]).normalized:
                    (i==0?path[1]-path[0]:i==path.Length-1?path[i]-path[i-1]:path[i+1]-path[i-1]).normalized;
                Vector3 across=Vector3.Cross(tangent,Mathf.Abs(tangent.y)<.9f?Vector3.up:Vector3.forward).normalized;
                Vector3 other=Vector3.Cross(tangent,across).normalized;
                if(i>0)distance+=Vector3.Distance(path[current],path[(i-1)%path.Length]);
                for(int j=0;j<=sides;j++){
                    float angle=j*Mathf.PI*2/sides;var normal=across*Mathf.Cos(angle)+other*Mathf.Sin(angle);
                    v.Add(path[current]+normal*radius-(origin??Vector3.zero));n.Add(normal);uv.Add(new Vector2(distance/AvatarTileSize,j/(float)sides));
                }
            }
            for(int i=0;i<count-1;i++)for(int j=0;j<sides;j++){
                int a=i*(sides+1)+j,b=a+sides+1;t.AddRange(new[]{a,a+1,b,a+1,b+1,b});
            }
            return Surface(name,v,t,uv,n,origin??Vector3.zero,Vector3.one,material,parent);
        }
        Vector3[] EllipsePath(Vector3 center,float width,float height,bool horizontal=false,int count=32)
        {
            var points=new Vector3[count];for(int i=0;i<count;i++){
                float a=i*Mathf.PI*2/count;points[i]=center+(horizontal?new Vector3(Mathf.Cos(a)*width,0,Mathf.Sin(a)*height):new Vector3(Mathf.Cos(a)*width,Mathf.Sin(a)*height,0));
            }return points;
        }
    }
}
