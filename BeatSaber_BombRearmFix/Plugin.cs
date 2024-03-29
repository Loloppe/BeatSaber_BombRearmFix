using BeatSaberMarkupLanguage.GameplaySetup;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using IPA.Loader;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace BeatSaber_BombRearmFix
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        internal static Harmony Harmony;
        internal static bool InReplay = false;

        [Init]
        public Plugin(IPALogger logger, IPA.Config.Config conf)
        {
            Instance = this;
            Log = logger;
            Config.Instance = conf.Generated<Config>();
            Harmony = new Harmony("Loloppe.BeatSaber.BombRearmFix");
        }

        static class BsmlWrapper
        {
            static readonly bool hasBsml = IPA.Loader.PluginManager.GetPluginFromId("BeatSaberMarkupLanguage") != null;

            public static void EnableUI()
            {
                try
                {
                    void wrap() => GameplaySetup.instance.AddTab("BombRearmFix", "BeatSaber_BombRearmFix.UI.settings.bsml", Config.Instance, MenuType.All);

                    if (hasBsml)
                    {
                        wrap();
                    }
                }
                catch (Exception e)
                {
                    Log.Warn(e.Message);
                }

            }
            public static void DisableUI()
            {
                void wrap() => GameplaySetup.instance.RemoveTab("BombRearmFix");

                if (hasBsml)
                {
                    wrap();
                }
            }
        }

        [OnEnable]
        public void OnEnable()
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            BsmlWrapper.EnableUI();
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name == "GameCore")
            {
                var scoresaber = PluginManager.GetPluginFromId("ScoreSaber");
                if (scoresaber != null)
                {
                    MethodBase ScoreSaber_playbackEnabled = AccessTools.Method("ScoreSaber.Core.ReplaySystem.HarmonyPatches.PatchHandleHMDUnmounted:Prefix");
                    if (ScoreSaber_playbackEnabled != null && (bool)ScoreSaber_playbackEnabled.Invoke(null, null) == false)
                    {
                        InReplay = true;
                        return;
                    }
                }

                var beatleader = PluginManager.GetPluginFromId("BeatLeader");
                if (beatleader != null)
                {
                    var _replayStarted = beatleader?.Assembly.GetType("BeatLeader.Replayer.ReplayerLauncher")?
                    .GetProperty("IsStartedAsReplay", BindingFlags.Static | BindingFlags.Public);
                    if (_replayStarted != null && (bool)_replayStarted.GetValue(null, null))
                    {
                        InReplay = true;
                        return;
                    }
                }
            }

            InReplay = false;
        }

        [OnDisable]
        public void OnDisable()
        {
            SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
            Harmony.UnpatchSelf();
            BsmlWrapper.DisableUI();
        }

    }
}
