using HarmonyLib;
using System;

namespace BeatSaber_BombRearmFix.HarmonyPatches
{
    [HarmonyPatch(typeof(BasicBeatmapObjectManager), nameof(BasicBeatmapObjectManager.DespawnInternal), typeof(NoteController))]
    internal class DespawnInternalPatch
    {
        static bool Prefix(NoteController noteController, ref BasicBeatmapObjectManager __instance)
        {
            // Temp fix for the bomb re-arming bug that happen when _memoryPool.Despawn happen. No idea how to actually fix it and doing it this way seems to have no impact whatsoever on the performance so whatever.
            switch (noteController.noteData.gameplayType)
            {
                case NoteData.GameplayType.Normal:
                    __instance._basicGameNotePoolContainer.Despawn((GameNoteController)noteController);
                    return false;
                case NoteData.GameplayType.Bomb:
                    //__instance._bombNotePoolContainer.Despawn((BombNoteController)noteController);
                    return false;
                case NoteData.GameplayType.BurstSliderHead:
                    __instance._burstSliderHeadGameNotePoolContainer.Despawn((GameNoteController)noteController);
                    return false;
                case NoteData.GameplayType.BurstSliderElement:
                    __instance._burstSliderGameNotePoolContainer.Despawn((BurstSliderGameNoteController)noteController);
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
