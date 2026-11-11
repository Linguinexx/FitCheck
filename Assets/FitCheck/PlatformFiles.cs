using System.IO;

namespace FitCheck
{
    internal static class PlatformFiles
    {
        public static void ReplaceValidated(string temporary,string destination,string backup)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // Browser IDBFS has no native ReplaceFile operation. The verified previous
            // version remains recoverable if an overwrite is interrupted.
            if(backup!=null)File.Copy(destination,backup,true);
            File.Copy(temporary,destination,true);
            File.Delete(temporary);
#else
            File.Replace(temporary,destination,backup);
#endif
        }
    }
}
