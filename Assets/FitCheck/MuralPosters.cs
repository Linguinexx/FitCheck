using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public static readonly Quaternion MuralFacing=Quaternion.Euler(0,180,0);
        static readonly Vector2[] MuralPosterSizes={new Vector2(1.08f,1.48f)*.43f,new Vector2(1.32f,1.04f)*.43f,new Vector2(.78f,1.75f)*.43f,new Vector2(1.10f,1.40f)*.43f,new Vector2(1.02f,1.35f)*.43f,new Vector2(1.30f,1.02f)*.43f,new Vector2(.86f,1.65f)*.43f};
        public static Vector2 MuralPosterSize(int id)=>MuralPosterSizes[id];
        public static Vector2 ClampMuralPoster(int id,Vector2 p,float angle){
            var half=MuralPosterSize(id)*.5f;float a=angle*Mathf.Deg2Rad,c=Mathf.Abs(Mathf.Cos(a)),s=Mathf.Abs(Mathf.Sin(a));
            var extent=new Vector2(c*half.x+s*half.y,s*half.x+c*half.y);
            return new Vector2(Mathf.Clamp(p.x,-1.40f+extent.x,1.40f-extent.x),Mathf.Clamp(p.y,-.99f+extent.y,.70f-extent.y));
        }
        public Vector3 MuralWorldPoint(Vector3 local)=>transform.TransformPoint(CollectiveMural+MuralFacing*local);
        public bool MuralPoint(Ray ray,out Vector2 point){
            point=default;var plane=new Plane(transform.TransformDirection(Vector3.forward),MuralWorldPoint(Vector3.back*.06f));
            if(!plane.Raycast(ray,out float distance))return false;
            var p=Quaternion.Inverse(MuralFacing)*(transform.InverseTransformPoint(ray.GetPoint(distance))-CollectiveMural);point=new Vector2(p.x,p.y);return true;
        }
        public bool LookingAtMural(Camera camera)=>Vector3.Dot(camera.transform.position-MuralWorldPoint(Vector3.zero),transform.TransformDirection(Vector3.forward))>.15f&&ActivityLook(camera,CollectiveMural,4.5f);
        void LambePaper(Vector3 p,int id,float size){
            var d=MuralPosterSize(id)*(size/.43f);float w=d.x*.5f,h=d.y*.5f;
            var outline=new[]{new Vector2(-w+.025f,h),new Vector2(w-.012f,h-.008f),new Vector2(w,h-.048f),new Vector2(w-.009f,-h+.037f),new Vector2(w-.039f,-h+.009f),new Vector2(w*.31f,-h+.025f),new Vector2(-w*.07f,-h),new Vector2(-w*.58f,-h+.018f),new Vector2(-w,-h+.031f),new Vector2(-w+.009f,h-.032f)};
            Color paper=id%3==0?Cream:new Color(.91f,.90f,.85f);
            for(int i=0;i<outline.Length;i++)Triangle(p,p+(Vector3)outline[i],p+(Vector3)outline[(i+1)%outline.Length],paper,"Paper");
            // Ink bands, torn lower edge and folded corner reuse the existing paper material.
            Block(p+new Vector3(0,-h*.69f,-.006f),new Vector3(w*1.74f,.066f,.007f),id%2==0?Kaos:Ink,"Paper",.002f);
            string[] labels={"FIT CHECK","LADO B","RADIO","AO VIVO","FUBA DONO DA RUA","GALERIA","QUADRA"};
            Words(labels[id],p+new Vector3(0,-h*.69f,-.012f),Mathf.Min(size*.012f,w*1.64f/(labels[id].Length*6)),Cream);
            Triangle(p+new Vector3(w-.048f,h-.008f,-.004f),p+new Vector3(w,h-.048f,-.004f),p+new Vector3(w-.048f,h-.048f,-.012f),paper*.77f,"Paper");
        }
    }
}
