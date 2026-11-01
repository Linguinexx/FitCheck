using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        bool faceExpressionNeedsRefresh;
        bool EnsureFaceBindings()
        {
            bool ready=face!=null&&smile!=null&&openMouth!=null&&surprisedMouth!=null&&mouthLineMesh!=null;
            for(int i=0;i<2;i++)ready&=eyes[i]!=null&&brows[i]!=null&&pupils[i]!=null&&pupilMeshes[i]!=null&&upperLids[i]!=null&&lowerLids[i]!=null&&upperLidObjects[i]!=null&&lowerLidObjects[i]!=null;
            if(ready)return true;
            if(Avatar==null)Avatar=transform.Find("Cliente");
            if(Avatar==null)return false; // Build has not created the avatar yet.
            if(face==null)face=Avatar.Find("Rosto — cabeça esférica");
            if(face==null)return false;
            int eye=0,brow=0;
            foreach(Transform child in face){
                if(child.name=="Olho expressivo"&&eye<2)eyes[eye++]=child;
                else if(child.name=="Sobrancelha"&&brow<2)brows[brow++]=child;
            }
            if(eye!=2||brow!=2)return false;
            for(int i=0;i<2;i++){
                pupils[i]=eyes[i].Find("Pupila");
                var upper=eyes[i].Find("Pálpebra superior");var lower=eyes[i].Find("Pálpebra inferior");
                if(pupils[i]==null||upper==null||lower==null)return false;
                upperLidObjects[i]=upper.gameObject;lowerLidObjects[i]=lower.gameObject;
                pupilMeshes[i]=FaceMeshOn(pupils[i]);upperLids[i]=FaceMeshOn(upper);lowerLids[i]=FaceMeshOn(lower);
                if(pupilMeshes[i]==null||upperLids[i]==null||lowerLids[i]==null)return false;
                lastUpper[i]=lastLower[i]=-1;
            }
            smile=face.Find("Boca");openMouth=face.Find("Boca animada");surprisedMouth=face.Find("Boca surpresa");
            if(smile==null||openMouth==null||surprisedMouth==null)return false;
            mouthLineMesh=FaceMeshOn(smile);if(mouthLineMesh==null)return false;
            // Face assets outlive every outfit. Recovering while Apply builds a
            // garment must never transfer these meshes into garmentAssets.
            foreach(var filter in face.GetComponentsInChildren<MeshFilter>(true)){
                var mesh=filter.sharedMesh;if(mesh!=null&&!owned.Contains(mesh))owned.Add(mesh);
            }
            foreach(var renderer in face.GetComponentsInChildren<MeshRenderer>(true))foreach(var material in renderer.sharedMaterials)
                if(material!=null&&!owned.Contains(material))owned.Add(material);
            faceExpressionNeedsRefresh=true;return true;
        }
        static Mesh FaceMeshOn(Transform part)
        {var filter=part.GetComponent<MeshFilter>();return filter!=null?filter.sharedMesh:null;}

        void RestoreCalfBindings()
        {
            if(Avatar==null)return;int index=0;
            foreach(Transform child in Avatar){
                if(child.name!="Canela"||index>=2)continue;
                calves[index]=child;calfMeshes[index]=FaceMeshOn(child);
                var rest=index==0?leftCalfRest:rightCalfRest;
                if(rest==null&&calfMeshes[index]!=null){
                    rest=calfMeshes[index].vertices;int side=index==0?-1:1;
                    // Older live sessions have only the posed mesh. Undo the
                    // height-based posture offset once to recover its rest data.
                    for(int v=0;v<rest.Length;v++)rest[v]=rest[v]*2-LowerPoint(rest[v],side);
                    if(index==0)leftCalfRest=rest;else rightCalfRest=rest;
                }
                calfRest[index]=rest;
                if(calfMeshes[index]!=null&&!owned.Contains(calfMeshes[index]))owned.Add(calfMeshes[index]);
                index++;
            }
        }

        public int ValidateFaceReloadRecovery()
        {
            if(!EnsureFaceBindings())throw new System.Exception("Rosto indisponível para teste de recuperação");
            int originalOverride=ExpressionOverride;var originalExpression=expression;bool wasBuilding=buildingGarments;
            var originalPupils=(Mesh[])pupilMeshes.Clone();int cases=0;
            calfRest[0]=calfRest[1]=null;RestoreCalfBindings();
            if(calfRest[0]==null||calfRest[1]==null)throw new System.Exception("Repouso das pernas perdido na recompilação");
            try{
                for(int pass=0;pass<2;pass++){
                    // Reproduce the old readonly caches being empty after a
                    // script reload, including the blink values and mouth refs.
                    System.Array.Clear(eyes,0,2);System.Array.Clear(brows,0,2);System.Array.Clear(pupils,0,2);
                    System.Array.Clear(pupilMeshes,0,2);System.Array.Clear(upperLids,0,2);System.Array.Clear(lowerLids,0,2);
                    System.Array.Clear(upperLidObjects,0,2);System.Array.Clear(lowerLidObjects,0,2);
                    System.Array.Clear(upperCover,0,2);System.Array.Clear(lowerCover,0,2);
                    face=null;smile=null;openMouth=null;surprisedMouth=null;mouthLineMesh=null;
                    buildingGarments=pass==1;
                    if(pass==0)AnimateFace(); // First entry can be Update, not a clothes change.
                    for(int value=0;value<6;value++){
                        SetExpression((AvatarExpression)value);AnimateFace();
                        for(int i=0;i<2;i++){
                            if(pupilMeshes[i]!=originalPupils[i]||pupilMeshes[i].vertexCount==0||brows[i]==null||upperLids[i]==null||lowerLids[i]==null)
                                throw new System.Exception("Recuperação perdeu pupila, sobrancelha ou pálpebra");
                            if(garmentAssets.Contains(pupilMeshes[i])||garmentAssets.Contains(upperLids[i])||garmentAssets.Contains(lowerLids[i]))
                                throw new System.Exception("Rosto foi associado aos assets descartáveis da roupa");
                        }
                        cases++;
                    }
                }
            }finally{buildingGarments=wasBuilding;ExpressionOverride=originalOverride;ApplyExpression(originalExpression);}
            return cases;
        }
    }
}
