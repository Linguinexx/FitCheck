using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void LookAtShopMoment(Vector2 position,Vector3 target){
            if(!city.Free(position))throw new Exception("Objeto sem acesso livre");
            city.RestoreWalk(position,0,0,mannequin);city.Frame(cam,true,0);cam.transform.LookAt(city.transform.TransformPoint(target));
            var a=cam.transform.eulerAngles;city.SetView(a.y,Mathf.DeltaAngle(0,a.x));city.Frame(cam,true,0);city.FramePlayerForCamera(cam,false);
        }
        void CheckShopMoments(string folder){
            storageOverride=Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N"));Directory.CreateDirectory(storageOverride);
            Home();progress=new DayProgress();EnterShop();paused=false;cityFreeCursor=false;
            if(ShopOpen||city.CustomerVisible||city.ResidentCount!=2||!city.RuntimeReady)throw new Exception("Loja perdeu papéis ou abriu sozinha");
            int lights=city.GetComponentsInChildren<Light>(true).Length;
            LookAtShopMoment(new Vector2(5.45f,-1.5f),CityView.ShopRadioSpot);
            CaptureInterface(Path.Combine(folder,"radio-interaction.png"));
            if(!TryShopMoment()||!shopMusicOpen||!cityFreeCursor)throw new Exception("Caixa de som inacessível");
            for(int i=0;i<3;i++){ChooseShopMusic(i);if(ShopRecordClip==null||ShopRecordClip!=shopRecords[i])throw new Exception("Disco selecionado incorreto");}
            CaptureInterface(Path.Combine(folder,"music.png"));HandleEscapeAction();if(ShopLeisurePanelOpen||cityFreeCursor)throw new Exception("Fechar não restaurou controle");
            LookAtShopMoment(new Vector2(4.08f,7.75f),CityView.ShopAlbumSpot);
            if(!InteractShop()||!shopAlbumOpen||ShopOpen||progress.posts.Count!=0)throw new Exception("Álbum vazio criou atendimento");
            CaptureInterface(Path.Combine(folder,"album-empty.png"));PinShopPhoto();if(StreetMemory.shopPhoto!=0)throw new Exception("Foto fantasma selecionada");CloseShopMoment();
            LookAtShopMoment(new Vector2(0,6.3f),city.transform.InverseTransformPoint(city.ShopGuideTarget));
            if(!InteractShop()||!shopGuideOpen)throw new Exception("Nando inacessível");
            shopGuideTab=1;shopGuideAnswerAt=contextClock-2;CaptureInterface(Path.Combine(folder,"nando-chat.png"));
            shopGuideTab=2;shopQuizAnswer=-1;CaptureInterface(Path.Combine(folder,"nando-quiz.png"));
            int reputation=progress.reputation;AnswerShopQuiz(0);if(StreetMemory.shopQuizDays!=0)throw new Exception("Resposta errada premiada");
            AnswerShopQuiz(1);AnswerShopQuiz(1);if(StreetMemory.shopQuizDays!=1||!StreetMemory.HasSticker(2)||progress.reputation!=reputation||progress.posts.Count!=0)throw new Exception("Pergunta afetou campanha ou repetiu prêmio");
            shopGuideAnswerAt=contextClock-2;CaptureInterface(Path.Combine(folder,"nando-reward.png"));CloseShopGuide();
            Home();customer=0;outfit=(string[])Catalog.Default.Clone();Apply();mannequin.gameObject.SetActive(true);RenderCurrentPhoto();
            string path=Path.Combine(storageOverride,"customer.png");File.WriteAllBytes(path,photo.EncodeToPNG());
            progress.Add(new Attempt{outfit=(string[])outfit.Clone(),photo=path,photoStyle=ArtRevision});EnterShop();
            LookAtShopMoment(new Vector2(4.08f,7.75f),CityView.ShopAlbumSpot);if(!TryShopMoment()||!shopAlbumOpen)throw new Exception("Álbum com atendimento inacessível");
            PinShopPhoto();if(StreetMemory.shopPhoto!=1)throw new Exception("Favorita não gravada");CaptureInterface(Path.Combine(folder,"album.png"));CloseShopMoment();
            SaveFirstPersonFrame(Path.Combine(folder,"counter-photo.png"));
            var loaded=ProgressStore.Load(DayPath);if(loaded.neighbourhood.shopPhoto!=1||loaded.neighbourhood.musicChoice!=2||loaded.neighbourhood.shopQuizDays!=1)throw new Exception("Lembranças não persistem");
            ProgressStore.Save(Path.Combine(storageOverride,"other.json"),new DayProgress());var other=ProgressStore.Load(Path.Combine(storageOverride,"other.json"));if(other.neighbourhood.shopPhoto!=0||other.neighbourhood.musicChoice!=0||other.neighbourhood.shopQuizDays!=0)throw new Exception("Lembranças vazaram entre saves");
            EnterHome();if(ShopLeisurePanelOpen||StreetMemory.musicChoice!=2)throw new Exception("Painel vazou para casa");EnterShop();
            MoveProof(CityView.ShopCounter,180,0);OpenShopForDay();LookAtShopMoment(new Vector2(5.45f,-1.5f),CityView.ShopRadioSpot);
            if(TryShopMoment()||ShopMomentHint().valid)throw new Exception("Lazer interrompe expediente");
            int choice=StreetMemory.musicChoice,quiz=StreetMemory.shopQuizDays;ChooseShopMusic(0);AnswerShopQuiz(1);if(choice!=StreetMemory.musicChoice||quiz!=StreetMemory.shopQuizDays)throw new Exception("Lazer alterou expediente");
            LookAtShopMoment(new Vector2(0,6.3f),city.transform.InverseTransformPoint(city.ShopGuideTarget));if(!InteractShop()||!shopGuideOpen)throw new Exception("Guia indisponível durante expediente");CloseShopGuide();
            if(city.ResidentCount!=2||city.GetComponentsInChildren<Light>(true).Length!=lights)throw new Exception("Interações adicionaram atores ou luzes");
            Home();progress=new DayProgress();EnterShop();
            MoveProof(CityView.ShopFitting,0,4);if(!InteractShop()||!shopFittingOpening)throw new Exception("Cortina vazia inacessivel");
            for(int i=0;i<30;i++){city.Frame(cam,true,1/30f);FinishShopFittingEntryIfReady();}
            if(city.CustomerVisible||ShopOpen||shopService||stage!=Stage.City||progress.posts.Count!=0)throw new Exception("Cortina criou atendimento");
            CheckShopServiceFlow(folder);
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"SHOP_MOMENTS_PASSED: physical access, radio choices, empty and attended-photo albums, favorite persistence, wrong/right/repeated quiz and no campaign changes, closed-shop gating, guide during service, isolated saves, unchanged actors/lights, empty curtain and complete service regression.");Debug.Log(File.ReadAllText(Path.Combine(folder,"checks.txt")));
        }
    }
}
