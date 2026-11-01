using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CreatorSmokeCheck(string folder)
        {
            var original=playerAppearance.Copy();var originalProgress=JsonUtility.ToJson(progress);
            string path=AppearancePath;byte[] saved=File.Exists(path)?File.ReadAllBytes(path):null;
            byte[] backup=File.Exists(path+".bak")?File.ReadAllBytes(path+".bak"):null;
            try{
                Home();OpenCreator();creatorDraft.hair=5;creatorDraft.skin=2;RefreshCreator();CancelCreator();
                if(JsonUtility.ToJson(playerAppearance)!=JsonUtility.ToJson(original))throw new Exception("Cancelar mudou a aparência");
                OpenCreator();creatorDraft.name="Teste do avatar";creatorDraft.hair=0;creatorDraft.skin=2;creatorDraft.hairColor=2;creatorDraft.expression=4;RefreshCreator();
                SaveFirstPersonFrame(Path.Combine(folder,"creator-character.png"));
                if(!SaveCreator())throw new Exception("Editor não salvou aparência");
                var loaded=AppearanceStore.Load(path);if(!loaded.created||loaded.name!="Teste do avatar"||loaded.skin!=2||loaded.hairColor!=2)throw new Exception("Aparência salva divergiu");
                var stored=File.ReadAllText(path);OpenCreator();creatorDraft.skin=1;RefreshCreator();CancelCreator();
                if(File.ReadAllText(path)!=stored||playerAppearance.skin!=2)throw new Exception("Cancelar sobrescreveu o jogador");
                for(int cut=0;cut<PlayerAppearance.HairNames.Length;cut++){
                    OpenCreator();creatorDraft.hair=cut;creatorDraft.hairColor=cut%PlayerAppearance.HairColors.Length;creatorDraft.expression=cut%PlayerAppearance.FaceNames.Length;RefreshCreator();studio.Animate(.05f,true);
                    foreach(var r in studio.Avatar.GetComponentsInChildren<MeshRenderer>())if(r.name=="Cabeça arredondada"&&r.bounds.size.y<1)throw new Exception("Criação alterou a base da cabeça");
                    CancelCreator();
                }
                var copy=loaded.Copy();copy.skin=4;AppearanceStore.Save(path,copy);File.WriteAllText(path,"invalid");
                if(AppearanceStore.Load(path).skin!=2)throw new Exception("Backup da aparência não foi recuperado");
                AppearanceStore.Save(path,loaded);
                if(JsonUtility.ToJson(progress)!=originalProgress)throw new Exception("Criação alterou a campanha");
                Debug.Log("FITCHECK_CREATOR_PASSED: editor save/cancel, all hairstyles/colors/expressions, player persistence, corrupt-save recovery and campaign separation.");
            }finally{
                playerAppearance=original;creatorDraft=null;studio.PlayerLook=playerAppearance;
                if(saved==null){if(File.Exists(path))File.Delete(path);}else File.WriteAllBytes(path,saved);
                if(backup==null){if(File.Exists(path+".bak"))File.Delete(path+".bak");}else File.WriteAllBytes(path+".bak",backup);
                Home();UpdateCamera();
            }
        }
    }
}
