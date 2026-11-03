using System;
using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        public void CitySmokeCheck()
        {
            if(stage==Stage.City)Home();
            var before=JsonUtility.ToJson(progress);var oldOutfit=outfit;int oldCustomer=customer;bool oldPaused=paused;int paths=0;
            try{
                EnterCity();paused=false;
                if(city.ResidentCount!=CityView.StreetResidentCount||city.GeometryBatches<6||cam.orthographic||mannequin.gameObject.activeSelf)throw new Exception("Bairro incompleto, câmera ou corpo em primeira pessoa incorretos");
                if(cam.clearFlags!=CameraClearFlags.Skybox||cam.GetComponent<Skybox>().material!=city.SkyMaterial||!city.SkyMaterial.shader.isSupported)throw new Exception("Skybox ausente ou incompatível");
                var shader=Resources.Load<Shader>("CityPaint");var texture=Resources.Load<Texture2D>("Painted/CityStone");
                if(shader==null||!shader.isSupported||texture==null||texture.width!=1024||texture.height!=256||texture.mipmapCount<2)throw new Exception("Material urbano indisponível ou sem mipmaps");
                var lettering=Resources.Load<Texture2D>("CitySigns");var letterShader=Resources.Load<Shader>("CitySign");
                if(lettering==null||lettering.mipmapCount<2||letterShader==null||!letterShader.isSupported)throw new Exception("Textura de letreiros indisponível");
                foreach(var filter in city.GetComponentsInChildren<MeshFilter>()){
                    if(filter.sharedMesh==null||filter.sharedMesh.vertexCount==0)throw new Exception("Malha vazia no bairro: "+filter.name+" / "+filter.transform.parent.name+" / "+(filter.sharedMesh==null?"null":filter.sharedMesh.name));
                    foreach(var renderer in filter.GetComponents<Renderer>())foreach(var material in renderer.sharedMaterials)if(material==null)throw new Exception("Material de morador destruído");
                }
                if(city.Free(new Vector2(7.5f,6))||city.Free(new Vector2(-10.3f,-.7f))||city.Free(new Vector2(60,0)))throw new Exception("Fachada, canteiro ou limite sem bloqueio");
                if(city.Free(new Vector2(-15.4f,2.8f))||city.Free(new Vector2(15.7f,-1.5f)))throw new Exception("Morador sem bloqueio de passagem");
                var destinations=new[]{new Vector2(10.4f,2.6f),new Vector2(-16.8f,3.6f),new Vector2(-10.3f,-6.0f),new Vector2(-20,-2.5f),new Vector2(21,-7.8f)};
                foreach(var target in destinations){
                    city.ResetWalk(mannequin);if(!city.RequestWalk(target))throw new Exception("Destino sem caminho: "+target);
                    int frames=0;while(Vector2.Distance(city.Position,target)>.03f&&frames<4000){city.Step(Vector2.zero,1/60f,false,mannequin);if(!city.Free(city.Position))throw new Exception("Caminhada atravessou cenário");frames++;}
                    if(frames>=4000)throw new Exception("Caminhada não chegou ao destino");paths++;
                }
                city.ResetWalk(mannequin);for(int i=0;i<60;i++)city.Step(Vector2.right,1/60f,false,mannequin);float straight=Vector2.Distance(CityView.Spawn,city.Position);
                city.ResetWalk(mannequin);for(int i=0;i<60;i++)city.Step(Vector2.one,1/60f,false,mannequin);float diagonal=Vector2.Distance(CityView.Spawn,city.Position);
                if(Mathf.Abs(straight-diagonal)>.01f||Mathf.Abs(straight-CityView.PlayerSpeed)>.02f)throw new Exception("Velocidade diagonal ou base incorreta");
                float skyTime=city.SkyMaterial.GetFloat("_CityTime");
                var position=city.Position;var rotation=mannequin.rotation;city.Step(Vector2.up,1,true,mannequin);
                city.Frame(cam,false,0);city.Frame(cam,true,1/60f);
                if(city.SkyMaterial.GetFloat("_CityTime")!=skyTime)throw new Exception("Pausa ou movimento reduzido não congelou céu");
                city.Frame(cam,false,1/60f);if(city.SkyMaterial.GetFloat("_CityTime")<=skyTime)throw new Exception("Céu não anima");
                if(city.Position!=position||Quaternion.Angle(mannequin.rotation,rotation)>.01f)throw new Exception("Pausa permitiu movimento");
                city.ResetWalk(mannequin);city.Frame(cam,true,0);
                if(Mathf.Abs(cam.transform.position.y-CityView.EyeHeight)>.001f||Mathf.Abs(cam.fieldOfView-CityView.ViewFieldOfView)>.001f)throw new Exception("Câmera não está na altura dos olhos");
                float yaw=city.ViewYaw;city.Look(new Vector2(10,0));if(Mathf.Abs(Mathf.DeltaAngle(yaw,city.ViewYaw))<10)throw new Exception("Mouse não altera a direção");
                city.Look(new Vector2(0,1000));if(city.ViewPitch!= -75)throw new Exception("Olhar vertical ultrapassou limite superior");
                city.Look(new Vector2(0,-2000));if(city.ViewPitch!=88)throw new Exception("Olhar vertical ultrapassou limite inferior");
                city.Frame(cam,true,0);Debug.Log("FITCHECK_PLAYER_BODY_PASSED: "+city.CheckPlayerBody(cam)+" visible vertices looking down; speed="+CityView.PlayerSpeed);
                city.SetView(0,0);city.Frame(cam,true,0);if(Vector3.Angle(cam.transform.forward,Vector3.forward)>.01f)throw new Exception("Olhar não coincide com direção da câmera");
                if(cityFreeCursor)throw new Exception("Entrada não solicita ocultar e capturar o cursor");
                var tab=new Event{type=EventType.KeyDown,keyCode=KeyCode.Tab};ProcessCityEvent(tab);
                if(!cityFreeCursor||tab.type!=EventType.Used)throw new Exception("Tab não libera o cursor");
                // Standalone batch players turn synthetic mouse Events into Ignore;
                // exercise the same pointer handler used by real OnGUI mouse events.
                if(CaptureCityPointer(new Vector2(100,130),0)||!cityFreeCursor)throw new Exception("UI capturou o olhar indevidamente");
                if(!CaptureCityPointer(new Vector2(720,450),0)||cityFreeCursor||city.Position!=CityView.Spawn)throw new Exception("Retomar olhar alterou posição ou não capturou cursor");
                OnApplicationFocus(false);if(paused)throw new Exception("Perda de foco paralisou o bairro");paused=false;
                Home();UpdateCamera();if(cam.clearFlags!=beforeCityClearFlags||cam.GetComponent<Skybox>().enabled!=beforeCitySkyEnabled||cam.GetComponent<Skybox>().material!=beforeCitySky)throw new Exception("Céu vazou para o provador");
                if(cam.orthographic||city.gameObject.activeSelf||mannequin.position!=Vector3.zero||!mannequin.gameObject.activeSelf||Mathf.Abs(cam.fieldOfView-beforeCityFov)>.01f)throw new Exception("Saída do bairro não restaurou o provador");
                // Replacing source clothes must not destroy the baked residents.
                Apply();EnterCity();if(city.ResidentCount!=CityView.StreetResidentCount)throw new Exception("Moradores duplicados ao entrar novamente");
                foreach(var r in city.GetComponentsInChildren<Renderer>())foreach(var m in r.sharedMaterials)if(m==null)throw new Exception("Material perdido após troca de roupa");
                if(JsonUtility.ToJson(progress)!=before)throw new Exception("Passeio alterou campanha");
                if(RetroCamera.SoftPixelHeight!=720)throw new Exception("Pixelização voltou ao nível forte");
                Debug.Log("FITCHECK_CITY_PASSED: "+paths+" trajetos com obstáculos; câmera em primeira pessoa, limites do olhar, Tab/cursor, WASD/diagonal, pausa/foco, corpo visível sem cabeça, moradores e retorno ao menu; campanha preservada; filtro opcional em 720 linhas.");
            }finally{
                if(stage==Stage.City)Home();paused=oldPaused;outfit=oldOutfit;customer=oldCustomer;Apply();UpdateCamera();
            }
        }
    }
}
