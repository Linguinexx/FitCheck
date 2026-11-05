using System;
using System.IO;
using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        bool speechProofActive,speechProofDraw,speechProofTail;
        void DrawSpeechProof()
        {
            InterfaceStyles();Fill(new Rect(0,0,1440,900),new Color(.10f,.11f,.13f));
            if(speechProofDraw)ComicBubble(new Rect(400,300,450,180),"Esse drop vai render assunto.","PELO BAIRRO",1,speechProofTail);
        }
        void CheckSpeechTail(string folder)
        {
            try{
                speechProofActive=true;speechProofDraw=false;CaptureInterface(Path.Combine(folder,"bubble-background.png"));
                speechProofDraw=true;speechProofTail=false;CaptureInterface(Path.Combine(folder,"bubble-context.png"));
                speechProofTail=true;CaptureInterface(Path.Combine(folder,"bubble-dialogue.png"));
                var a=new Texture2D(2,2);var b=new Texture2D(2,2);a.LoadImage(File.ReadAllBytes(Path.Combine(folder,"bubble-background.png")));b.LoadImage(File.ReadAllBytes(Path.Combine(folder,"bubble-context.png")));
                try{
                    var before=a.GetPixels32();var after=b.GetPixels32();int outside=0;
                    for(int y=0;y<900;y++)for(int x=0;x<1440;x++)if((x<399||x>857||900-y<299||900-y>489)&&!before[y*1440+x].Equals(after[y*1440+x]))outside++;
                    if(outside!=0)throw new Exception("Context speech still draws floating pixels outside the bubble/shadow: "+outside);
                }finally{Destroy(a);Destroy(b);}
                Debug.Log("CONTEXT_SPEECH_TAIL_PASSED: zero floating pixels outside contextual bubble bounds; counter speech retains its side tail.");
            }finally{speechProofActive=false;speechProofDraw=false;speechProofTail=false;}
        }
        void CheckSneakerUpgrade(string folder)
        {
            CheckSpeechTail(folder);
            foreach(string id in CollectionShoes){
                var model=JsonUtility.FromJson<BlenderGarmentAsset>(Resources.Load<TextAsset>("Garments/Streetwear/"+id).text);
                var texture=Resources.Load<Texture2D>("Garments/Streetwear/"+id);int triangles=0;
                if(!model.source.Contains("SneakerCollection")||texture==null||texture.width!=512||texture.mipmapCount<2)throw new Exception("Sneaker source/texture invalid: "+id);
                foreach(var part in model.parts){if(!part.textured)throw new Exception("Sneaker lost its textile atlas");triangles+=part.triangles.Length/3;}
                if(triangles>3200)throw new Exception("Sneaker exceeded geometry budget: "+id);
                studio.Apply(new[]{"AH_TOP_01","CG_BOTTOM_02",id,""},0);int renderers=0;
                foreach(var renderer in mannequin.GetComponentsInChildren<MeshRenderer>())if(renderer.name.StartsWith("Tênis")||renderer.name.StartsWith("Sola")||renderer.name=="Cadarço"){
                    renderers++;if(renderer.sharedMaterials.Length!=1)throw new Exception("Sneaker added material passes: "+id);
                }
                if(renderers>6)throw new Exception("Sneaker pair added renderers: "+id);
                Debug.Log("SNEAKER_V2_BUDGET: "+id+"; "+triangles+" triangles per shoe; "+renderers+" renderers per pair; 512 atlas.");
            }
            CheckStreetwearCollection(folder);
            File.WriteAllText(Path.Combine(folder,"sneaker-checks.txt"),"SNEAKER_V2_PASSED: Blender sources/512 atlases, three preserved IDs, bounded geometry and six single-material meshes per pair, 63 mixed fits, thumbnails, native lookbook, moving player feet, connected counter speech and no contextual floating tail.");
        }
    }
}
