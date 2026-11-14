using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace FitCheck.Editor
{
    public static class ShopMomentsReview
    {
        public static void Build(){
            CampaignChecks.Validate();DropCatalogChecks.Validate();
            var root=Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName;
            string output=Path.Combine(root,"tools/.shop-moments-review");Directory.CreateDirectory(output);
            var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{"Assets/Scenes/FirstCustomer.unity"},locationPathName=Path.Combine(output,"FitCheck.exe"),target=BuildTarget.StandaloneWindows64,options=BuildOptions.CompressWithLz4HC});
            Debug.Log("SHOP_MOMENTS_BUILD: "+report.summary.result+" errors="+report.summary.totalErrors+" warnings="+report.summary.totalWarnings);
            if(report.summary.result!=BuildResult.Succeeded)throw new Exception("Revisão da loja não compilou");
        }
    }
}
