using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace FitCheck.Editor
{
    public static class PrototypeChecks
    {
        [MenuItem("Tools/Fit Check/Run runtime smoke check")]
        public static void RuntimeCheck()
        {
            var game=UnityEngine.Object.FindAnyObjectByType<FirstCustomer>();
            if(!Application.isPlaying || game==null)throw new Exception("Inicie Play Mode antes do teste");
            game.SmokeCheck();
        }
        [MenuItem("Tools/Fit Check/Validate first customer")]
        public static void Validate()
        {
            if(Catalog.Items.Length!=11)throw new Exception("Catálogo inicial inválido");
            var perfect=new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""};
            if(Rules.Score(perfect)!=10 || Rules.TutorialReputation(10)!=100)throw new Exception("Nota perfeita inválida");
            var straight=new[]{"SB_TOP_01","SB_BOTTOM_01","SB_SHOES_01",""};
            if(Rules.Score(straight)!=6)throw new Exception("Corte reto deve perder 4 pontos");
            if(Rules.Score(Catalog.Default)!=3 || Rules.TutorialReputation(3)!=0)throw new Exception("Tutorial penalizou nota baixa");
            var roundtrip=JsonUtility.FromJson<Attempt>(JsonUtility.ToJson(new Attempt{outfit=perfect,completed=true,score=10}));
            if(!roundtrip.completed || Rules.Score(roundtrip.outfit)!=10)throw new Exception("Persistência inválida");
            bool rejected=false; try {Rules.Score(new[]{"AH_TOP_01","","AH_SHOES_01",""});}catch(ArgumentException){rejected=true;}
            if(!rejected)throw new Exception("Conjunto incompleto aceito");
            ValidateDay();
            Debug.Log("FITCHECK_CHECKS_PASSED: catálogo, pesos, tutorial sem perda, serialização e categorias obrigatórias.");
        }
        public static void ValidateDay()
        {
            var bia=new[]{"CG_TOP_01","SB_BOTTOM_01","SB_SHOES_01","CG_HEAD_01"};
            var pico=new[]{"AH_TOP_01","CG_BOTTOM_02","AH_SHOES_01",""};
            if(DayOne.Score(1,bia)!=10 || DayOne.Score(2,pico)!=10)throw new Exception("Soluções iniciais do GDD falharam");
            var noCap=(string[])bia.Clone(); noCap[3]="";
            if(DayOne.Score(1,noCap)!=7)throw new Exception("Bia precisa perder só os pontos do boné");
            var noShorts=(string[])pico.Clone(); noShorts[1]="CG_BOTTOM_01";
            if(DayOne.Score(2,noShorts)!=3)throw new Exception("Pico: comprimento/cor incorretos aceitos");
            var day=new DayProgress();
            day.Add(new Attempt{outfit=new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""}});
            day.Add(new Attempt{outfit=bia});day.Add(new Attempt{outfit=pico});
            if(day.reputation!=300 || day.rewards.Count!=0)throw new Exception("Recompensa antecipada ou soma inválida");
            day.Close();day.Close();
            if(day.rewards.Count!=6)throw new Exception("Fechamento duplicou recompensas");
            JsonUtility.FromJson<DayProgress>(JsonUtility.ToJson(day)).Validate();
            bool rejected=false;try{day.Add(new Attempt{outfit=pico});}catch(InvalidOperationException){rejected=true;}
            if(!rejected)throw new Exception("Permitiu quarto atendimento");
            var falling=new DayProgress();falling.Add(new Attempt{outfit=new[]{"AH_TOP_01","AH_BOTTOM_01","AH_SHOES_01",""}});
            falling.Add(new Attempt{outfit=new[]{"CG_TOP_01","CG_BOTTOM_01","AH_SHOES_01",""}});
            if(falling.reputation!=70 || !falling.reached.Contains(100))throw new Exception("Marco alcançado foi perdido");
            var floor=new DayProgress();floor.Add(new Attempt{outfit=Catalog.Default});
            floor.Add(new Attempt{outfit=new[]{"CG_TOP_01","CG_BOTTOM_01","AH_SHOES_01",""}});
            if(floor.reputation!=0 || floor.posts[1].reputation!=0)throw new Exception("Piso zero incorreto");
            Debug.Log("FITCHECK_DAY_PASSED: pedidos Bia/Pico, penalidade/piso, marcos preservados, fechamento idempotente e save.");
        }
        [MenuItem("Tools/Fit Check/Prepare prototype scene")]
        public static void Prepare()
        {
            Directory.CreateDirectory("Assets/Resources");
            if(!AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/PrototypeBase.mat"))
                AssetDatabase.CreateAsset(new Material(Shader.Find("Standard")),"Assets/Resources/PrototypeBase.mat");
            AssetDatabase.SaveAssets();
            Directory.CreateDirectory("Assets/Scenes");
            var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            EditorSceneManager.SaveScene(scene,"Assets/Scenes/FirstCustomer.unity");
            EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(scene.path,true)};
            PlayerSettings.companyName="DripLab"; PlayerSettings.productName="Fit Check — protótipo";
            Validate();
        }
    }
}
