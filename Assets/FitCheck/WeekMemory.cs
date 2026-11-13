using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    public partial class CityView
    {
        public static readonly Vector3 WeekFramePosition=new Vector3(-3.35f,1.79f,-3.02f);
        Material weekFrameMaterial;
        void BuildWeekFrame(){
            var frame=FramedPrint("Porta-retrato — nossa semana",WeekFramePosition,180,0,new Vector2(.95f,.535f));
            frame.localRotation=Quaternion.Euler(10,180,0);
            frame.Find("Moldura").localScale=new Vector3(1.09f,.675f,.08f);frame.Find("Passe-partout").localScale=new Vector3(.995f,.58f,.04f);
            var image=frame.Find("Arte fornecida");image.localScale=new Vector3(.95f,.535f,1);image.name="Fotografia da semana";weekFrameMaterial=image.GetComponent<MeshRenderer>().sharedMaterial;
            Block(WeekFramePosition+new Vector3(0,-.36f,-.05f),new Vector3(.61f,.055f,.28f),Ink,"Wood");
        }
        public void SetWeekFrame(Texture2D image){if(weekFrameMaterial!=null)weekFrameMaterial.mainTexture=image!=null?image:GalleryArt.Load(0);}
        public bool LookingAtWeekFrame(Camera camera){
            if(!IsHome)return false;var delta=transform.TransformPoint(WeekFramePosition)-camera.transform.position;
            return delta.sqrMagnitude<12.25f&&Vector3.Dot(camera.transform.forward,delta.normalized)>.974f;
        }
        public int ComposeWeekPortrait(StudioView source,DayProgress campaign,PlayerAppearance look){
            transform.position=new Vector3(1200,0,0);
            Ground(0,1,19,15,-.055f,Cream,"CityStone");
            Block(new Vector3(0,.425f,1.7f),new Vector3(12,.85f,2),Ink,"Wood");
            Block(new Vector3(0,4.6f,4),new Vector3(12,1,.09f),Kaos,"Paper");WeekWords("SETE NOITES. NOSSO BAIRRO.",new Vector3(0,4.6f,3.94f),.075f,Cream);
            for(int i=-1;i<=1;i+=2){Rod(new Vector3(i*7,0,3),new Vector3(i*7,4,3),.16f,new Color(.31f,.19f,.12f));Foliage(new Vector3(i*7,4.1f,3),new Vector3(1.7f,2,1.6f),LeafGreen,i+4);}
            FinishGeometry();
            var indices=new List<int>();var seen=new HashSet<int>();
            for(int i=campaign.posts.Count-1;i>=0;i--)if(seen.Add(DayOne.CustomerProfile(i)))indices.Add(i);
            indices.Reverse();
            bool visible=source.Avatar.gameObject.activeSelf;var pos=source.Avatar.position;var rot=source.Avatar.rotation;var oldLook=source.PlayerLook;
            try{
                source.Avatar.gameObject.SetActive(true);source.Avatar.rotation=Quaternion.identity;
                int count=indices.Count+1,front=(count+1)/2;
                for(int slot=0;slot<count;slot++){
                    bool player=slot==front/2;int entry=slot<front/2?slot:slot-1;
                    if(player){source.PlayerLook=look;source.ApplyUniform(campaign.EquippedUniform);}else source.Apply(campaign.posts[indices[entry]].outfit,indices[entry]);
                    bool back=slot>=front;int col=back?slot-front:slot,cols=back?count-front:front;
                    var actor=CopyAvatar(source,player?"Você na foto":"Na foto — "+DayOne.Customers[indices[entry]].name,new Vector3((col-(cols-1)*.5f)*1.67f,back?.85f:0,back?1.85f:0),0,false);
                    actor.idleStyle=3+slot%3;actor.cycle=slot*.9f;actor.blinkClock=2;PoseResident(actor,slot,0);
                }
                foreach(var asset in assets)if(asset is Material material&&material.HasProperty("_NightCity")){material.SetFloat("_NightCity",0);if(material.HasProperty("_CharacterLit"))material.SetFloat("_CharacterLit",1);}
                return count;
            }finally{source.PlayerLook=oldLook;source.ApplyUniform(campaign.EquippedUniform);source.Avatar.SetPositionAndRotation(pos,rot);source.Avatar.gameObject.SetActive(visible);}
        }
    }
    public partial class FirstCustomer
    {
        Texture2D weekMemoryPhoto;string weekMemoryFolder;Stage weekMemoryReturn;
        string WeekPhotoPath=>Path.Combine(SessionFolder,"week-group.png");
        Texture2D LoadWeekMemory(){
            bool exists=File.Exists(WeekPhotoPath);
            if(exists&&weekMemoryFolder==SessionFolder&&weekMemoryPhoto!=null)return weekMemoryPhoto;
            if(weekMemoryPhoto!=null)Destroy(weekMemoryPhoto);weekMemoryPhoto=null;weekMemoryFolder=SessionFolder;
            if(!exists)return null;
            try{var bytes=File.ReadAllBytes(WeekPhotoPath);if(bytes.Length>4000000)return null;var texture=new Texture2D(2,2,TextureFormat.RGB24,false);if(texture.LoadImage(bytes,true)){weekMemoryPhoto=texture;}else Destroy(texture);}
            catch(Exception error) when(error is IOException||error is UnauthorizedAccessException){message="Não foi possível ler a foto da semana.";}
            return weekMemoryPhoto;
        }
        void CaptureWeekMemory(){
            if(!progress.Complete||LoadWeekMemory()!=null)return;
            GameObject group=null,cameraObject=null;RenderTexture target=null;Texture2D image=null;
            var active=RenderTexture.active;
            try{
                group=new GameObject("Ensaio de encerramento — temporário");var scene=group.AddComponent<CityView>();scene.ComposeWeekPortrait(studio,progress,playerAppearance);
                cameraObject=new GameObject("Câmera da lembrança");var camera=cameraObject.AddComponent<Camera>();camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=new Color(.025f,.035f,.055f);camera.nearClipPlane=.1f;camera.farClipPlane=40;camera.fieldOfView=36;camera.aspect=16f/9;
                camera.transform.position=scene.transform.position+new Vector3(0,4.1f,-12.8f);camera.transform.LookAt(scene.transform.position+new Vector3(0,2.1f,.8f));
                target=RenderTexture.GetTemporary(960,540,24);camera.targetTexture=target;camera.Render();RenderTexture.active=target;
                image=new Texture2D(960,540,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,960,540),0,0);image.Apply();
                Directory.CreateDirectory(SessionFolder);string temporary=WeekPhotoPath+".tmp";File.WriteAllBytes(temporary,image.EncodeToPNG());
                if(File.Exists(WeekPhotoPath))PlatformFiles.ReplaceValidated(temporary,WeekPhotoPath,null);else File.Move(temporary,WeekPhotoPath);
                weekMemoryPhoto=image;weekMemoryFolder=SessionFolder;image=null;if(homeRoom!=null)homeRoom.SetWeekFrame(weekMemoryPhoto);
            }finally{RenderTexture.active=active;if(target!=null)RenderTexture.ReleaseTemporary(target);if(image!=null)Destroy(image);if(cameraObject!=null){cameraObject.SetActive(false);Destroy(cameraObject);}if(group!=null){group.SetActive(false);Destroy(group);}}
        }
        void OpenWeekMemory(){
            if(!progress.Complete){message="A foto do bairro chega ao terminar a semana.";return;}
            weekMemoryReturn=stage;
            try{CaptureWeekMemory();if(LoadWeekMemory()==null)return;stage=Stage.WeekMemory;paused=false;cityFreeCursor=true;dragging=false;SyncCityCursor(true);}
            catch(Exception error){message="Não foi possível preparar a lembrança. Você pode tentar pelo porta-retrato.";Debug.LogWarning(message+" "+error.Message);}
        }
        void CloseWeekMemory(){stage=weekMemoryReturn;paused=false;cityFreeCursor=false;SyncCityCursor(true);}
        void WeekMemoryUI(){
            Fill(new Rect(0,0,1440,900),new Color(.025f,.022f,.028f,.97f));
            Txt(new Rect(112,38,1216,55),"Uma semana. Um bairro inteiro.",heading,paper);
            RoundedPanel(new Rect(108,113,1224,700),KaosControl,12);
            if(weekMemoryPhoto!=null)GUI.DrawTexture(new Rect(124,128,1192,670),weekMemoryPhoto,ScaleMode.ScaleToFit);
            Txt(new Rect(112,827,850,46),"Os looks que você criou ficaram na história. Foto guardada no seu quarto.",caption,paper);
            if(Tap(new Rect(1110,826,218,48),"Guardar lembrança",true))CloseWeekMemory();
        }
    }
}
