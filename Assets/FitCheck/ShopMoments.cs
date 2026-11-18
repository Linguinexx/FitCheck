using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool shopMusicOpen,shopAlbumOpen,shopLeisureCursor;
        int shopGuideTab,shopQuizAnswer=-1,shopAlbumSelection;Vector2 shopAlbumScroll;
        bool ShopLeisurePanelOpen=>shopMusicOpen||shopAlbumOpen;
        static readonly string[] ShopMusicNames={"Laje 92","Passo Torto","Depois das Dez"};
        AudioClip[] shopRecords;int shopRecordSelected;
        AudioClip ShopRecordClip=>StreetMemory.musicStopped?null:shopRecords[StreetMemory.musicChoice];
        static readonly string[] NandoDays={
            "Antes de abrir eu sempre dou uma volta pela loja. Parece bobeira, mas o primeiro encontro já começa no jeito que a gente recebe.",
            "Ontem eu quase chamei um cliente pelo nome do gato. Se eu falar 'pspsps' no atendimento, você me interrompe.",
            "A galera está comentando as fotos do FitFeed. O legal é reconhecer a pessoa no look, não só reconhecer a marca.",
            "Tem gente que chega sabendo exatamente o que quer. Tem gente que descobre conversando. Os dois jeitos fazem parte.",
            "A loja está ficando com memória. Cada foto no balcão me lembra um pedido, uma conversa e alguma correria.",
            "Se amanhã bater a dúvida, volta pro pedido. Às vezes a melhor combinação é a que deixa uma peça respirar.",
            "Sete dias! Olha quanta história passou por essa porta. Depois a gente fecha o caixa e fica só lembrando dos encontros."
        };
        static readonly string[] NandoQuestions={
            "Qual drop tem a proposta 'A rua é de geral'?","Se o pedido quer uma peça de destaque, por onde você começa?","O cliente quer a cabeça livre. Qual escolha respeita o pedido?","Quando as pistas deixam dúvida, o que ajuda mais?","Uma peça ainda bloqueada vira disponível só por aparecer na arara?","Onde você pode rever o resultado e a foto do atendimento?","Um cliente pediu contraste claro/escuro. Qual combinação atende essa pista?"
        };
        static readonly string[][] NandoChoices={
            new[]{"After Hours","Common Ground","Side B"},new[]{"Destacar tudo ao mesmo tempo","Ignorar a ocasião","Escolher o destaque e equilibrar o resto"},new[]{"Sem acessório de cabeça","Boné por cima de tudo","Trocar o cabelo do cliente"},new[]{"Sortear uma roupa","Conversar e reler o pedido","Usar sempre o mesmo fit"},new[]{"Sim, basta olhar para ela","Sim, se combinar com o tênis","Não, valem os desbloqueios do catálogo"},new[]{"No FitFeed","Na caixa de som","Na cortina vazia"},new[]{"Tudo na mesma luminosidade","Parte de cima clara e de baixo escura","Somente duas cores claras"}
        };
        static readonly int[] NandoCorrect={1,2,0,1,2,0,1};
        void AnswerShopQuiz(int answer){
            int day=progress.day-1;if(ShopOpen||answer<0||answer>2||(StreetMemory.shopQuizDays&(1<<day))!=0)return;
            shopQuizAnswer=answer;shopGuideAnswerAt=contextClock;
            if(answer!=NandoCorrect[day])return;
            int oldDays=StreetMemory.shopQuizDays,oldStickers=StreetMemory.stickers;
            StreetMemory.shopQuizDays|=1<<day;StreetMemory.Collect((day+2)%7);
            if(TrySave())PlayClip(Clip("ui_confirm"),.45f);else{StreetMemory.shopQuizDays=oldDays;StreetMemory.stickers=oldStickers;}
        }
        void ChooseShopMusic(int choice){if(ShopOpen||choice<0||choice>2)return;int before=StreetMemory.musicChoice;bool stopped=StreetMemory.musicStopped;StreetMemory.musicChoice=choice;StreetMemory.musicStopped=false;if(!TrySave()){StreetMemory.musicChoice=before;StreetMemory.musicStopped=stopped;}}
        void StopShopRecord(){if(ShopOpen)return;bool before=StreetMemory.musicStopped;StreetMemory.musicStopped=true;if(!TrySave())StreetMemory.musicStopped=before;}
        void PinShopPhoto(){if(ShopOpen||shopAlbumSelection<0||shopAlbumSelection>=progress.posts.Count)return;int before=StreetMemory.shopPhoto;StreetMemory.shopPhoto=shopAlbumSelection+1;if(TrySave())SyncShopMemory();else StreetMemory.shopPhoto=before;}
        void SyncShopMemory(){if(city==null||!city.IsInterior||city.IsHome)return;int index=StreetMemory.shopPhoto-1;city.SetShopMemory(index>=0&&index<progress.posts.Count?FeedPhoto(progress.posts[index].photo):null);}
        WorldHint ShopMomentHint(){
            if(ShopOpen||ShopLeisurePanelOpen)return default;
            if(city.LookingAtShopRadio(cam))return Hint("E","Escolher a trilha da loja",city.transform.TransformPoint(CityView.ShopRadioSpot),new Vector3(1,.65f,.4f));
            if(city.LookingAtShopAlbum(cam))return Hint("E","Álbum dos encontros",city.transform.TransformPoint(CityView.ShopAlbumSpot),new Vector3(.7f,.7f,.5f));
            return default;
        }
        bool TryShopMoment(){
            if(ShopOpen||ShopSequenceActive||shopGuideOpen||!city.IsInterior||city.IsHome)return false;
            bool radio=city.LookingAtShopRadio(cam),album=city.LookingAtShopAlbum(cam);if(!radio&&!album)return false;
            shopLeisureCursor=cityFreeCursor;shopMusicOpen=radio;shopAlbumOpen=album;shopAlbumSelection=Mathf.Clamp(StreetMemory.shopPhoto>0?StreetMemory.shopPhoto-1:progress.posts.Count-1,0,Mathf.Max(0,progress.posts.Count-1));shopAlbumScroll=Vector2.zero;
            shopRecordSelected=StreetMemory.musicChoice;
            city.StopWalk();city.CancelPlayerEmote(true);city.FramePlayerForCamera(cam,false);cityFreeCursor=true;cityDragLook=false;SyncCityCursor(true);SyncShopMemory();return true;
        }
        void CloseShopMoment(){if(!ShopLeisurePanelOpen)return;shopMusicOpen=shopAlbumOpen=false;cityFreeCursor=shopLeisureCursor;SyncCityCursor(true);}
        void NandoLeisureUI(){
            DrawKaosPanel(new Rect(916,162,482,620));Txt(new Rect(944,184,425,50),"Bora trocar ideia?",heading,paper);
            for(int i=0;i<3;i++)if(Tap(new Rect(944+i*143,249,135,42),new[]{"Guia","Resenha","Desafio"}[i],shopGuideTab==i,i==0||!ShopOpen)){shopGuideTab=i;shopGuideTopic=-1;shopQuizAnswer=-1;shopGuideAnswerAt=contextClock;if(i==1){StreetMemory.shopChatDays|=1<<(progress.day-1);TrySave();}}
            if(ShopOpen&&shopGuideTab>0)shopGuideTab=0;
            string line;
            if(shopGuideTab==0){
                for(int i=0;i<ShopGuideTopics.Length;i++)if(Tap(new Rect(944,316+i*58,424,48),ShopGuideTopics[i],shopGuideTopic==i)){shopGuideTopic=i;shopGuideAnswerAt=contextClock;}
                line=shopGuideTopic<0?"Manda a dúvida. E, com a loja fechada, a gente troca uma ideia sem pressa.":ShopGuideAnswers[shopGuideTopic];
                if(ShopOpen)Txt(new Rect(944,621,420,60),"Resenha e desafios quando a loja estiver fechada.",caption,muted);
            }else if(shopGuideTab==1){
                Txt(new Rect(944,323,424,44),"Resenha / dia "+progress.day,copy,paper);line=NandoDays[progress.day-1];
                Txt(new Rect(944,394,424,144),"A loja também tem seus momentos entre um expediente e outro.\n\nA caixa de som e o álbum ficam disponíveis com a loja fechada.",caption,muted);
            }else{
                int day=progress.day-1;bool done=(StreetMemory.shopQuizDays&(1<<day))!=0;line=done?"Mandou bem hoje! Amanhã tem outra. O adesivo já está guardado para o mural.":shopQuizAnswer<0?NandoQuestions[day]:"Essa ainda não. Volta às pistas do pedido e tenta de novo, sem perder nada.";
                Txt(new Rect(944,323,424,44),"Pergunta do dia "+progress.day,copy,paper);
                if(!done)for(int i=0;i<3;i++)if(Tap(new Rect(944,391+i*76,424,65),NandoChoices[day][i],shopQuizAnswer==i)){
                    AnswerShopQuiz(i);
                }
                Txt(new Rect(944,635,424,50),"Brincadeira opcional. Vale um adesivo, não reputação.",caption,muted);
            }
            if(Tap(new Rect(944,714,424,48),"Valeu, até mais")){CloseShopGuide();return;}
            ComicBubble(new Rect(80,570,760,250),line,CityView.ShopGuideName,contextClock-shopGuideAnswerAt,false);
        }
        void ShopMomentsUI(){
            if(shopMusicOpen){
                ShopRecordsUI();return;
            }
            DrawKaosPanel(new Rect(932,123,468,680));Txt(new Rect(960,147,412,52),"Álbum da loja",heading,paper);
            if(progress.posts.Count==0){Txt(new Rect(960,243,410,180),"O primeiro atendimento começa a nossa coleção de encontros. As fotos vão aparecer aqui depois de atender os clientes.",copy,paper);}
            else{
                shopAlbumSelection=Mathf.Clamp(shopAlbumSelection,0,progress.posts.Count-1);var post=progress.posts[shopAlbumSelection];
                RoundedPanel(new Rect(52,143,816,660),KaosPanel,12);var photoRect=new Rect(148,160,620,580);var image=FeedPhoto(post.photo);if(image!=null)GUI.DrawTexture(photoRect,image,ScaleMode.ScaleToFit);
                Txt(new Rect(80,747,760,44),DayOne.Customers[shopAlbumSelection].name+" / Dia "+(shopAlbumSelection/3+1)+" / Nota "+post.score+" de 10",caption,paper);
                shopAlbumScroll=GUI.BeginScrollView(new Rect(960,226,414,366),shopAlbumScroll,new Rect(0,0,390,progress.posts.Count*57),false,false);
                for(int i=0;i<progress.posts.Count;i++)if(Tap(new Rect(0,i*57,385,48),DayOne.Customers[i].name+" / Dia "+(i/3+1),shopAlbumSelection==i))shopAlbumSelection=i;
                GUI.EndScrollView();
                if(Tap(new Rect(960,621,412,55),StreetMemory.shopPhoto==shopAlbumSelection+1?"Foto exposta no balcão":"Expor esta foto no balcão",true))PinShopPhoto();
            }
            if(Tap(new Rect(960,729,412,48),"Voltar à loja"))CloseShopMoment();
        }
    }
}
