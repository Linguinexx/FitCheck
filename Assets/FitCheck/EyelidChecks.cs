using System;
using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        public int ValidateEyelidClosures()
        {
            var original=expression;int cases=0;
            try{
                for(int e=0;e<6;e++)foreach(float blink in new[]{0f,.5f,1f}){
                    ApplyExpression((AvatarExpression)e);
                    for(int eye=0;eye<2;eye++){
                        UpdateLid(eye,false,Mathf.Max(upperCover[eye],blink));UpdateLid(eye,true,lowerCover[eye]);
                        for(int lower=0;lower<2;lower++){
                            var go=lower==1?lowerLidObjects[eye]:upperLidObjects[eye];if(!go.activeSelf)continue;
                            var mesh=lower==1?lowerLids[eye]:upperLids[eye];var edges=new Dictionary<long,int>();var triangles=mesh.triangles;var vertices=mesh.vertices;
                            for(int j=0;j<triangles.Length;j+=3){
                                int a=triangles[j],b=triangles[j+1],c=triangles[j+2];
                                if(Vector3.Cross(vertices[b]-vertices[a],vertices[c]-vertices[a]).sqrMagnitude<1e-18f)throw new Exception("Triângulo degenerado na pálpebra");
                                for(int edge=0;edge<3;edge++){
                                    int x=triangles[j+edge],y=triangles[j+(edge+1)%3];long key=((long)Mathf.Min(x,y)<<32)|(uint)Mathf.Max(x,y);
                                    int count;edges.TryGetValue(key,out count);edges[key]=count+1;
                                }
                            }
                            foreach(var edge in edges)if(edge.Value!=2)throw new Exception("Pálpebra tem borda aberta: "+go.name);
                            // Outer/inner surfaces straddle the white eye closely,
                            // and the rear half must exist to bury the seam in the face.
                            if(mesh.bounds.max.z<.03f||mesh.bounds.min.z>-.02f||mesh.bounds.size.x>.36f||mesh.bounds.size.y>.38f)throw new Exception("Pálpebra perdeu encaixe no globo ocular");
                            cases++;
                        }
                    }
                }
                return cases;
            }finally{ApplyExpression(original);AnimateFace();}
        }
    }
    public partial class FirstCustomer
    {
        public void EyelidSmokeCheck()
        {
            bool oldPreview=studio.PreviewProportions,oldTall=studio.TallPreview;int cases=0;
            try{
                studio.PreviewProportions=true;
                foreach(bool tall in new[]{false,true}){studio.TallPreview=tall;Apply();cases+=studio.ValidateEyelidClosures();}
                Debug.Log("FITCHECK_EYELID_CLOSURE_PASSED: "+cases+" pálpebras em seis expressões, três fases de piscada e dois corpos; cascas fechadas, espessura e encaixe preservados.");
            }finally{studio.PreviewProportions=oldPreview;studio.TallPreview=oldTall;Apply();}
        }
    }
}
