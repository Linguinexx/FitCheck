using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace FitCheck.Editor
{
    [InitializeOnLoad] public static class InkOutlineReview
    {
        static double nextCheck;
        static InkOutlineReview(){InkOutlinePreview.Restore(true);EditorApplication.update+=Refresh;EditorApplication.playModeStateChanged+=state=>{if(state==PlayModeStateChange.ExitingPlayMode||state==PlayModeStateChange.EnteredEditMode)InkOutlinePreview.Restore();};}
        static void Refresh(){if(!EditorApplication.isPlaying||!InkOutlinePreview.Enabled||!InkOutlinePreview.UserEnabled||EditorApplication.timeSinceStartup<nextCheck)return;nextCheck=EditorApplication.timeSinceStartup+.5;var game=UnityEngine.Object.FindAnyObjectByType<FirstCustomer>();if(game!=null)game.SetInkOutlinePreview(true);}
        [MenuItem("Tools/Fit Check/Prévia de contorno/Ativar no Play")]
        public static void Enable(){var game=UnityEngine.Object.FindAnyObjectByType<FirstCustomer>();if(!EditorApplication.isPlaying||game==null){Debug.Log("Entre no Play para comparar o contorno.");return;}game.SetInkOutlinePreview(true);}
        [MenuItem("Tools/Fit Check/Prévia de contorno/Restaurar original")]
        public static void Disable(){InkOutlinePreview.Restore();}
        public static void Build(){
            CampaignChecks.Validate();DropCatalogChecks.Validate();
            var root=Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName;
            var output=Path.Combine(root,"tools/.ink-outline-review/FitCheck.exe");Directory.CreateDirectory(Path.GetDirectoryName(output));
            var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{"Assets/Scenes/FirstCustomer.unity"},locationPathName=output,target=BuildTarget.StandaloneWindows64,options=BuildOptions.CompressWithLz4HC});
            Debug.Log("INK_OUTLINE_BUILD: "+report.summary.result+" errors="+report.summary.totalErrors+" warnings="+report.summary.totalWarnings);
            if(report.summary.result!=BuildResult.Succeeded)throw new Exception("Prévia de contorno não compilou");
        }
    }
}
