using System.Collections;
using System.IO;
using UnityEngine;

namespace FitCheck
{
    public partial class FirstCustomer
    {
        public void CaptureReview(string file)
        {StartCoroutine(CaptureAtFrameEnd(file));}
        IEnumerator CaptureAtFrameEnd(string file)
        {
            yield return new WaitForSecondsRealtime(.6f);yield return new WaitForEndOfFrame();
            var image=ScreenCapture.CaptureScreenshotAsTexture();
            if(image!=null){Directory.CreateDirectory(Path.GetDirectoryName(file));File.WriteAllBytes(file,image.EncodeToPNG());Destroy(image);Debug.Log("FITCHECK_CAPTURE: "+file);}
        }
    }
}
