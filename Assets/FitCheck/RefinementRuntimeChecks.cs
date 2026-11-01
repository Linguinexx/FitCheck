using System;
using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        public void RefinementSmokeCheck()
        {
            int oldCustomer=customer,oldHair=studio.HairOverride,oldExpression=studio.ExpressionOverride;var oldOutfit=outfit;
            bool oldPreview=studio.PreviewProportions,oldTall=studio.TallPreview;string saved=JsonUtility.ToJson(progress);
            var skins=new HashSet<Color>();var hairs=new HashSet<Color>();var expressions=new HashSet<AvatarExpression>();var cuts=new HashSet<string>();
            try{
                studio.HairOverride=-1;studio.ExpressionOverride=-1;studio.PreviewProportions=false;studio.TallPreview=false;
                foreach(int client in new[]{0,1,2,3,5,8}){
                    customer=client;outfit=Catalog.Default;outfit[3]="";Apply();expressions.Add(studio.CurrentExpression);cuts.Add(studio.HairName);
                    foreach(var r in studio.Avatar.GetComponentsInChildren<Renderer>()){
                        if(r.name=="Cabeça arredondada")skins.Add(r.sharedMaterial.color);
                        if(r.name=="Cabelo ajustado à cabeça")hairs.Add(r.sharedMaterial.color);
                    }
                }
                if(skins.Count!=6||hairs.Count!=5||expressions.Count!=6||cuts.Count!=6)throw new Exception("Perfis não possuem identidade suficiente");
                outfit[3]="AH_HEAD_01";Apply();Renderer crown=null,cuff=null;bool label=false;
                foreach(var r in studio.Avatar.GetComponentsInChildren<Renderer>()){
                    if(r.name=="Gorro")crown=r;if(r.name=="Dobra do gorro")cuff=r;if(r.name=="Etiqueta do gorro")label=true;
                }
                if(crown==null||cuff==null||!label||cuff.bounds.size.y<.13f)throw new Exception("Gorro sem dobra/etiqueta");
                var crownVertices=crown.GetComponent<MeshFilter>().sharedMesh.vertices;Vector3 apex=crownVertices[crownVertices.Length-1];
                if(Mathf.Abs(apex.x)>.08f||apex.y<.30f||apex.y>.40f)throw new Exception("Gorro perdeu o topo compacto e arredondado");
                ValidateDescendingBackdrop();cam.GetComponent<RetroCamera>().ValidateBackdropClock();
                if(JsonUtility.ToJson(progress)!=saved)throw new Exception("Teste visual alterou a campanha");
                Debug.Log("FITCHECK_REFINEMENT_PASSED: 6 tons de pele, 5 cores de cabelo, 6 cortes/estados e 6 expressões; gorro com dobra e topo folgado; fundo desce, fecha o ciclo e respeita pausa/movimento reduzido.");
            }finally{
                customer=oldCustomer;outfit=oldOutfit;studio.HairOverride=oldHair;studio.ExpressionOverride=oldExpression;studio.PreviewProportions=oldPreview;studio.TallPreview=oldTall;Apply();
            }
        }
        void ValidateDescendingBackdrop()
        {
            var panel=GameObject.Find("Fundo abstrato — fluxo descendente");if(panel==null)throw new Exception("Painel abstrato ausente");
            int oldLayer=panel.layer;float oldTime=Shader.GetGlobalFloat("_BackdropTime");var active=RenderTexture.active;
            var cameraObject=new GameObject("Teste isolado do fluxo");var testCamera=cameraObject.AddComponent<Camera>();testCamera.enabled=false;
            var target=RenderTexture.GetTemporary(256,512,24);var image=new Texture2D(256,512,TextureFormat.RGB24,false);
            try{
                panel.layer=31;testCamera.cullingMask=1<<31;testCamera.clearFlags=CameraClearFlags.SolidColor;testCamera.backgroundColor=Color.black;
                testCamera.fieldOfView=36;testCamera.aspect=.5f;testCamera.targetTexture=target;
                testCamera.transform.position=new Vector3(0,2.2f,2.51f-2.2f/Mathf.Tan(18*Mathf.Deg2Rad));testCamera.transform.rotation=Quaternion.identity;
                Func<float,Color32[]> render=time=>{Shader.SetGlobalFloat("_BackdropTime",time);testCamera.Render();RenderTexture.active=target;image.ReadPixels(new Rect(0,0,256,512),0,0);image.Apply();return image.GetPixels32();};
                var a=render(0);var b=render(32f/(512*.065f));var loop=render(1/.065f);var still=render(1/.065f);
                long down=0,up=0,seam=0;int pixels=0;
                for(int y=32;y<480;y++)for(int x=0;x<256;x++){
                    int i=y*256+x;down+=PixelDifference(b[i],a[i+32*256]);up+=PixelDifference(b[i],a[i-32*256]);pixels++;
                }
                for(int i=0;i<a.Length;i++){
                    seam+=PixelDifference(a[i],loop[i]);if(!loop[i].Equals(still[i]))throw new Exception("Fundo congelado varia sem avanço do relógio");
                }
                if(down/(pixels*3f)>.8f||up<=down+pixels)throw new Exception("Fluxo não está descendo por deslocamento contínuo");
                if(seam/(a.Length*3f)>.1f)throw new Exception("Emenda visível no ciclo do fundo");
                Debug.Log("FITCHECK_BACKDROP_PASSED: deslocamento real de 32 pixels para baixo; erro médio "+(down/(pixels*3f)).ToString("F4")+", ciclo "+(seam/(a.Length*3f)).ToString("F4")+".");
            }finally{
                panel.layer=oldLayer;Shader.SetGlobalFloat("_BackdropTime",oldTime);RenderTexture.active=active;testCamera.targetTexture=null;
                RenderTexture.ReleaseTemporary(target);Destroy(image);Destroy(cameraObject);
            }
        }
        static int PixelDifference(Color32 a,Color32 b){return Math.Abs(a.r-b.r)+Math.Abs(a.g-b.g)+Math.Abs(a.b-b.b);}
    }
}
