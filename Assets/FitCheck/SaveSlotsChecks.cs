using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        void CheckSaveSlots(string folder)
        {
            var originalProgress=progress;var originalAppearance=playerAppearance;var originalRecord=record;
            string originalStorage=storageOverride,originalLibrary=saveLibraryOverride;
            int originalSlot=activeSaveSlot,originalSelected=selectedSave;
            string library=Path.Combine(folder,"save-slots-"+Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(library);
            try{
                Home();saveLibraryOverride=library;
                var legacy=new DayProgress();
                for(int i=0;i<9;i++){
                    legacy.Add(new Attempt{outfit=(string[])Catalog.Default.Clone(),question=1});
                    if(legacy.DayComplete){legacy.Close();if(i<8)legacy.Advance();}
                }
                ProgressStore.Save(Path.Combine(library,"day-one.json"),legacy);
                AppearanceStore.Save(Path.Combine(library,"player-appearance.json"),new PlayerAppearance{created=true,name="Lia",hair=1,skin=2,hairColor=5});
                string originalLegacy=File.ReadAllText(Path.Combine(library,"day-one.json"));
                OpenSaveSelection();
                if(saveSlots[0].progress.Complete||!saveSlots[0].progress.closed||saveSlots[0].progress.day!=3||
                    saveSlots[0].appearance.name!="Lia"||saveSlots[1].exists||saveSlots[2].exists)
                    throw new Exception("Migração não preservou o terceiro dia ou ocupou outro espaço");
                PreviewSave(0);OpenSelectedSave();
                if(stage!=Stage.City||!city.IsHome||progress.Complete||!progress.closed||progress.day!=3)
                    throw new Exception("Continuar save não retomou o terceiro dia encerrado em casa");
                string first=File.ReadAllText(DayPath);
                OpenSaveSelection();PreviewSave(1);OpenSelectedSave();
                if(stage!=Stage.CharacterCreator||progress.posts.Count!=0)throw new Exception("Novo save não abriu criação independente");
                CancelCreator();
                if(SaveSlots.Read(library,1).exists)throw new Exception("Cancelar criação ocupou espaço vazio");
                OpenSaveSelection();PreviewSave(1);OpenSelectedSave();creatorDraft.name="Duda";creatorDraft.hair=3;creatorDraft.skin=1;RefreshCreator();
                if(!SaveCreator()||stage!=Stage.City||!city.IsHome||!File.Exists(DayPath))throw new Exception("Novo save não persistiu personagem e campanha");
                EnterShop();SettleShopArrival();MoveProof(CityView.ShopCounter,180,0);InteractShop();
                if(stage!=Stage.Dialogue||!shopService)throw new Exception("Save novo abriu resumo em vez de cliente");
                Home();progress.Add(new Attempt{outfit=(string[])Catalog.Default.Clone(),question=1});SaveDay();
                OpenSaveSelection();PreviewSave(2);OpenSelectedSave();creatorDraft.name="Nico";creatorDraft.hair=2;creatorDraft.skin=4;RefreshCreator();
                if(!SaveCreator())throw new Exception("Terceiro espaço não salvou");
                bool refused=false;try{SaveSlots.Folder(library,3);}catch(ArgumentOutOfRangeException){refused=true;}
                if(!refused)throw new Exception("Mais de três espaços permitidos");
                if(File.ReadAllText(Path.Combine(SaveSlots.Folder(library,0),"day-one.json"))!=first||File.ReadAllText(Path.Combine(library,"day-one.json"))!=originalLegacy)throw new Exception("Novo jogo alterou o save antigo");
                OpenSaveSelection();PreviewSave(1);OpenSelectedSave();
                if(progress.posts.Count!=1||playerAppearance.name!="Duda")throw new Exception("Troca de save misturou progresso/aparência");
                string second=DayPath;ProgressStore.Save(second,progress);File.WriteAllText(second,"invalid");
                if(SaveSlots.Read(library,1).progress.posts.Count!=1||SaveSlots.Read(library,1).error!=null)throw new Exception("Backup do slot não recuperou campanha");
                File.WriteAllText(second+".bak","invalid");
                OpenSaveSelection();PreviewSave(1);
                if(OpenSelectedSave()||File.ReadAllText(second)!="invalid")throw new Exception("Slot corrompido sobrescrito");
                ProgressStore.Save(second,progress);
                OpenSaveSelection();PreviewSave(0);RequestSaveDeletion();
                if(pendingDeleteSlot!=0||!SaveSlots.Read(library,0).exists)throw new Exception("Pedido de exclusão apagou antes da confirmação");
                CancelSaveDeletion();if(!SaveSlots.Read(library,0).exists)throw new Exception("Cancelar exclusão removeu save");
                string thirdBeforeDelete=File.ReadAllText(Path.Combine(SaveSlots.Folder(library,2),"day-one.json"));
                RequestSaveDeletion();selectedSave=2;
                if(!ConfirmSaveDeletion()||SaveSlots.Read(library,0).exists||File.ReadAllText(Path.Combine(SaveSlots.Folder(library,2),"day-one.json"))!=thirdBeforeDelete)throw new Exception("Confirmação não isolou o save escolhido");
                SaveSlots.ImportLegacy(library);if(SaveSlots.Read(library,0).exists)throw new Exception("Save legado reapareceu após exclusão");
                PreviewSave(1);RequestSaveDeletion();if(!ConfirmSaveDeletion()||activeSaveSlot!=-1||SaveSlots.Read(library,1).exists)throw new Exception("Exclusão manteve save ativo na memória");
                bool refusedWrite=false;try{SaveDay();}catch(InvalidOperationException){refusedWrite=true;}if(!refusedWrite||SaveSlots.Read(library,1).exists)throw new Exception("Salvar recriou espaço excluído");
                PreviewSave(0);OpenSelectedSave();creatorDraft.name="Nova história";RefreshCreator();if(!SaveCreator()||progress.posts.Count!=0)throw new Exception("Espaço excluído não permite novo jogo");
                BuildTitlePhoto();File.WriteAllBytes(Path.Combine(folder,"title-facade-photo.png"),titlePhoto.EncodeToPNG());
                Debug.Log("FITCHECK_SAVE_SLOTS_PASSED: three independent slots; legacy completed week retained; creation/cancel, switching, new-store customer, backup recovery and corrupt-slot protection.");
            }finally{
                Home();progress=originalProgress;playerAppearance=originalAppearance;record=originalRecord;storageOverride=originalStorage;saveLibraryOverride=originalLibrary;
                activeSaveSlot=originalSlot;selectedSave=originalSelected;Home();UpdateCamera();
            }
        }
    }
}
