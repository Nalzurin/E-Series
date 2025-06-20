using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Bastion
{
    public static class PatchesUtility
    {
        public static bool SelectedOnlyBastion(Pawn mechanitor)
        {
            return Extensions.GetSelectedDraftedMechs(mechanitor).All((Pawn x) => x.def == Definitions.Mech_Bastion);
        }
        public static bool SelectedOnlyGamma(Pawn mechanitor)
        {
            return Extensions.GetSelectedDraftedMechs(mechanitor).All((Pawn x) => x.def == Definitions.Mech_Gamma);
        }
    }
    [HarmonyPatch]
    public static class CanCommandToPatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Pawn_MechanitorTracker), "CanCommandTo");
        }

        private static bool Postfix(bool __result, LocalTargetInfo target, Pawn ___pawn)
        {
            if (PatchesUtility.SelectedOnlyBastion(___pawn) || PatchesUtility.SelectedOnlyGamma(___pawn))
            {
                return true;
            }
            return __result;
        }
    }

    [HarmonyPatch]
    public static class DrawCommandRadiusPatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Pawn_MechanitorTracker), "DrawCommandRadius");
        }

        private static bool Prefix(Pawn_MechanitorTracker __instance, Pawn ___pawn)
        {
            if (PatchesUtility.SelectedOnlyBastion(___pawn) || PatchesUtility.SelectedOnlyGamma(___pawn))
            {
                return false;
            }
            return true;
        }
    }
}
