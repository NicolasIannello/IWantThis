using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace IWantThis.Patches
{
    [HarmonyPatch]
    public static class Patch_MedievalOverhaul_ScribeTable
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method("MedievalOverhaul.Building_ScribeTable:GetFloatMenuOptions");
        }

        public static bool Prepare() => TargetMethod() != null;

        public static void Postfix(ref IEnumerable<FloatMenuOption> __result, Building_CommsConsole __instance, Pawn myPawn)
        {
            var list = __result?.ToList() ?? new List<FloatMenuOption>();
            var worldComp = WorldComponent_IWantThis.Instance;

            list.Add(FloatMenuUtility.DecoratePrioritizedTask(
                worldComp.ActiveBounty ?
                    new FloatMenuOption("IWantThis.PlaceBounty".Translate("(" + "IWantThis.BountyActive".Translate() + ")"),
                        null, ModsConfig.IdeologyActive ? ThingDef.Named("RelicInertTablet") : __instance.def, null, false)
                    :
                    new FloatMenuOption("IWantThis.PlaceBounty".Translate(""), delegate { __instance.GiveUseCommsJob(myPawn, worldComp); },
                        ModsConfig.IdeologyActive ? ThingDef.Named("RelicInertTablet") : __instance.def, null, false, MenuOptionPriority.InitiateSocial)
                , myPawn, __instance)
            );

            __result = list;
        }
    }
}
