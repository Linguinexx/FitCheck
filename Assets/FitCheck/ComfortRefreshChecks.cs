using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckComfortRefresh(string folder){
            Home();storageOverride=Path.Combine(folder,"sessions",Guid.NewGuid().ToString("N"));Directory.CreateDirectory(storageOverride);progress=new DayProgress();paused=false;mutedAudio=false;masterVolume=0;
            var retro=cam.GetComponent<RetroCamera>();retro.Filter=false;retro.Paused=true;retro.OverrideFrame=0;Shader.SetGlobalFloat("_PaintFrame",0);
            EnterHome();SetInkOutlinePreview(true);city.PlayerBodyVisible=false;
            cam.transform.position=city.transform.TransformPoint(new Vector3(.3f,3.1f,-.3f));cam.transform.LookAt(city.transform.TransformPoint(new Vector3(-3.4f,1,2.3f)));cam.fieldOfView=55;
            SaveFirstPersonFrame(Path.Combine(folder,"bed.png"),1440,900);
            var pet=city.Cat;for(int i=0;i<60;i++){pet.Tick(.02f,false,false,true);pet.ValidateEyeVisibility();}
            cam.transform.position=pet.transform.TransformPoint(new Vector3(1.15f,1.3f,-1.8f));cam.transform.LookAt(pet.PetTarget);cam.fieldOfView=46;
            SaveFirstPersonFrame(Path.Combine(folder,"pet-awake.png"),1440,900);
            for(int i=0;i<450;i++){pet.Tick(.02f,false,false,true);pet.ValidateEyeVisibility();}
            SaveFirstPersonFrame(Path.Combine(folder,"pet-resting.png"),1440,900);
            float age=pet.Age;pet.Tick(1,true,false,true);if(pet.Age!=age)throw new Exception("Pet avançou na pausa");
            for(int species=0;species<2;species++)for(int breed=0;breed<(species==0?4:6);breed++)for(int coat=0;coat<6;coat++){
                var node=new GameObject("Verificação isolada do pet");var check=node.AddComponent<ShopCat>();check.Build(new PetAppearance{species=species,breed=breed,coat=coat,name="Revisão"});check.ValidateEyeVisibility();for(int i=0;i<140;i++){check.Tick(.05f,false,i%2==0,true);check.ValidateEyeVisibility();}node.SetActive(false);Destroy(node);
            }
            for(int id=1;id<=6;id++){
                city.CancelPlayerEmote(true);MoveProof(CityView.HomeSpawn,310,12);city.PlayPlayerEmote(id);for(int i=0;i<18;i++)city.Step(Vector2.zero,.05f,false,mannequin);city.Frame(cam,true,0);city.FramePlayerForCamera(cam,false);SetInkOutlinePreview(true);
                SaveFirstPersonFrame(Path.Combine(folder,"wrist-emote-"+id+".png"),1440,900);
                foreach(var hand in city.GetComponentsInChildren<MeshFilter>())if(hand.name=="Junção suave — pulso"&&hand.sharedMesh.vertexCount!=65)throw new Exception("Junção do pulso cresceu indevidamente");
            }
            city.CancelPlayerEmote(true);MoveProof(new Vector2(2.75f,1.36f),90,10);city.PlayPlayerEmote(1);for(int i=0;i<18;i++)city.Step(Vector2.zero,.05f,false,mannequin);city.Frame(cam,true,0);city.FramePlayerForCamera(cam,false);city.RefreshInkPreviewMirror(cam);SaveFirstPersonFrame(Path.Combine(folder,"mirror-wrist.png"),1440,900);
            city.CancelPlayerEmote(true);mannequin.gameObject.SetActive(true);studio.ApplyUniform(2);city.PreparePlayerBody(studio);mannequin.gameObject.SetActive(false);if(!city.PlayerBodyReady)throw new Exception("Prévia de manga curta sem corpo");MoveProof(CityView.HomeSpawn,310,12);city.PlayPlayerEmote(3);for(int i=0;i<18;i++)city.Step(Vector2.zero,.05f,false,mannequin);city.Frame(cam,true,0);city.FramePlayerForCamera(cam,false);SetInkOutlinePreview(true);SaveFirstPersonFrame(Path.Combine(folder,"wrist-short-sleeve.png"),1440,900);
            Home();EnterShop();SetInkOutlinePreview(true);LookAtShopMoment(new Vector2(5.45f,-1.5f),CityView.ShopRadioSpot);if(!TryShopMoment())throw new Exception("Seletor de discos inacessível");
            for(int i=0;i<3;i++){
                var song=ShopSongs[i][0];var record=Resources.Load<AudioClip>("Audio/ShopRecords/"+song.path);shopSongCache[song.path]=record;if(record==null||record.length<30||Array.IndexOf(tracks,record)>=0)throw new Exception("Disco está ausente");
                int before=StreetMemory.musicChoice;shopRecordSelected=i;if(StreetMemory.musicChoice!=before)throw new Exception("Navegar trocou a música");ChooseShopMusic(i);TickPresentation(.1f);
                if(ShopRecordClip!=record)throw new Exception("Disco incorreto");CaptureInterface(Path.Combine(folder,"record-"+i+".png"));
            }
            StopShopRecord();TickPresentation(.1f);if(music.clip!=null||incomingMusic.clip!=null)throw new Exception("Retirar deixou música tocando");CaptureInterface(Path.Combine(folder,"records-stopped.png"));
            var loaded=ProgressStore.Load(DayPath);if(!loaded.neighbourhood.musicStopped||loaded.neighbourhood.musicChoice!=2)throw new Exception("Preferência do disco não persistiu");
            var legacy=JsonUtility.FromJson<NeighbourhoodProgress>("{\"musicChoice\":1}");if(legacy.musicStopped)throw new Exception("Save legado perdeu a reprodução");
            ChooseShopMusic(0);TickPresentation(.2f);paused=true;TickPresentation(.5f);if(!music.isPlaying&&!incomingMusic.isPlaying)throw new Exception("Disco parou na pausa");paused=false;TickPresentation(.2f);
            CloseShopMoment();if(ShopLeisurePanelOpen||cityFreeCursor)throw new Exception("Seletor não devolveu o controle");
            EnterHome();TickPresentation(.2f);if(ShopLeisurePanelOpen||incomingMusic.clip!=tracks[2]&&music.clip!=tracks[2])throw new Exception("Disco vazou para o quarto");
            CheckNeighbourhoodActivities(Path.Combine(folder,"activities"));Home();progress=new DayProgress();EnterCity();SetInkOutlinePreview(true);
            PlaceForStreetActivity(CityView.CourtBallSpot);cam.transform.LookAt(city.transform.TransformPoint(CityView.CourtHoop));SaveFirstPersonFrame(Path.Combine(folder,"hoop.png"),1440,900);
            for(int i=0;i<7;i++){PlaceForStreetActivity(city.StickerPoint(i));SaveFirstPersonFrame(Path.Combine(folder,"collectible-"+i+".png"),1440,900);}
            StreetMemory.stickers=127;PlaceForStreetActivity(CityView.CollectiveMural);TryStreetActivity();contextClock+=1;FrameStreetActivities();muralDraft=127;PreviewMural();SelectMuralPoster(0);TurnMuralPoster(30);CaptureInterface(Path.Combine(folder,"mural-selection.png"));CloseStreetPanel();
            CheckShopMoments(Path.Combine(folder,"shop-regression"));
            File.WriteAllText(Path.Combine(folder,"checks.txt"),"COMFORT_REFRESH_PASSED: exclusive record clips/play/stop/browse/pause/persistence/legacy/home isolation; 60 pet breed-coat combinations and mutually exclusive eye states; six wrist joints and mirror captures; Blender bed/hoop; three-shot court and collectible access; full shop/service regression. Native visual review required alongside these checks.");Debug.Log(File.ReadAllText(Path.Combine(folder,"checks.txt")));
        }
    }
}
