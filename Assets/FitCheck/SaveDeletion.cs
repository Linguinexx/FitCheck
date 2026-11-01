using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace FitCheck
{
    public static partial class SaveSlots
    {
        public static void Delete(string library,int slot)
        {
            string parent=Path.GetFullPath(Path.Combine(library,"saves")),target=Path.GetFullPath(Folder(library,slot));
            if(!string.Equals(Path.GetDirectoryName(target),parent,StringComparison.OrdinalIgnoreCase))throw new IOException("Pasta de save inválida.");
            var folders=new Stack<string>();if(Directory.Exists(target))folders.Push(target);
            if(Directory.Exists(parent)&&(File.GetAttributes(parent)&FileAttributes.ReparsePoint)!=0)throw new IOException("Pasta de saves redirecionada; exclusão cancelada.");
            while(folders.Count>0){
                var current=folders.Pop();if((File.GetAttributes(current)&FileAttributes.ReparsePoint)!=0)throw new IOException("Save contém pasta redirecionada; exclusão cancelada.");
                foreach(string entry in Directory.GetFileSystemEntries(current)){
                    var attrs=File.GetAttributes(entry);if((attrs&FileAttributes.ReparsePoint)!=0)throw new IOException("Save contém arquivo redirecionado; exclusão cancelada.");
                    if((attrs&FileAttributes.Directory)!=0)folders.Push(entry);
                }
            }
            // The legacy originals may still exist outside this slot. Never import them again after deletion.
            if(slot==0){Directory.CreateDirectory(parent);File.WriteAllText(Path.Combine(parent,"legacy-imported.flag"),"1");}
            if(Directory.Exists(target))Directory.Delete(target,true);
        }
    }
    public partial class FirstCustomer
    {
        int pendingDeleteSlot=-1;string deleteSaveName="";
        void RequestSaveDeletion()
        {
            var info=SaveSlots.Read(SaveLibrary,selectedSave);if(!info.exists)return;
            pendingDeleteSlot=selectedSave;deleteSaveName=info.appearance.name;message="";
        }
        void CancelSaveDeletion(){pendingDeleteSlot=-1;deleteSaveName="";message="";}
        bool ConfirmSaveDeletion()
        {
            if(pendingDeleteSlot<0)return false;int slot=pendingDeleteSlot;
            try{
                SaveSlots.Delete(SaveLibrary,slot);
                if(activeSaveSlot==slot){
                    pendingPhotoBytes=null;pendingPhotoPath=null;campaignBeforePreview=null;activeSaveSlot=-1;storageOverride=null;
                    progress=new DayProgress();playerAppearance=new PlayerAppearance();record=new Attempt();inventory=null;photo=null;
                }
                CancelSaveDeletion();OpenSaveSelection();message="Save excluído. Este espaço está livre.";return true;
            }catch(Exception e){message="Não foi possível excluir: "+e.Message;return false;}
        }
        void SaveDeletionUI()
        {
            Fill(new Rect(0,0,1440,900),new Color(.02f,.015f,.025f,.93f));Fill(new Rect(360,245,720,405),surface);
            Txt(new Rect(396,281,645,56),"Excluir este save?",heading);
            Txt(new Rect(396,350,645,45),"0"+(pendingDeleteSlot+1)+"  /  "+deleteSaveName,copy,CreatorRed);
            Txt(new Rect(396,410,645,85),"O personagem e o progresso deste espaço serão apagados. Essa ação não pode ser desfeita.",copy,paper);
            if(Tap(new Rect(396,542,280,65),"Cancelar",true))CancelSaveDeletion();
            if(Tap(new Rect(696,542,348,65),"Excluir definitivamente"))ConfirmSaveDeletion();
            Txt(new Rect(396,613,645,30),message,tiny,muted);
        }
    }
}
