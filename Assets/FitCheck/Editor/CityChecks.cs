using System;
using UnityEditor;
using UnityEngine;

namespace FitCheck.Editor
{
    public static class CityChecks
    {
        [MenuItem("Tools/Fit Check/Run city preview check")]
        public static void Runtime()
        {
            var game=UnityEngine.Object.FindAnyObjectByType<FirstCustomer>();if(!Application.isPlaying||game==null)throw new Exception("Inicie Play Mode");
            var shader=Resources.Load<Shader>("CityPaint");if(shader==null||!shader.isSupported)throw new Exception("Shader urbano indisponível");
            foreach(var message in ShaderUtil.GetShaderMessages(shader))if(message.severity.ToString()=="Error")throw new Exception(message.message);
            game.CitySmokeCheck();
        }
    }
}
