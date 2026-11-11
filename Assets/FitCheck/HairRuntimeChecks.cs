using System;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        public void HairFitSmokeCheck()
        {
            int oldCustomer=customer,oldHair=studio.HairOverride;var oldOutfit=outfit;float oldRotation=rotation;
            bool oldPreview=studio.PreviewProportions,oldTall=studio.TallPreview;string state=JsonUtility.ToJson(progress);int cases=0,worn=0;
            try{
                foreach(bool tall in new[]{false,true})for(int cut=0;cut<PlayerAppearance.HairNames.Length;cut++)foreach(string hat in new[]{"","CG_HEAD_01","SB_HEAD_01","AH_HEAD_01"}){
                    studio.PreviewProportions=true;studio.TallPreview=tall;studio.HairOverride=cut;customer=0;
                    outfit=new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",hat};rotation=cases%2==0?131:-46;UpdateCamera();Apply();
                    Renderer scalp=null,crown=null,bun=null;
                    foreach(var r in studio.Avatar.GetComponentsInChildren<Renderer>()){
                        if(cut==6&&r.name=="Orelha")throw new Exception("Ondulado longo deixou a orelha atravessar o volume");
                        if(r.name=="Cabelo ajustado à cabeça")scalp=r;
                        if(r.name=="Boné"||r.name=="Gorro")crown=r;
                        if(r.name=="Coque — união à nuca")bun=r;
                        if(r.name=="Mecha K7")throw new Exception("Bolinha removida voltou ao cabelo");
                    }
                    if((cut==2)!=(scalp==null))throw new Exception("Corte ausente ou cabelo apareceu no perfil sem cabelo");
                    if(hat=="AH_HEAD_01"){
                        Renderer cuff=null;float eyeTop=float.MinValue;
                        foreach(var r in studio.Avatar.GetComponentsInChildren<Renderer>()){
                            if(r.name=="Dobra do gorro")cuff=r;if(r.name=="Branco do olho")eyeTop=Mathf.Max(eyeTop,r.bounds.max.y);
                        }
                        if(cuff==null||cuff.bounds.min.y-eyeTop<.075f)throw new Exception("Gorro encostou no olho");
                    }
                    if(scalp!=null){
                        var mesh=scalp.GetComponent<MeshFilter>().sharedMesh;
                        foreach(var v in mesh.vertices){
                            float radius=Vector3.Distance(scalp.transform.TransformPoint(v),scalp.transform.position);
                            if(radius<.561f||radius>(cut==6?1.2f:cut>=PlayerAppearance.BlackPowerHair?1.0f:.77f))throw new Exception("Cabelo fora do limite: corte="+cut+" cobertura="+hat+" raio="+radius+" escala="+scalp.transform.lossyScale+" ponto="+v);
                            if(cut!=6&&hat!=""&&radius>.577f)throw new Exception("Cabelo não foi recolhido sob a cobertura");
                        }
                        if(hat!=""){
                            if(crown==null||scalp.bounds.min.y>=crown.bounds.min.y-.03f)throw new Exception("Cabelo sumiu embaixo do boné/touca");
                            worn++;
                        }
                        if(cut==1&&(bun==null||!bun.bounds.Intersects(scalp.bounds)))throw new Exception("Coque perdeu a união à nuca");
                    }
                    cases++;
                }
                if(JsonUtility.ToJson(progress)!=state)throw new Exception("Teste de encaixe alterou a campanha");
                Debug.Log("FITCHECK_HAIR_FIT_PASSED: "+cases+" combinações de "+PlayerAppearance.HairNames.Length+" cabelos, 4 coberturas e 2 corpos; "+worn+" encaixes com cabelo visível sob a peça, coque unido e campanha preservada.");
            }finally{
                customer=oldCustomer;outfit=oldOutfit;rotation=oldRotation;studio.HairOverride=oldHair;
                studio.PreviewProportions=oldPreview;studio.TallPreview=oldTall;Apply();UpdateCamera();
            }
        }
    }
}
