using System.Collections.Generic;
using UnityEngine;
namespace FitCheck
{
    // Selective character ink; player preference also controls the furniture pass.
    public static class InkOutlinePreview
    {
        static readonly Dictionary<Material,Shader> originals=new Dictionary<Material,Shader>();
        static readonly string[] source={"FitCheck/Retro Painted","FitCheck/Blender Garment Fabric","FitCheck/Selective Garment Finishes","FitCheck/Cat Fur"};
        static readonly string[] previews={"InkPreviewRetro","InkPreviewFabric","InkPreviewFinishes","InkPreviewPet"};
        static readonly string[] previewNames={"FitCheck/Preview/Ink Retro","FitCheck/Preview/Ink Fabric","FitCheck/Preview/Ink Finishes","FitCheck/Preview/Ink Pet"};
        static readonly string[] originalResources={"RetroPaint","GarmentFabric","GarmentFinishEffects","CatFur"};
        public static bool Enabled {get;private set;}
        public const string PreferenceKey="FitCheck.InkOutline";
        public static bool UserEnabled=>PlayerPrefs.GetInt(PreferenceKey,1)==1;
#if UNITY_WEBGL && !UNITY_EDITOR
        public static bool GameplayEnabled=>UserEnabled;
#else
        public static bool GameplayEnabled=>UserEnabled&&System.Array.IndexOf(System.Environment.GetCommandLineArgs(),"-city-proof")<0;
#endif
        public static int MaterialCount=>originals.Count;
        public static void Apply(Transform root){
            if(root==null)return;
            Shader.SetGlobalFloat("_InkPreviewPixels",1.15f);Shader.SetGlobalFloat("_InkPreviewWorldLimit",.012f);Shader.SetGlobalColor("_InkPreviewColor",new Color(.025f,.022f,.028f,1));
            foreach(var renderer in root.GetComponentsInChildren<MeshRenderer>(true))foreach(var material in renderer.sharedMaterials){
                if(material==null||originals.ContainsKey(material))continue;
                int index=System.Array.IndexOf(source,material.shader.name);bool clone=index<0;
                if(clone)index=System.Array.IndexOf(previewNames,material.shader.name);if(index<0)continue;
                if(index==1||index==2||material.name.StartsWith("Roupa —")){
                    if(clone)material.shader=Resources.Load<Shader>(originalResources[index]);
                    continue;
                }
                // Flat facial overlays include lids, pupils and mouths; outlining
                // those open meshes would obscure the established expressions.
                if(index==0&&material.HasProperty("_CharacterLit")&&Mathf.Abs(material.GetFloat("_CharacterLit")-.25f)<.001f)continue;
                var shader=Resources.Load<Shader>(previews[index]);if(shader==null||!shader.isSupported)throw new System.InvalidOperationException("Shader da prévia de contorno indisponível: "+previews[index]);
                originals.Add(material,clone?Resources.Load<Shader>(originalResources[index]):material.shader);material.shader=shader;
            }
            Enabled=true;
        }
        public static void Restore(bool force=false){
            if(!force&&!Enabled&&originals.Count==0)return;
            foreach(var pair in originals)if(pair.Key!=null)pair.Key.shader=pair.Value;
            // A new hair/hand/garment may clone an outlined runtime material.
            // Restore those copies too, including after an editor reload.
            foreach(var material in Resources.FindObjectsOfTypeAll<Material>()){
                if(material==null||material.shader==null)continue;int i=System.Array.IndexOf(previewNames,material.shader.name);
                if(i>=0)material.shader=Resources.Load<Shader>(originalResources[i]);
            }
            originals.Clear();Enabled=false;
        }
    }
    public partial class CityView
    {
        public void PreviewCharacterInk(){
            foreach(var actor in residents)if(actor.root!=null)InkOutlinePreview.Apply(actor.root);
            if(PlayerBodyReady)InkOutlinePreview.Apply(playerBody.root);
            if(cat!=null)InkOutlinePreview.Apply(cat.transform);
        }
        public bool RefreshInkPreviewMirror(Camera camera){
            if(!IsInterior)return false;nextHomeMirrorFrame=nextShopMirrorFrame=0;
            return IsHome?RenderHomeMirror(camera):RenderShopMirror(camera);
        }
    }
    public partial class FirstCustomer
    {
        bool characterOutline=true;
        void SetOutlinePreference(bool enabled,bool save=false){
            characterOutline=enabled;
            Shader.SetGlobalFloat("_InkEffectsDisabled",enabled?0:1);
            SetInkOutlinePreview(enabled);
            if(save&&!ProofMode){PlayerPrefs.SetInt(InkOutlinePreview.PreferenceKey,enabled?1:0);PlayerPrefs.Save();}
        }
        public void SetInkOutlinePreview(bool enabled){
            Shader.SetGlobalFloat("_InkEffectsDisabled",enabled?0:1);
            if(!enabled){InkOutlinePreview.Restore();return;}
            InkOutlinePreview.Apply(mannequin);
            if(city!=null&&city.gameObject.activeInHierarchy)city.PreviewCharacterInk();
        }
    }
}
