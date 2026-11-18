using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace FitCheck.Editor
{
    public static class SuppliedPlaylistReview
    {
        public static void Build(){
            CampaignChecks.Validate();DropCatalogChecks.Validate();
            var root=Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName;
            var output=Path.Combine(root,"tools/.supplied-playlist-review/FitCheck.exe");Directory.CreateDirectory(Path.GetDirectoryName(output));
            for(int disc=0;disc<3;disc++)for(int song=1;song<=5;song++){
                string key=new[]{"laje","passo","dez"}[disc]+"_"+song.ToString("00");
                string path="Assets/Resources/Audio/ShopRecords/"+key+".ogg";
                var importer=AssetImporter.GetAtPath(path) as AudioImporter;
                if(importer==null)throw new Exception("Importador ausente: "+path);
                importer.loadInBackground=true;
                var settings=importer.defaultSampleSettings;settings.preloadAudioData=false;settings.loadType=AudioClipLoadType.Streaming;settings.compressionFormat=AudioCompressionFormat.Vorbis;settings.quality=.35f;importer.defaultSampleSettings=settings;importer.SaveAndReimport();
            }
            var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{"Assets/Scenes/FirstCustomer.unity"},locationPathName=output,target=BuildTarget.StandaloneWindows64,options=BuildOptions.CompressWithLz4HC});
            Debug.Log("SUPPLIED_PLAYLIST_BUILD: "+report.summary.result+" errors="+report.summary.totalErrors+" warnings="+report.summary.totalWarnings);
            if(report.summary.result!=BuildResult.Succeeded)throw new Exception("Revisão não compilou");
        }
    }
}
