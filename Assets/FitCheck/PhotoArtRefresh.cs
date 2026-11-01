using System;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        public const int ArtRevision=3;
        void RefreshPhotoArt()
        {
            int originalCustomer=customer;var originalOutfit=outfit;
            try{
                for(int i=0;i<progress.posts.Count;i++){
                    var post=progress.posts[i];if(post.photoStyle>=ArtRevision)continue;
                    customer=i;outfit=(string[])post.outfit.Clone();Apply();RenderCurrentPhoto();
                    string path=NewPhotoPath();File.WriteAllBytes(path,photo.EncodeToPNG());
                    post.photo=path;post.photoStyle=ArtRevision;SaveDay();
                }
            }catch(Exception e){message="As fotos anteriores foram preservadas; atualização visual pendente: "+e.Message;}
            finally{customer=originalCustomer;outfit=originalOutfit;Apply();UpdateCamera();}
        }
    }
}
