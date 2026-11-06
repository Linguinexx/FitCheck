using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class StudioView
    {
        public int ValidateVisibleBrows()
        {
            int count=0;
            foreach(var brow in brows){
                var mesh=brow.GetComponent<MeshFilter>().sharedMesh;
                foreach(int sample in new[]{4,8,12}){
                    int index=sample*4;var p=brow.TransformPoint((mesh.vertices[index]+mesh.vertices[index+1])*.5f);
                    var outward=Avatar.TransformDirection(Vector3.back);var origin=p+outward*.8f;
                    foreach(var renderer in hair.GetComponentsInChildren<MeshRenderer>()){
                        if(renderer.name!="Cabelo ajustado à cabeça")continue;
                        var filter=renderer.GetComponent<MeshFilter>();var hairMesh=filter.sharedMesh;
                        var o=renderer.transform.InverseTransformPoint(origin);var d=renderer.transform.InverseTransformVector(-outward);
                        var vertices=hairMesh.vertices;var triangles=hairMesh.triangles;
                        for(int i=0;i<triangles.Length;i+=3){
                            var a=vertices[triangles[i]];var edge1=vertices[triangles[i+1]]-a;var edge2=vertices[triangles[i+2]]-a;
                            var cross=Vector3.Cross(d,edge2);float det=Vector3.Dot(edge1,cross);if(Mathf.Abs(det)<.000001f)continue;
                            var delta=o-a;float u=Vector3.Dot(delta,cross)/det;if(u<0||u>1)continue;
                            var q=Vector3.Cross(delta,edge1);float v=Vector3.Dot(d,q)/det;if(v<0||u+v>1)continue;
                            float distance=Vector3.Dot(edge2,q)/det;
                            if(currentHairProfile!=3&&distance>.001f&&distance<.799f)throw new Exception("Cabelo esconde sobrancelha: corte="+currentHairProfile+" expressão="+expression+" amostra="+sample);
                        }
                    }
                    count++;
                }
                if(mesh.vertexCount<60)throw new Exception("Sobrancelha voltou a ser traço simples");
            }
            return count;
        }
    }
    public partial class FirstCustomer
    {
        void ExpressionLifeReview(string folder)
        {
            Home();var oldLook=studio.PlayerLook;int samples=0;
            try{
                for(int cut=0;cut<6;cut++)for(int face=0;face<6;face++){
                    studio.PlayerLook=new PlayerAppearance{hair=cut,hairColor=0,skin=3,expression=face};studio.ApplyUniform(progress.EquippedUniform);studio.Animate(0,false);
                    samples+=studio.ValidateVisibleBrows();
                }
            }finally{studio.PlayerLook=oldLook;Home();}
            EnterCity();paused=false;bool oldReduced=reducedMotion;
            try{
                reducedMotion=true;var before=city.ResidentPosition(2);float clock=city.AmbientTime;
                for(int i=0;i<360;i++){UpdateCity(1/30f);city.Frame(cam,reducedMotion,1/30f);city.CheckResidents();}
                if(Vector3.Distance(before,city.ResidentPosition(2))<3||city.AmbientTime!=clock)throw new Exception("NPC não caminha com movimento reduzido");
                var frozen=city.ResidentPosition(2);paused=true;for(int i=0;i<30;i++){UpdateCity(1/30f);city.Frame(cam,false,0);}
                if(frozen!=city.ResidentPosition(2))throw new Exception("Pausa não congelou NPC");paused=false;
                var state=city.Position;float yaw=city.ViewYaw;
                var field=typeof(CityView).GetField("residents",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic);
                ((IList)field.GetValue(city)).Clear();UpdateCity(0);
                if(!city.RuntimeReady||city.ResidentCount!=CityView.StreetResidentCount||Vector2.Distance(state,city.Position)>.001f||Mathf.Abs(Mathf.DeltaAngle(yaw,city.ViewYaw))>.001f)throw new Exception("Recuperação da simulação após recompilação falhou");
                MoveProof(new Vector2(-3,-15),0,-8);
                for(int i=0;i<300;i++)city.Frame(cam,true,1/30f);
                var motion=Path.Combine(folder,"motion");Directory.CreateDirectory(motion);
                for(int frame=0;frame<180;frame++){
                    city.Frame(cam,true,1/30f);SaveFirstPersonFrame(Path.Combine(motion,frame.ToString("D3")+".png"),640,360);
                }
                Debug.Log("FITCHECK_EXPRESSION_LIFE_PASSED: "+samples+" brow visibility rays, 36 hair/expression combinations; NPCs moving with reduced motion, pause, focus and hot-reload fallback; filter default off.");
            }finally{reducedMotion=oldReduced;Home();}
        }
    }
}
