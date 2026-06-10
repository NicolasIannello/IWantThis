using HarmonyLib;
using IWantThis.UI;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace IWantThis
{
    [HarmonyPatch]
    public class WorldComponent_IWantThis : WorldComponent, ICommunicable, ILoadReferenceable
    {
        public static WorldComponent_IWantThis Instance;
        public WorldComponent_IWantThis(World world) : base(world) => Instance = this;

        public string GetCallLabel() => null;
        public string GetInfoText() => null;
        public Faction GetFaction() => null;
        public string GetUniqueLoadID() => world.GetUniqueLoadID() + "_IWantThis";

        public void TryOpenComms(Pawn negotiator)
        {
            Find.WindowStack.Add(new Bounty(negotiator.MapHeld));
        }

        public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator) =>
                FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(
                    "IWantThis.PlaceBounty".Translate(), 
                    delegate { console.GiveUseCommsJob(negotiator, this); },
                    MenuOptionPriority.InitiateSocial), negotiator, console);

        [HarmonyPatch(typeof(Building_CommsConsole), nameof(Building_CommsConsole.GetCommTargets))]
        [HarmonyPostfix]
        public static IEnumerable<ICommunicable> Postfix(IEnumerable<ICommunicable> targets) => targets.Append(Instance);

    }
}