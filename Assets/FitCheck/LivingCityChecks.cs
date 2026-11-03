using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public void CheckCitizenVariety()
        {
            var skins=new HashSet<Color>();var shirts=new HashSet<Color>();
            foreach(var actor in residents)foreach(var r in actor.root.GetComponentsInChildren<MeshRenderer>()){
                if(r.name=="Cabeça arredondada")skins.Add(r.sharedMaterial.color);
                if(r.name=="Camiseta com caimento")shirts.Add(r.sharedMaterial.color);
                if(r.name.Contains("Estampa")||r.name.Contains("Identificação"))throw new Exception("Morador com identificação da loja");
            }
            if(skins.Count<5||shirts.Count<5||!residents[6].wallLean||residents[0].idleStyle!=2)throw new Exception("Moradores sem variedade/poses contextuais: "+skins.Count+" / "+shirts.Count);
        }
    }
    public partial class FirstCustomer
    {
        Color32[] ReadReviewPixels()
        {
            var previous=cam.targetTexture;var active=RenderTexture.active;var rt=RenderTexture.GetTemporary(320,180,24);var tex=new Texture2D(320,180,TextureFormat.RGB24,false);
            try{cam.targetTexture=rt;cam.Render();RenderTexture.active=rt;tex.ReadPixels(new Rect(0,0,320,180),0,0);tex.Apply();return tex.GetPixels32();}
            finally{cam.targetTexture=previous;RenderTexture.active=active;RenderTexture.ReleaseTemporary(rt);Destroy(tex);}
        }
        void CheckLivingCity(string folder)
        {
            var before=JsonUtility.ToJson(progress);string storage=storageOverride;
            try{
                Home();EnterCity();paused=false;
                if(studio.VisibleStudioDecor!=0||!studio.ExplorationMode)throw new Exception("Estúdio visível durante exploração");
                city.CheckCitizenVariety();city.ResetWalk(mannequin);
                city.SprintRequested=true;for(int i=0;i<60;i++)city.Step(Vector2.right,1/60f,false,mannequin);
                float run=Vector2.Distance(CityView.Spawn,city.Position);if(Mathf.Abs(run-CityView.RunSpeed)>.03f||!city.Running)throw new Exception("Shift não acelerou a corrida: "+run);
                var stop=city.Position;city.Step(Vector2.right,.05f,true,mannequin);if(city.Position!=stop||city.Running)throw new Exception("Corrida não respeitou pausa");
                city.ResetWalk(mannequin);city.SprintRequested=true;for(int i=0;i<60;i++)city.Step(Vector2.one,1/60f,false,mannequin);
                if(Mathf.Abs(Vector2.Distance(CityView.Spawn,city.Position)-run)>.03f)throw new Exception("Diagonal de corrida acelerada");
                city.SprintRequested=false;
                foreach(var target in new[]{new Vector2(-137,-38),new Vector2(20,-38),new Vector2(-50,54)}){
                    MoveProof(target,target.y<0?180:0,-4);SaveFirstPersonFrame(Path.Combine(folder,"edge-"+target.x+"-"+target.y+".png"));
                }
                foreach(var p in new[]{new Vector2(-145,0),new Vector2(27,0),new Vector2(-60,-45),new Vector2(-60,59)})if(city.Free(p))throw new Exception("Borda urbana aberta");
                MoveProof(new Vector2(-77,27),0,-53);city.SkyMaterial.SetFloat("_CityTime",0);var a=ReadReviewPixels();SaveFirstPersonFrame(Path.Combine(folder,"sky-night-0.png"));
                city.SkyMaterial.SetFloat("_CityTime",35);var b=ReadReviewPixels();SaveFirstPersonFrame(Path.Combine(folder,"sky-night-35.png"));int changed=0;
                for(int i=0;i<a.Length;i++)if(!a[i].Equals(b[i]))changed++;if(changed<100)throw new Exception("Céu noturno não animou");
                Home();if(studio.ExplorationMode||studio.VisibleStudioDecor==0)throw new Exception("Estúdio não voltou ao menu");
                var expression=studio.CurrentExpression;int oldOverride=studio.ExpressionOverride;bool oldReduced=studio.ReducedMotion;studio.ReducedMotion=false;studio.SetExpression(AvatarExpression.Neutral);studio.Talking=true;studio.Animate(.07f,true);
                Mesh mouth=null;foreach(var filter in studio.Avatar.GetComponentsInChildren<MeshFilter>())if(filter.name=="Boca")mouth=filter.sharedMesh;
                if(mouth==null)throw new Exception("Boca não encontrada");var first=mouth.vertices;float movement=0;
                for(int frame=0;frame<30;frame++){studio.Animate(.04f,true);var second=mouth.vertices;for(int i=0;i<first.Length;i++)movement+=(first[i]-second[i]).sqrMagnitude;first=second;}
                studio.Talking=false;studio.ReducedMotion=oldReduced;studio.SetExpression(expression);studio.ExpressionOverride=oldOverride;if(movement<.000001f)throw new Exception("Boca estática durante diálogo");

                CheckShopDecoration(folder);
                if(JsonUtility.ToJson(progress)!=before)throw new Exception("Revisão alterou campanha");
                Debug.Log("FITCHECK_LIVING_CITY_PASSED: studio isolation/restoration, 7.2 sprint/diagonal/pause, closed edges, sky pixels="+changed+", varied civilian looks/poses, animated mouth, decor preview/cancel/save/reentry/isolation, presentation pause and SFX/VFX.");
            }finally{Home();paused=false;storageOverride=storage;savedDecor=null;decorPathLoaded=null;}
        }
        void CheckShopDecoration(string folder)
        {
            string storage=storageOverride;string before=JsonUtility.ToJson(progress);
            try{
                Home();storageOverride=Path.Combine(folder,"decoration-"+Guid.NewGuid().ToString("N"));savedDecor=null;
                EnterShop();OpenShopDecor();if(!showShopDecor)throw new Exception("Personalização não abriu com a loja fechada");
                decorDraft.wall=2;decorDraft.floor=1;decorDraft.accent=1;decorDraft.poster=2;decorDraft.plants=false;city.ApplyShopDecor(decorDraft);
                FrameShopDecor();SaveFirstPersonFrame(Path.Combine(folder,"shop-custom-preview.png"));CloseShopDecor(false);
                if(File.Exists(ShopDecorPath)||city.ActiveDecor.wall!=0)throw new Exception("Cancelar gravou decoração");
                OpenShopDecor();decorDraft.wall=1;decorDraft.floor=2;decorDraft.accent=2;decorDraft.poster=1;city.ApplyShopDecor(decorDraft);CloseShopDecor(true);
                var loaded=ShopDecorStore.Load(ShopDecorPath);if(loaded.wall!=1||loaded.floor!=2||loaded.accent!=2||loaded.poster!=1)throw new Exception("Decoração não persistiu");
                string path=ShopDecorPath;string other=Path.Combine(storageOverride,"other-slot","shop-style.json");ShopDecorStore.Save(other,new ShopDecor{wall=3});if(ShopDecorStore.Load(path).wall!=1)throw new Exception("Decoração vazou entre saves");
                Home();savedDecor=null;EnterShop();if(city.ActiveDecor.wall!=1)throw new Exception("Reentrada perdeu decoração");
                FrameShopDecor();SaveFirstPersonFrame(Path.Combine(folder,"shop-custom-saved.png"));
                for(int view=1;view<=2;view++){decorView=view;FrameShopDecor();SaveFirstPersonFrame(Path.Combine(folder,"shop-custom-view-"+view+".png"));}
                paused=true;arrivalFade=.8f;photoFlash=.1f;TickPresentation(.2f);if(arrivalFade!=.8f||photoFlash!=.1f)throw new Exception("Pausa não congelou efeitos");paused=false;
                if(doorFx==null||stepStone==null||stepWood==null||clothFx==null||motes==null||!moteMaterial.shader.isSupported)throw new Exception("SFX/VFX indisponível");
                if(JsonUtility.ToJson(progress)!=before)throw new Exception("Revisão alterou campanha");
                Debug.Log("FITCHECK_SHOP_DECOR_PASSED: preview/cancel/save/reentry, independent files, all camera views, curtain colors, presentation pause and SFX/VFX.");
            }finally{Home();paused=false;storageOverride=storage;savedDecor=null;decorPathLoaded=null;}
        }
    }
}
