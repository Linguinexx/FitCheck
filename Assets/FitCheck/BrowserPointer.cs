using System.Runtime.InteropServices;
using UnityEngine;

namespace FitCheck
{
    internal static class BrowserPointer
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] static extern void FitCheck_SetPointerCapture(int wanted);
        [DllImport("__Internal")] static extern int FitCheck_HasPointerCapture();
        [DllImport("__Internal")] static extern int FitCheck_TakeRelease();
        [DllImport("__Internal")] static extern void FitCheck_ExitFullscreen();
        [DllImport("__Internal")] static extern void FitCheck_EnterFullscreen();
        [DllImport("__Internal")] static extern int FitCheck_HasFullscreen();
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
        public static bool TakeRelease()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return FitCheck_TakeRelease()!=0;
#else
            return false;
#endif
        }
        public static bool Fullscreen {
            get {
#if UNITY_WEBGL && !UNITY_EDITOR
                return FitCheck_HasFullscreen()!=0;
#else
                return Screen.fullScreen;
#endif
            }
        }
        public static void EnterFullscreen()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            FitCheck_EnterFullscreen();
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
