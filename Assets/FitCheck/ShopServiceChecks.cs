using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        void SettleShopArrival()
        {
            int frames=0;
            while(shopSequence==ShopSequence.Arriving&&frames++<1400){city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);}
            if(ShopSequenceActive)throw new Exception("Cliente não terminou de entrar na loja");
            city.Frame(cam,true,0);
        }
        void CheckShopServiceFlow(string folder)
        {
            var original=progress;var originalRecord=record;string originalStorage=storageOverride;
            try{
                Home();progress=new DayProgress();storageOverride=Path.Combine(folder,"service-session");Directory.CreateDirectory(storageOverride);
                EnterShop();SettleShopArrival();
                var ids=new System.Collections.Generic.HashSet<string>();
                foreach(Transform child in city.transform)if(child.name.StartsWith("Peça na arara — ")){
                    string id=child.name.Substring("Peça na arara — ".Length);
                    if(!ids.Add(id))throw new Exception("Peça repetida na arara: "+id);
                    int drop=id.StartsWith("SB")?0:id.StartsWith("AH")?1:2;
                    if(Mathf.Abs(child.localPosition.z-(-2+drop*3))>1.0f||Mathf.Abs(child.localPosition.x+6.9f)>.01f)throw new Exception("Peça fora do trilho: "+id);
                }
                if(ids.Count!=9)throw new Exception("Araras sem os nove tops dos três drops");
                MoveProof(CityView.ShopCounter,0,0);InteractShop();question=1;
                if(!shopService||stage!=Stage.Dialogue)throw new Exception("Conversa não inicia atendimento da loja");
                FrameShopDialogue();SaveFirstPersonFrame(Path.Combine(folder,"service-conversation.png"));
                ProceedToFitting();
                if(stage!=Stage.City||shopSequence!=ShopSequence.WalkingToFitting)throw new Exception("Atendimento pulou a aproximação do provador");
                paused=true;var p=city.ResidentPosition(0);float elapsed=shopCameraTime;
                city.Frame(cam,true,0);UpdateShopSequence(1);
                if(city.ResidentPosition(0)!=p||shopCameraTime!=elapsed)throw new Exception("Sequência avança durante pausa");paused=false;
                for(int i=0;i<1400&&stage==Stage.City;i++){
                    city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);
                    if(i==65)SaveFirstPersonFrame(Path.Combine(folder,"service-camera-approach.png"));
                    if(i==190)SaveFirstPersonFrame(Path.Combine(folder,"service-fitting-entry.png"));
                }
                if(stage!=Stage.Dressing||campaignBeforePreview!=null||question!=1||progress.posts.Count!=0||remaining!=120)throw new Exception("Provador perdeu campanha/pergunta ou começou relógio antes da montagem");
                outfit=new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""};Apply();Submit();
                if(shopSequence!=ShopSequence.Exiting||stage!=Stage.City||!city.RuntimeReady)throw new Exception("Cliente não sai visível vestindo o fit");
                for(int i=0;i<1600&&stage==Stage.City;i++){
                    city.Frame(cam,true,1/30f);UpdateShopSequence(1/30f);
                    if(i==170)SaveFirstPersonFrame(Path.Combine(folder,"service-customer-exit.png"));
                }
                if(stage!=Stage.Post||progress.posts.Count!=1||!File.Exists(DayPath))throw new Exception("Saída não chegou ao FitFeed com um atendimento salvo");
                GoHome();if(shopService)throw new Exception("Atendimento vazou para a casa/repetição");Home();EnterShop();SettleShopArrival();
                if(city.ShopCustomerIndex!=1)throw new Exception("Próximo cliente não corresponde ao próximo pedido");
                Debug.Log("FITCHECK_SHOP_SERVICE_PASSED: nine unique stock pieces, arrival, conversation, paused approach, curtain before dressing, preserved question/inventory, dressed departure, saved FitFeed, next customer.");
            }finally{Home();progress=original;record=originalRecord;storageOverride=originalStorage;Home();}
        }
    }
}
