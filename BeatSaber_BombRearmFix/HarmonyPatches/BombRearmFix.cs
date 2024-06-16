using HarmonyLib;

namespace BeatSaber_BombRearmFix.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteJump), nameof(NoteJump.Init))]
    internal class HalfJumpMarkReportedPatch
    {
        static void Postfix(ref bool ____halfJumpMarkReported)
        {
            ____halfJumpMarkReported = false;
        }
    }
}
