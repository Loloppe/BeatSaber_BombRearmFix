using HarmonyLib;
using UnityEngine;

namespace BeatSaber_BombRearmFix.HarmonyPatches
{
    [HarmonyPatch(typeof(BasicBeatmapObjectManager), nameof(BasicBeatmapObjectManager.DespawnInternal), typeof(NoteController))]
    internal class DespawnInternalPatch
    {
        static bool Prefix(NoteController noteController)
        {
            if (Config.Instance.Enabled && noteController.noteData.gameplayType == NoteData.GameplayType.Bomb && !Plugin.InReplay)
            {
                // Hide the bomb model and attempt to destroy it from memory.
                var bomb = (BombNoteController)noteController;
                bomb._wrapperGO.SetActive(false);
                GameObject.Destroy(bomb._wrapperGO);
                GameObject.Destroy(bomb);
                return false;
            }

            // Default behavior if the plugin is disabled or if it's not a bomb.
            return true;
        }
    }
}
