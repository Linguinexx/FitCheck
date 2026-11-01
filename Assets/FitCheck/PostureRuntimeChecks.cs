using System;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        public void PostureSmokeCheck()
        {
            int oldCustomer=customer;var oldOutfit=outfit;float oldRotation=rotation;
            bool oldPreview=studio.PreviewProportions,oldTall=studio.TallPreview,oldReduced=studio.ReducedMotion;
            string saved=JsonUtility.ToJson(progress);int cases=0;
            try{
                foreach(bool tall in new[]{false,true})foreach(var top in Catalog.All){
                    if(top.slot!=0)continue;
                    studio.PreviewProportions=true;studio.TallPreview=tall;customer=cases%9;
                    outfit=Catalog.Default;outfit[0]=top.id;rotation=cases%2==0?73:-51;UpdateCamera();Apply();
                    foreach(bool reduced in new[]{false,true}){
                        studio.ReducedMotion=reduced;studio.Animate(.37f,true);studio.ValidateArmFit();
                        Transform torso=null;foreach(var transform in studio.Avatar.GetComponentsInChildren<Transform>())if(transform.name=="Postura — tronco e braços")torso=transform;
                        if(torso==null||Quaternion.Angle(torso.localRotation,Quaternion.identity)<1)throw new Exception("Postura relaxada ausente");
                        var position=torso.localPosition;var angle=torso.localRotation;studio.Animate(.5f,false);
                        if(Vector3.Distance(position,torso.localPosition)>.0001f||Quaternion.Angle(angle,torso.localRotation)>.01f)throw new Exception("Postura continuou animando durante a pausa");
                        cases++;
                    }
                }
                if(JsonUtility.ToJson(progress)!=saved)throw new Exception("Teste de postura alterou a campanha");
                Debug.Log("FITCHECK_POSTURE_PASSED: "+cases+" combinações de blusas, corpos e movimento reduzido; braços centrados, aberturas sem atravessar a pele, mãos unidas e pausa estável.");
            }finally{
                customer=oldCustomer;outfit=oldOutfit;rotation=oldRotation;studio.PreviewProportions=oldPreview;studio.TallPreview=oldTall;studio.ReducedMotion=oldReduced;Apply();UpdateCamera();
            }
        }
    }
}
