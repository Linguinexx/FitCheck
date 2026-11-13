using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace FitCheck.Editor
{
    public static class CitySliceBuild
    {
        // Also callable with -batchmode -executeMethod FitCheck.Editor.CitySliceBuild.Build.
        [MenuItem("Tools/Fit Check/Build Windows first person")]
        public static void Build()
        {BuildTo("Windows-Test01");}
        public static void BuildTeaserCapture()
        {BuildTo("Windows-TeaserCurrent");}
        public static void BuildTest04()
        {BuildTo("Windows-Test04");}
        public static void BuildTest05()
        {BuildTo("Windows-Test05");}
        public static void BuildTest07()
        {BuildTo("Windows-Test07");}
        public static void BuildTest08()
        {DropCatalogChecks.Validate();BuildTo("Windows-Test08");}
        public static void BuildTest09()
        {CampaignChecks.Validate();DropCatalogChecks.Validate();BuildTo("Windows-Test09");}
        static void BuildTo(string folder)
        {
            AmbientSurfaceImporter.PrepareAll();
            var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{
                scenes=new[]{"Assets/Scenes/FirstCustomer.unity"},locationPathName="Builds/"+folder+"/FitCheck.exe",
                target=BuildTarget.StandaloneWindows64,targetGroup=BuildTargetGroup.Standalone,options=BuildOptions.CompressWithLz4HC
            });
            var summary=report.summary;
            var result="FITCHECK_CITY_BUILD: "+summary.result+"; size="+Math.Round(summary.totalSize/(1024.0*1024.0),2)+" MB; errors="+summary.totalErrors+"; warnings="+summary.totalWarnings;
            File.WriteAllText("Builds/"+folder+"-build-report.txt",result);Debug.Log(result);
            if(summary.result!=BuildResult.Succeeded)throw new Exception(result);
        }
    }
}
