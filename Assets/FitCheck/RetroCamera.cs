using UnityEngine;

namespace FitCheck
{
    [RequireComponent(typeof(Camera))]
    public class RetroCamera : MonoBehaviour
    {
        public bool Filter=true,ReducedMotion,Paused;float paintTime;
        public const int SoftPixelHeight=720;
        Camera sceneCamera,clearCamera;RenderTexture world;
        public int OverrideFrame=-1;
        void Awake()
        {
            sceneCamera=GetComponent<Camera>();
            var clear=new GameObject("Composição — fundo da tela");clear.transform.SetParent(transform);
            clearCamera=clear.AddComponent<Camera>();clearCamera.depth=-100;clearCamera.cullingMask=0;
            clearCamera.clearFlags=CameraClearFlags.SolidColor;clearCamera.backgroundColor=new Color(.04f,.025f,.06f);
            EnsureTarget();
        }
        void Update()
        {
            AdvancePaint(Time.unscaledDeltaTime);
        }
        public void AdvancePaint(float delta)
        {
            if(!ReducedMotion&&!Paused)paintTime+=delta;
            Shader.SetGlobalFloat("_PaintFrame",OverrideFrame>=0?OverrideFrame:ReducedMotion?0:paintTime*2.5f);
            Shader.SetGlobalFloat("_BackdropTime",OverrideFrame>=0?OverrideFrame/2.5f:ReducedMotion?0:paintTime);
        }
        public void ValidateBackdropClock()
        {
            float oldTime=paintTime;bool oldPaused=Paused,oldReduced=ReducedMotion;int oldOverride=OverrideFrame;
            try{
                OverrideFrame=-1;Paused=false;ReducedMotion=false;AdvancePaint(.1f);float before=Shader.GetGlobalFloat("_BackdropTime");
                AdvancePaint(.1f);if(Shader.GetGlobalFloat("_BackdropTime")<=before)throw new System.Exception("Fundo não avança");
                Paused=true;before=Shader.GetGlobalFloat("_BackdropTime");AdvancePaint(.5f);
                if(Shader.GetGlobalFloat("_BackdropTime")!=before)throw new System.Exception("Fundo continuou durante a pausa");
                ReducedMotion=true;AdvancePaint(.5f);if(Shader.GetGlobalFloat("_BackdropTime")!=0)throw new System.Exception("Fundo não congelou com movimento reduzido");
            }finally{paintTime=oldTime;Paused=oldPaused;ReducedMotion=oldReduced;OverrideFrame=oldOverride;AdvancePaint(0);}
        }
        void LateUpdate(){EnsureTarget();}
        void EnsureTarget()
        {
            if(sceneCamera==null)return;
            if(!Filter){sceneCamera.targetTexture=null;return;}
            int height=Mathf.Min(SoftPixelHeight,Screen.height),width=Mathf.Max(1,Mathf.RoundToInt(height*(float)Screen.width/Mathf.Max(1,Screen.height)));
            if(world==null||world.width!=width||world.height!=height){
                sceneCamera.targetTexture=null;if(world!=null){world.Release();Destroy(world);}
                world=new RenderTexture(width,height,24,RenderTextureFormat.ARGB32){name="PS1 — imagem 3D",filterMode=FilterMode.Point,antiAliasing=1,useMipMap=false};world.Create();
            }
            sceneCamera.targetTexture=world;
        }
        public void DrawWorld(){if(Filter&&world!=null)GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),world,ScaleMode.StretchToFill,false);}
        void OnDestroy(){if(world!=null){world.Release();Destroy(world);}}
    }
}
