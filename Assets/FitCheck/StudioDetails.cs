using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        public int HairOverride=-1;
        int currentHairProfile;
        public string HairName=>PlayerAppearance.HairNames[currentHairProfile];
        public void CycleHair(){HairOverride=(currentHairProfile+1)%PlayerAppearance.HairNames.Length;}
        void BuildHair(int profile,Garment headwear)
        {
            int cut=applyingPlayer?PlayerLook.hair:HairOverride>=0?HairOverride:profile==6?0:profile;currentHairProfile=cut;
            foreach(Transform part in face)if(part.name=="Orelha")part.gameObject.SetActive(cut!=6);
            var color=new[]{new Color(.065f,.038f,.025f),new Color(.49f,.17f,.065f),new Color(.08f,.045f,.03f),new Color(.12f,.065f,.115f),new Color(.36f,.37f,.39f),new Color(.78f,.59f,.27f),new Color(.21f,.09f,.055f)}[cut];
            if(applyingPlayer)color=PlayerAppearance.HairColors[PlayerLook.hairColor];
            var strands=SurfaceMaterial(color,"Hair");strands.SetFloat("_HairFlow",1);strands.SetFloat("_Outline",0);
            if(cut!=2){
                HairShell(cut,strands,headwear,null);
                if(cut==1){
                    Vector3 p=headwear==null?new Vector3(0,3.40f,.23f):headwear.cap?new Vector3(0,2.96f,.59f):new Vector3(0,2.87f,.60f);
                    Vector3 size=headwear==null?new Vector3(.49f,.49f,.46f):new Vector3(.32f,.28f,.34f);
                    Ellipsoid("Coque — união à nuca",p,size,strands,hair,24,12);
                }
                if(cut==6)LongWavyHair(strands);
            }
            if(profile==4)Ellipsoid("Bigode",new Vector3(0,2.60f,-.534f),new Vector3(.25f,.055f,.05f),SurfaceMaterial(new Color(.40f,.37f,.33f),"Hair"),hair,16,8);
            if(profile==3){
                var frame=SurfaceMaterial(new Color(.08f,.055f,.04f),"Leather");
                for(int side=-1;side<=1;side+=2){
                    Tube("Armação arredondada",EllipsePath(new Vector3(side*.23f,2.88f,-.63f),.205f,.215f),.013f,frame,hair,true);
                    Tube("Haste dos óculos",new[]{new Vector3(side*.44f,2.93f,-.63f),new Vector3(side*.52f,2.93f,-.34f),new Vector3(side*.56f,2.90f,.03f)},.011f,frame,hair);
                }
                Tube("Ponte dos óculos",new[]{new Vector3(-.035f,2.92f,-.63f),new Vector3(0,2.94f,-.64f),new Vector3(.035f,2.92f,-.63f)},.011f,frame,hair);
            }
        }
        void LongWavyHair(Material material)
        {
            const int sides=48,rows=14;int stride=sides+1,layer=(rows+1)*stride;
            var v=new List<Vector3>();var uv=new List<Vector2>();var t=new List<int>();
            for(int shell=0;shell<2;shell++)for(int row=0;row<=rows;row++)for(int i=0;i<=sides;i++){
                float f=row/(float)rows,theta=Mathf.Lerp(.88f,Mathf.PI*2-.88f,i/(float)sides);
                float wave=Mathf.Sin(theta*7-f*5)*.019f*Mathf.Sin(f*Mathf.PI);
                float radius=Mathf.Lerp(.535f,.66f,Mathf.SmoothStep(0,1,f))+wave;
                float y=Mathf.Lerp(3.075f,2.07f+.035f*Mathf.Cos(theta*7),f);
                float headSection=Mathf.Sqrt(Mathf.Max(0,.61f*.61f-(y-2.8f)*(y-2.8f)));
                radius=Mathf.Max(radius,headSection+.015f)-shell*.065f;
                v.Add(new Vector3(Mathf.Sin(theta)*radius,y,-Mathf.Cos(theta)*radius*.95f));uv.Add(new Vector2(theta*.44f+f*.14f,y/AvatarTileSize));
            }
            for(int shell=0;shell<2;shell++)for(int r=0;r<rows;r++)for(int i=0;i<sides;i++){
                int a=shell*layer+r*stride+i,b=a+stride;
                if(shell==0)t.AddRange(new[]{a,b,a+1,a+1,b,b+1});else t.AddRange(new[]{a,a+1,b,a+1,b+1,b});
            }
            for(int i=0;i<sides;i++){
                int a=i,b=rows*stride+i;t.AddRange(new[]{a,a+1,a+layer,a+1,a+layer+1,a+layer,b,b+layer,b+1,b+1,b+layer,b+layer+1});
            }
            for(int r=0;r<rows;r++){
                int a=r*stride,b=a+sides;t.AddRange(new[]{a,a+layer,a+stride,a+stride,a+layer,a+layer+stride,b,b+stride,b+layer,b+stride,b+layer+stride,b+layer});
            }
            for(int triangle=0;triangle<t.Count;triangle+=3){int swap=t[triangle+1];t[triangle+1]=t[triangle+2];t[triangle+2]=swap;}
            Surface("Ondulado longo — volume contínuo",v,t,uv,null,Vector3.zero,Vector3.one,material,hair);
        }
    }
}
