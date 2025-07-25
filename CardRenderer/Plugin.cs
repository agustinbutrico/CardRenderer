using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using CardRenderer.Core;

namespace CardRenderer
{
    [BepInPlugin("AgusBut.CardRenderer", "CardRenderer", "1.0.0")]
    [BepInDependency("AgusBut.TexturesLib.Shared")]
    [BepInDependency("AgusBut.TexturesLib.UI")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public static BepInEx.Logging.ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Instance = this;
            Log = base.Logger;

            var harmony = new Harmony("AgusBut.CardRenderer");
            harmony.PatchAll();

            Logger.LogInfo("CardRenderer loaded successfully.");

        }

        private void Start()
        {
            StartCoroutine(CapturePrefabsWhenMainMenuLoads());
        }

        private System.Collections.IEnumerator CapturePrefabsWhenMainMenuLoads()
        {
            while (SceneManager.GetActiveScene().name != "MainMenu")
                yield return null;

            // Capture DefaultHorizontalCard if not captured
            if (GameObject.Find("CardRenderer/CardsAspects/DefaultHorizontalCard") == null)
            {
                AspectsCapturer.CaptureBaseCardPrefab();
            }

            // Capture DefaultPanel if not captured
            if (GameObject.Find("CardRenderer/MenusAspects/DefaultMenu") == null)
            {
                AspectsCapturer.CaptureBaseMenuPrefab();
            }
        }
    }
}
