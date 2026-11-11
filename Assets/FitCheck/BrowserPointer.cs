using System.Runtime.InteropServices;
using UnityEngine;

namespace FitCheck
{
    internal static class BrowserPointer
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] static extern void FitCheck_SetPointerCapture(int wanted);
        [DllImport("__Internal")] static extern int FitCheck_HasPointerCapture();
        [DllImport("__Internal")] static extern int FitCheck_TakeEscape();
        [DllImport("__Internal")] static extern void FitCheck_ExitFullscreen();
#endif
        public static bool Locked {
            get {
#if UNITY_WEBGL && !UNITY_EDITOR
                return FitCheck_HasPointerCapture()!=0;
#else
                return Cursor.lockState==CursorLockMode.Locked;
#endif
            }
        }
        public static void Capture(bool wanted)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            FitCheck_SetPointerCapture(wanted?1:0);
#endif
        }
        public static bool TakeEscape()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return FitCheck_TakeEscape()!=0;
#else
            return false;
#endif
        }
        public static void Minimize()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            FitCheck_ExitFullscreen();
#endif
        }
    }
}
