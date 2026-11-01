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
    }
}
