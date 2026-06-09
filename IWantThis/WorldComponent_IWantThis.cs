using HarmonyLib;
using IWantThis.UI;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace IWantThis
{
    [HarmonyPatch(typeof(Building_CommsConsole), nameof(Building_CommsConsole.GetFloatMenuOptions))]
    public static class Patch_Building_CommsConsole_GetFloatMenuOptions
    {
        [HarmonyPostfix]
        public static void Postfix(Building_CommsConsole __instance, Pawn myPawn, ref IEnumerable<FloatMenuOption> __result)
        {
            if (__instance.GetComp<CompPowerTrader>().PowerOn)
            {
                List<FloatMenuOption> optionsList = new List<FloatMenuOption>(__result);

                void action()
                {
                    Find.WindowStack.Add(new Dialog_IWantThis(__instance.MapHeld));
                    //Messages.Message("status "+__instance.GetComp<CompPowerTrader>().PowerOn, MessageTypeDefOf.PositiveEvent);
                }

                FloatMenuOption customOption = new FloatMenuOption("IWantThis.PlaceBounty".Translate(), action, MenuOptionPriority.InitiateSocial);

                optionsList.Add(customOption);

                __result = optionsList;
            }
        }
    }




    //[HarmonyPatch]
    //public class WorldComponent_IWantThis : WorldComponent, ICommunicable, ILoadReferenceable
    //{
    //    public static WorldComponent_IWantThis Instance;

    //    public WorldComponent_IWantThis(World world) : base(world) => Instance = this;
    //    public string GetCallLabel() => null;

    //    public string GetInfoText() => null;
    //    public Faction GetFaction() => null;

    //    public void TryOpenComms(Pawn negotiator)
    //    {
    //        Find.WindowStack.Add(new Dialog_IWantThis(negotiator.MapHeld));
    //    }

    //    public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator) =>
    //        FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("IWantThis.PlaceBounty".Translate(), delegate { console.GiveUseCommsJob(negotiator, this); },
    //             MenuOptionPriority.InitiateSocial), negotiator, console);

    //    public string GetUniqueLoadID() => world.GetUniqueLoadID() + "_IWantThis";

    //    public override void WorldComponentTick()
    //    {
    //        base.WorldComponentTick();

    //    }

    //    [HarmonyPatch(typeof(Building_CommsConsole), nameof(Building_CommsConsole.GetCommTargets))]
    //    [HarmonyPostfix]
    //    public static IEnumerable<ICommunicable> GetCommTargets_Postfix(IEnumerable<ICommunicable> targets) => targets.Append(Instance);

    //}
}