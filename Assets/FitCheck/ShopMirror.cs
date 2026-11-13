using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public static readonly Vector3 ShopMirrorCenter=new Vector3(-2.365f,1.88f,6.15f);
        const float ShopMirrorHalfWidth=.63f,ShopMirrorHalfHeight=1.65f;
        [SerializeField] Material shopMirrorMaterial;
        Camera shopMirrorCamera;float nextShopMirrorFrame;
        void BuildShopMirror(){
            // Outside face of the fitting partition: hooks, bench and animated
            // interior panel stay untouched; no floor footprint or new collider.
            Block(ShopMirrorCenter-Vector3.right*.065f,new Vector3(.12f,3.47f,1.45f),Ink,"CityMetal",.035f);
            Block(ShopMirrorCenter-Vector3.right*.018f,new Vector3(.025f,3.37f,1.32f),new Color(.46f,.44f,.40f),"CityMetal",.012f);
            Block(ShopMirrorCenter+new Vector3(.005f,1.71f,0),new Vector3(.025f,.028f,.30f),Kaos,"CityMetal",.005f);
            float w=ShopMirrorHalfWidth,h=ShopMirrorHalfHeight;
            var mesh=new Mesh{name="Espelho da loja — vidro",
                vertices=new[]{new Vector3(0,-h,-w),new Vector3(0,h,-w),new Vector3(0,h,w),new Vector3(0,-h,w)},
                uv=new[]{Vector2.right,Vector2.one,Vector2.up,Vector2.zero},triangles=new[]{0,1,2,0,2,3}};
            mesh.RecalculateNormals();mesh.RecalculateBounds();assets.Add(mesh);
            var glass=new GameObject("Espelho da loja — lateral externa do provador");glass.transform.SetParent(transform,false);glass.transform.localPosition=ShopMirrorCenter;
            glass.AddComponent<MeshFilter>().sharedMesh=mesh;
            shopMirrorMaterial=new Material(Resources.Load<Shader>("Artwork")){mainTexture=Texture2D.blackTexture};assets.Add(shopMirrorMaterial);
            glass.AddComponent<MeshRenderer>().sharedMaterial=shopMirrorMaterial;
        }
        public bool RenderShopMirror(Camera source,bool external=false){
            if(!IsInterior||IsHome)return false;
            return RenderWallMirror(source,ShopMirrorCenter,ShopMirrorHalfWidth,ShopMirrorHalfHeight,1,ShopBounds,
                shopMirrorMaterial,ref shopMirrorCamera,ref nextShopMirrorFrame,"Reflexo da loja",external);
        }
        bool RenderWallMirror(Camera source,Vector3 localCenter,float halfWidth,float halfHeight,float facing,Rect room,
            Material surface,ref Camera reflection,ref float nextFrame,string name,bool external){
            if(source==null||surface==null||!PlayerBodyReady||Time.unscaledTime<nextFrame)return false;
            var center=transform.TransformPoint(localCenter);var eye=source.transform.position;
            float distance=(eye.x-center.x)*facing;if(distance<.1f)return false;
            GeometryUtility.CalculateFrustumPlanes(source,mirrorViewPlanes);
            if(!GeometryUtility.TestPlanesAABB(mirrorViewPlanes,new Bounds(center,new Vector3(.05f,2*halfHeight,2*halfWidth))))return false;
            nextFrame=Time.unscaledTime+1f/15;
            if(reflection==null){
                var target=new RenderTexture(128,352,16,RenderTextureFormat.ARGB32){name=name,filterMode=FilterMode.Point};assets.Add(target);
                var node=new GameObject("Câmera — "+name);node.transform.SetParent(transform,false);reflection=node.AddComponent<Camera>();reflection.enabled=false;
                reflection.clearFlags=CameraClearFlags.SolidColor;reflection.backgroundColor=new Color(.065f,.06f,.07f);
                reflection.allowHDR=false;reflection.allowMSAA=false;reflection.targetTexture=target;surface.mainTexture=target;
            }
            var reflectedEye=eye-Vector3.right*(2*(eye.x-center.x));
            reflection.transform.SetPositionAndRotation(reflectedEye,Quaternion.LookRotation(Vector3.right*facing,Vector3.up));
            float near=distance+.015f,scale=near/distance,x=(center.z-eye.z)*-facing,y=center.y-eye.y;
            float far=distance+Mathf.Sqrt(room.width*room.width+room.height*room.height)+2;
            reflection.cullingMask=source.cullingMask;reflection.nearClipPlane=near;reflection.farClipPlane=far;
            reflection.projectionMatrix=Matrix4x4.Frustum((x-halfWidth)*scale,(x+halfWidth)*scale,(y-halfHeight)*scale,(y+halfHeight)*scale,near,far);
            EnsurePlayerPose();FrameEmoteArms(true);
            try{foreach(var piece in playerHeadPieces)if(piece!=null)piece.SetActive(PlayerFaceVisible(piece));reflection.Render();}
            finally{FramePlayerForCamera(source,external);}
            return true;
        }
    }
}
