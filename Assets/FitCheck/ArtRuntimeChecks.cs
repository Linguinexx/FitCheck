using System;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        public void ArtSmokeCheck()
        {
            int oldCustomer=customer;var oldOutfit=outfit;var originalProgress=progress;var originalStorage=storageOverride;string state=JsonUtility.ToJson(progress);
            var retro=cam.GetComponent<RetroCamera>();int oldFrame=retro.OverrideFrame;
            try{
                customer=0;outfit=Catalog.Default;Apply();retro.OverrideFrame=0;Shader.SetGlobalFloat("_PaintFrame",0);RenderCurrentPhoto();var a=photo.GetPixels32();
                retro.OverrideFrame=1;Shader.SetGlobalFloat("_PaintFrame",1);RenderCurrentPhoto();var b=photo.GetPixels32();int changed=0;long difference=0;
                for(int i=0;i<a.Length;i++){int d=Math.Abs(a[i].r-b[i].r)+Math.Abs(a[i].g-b[i].g)+Math.Abs(a[i].b-b[i].b);if(d>0)changed++;difference+=d;}
                if(changed<100||difference/(a.Length*3f)>10)throw new Exception("Animação de pintura ausente ou excessiva");
                // Fractional phases must actually produce in-between images.
                // Testing endpoints alone would allow the old hard frame swaps.
                Shader.SetGlobalFloat("_PaintFrame",.5f);RenderCurrentPhoto();var middle=photo.GetPixels32();int intermediate=0;
                for(int i=0;i<a.Length;i++)if(!middle[i].Equals(a[i])&&!middle[i].Equals(b[i]))intermediate++;
                if(intermediate<100)throw new Exception("Pintura ainda troca quadros sem transição");
                Shader.SetGlobalFloat("_PaintFrame",3.99f);RenderCurrentPhoto();var wrap=photo.GetPixels32();long seam=0;
                for(int i=0;i<a.Length;i++)seam+=Math.Abs(a[i].r-wrap[i].r)+Math.Abs(a[i].g-wrap[i].g)+Math.Abs(a[i].b-wrap[i].b);
                if(seam/(a.Length*3f)>.05f)throw new Exception("Salto na emenda do ciclo da pintura");
                Shader.SetGlobalFloat("_PaintFrame",0);RenderCurrentPhoto();var stillA=photo.GetPixels32();RenderCurrentPhoto();var stillB=photo.GetPixels32();
                for(int i=0;i<stillA.Length;i++)if(!stillA[i].Equals(stillB[i]))throw new Exception("Quadro congelado não é estável");
                if(JsonUtility.ToJson(progress)!=state)throw new Exception("Teste visual alterou progressão");
                var folder=System.IO.Path.Combine(Application.temporaryCachePath,"FitCheck-Art-"+Guid.NewGuid());System.IO.Directory.CreateDirectory(folder);
                try{
                    progress=new DayProgress();progress.Add(new Attempt{outfit=Catalog.Default,question=1});storageOverride=folder;
                    string oldImage=System.IO.Path.Combine(folder,"previous.png");System.IO.File.WriteAllBytes(oldImage,photo.EncodeToPNG());progress.posts[0].photo=oldImage;progress.posts[0].photoStyle=ArtRevision-1;SaveDay();
                    RefreshPhotoArt();
                    var updated=ProgressStore.Load(DayPath);var backup=ProgressStore.Load(DayPath+".bak");
                    if(updated.posts.Count!=1||updated.reputation!=0||updated.posts[0].score!=3||updated.posts[0].question!=1||updated.posts[0].photoStyle!=ArtRevision||updated.posts[0].photo==oldImage||!System.IO.File.Exists(oldImage)||backup.posts[0].photo!=oldImage)throw new Exception("Atualização visual mudou progresso ou perdeu imagem anterior");
                }finally{progress=originalProgress;storageOverride=originalStorage;foreach(var file in System.IO.Directory.GetFiles(folder))System.IO.File.Delete(file);System.IO.Directory.Delete(folder);}
                Debug.Log("FITCHECK_ART_RUNTIME_PASSED: "+changed+" pixels animados, "+intermediate+" pixels intermediários, ciclo contínuo, quadro congelado estável e progressão preservada.");
            }finally{progress=originalProgress;storageOverride=originalStorage;customer=oldCustomer;outfit=oldOutfit;retro.OverrideFrame=oldFrame;Apply();}
        }
    }
}
