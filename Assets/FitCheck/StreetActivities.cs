using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool muralOpen,streetStoryOpen,streetPreviousCursor,courtPending,courtPendingMade;
        int muralDraft,streetStory,streetStoryResident,challengeSerial,courtAttempts,courtMade;
        float streetPanelAt,challengeAt,courtReadyAt,streetNoticeUntil;
        string streetNotice="",streetReply="";
        readonly bool[] courtShotMade=new bool[3];int courtRoundResult=-1;float courtResultUntil;
        Vector3 muralCameraStart;Quaternion muralCameraRotation;
        bool StreetPanelOpen=>muralOpen||streetStoryOpen;
        NeighbourhoodProgress StreetMemory=>progress.neighbourhood??(progress.neighbourhood=new NeighbourhoodProgress());
        static readonly int[] StreetGestures={3,2,4,6};
        static readonly string[] StreetStoryNames={"NO CAFÉ","PELAS LAJES","NA GALERIA"};
        static readonly string[][] StreetChapters={
            new[]{"Quero montar uma noite de música aqui no café. Só falta convencer o bairro de que meu gosto musical presta.","A primeira pessoa já topou tocar. Agora preciso escolher: começo tranquilo ou já entro na batida?","Ontem apareceu um DJ com uma caixa de discos maior que ele. Acho que a noite ganhou uma direção.","Deram a ideia de cada pessoa trazer uma música. O problema é que todo mundo diz: 'só mais uma'.","A tomada está pronta, a playlist também. Hoje descobri que organizar a festa dá mais trabalho que fazer café.","No ensaio, até o pessoal da oficina parou pra ouvir. Se der certo, isso vira tradição.","Conseguimos! O bairro trouxe as músicas e eu fiz café pra geral. Você também faz parte desse rolê."},
            new[]{"Alguém perdeu um boné perto do residencial. Vou deixar guardado até aparecer o dono.","O boné ainda está comigo. O gato já tentou reivindicar como cama. Tem bom gosto, esse bicho.","Descobri que o boné veio da quadra. Agora preciso achar alguém que admita ter perdido aquela cesta.","O dono apareceu! Disse que o boné não dava sorte. Olhando o resultado do jogo, talvez ele tenha razão.","Lavamos o boné e fizemos as pazes com ele. Amanhã vai ter revanche na quadra.","Dessa vez teve cesta! Estão dizendo que o segredo foi lavar o boné. Eu acho que foi treinar.","O boné virou troféu do bairro. Quem errar menos leva pra casa. O gato apresentou um recurso."},
            new[]{"Quero fazer uma exposição de fotos do bairro. Fotos das pessoas de verdade, no corre do dia a dia.","Já tenho a primeira foto. Não é de um prédio bonito: é alguém rindo na porta da loja.","Hoje fotografei a quadra. Você acha que uma foto conta mais sobre a roupa ou sobre quem está nela?","O mural está começando a ganhar cor. Cada adesivo parece uma assinatura de quem passou por aqui.","Pensei em chamar a exposição de 'Nosso bairro'. Não é muito original, mas é exatamente o que ela é.","Estamos escolhendo as últimas fotos. A melhor parte é que todo mundo se reconhece em algum detalhe.","A exposição abriu. Sete noites, um monte de encontros e uma cidade que ficou com a nossa cara."}
        };
        void StreetNotice(string text){streetNotice=text;streetNoticeUntil=contextClock+3.3f;}
        static int StoryForResident(int index)=>index==0?0:index==4?1:index==5?2:-1;
        string GestureLabel(int id)=>id==2?"Palmas [2]":id==3?"Aceno [3]":id==4?"67 [4]":"Absolute cinema [5]";
        WorldHint StreetActivityHint(){
            if(!StreetBubblesAllowed||StreetPanelOpen||city.ChallengeResident>=0)return default;
            if(city.BallHeld)return Hint("E","Arremessar no centro da barra",city.BallPoint,Vector3.one*.6f);
            int sticker=city.LookedSticker(cam,StreetMemory);if(sticker>=0)return Hint("E","Guardar adesivo: "+CityView.StickerNames[sticker],city.transform.TransformPoint(city.StickerPoint(sticker)),new Vector3(.5f,.7f,.1f));
            if(city.LookingAtMural(cam))return Hint("E","Montar o mural do bairro",city.transform.TransformPoint(CityView.CollectiveMural),new Vector3(3,2.3f,.12f));
            if(!city.BallFlying&&city.ActivityLook(cam,CityView.CourtBallSpot))return Hint("E","Pegar a bola / rodada de 3",city.BallPoint,Vector3.one*.65f);
            int resident=city.LookedStreetResident(cam);if(resident>=0){int story=StoryForResident(resident);if(story>=0||!StreetMemory.ChallengeDone(progress.day))return Hint("E",story>=0?"Conversar / história do dia":"Desafio de emotes",city.transform.position+city.ResidentPosition(resident)+Vector3.up*1.6f,new Vector3(1.2f,3.3f,1));}
            return default;
        }
        bool TryStreetActivity(){
            if(!StreetBubblesAllowed||StreetPanelOpen)return false;
            if(city.ChallengeResident>=0){StreetNotice("Repita o gesto usando os números indicados.");return true;}
            if(city.BallHeld){
                if(!city.ActivityLook(cam,CityView.CourtHoop,15)){StreetNotice("Olhe para a cesta antes de arremessar.");return true;}
                city.ThrowCourtBall(cam.transform.position+cam.transform.forward*.65f,Mathf.Abs(CourtPower-.5f)<.13f);courtPending=true;courtPendingMade=Mathf.Abs(CourtPower-.5f)<.13f;PlayClip(Clip("equip_tecido_01"),.25f);return true;
            }
            int sticker=city.LookedSticker(cam,StreetMemory);
            if(sticker>=0){StreetMemory.Collect(sticker);city.ApplyStreetMemories(StreetMemory);TrySave();Sound(0);StreetNotice("Adesivo guardado: "+CityView.StickerNames[sticker]+". Leve ao mural perto da galeria!");return true;}
            if(city.LookingAtMural(cam)){OpenStreetPanel(true);BeginMuralDraft();return true;}
            if(!city.BallFlying&&city.ActivityLook(cam,CityView.CourtBallSpot)){if(courtAttempts==0){courtRoundResult=-1;courtResultUntil=0;System.Array.Clear(courtShotMade,0,3);}city.HoldCourtBall();courtReadyAt=contextClock;city.CancelPlayerEmote(true);return true;}
            int resident=city.LookedStreetResident(cam);if(resident<0)return false;
            int story=StoryForResident(resident);
            if(story>=0){streetStory=story;streetStoryResident=resident;streetReply=StreetMemory.Talked(progress.day,story)?"Bom te encontrar de novo. "+(progress.day<7?"Amanhã eu te conto o resto!":"Essa semana vai ficar na memória."):"";OpenStreetPanel(false);return true;}
            if(StreetMemory.ChallengeDone(progress.day))return false;
            city.CancelPlayerEmote(true);city.StartStreetChallenge(resident,StreetGestures[(progress.day-1)%4]);challengeAt=contextClock;challengeSerial=city.PlayerEmoteSerial;streetNoticeUntil=0;return true;
        }
        void OpenStreetPanel(bool mural){streetPreviousCursor=cityFreeCursor;muralOpen=mural;streetStoryOpen=!mural;streetPanelAt=contextClock;muralCameraStart=cam.transform.position;muralCameraRotation=cam.transform.rotation;city.StopWalk();city.CancelPlayerEmote(true);cityFreeCursor=true;cityDragLook=false;SyncCityCursor(true);}
        void CloseStreetPanel(){ReleaseMuralDrag();bool editingMural=muralOpen;bool open=StreetPanelOpen;muralOpen=streetStoryOpen=false;if(open&&city!=null){city.EndStreetSpeaker();if(!city.IsInterior)city.ApplyStreetMemories(StreetMemory);if(editingMural){city.Frame(cam,reducedMotion,0);city.FramePlayerForCamera(cam,false);}cityFreeCursor=streetPreviousCursor;SyncCityCursor(true);}}
        void ResetStreetActivities(){CloseStreetPanel();if(city!=null){city.EndStreetChallenge();city.ResetCourtBall();}courtPending=false;courtAttempts=courtMade=0;courtRoundResult=-1;courtResultUntil=0;streetNoticeUntil=0;}
        float CourtPower=>.5f+.5f*Mathf.Sin((contextClock-courtReadyAt)*2.8f-Mathf.PI*.5f);
        void TickStreetActivities(){
            if(!StreetBubblesAllowed||paused||loadingActive)return;
            if(streetStoryOpen)city.HoldStreetSpeaker(streetStoryResident);
            if(city.ChallengeResident>=0){
                float age=contextClock-challengeAt;var point=city.ResidentPosition(city.ChallengeResident);
                if(age>14||Vector2.Distance(city.Position,new Vector2(point.x,point.z))>7){city.EndStreetChallenge();StreetNotice("Depois a gente tenta de novo. Sem pressa!");}
                else if(age>1.2f&&city.PlayerEmoteSerial>challengeSerial){
                    challengeSerial=city.PlayerEmoteSerial;int expected=StreetGestures[(progress.day-1)%4];
                    if(city.PlayerEmoteId==expected){StreetMemory.CompleteChallenge(progress.day);city.EndStreetChallenge();city.ApplyStreetMemories(StreetMemory);TrySave();PlayClip(Clip("small_applause_01"),.35f);StreetNotice("Aí sim! Desafio do dia concluído. Confira seus adesivos no mural.");}
                    else StreetNotice("Quase! Repita "+GestureLabel(expected)+".");
                }
            }
            if(city.BallHeld&&!city.InCourt){city.ResetCourtBall();courtAttempts=courtMade=0;StreetNotice("A bola fica na quadra. A gente joga depois!");}
            if(courtPending&&!city.BallFlying){courtPending=false;courtShotMade[courtAttempts]=courtPendingMade;courtAttempts++;if(courtPendingMade)courtMade++;courtResultUntil=contextClock+3.3f;city.ReactToCourt(courtPendingMade);PlayClip(Clip(courtPendingMade?"small_applause_01":"court_ball_02"),.35f);
                if(courtAttempts>=3){courtRoundResult=courtMade;StreetMemory.Court(progress.day,courtMade);city.ApplyStreetMemories(StreetMemory);TrySave();StreetNotice(courtMade+" de 3! "+(courtMade>=2?"Adesivo Só cesta conquistado!":"Mais uma rodada?"));courtAttempts=courtMade=0;}
                else StreetNotice((courtPendingMade?"Cesta! ":"Na próxima vai! ")+courtMade+" acertos / "+courtAttempts+" de 3 arremessos.");
            }
        }
        void FrameStreetActivities(){
            if(!StreetBubblesAllowed)return;city.FrameHeldCourtBall(cam);
            if(!muralOpen)return;
            var p=city.MuralWorldPoint(new Vector3(.8f,.65f,-4.2f));var r=Quaternion.LookRotation(city.MuralWorldPoint(Vector3.right*.7f)-p);
            float t=reducedMotion?1:Mathf.SmoothStep(0,1,(contextClock-streetPanelAt)/.55f);cam.transform.position=Vector3.Lerp(muralCameraStart,p,t);cam.transform.rotation=Quaternion.Slerp(muralCameraRotation,r,t);cam.fieldOfView=50;city.PlayerBodyVisible=false;
        }
        bool DrawStreetActivityBubble(){
            if(StreetPanelOpen)return true;
            if(city.ChallengeResident>=0){int expected=StreetGestures[(progress.day-1)%4];ComicBubble(new Rect(420,650,600,170),contextClock<streetNoticeUntil?streetNotice:contextClock-challengeAt<1.2f?"Olha o gesto! Depois é sua vez.":"Sua vez: "+GestureLabel(expected)+". Você tem "+Mathf.CeilToInt(14-contextClock+challengeAt)+" s.","DESAFIO DO BAIRRO",contextClock-challengeAt,false);return true;}
            if(city.BallHeld||city.BallFlying||city.InCourt&&contextClock<courtResultUntil)return false;
            if(contextClock<streetNoticeUntil){ComicBubble(new Rect(410,660,620,165),streetNotice,"NO MESMO CORRE",contextClock-(streetNoticeUntil-3.3f),false);return true;}
            return false;
        }
        void DrawStreetActivitiesHUD(){
            bool result=contextClock<courtResultUntil;bool holding=city.BallHeld;
            if(!holding&&!city.BallFlying&&!(result&&city.InCourt))return;
            bool finished=!holding&&courtAttempts==0&&courtRoundResult>=0&&result;
            DrawKaosPanel(new Rect(438,635,564,208));
            RoundedPanel(new Rect(455,653,58,27),CreatorRed,5);Txt(new Rect(460,657,53,24),"3x RUA",tiny,paper);
            Txt(new Rect(529,651,265,32),finished?"FIM DE RODADA":"SÓ CESTA",copy,paper);
            int attempts=finished?3:courtAttempts;
            for(int i=0;i<3;i++){
                var r=new Rect(864+i*38,651,29,29);bool shot=i<attempts;var color=shot?(courtShotMade[i]?new Color(.24f,.67f,.43f):new Color(.56f,.24f,.26f)):KaosControl;
                RoundedPanel(r,color,15);Txt(new Rect(r.x,r.y-1,r.width,29),shot?(courtShotMade[i]?"+":"–"):(i+1).ToString(),action,paper);
            }
            if(holding){
                Txt(new Rect(462,694,440,32),"Aperte E na zona marcada",caption,paper);
                RoundedPanel(new Rect(930,692,42,35),CreatorRed,6);Txt(new Rect(930,692,42,33),"E",action,paper);
                RoundedPanel(new Rect(462,744,516,25),KaosTrack,7);
                RoundedPanel(new Rect(462+516*.37f,740,516*.26f,33),new Color(.18f,.58f,.36f),5);
                Fill(new Rect(462+516*.5f-1,736,2,42),new Color(1,1,1,.45f));
                float cursor=462+512*CourtPower;RoundedPanel(new Rect(cursor-4,735,8,43),paper,3);
                Txt(new Rect(462,787,510,28),"Mire na cesta  /  "+courtMade+" acerto"+(courtMade==1?"":"s")+"  /  arremesso "+(courtAttempts+1)+" de 3",tiny,muted);
            }else{
                string title=city.BallFlying?"NO AR…":finished?courtRoundResult+" DE 3!":courtPendingMade?"CESTA!":"NO ARO!";
                Txt(new Rect(463,700,500,53),title,heading,finished||courtPendingMade?paper:new Color(.96f,.65f,.48f));
                Txt(new Rect(463,768,506,53),city.BallFlying?"Acompanhe o arremesso.":finished?(courtRoundResult>=2?"Adesivo Só cesta conquistado!":"Bora tentar outra rodada?"):"Pegue a bola para a próxima tentativa.",caption,muted);
            }
        }
        void DrawStreetPanel(){
            if(muralOpen){
                MuralEditorUI();
            }else{
                string text=streetReply.Length>0?streetReply:StreetChapters[streetStory][progress.day-1];
                if(streetReply.Length==0&&progress.day>1&&(StreetMemory.supported&(1<<((progress.day-2)*3+streetStory)))!=0)text+=" Levei a sua ideia de ontem comigo.";
                ComicBubble(new Rect(48,537,820,261),text,StreetStoryNames[streetStory]+" / DIA "+progress.day,contextClock-streetPanelAt,false);
                DrawKaosPanel(new Rect(928,440,466,360));Txt(new Rect(954,466,414,48),"Trocar uma ideia",heading,paper);
                if(streetReply.Length==0){
                    if(Tap(new Rect(954,540,414,55),"Bora, eu apoio!",true)){StreetMemory.Talk(progress.day,streetStory,true);TrySave();streetReply="Fechou! Vou lembrar disso. É bom saber que tem gente no mesmo corre.";streetPanelAt=contextClock;city.ApplyStreetMemories(StreetMemory);}
                    if(Tap(new Rect(954,609,414,55),"Hoje vou só acompanhar")){StreetMemory.Talk(progress.day,streetStory,false);TrySave();streetReply="Tranquilo. Só de parar pra ouvir você já faz parte da história.";streetPanelAt=contextClock;city.ApplyStreetMemories(StreetMemory);}
                }
                if(Tap(new Rect(954,717,414,50),streetReply.Length>0?"Até mais!":"Voltar à rua"))CloseStreetPanel();
            }
        }
    }
}
