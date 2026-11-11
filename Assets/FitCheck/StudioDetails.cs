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
        enum HairFit { Free, Glasses, Cap, Knit, Hood, Headphones }
        static HairFit HairFitFor(Garment headwear)
        {
            if(headwear==null)return HairFit.Free;
            if(headwear.id=="PLAYER_PINK_GLASSES")return HairFit.Glasses;
            if(headwear.id==GhostHood)return HairFit.Hood;
            if(headwear.id==GarageDjHeadphones)return HairFit.Headphones;
            return headwear.cap?HairFit.Cap:HairFit.Knit;
        }
        void BuildHair(int profile,Garment headwear,PlayerAppearance appearance)
        {
            int naturalCut=appearance?.hair??(profile==5?PlayerAppearance.BlackPowerHair:profile==3?6:profile==6?0:profile);
            int cut=!applyingPlayer&&HairOverride>=0?HairOverride:naturalCut;
            currentHairProfile=cut;
            foreach(Transform part in face)if(part.name=="Orelha")part.gameObject.SetActive(cut!=6&&cut!=PlayerAppearance.BlackPowerHair);
            var color=cut>=PlayerAppearance.BlackPowerHair?new Color(.065f,.043f,.034f):
                new[]{new Color(.065f,.038f,.025f),new Color(.49f,.17f,.065f),new Color(.08f,.045f,.03f),new Color(.12f,.065f,.115f),new Color(.36f,.37f,.39f),new Color(.78f,.59f,.27f),new Color(.21f,.09f,.055f)}[cut];
            if(appearance!=null)color=PlayerAppearance.HairColors[appearance.hairColor];
            bool coils=cut>=PlayerAppearance.BlackPowerHair;
            if(coils&&color.maxColorComponent<.10f)color=new Color(.115f,.093f,.077f);
            var strands=SurfaceMaterial(color,"Hair");strands.SetFloat("_HairFlow",coils?0:1);strands.SetFloat("_Outline",0);
            if(coils){
                var texture=new Texture2D(64,64,TextureFormat.RGB24,false){name="Cachos crespos",filterMode=FilterMode.Point,wrapMode=TextureWrapMode.Repeat};
                var pixels=new Color[64*64];
                for(int y=0;y<64;y++)for(int x=0;x<64;x++){
                    float nearest=4;
                    Vector2 point=new Vector2(x/8f,y/8f);
                    var cell=new Vector2(Mathf.Floor(point.x),Mathf.Floor(point.y));
                    for(int cy=-1;cy<=1;cy++)for(int cx=-1;cx<=1;cx++){
                        Vector2 neighbor=cell+new Vector2(cx,cy);
                        float seed=neighbor.x*127.1f+neighbor.y*311.7f;
                        float jitterX=Mathf.Repeat(Mathf.Sin(seed)*43758.5453f,1);
                        float jitterY=Mathf.Repeat(Mathf.Sin(seed+74.7f)*24634.6345f,1);
                        nearest=Mathf.Min(nearest,(point-neighbor-new Vector2(jitterX,jitterY)).sqrMagnitude);
                    }
                    float curl=.5f+.5f*Mathf.Cos(Mathf.Sqrt(nearest)*17);
                    pixels[y*64+x]=Color.white*(.84f+curl*.15f);
                }
                texture.SetPixels(pixels);texture.Apply(false,true);Own(texture);strands.mainTexture=texture;
            }
            if(cut!=2){
                HairFit fit=HairFitFor(headwear);
                HairShell(cut,strands,headwear,fit);
                if(cut==1)BuildFittedBun(fit,strands);
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
        void BuildFittedBun(HairFit fit,Material strands)
        {
            Vector3 center=new Vector3(0,3.40f,.23f);
            Vector3 size=new Vector3(.49f,.49f,.46f);
            switch(fit){
                case HairFit.Cap:
                    center=new Vector3(0,3.04f,.565f);
                    size=new Vector3(.35f,.30f,.36f);
                    break;
                case HairFit.Knit:
                    center=new Vector3(0,2.995f,.55f);
                    size=new Vector3(.37f,.30f,.36f);
                    break;
                case HairFit.Hood:
                    center=new Vector3(0,3.12f,.38f);
                    size=new Vector3(.32f,.32f,.32f);
                    break;
                case HairFit.Headphones:
                    center=new Vector3(0,3.32f,.36f);
                    size=new Vector3(.43f,.43f,.43f);
                    break;
            }
            Ellipsoid("Coque — união à nuca",center,size,strands,hair,24,12);
        }
        void FitAppearanceSilhouette()
        {
            if(!feminineAppearance)return;
            foreach(var filter in clothes.GetComponentsInChildren<MeshFilter>()){
                var source=filter.sharedMesh;
                if(source==null)continue;
                var vertices=source.vertices;
                bool changed=false;
                for(int i=0;i<vertices.Length;i++){
                    Vector3 point=Avatar.InverseTransformPoint(filter.transform.TransformPoint(vertices[i]));
                    if(point.y<.92f||point.y>1.84f||Mathf.Abs(point.x)>.44f)continue;
                    float waist=Mathf.Exp(-Mathf.Pow((point.y-1.51f)/.18f,2));
                    float hip=Mathf.Exp(-Mathf.Pow((point.y-1.15f)/.17f,2));
                    point.x*=1-.075f*waist+.055f*hip;
                    vertices[i]=filter.transform.InverseTransformPoint(Avatar.TransformPoint(point));
                    changed=true;
                }
                if(!changed)continue;
                var fitted=Instantiate(source);fitted.name=source.name;
                fitted.vertices=vertices;fitted.RecalculateNormals();fitted.RecalculateBounds();
                Own(fitted);filter.sharedMesh=fitted;
            }
        }
    }
}
