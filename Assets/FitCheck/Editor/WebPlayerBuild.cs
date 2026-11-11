using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering;

namespace FitCheck.Editor
{
    public static class WebPlayerBuild
    {
        [MenuItem("Tools/Fit Check/Build browser WebGL")]
        public static void Build()
        {
            if(!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.WebGL,BuildTarget.WebGL))
                throw new InvalidOperationException("Instale Web Build Support para a Unity 6000.6.7f1 no Unity Hub.");
            DropCatalogChecks.Validate();AmbientSurfaceImporter.PrepareAll();
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.WebGL,ScriptingImplementation.IL2CPP);
            PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.WebGL,ManagedStrippingLevel.Low);
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL,false);
            PlayerSettings.SetGraphicsAPIs(BuildTarget.WebGL,new[]{GraphicsDeviceType.OpenGLES3});
            PlayerSettings.WebGL.template="PROJECT:FitCheck";
            PlayerSettings.WebGL.compressionFormat=WebGLCompressionFormat.Gzip;
            PlayerSettings.WebGL.decompressionFallback=true;
            PlayerSettings.WebGL.dataCaching=true;
            PlayerSettings.WebGL.nameFilesAsHashes=true;
            PlayerSettings.WebGL.threadsSupport=false;
            PlayerSettings.WebGL.initialMemorySize=256;
            PlayerSettings.WebGL.maximumMemorySize=2048;
            PlayerSettings.WebGL.memoryGrowthMode=WebGLMemoryGrowthMode.Geometric;
            PlayerSettings.WebGL.exceptionSupport=WebGLExceptionSupport.FullWithoutStacktrace;
            PlayerSettings.defaultWebScreenWidth=1440;PlayerSettings.defaultWebScreenHeight=900;
            const string folder="Builds/Web-Test01";
            var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{
                scenes=new[]{"Assets/Scenes/FirstCustomer.unity"},locationPathName=folder,
                target=BuildTarget.WebGL,targetGroup=BuildTargetGroup.WebGL,options=BuildOptions.None
            });
            var summary=report.summary;
            string result="FITCHECK_WEB_BUILD: "+summary.result+"; bytes="+summary.totalSize+"; errors="+summary.totalErrors+"; warnings="+summary.totalWarnings;
            Directory.CreateDirectory("Builds");File.WriteAllText("Builds/Web-Test01-build-report.txt",result);Debug.Log(result);
            if(summary.result!=BuildResult.Succeeded)throw new Exception(result);
        }
    }
}
