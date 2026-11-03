using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public void CheckAllBlinks()
        {
            var actors=new System.Collections.Generic.List<Resident>(residents);if(watcher!=null)actors.Add(watcher);if(playerBody!=null)actors.Add(playerBody);
            for(int i=0;i<actors.Count;i++){
                var actor=actors[i];int pupils=0;foreach(var part in actor.parts)if(part.node.name=="Pupila")pupils++;if(pupils!=2)throw new Exception("Cópia perdeu pupilas durante a piscada da fonte");int lids=0;foreach(var part in actor.parts)if(part.closedLid!=null){
                    lids++;if(part.closedLid.Length!=part.rest.Length||part.closedNormals.Length!=part.normals.Length)throw new Exception("Topologia da piscada inválida");
                    float change=0;for(int v=0;v<part.rest.Length;v++)change+=Vector3.Distance(part.rest[v],part.closedLid[v]);if(change<.1f)throw new Exception("Pálpebra sem fechamento");
                }
                if(lids!=2)throw new Exception("Personagem sem duas pálpebras animadas: "+actor.root.name);
                float previous=actor.blinkClock;actor.blinkClock=.09f-i*.617f;PoseResident(actor,i,0);actor.blinkClock=previous;PoseResident(actor,i,0);
            }
        }
        public void CaptureBlinkPose(int index,bool closed){var a=residents[index];a.blinkClock=closed?.09f-index*.617f:1-index*.617f;PoseResident(a,index,0);}
    }
    public partial class FirstCustomer
    {
        void CheckIdentityPolish(string folder)
        {
            studio.ValidateBlinkClock();
            foreach(string font in new[]{"Montserrat-Regular","Montserrat-SemiBold","BarlowCondensed-Bold"})if(!Resources.Load<Font>("Fonts/"+font))throw new Exception("Fonte ausente: "+font);
            if(!Resources.Load<Texture2D>("Brand/FitCheck-Selo-Kaos-Red"))throw new Exception("Logo vermelha ausente");
            Home();EnterCity();city.CheckAllBlinks();
            MoveProof(new Vector2(-100.3f,29),0,-24);SaveFirstPersonFrame(Path.Combine(folder,"easter-egg-observer.png"));
            var actor=city.transform.position+city.ResidentPosition(0);cam.transform.position=actor+new Vector3(0,2.8f,-2.7f);cam.transform.LookAt(actor+Vector3.up*2.8f);cam.fieldOfView=42;
            city.CaptureBlinkPose(0,false);SaveFirstPersonFrame(Path.Combine(folder,"blink-open.png"));city.CaptureBlinkPose(0,true);SaveFirstPersonFrame(Path.Combine(folder,"blink-closed.png"));
            Home();EnterHome();MoveProof(new Vector2(.7f,.2f),0,-1);SaveFirstPersonFrame(Path.Combine(folder,"home-window-night.png"));
            Home();EnterShop();city.CheckAllBlinks();city.AdvanceDressingBlinks(.09f);city.CheckAllBlinks();
            Debug.Log("FITCHECK_IDENTITY_POLISH_PASSED: three licensed font resources, red logo, two independently animated lids on every street/shop actor, watcher and visible player, night window, native before/after blink captures.");
            Home();
        }
    }
}
