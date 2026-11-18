using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace FitCheck.Editor
{
    public static class ComfortRefreshReview
    {
        public static void Build(){
            CampaignChecks.Validate();DropCatalogChecks.Validate();
            var root=Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName;
            var output=Path.Combine(root,"tools/.comfort-refresh-review/FitCheck.exe");Directory.CreateDirectory(Path.GetDirectoryName(output));
            var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{"Assets/Scenes/FirstCustomer.unity"},locationPathName=output,target=BuildTarget.StandaloneWindows64,options=BuildOptions.CompressWithLz4HC});
            Debug.Log("COMFORT_REFRESH_BUILD: "+report.summary.result+" errors="+report.summary.totalErrors+" warnings="+report.summary.totalWarnings);
            if(report.summary.result!=BuildResult.Succeeded)throw new Exception("Revisão não compilou");
        }
    }
}
