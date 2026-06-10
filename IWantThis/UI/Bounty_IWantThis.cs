using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace IWantThis.UI
{
    public class Bounty_IWantThis : Window
    {
        public Map Map;
        private readonly string gold = "IWantThis.Misc".Translate(ThingDefOf.Gold.LabelCap);
        private readonly string silver = "IWantThis.Misc".Translate(ThingDefOf.Silver.LabelCap);
        private readonly string option1 = "ItemsTab".Translate();
        private readonly string option2 = "TabPenAnimals".Translate();
        private readonly string option3 = "Xenotype".Translate();
        private string selectedOption = "ItemsTab".Translate();
        private bool Open = false;
        private ThingDef bountyTarget = null;

        public Bounty_IWantThis(Map map)
        {
            forcePause = true;
            doCloseButton = false;
            doCloseX = true;
            closeOnAccept = false;
            closeOnCancel = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = false;
            soundAppear = SoundDefOf.CommsWindow_Open;
            soundClose = SoundDefOf.CommsWindow_Close;
            soundAmbient = SoundDefOf.RadioComms_Ambience;
            Map = map;
        }

        public override Vector2 InitialSize => new Vector2(700, 800);

        public override void PostOpen()
        {
            base.PostOpen();
            //var seenThings = new HashSet<Thing>();
            //foreach (var beacon in Building_OrbitalTradeBeacon.AllPowered(Map))
            //    foreach (var cell in beacon.TradeableCells)
            //        foreach (var thing in cell.GetThingList(Map))
            //        {
            //            if (!seenThings.Add(thing)) continue;
            //            if (thing.def == VFED_DefOf.VFED_Intel) TotalIntel += thing.stackCount;
            //            if (thing.def == VFED_DefOf.VFED_CriticalIntel) TotalCriticalIntel += thing.stackCount;
            //        }

            //foreach (var tab in DefDatabase<IWantThisTabDef>.AllDefs) tab.Worker.Notify_Open(this);
            //curTab = DefDatabase<IWantThisTabDef>.AllDefs.First();
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Medium;
            Rect labelRect = listing.GetRect(40f);
            Widgets.Label(labelRect, "IWantThis.PlaceBounty".Translate());
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            Rect labelRect2 = listing.GetRect(35f);
            if (Widgets.ButtonText(labelRect2, selectedOption))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>
                {
                    new FloatMenuOption(option1, () => selectedOption = option1),
                    new FloatMenuOption(option2, () => selectedOption = option2),
                    new FloatMenuOption(option3, () => selectedOption = option3)
                };

                Find.WindowStack.Add(new FloatMenu(options));
            }
            listing.Gap();

            if (selectedOption == option2)
            {
                Rect labelRect3 = listing.GetRect(65f);
                if (Widgets.ButtonText(labelRect3, "IWantThis.Select".Translate(option2)))
                {
                    Open = true;
                }
                listing.Gap(24f);

                if (Open)
                {
                    Open = false;
                    Find.WindowStack.Add(new AnimalSelector(delegate (ThingDef chosenAnimal)
                    {
                        this.bountyTarget = chosenAnimal;
                    }));
                }
                
            }

            listing.End();

            //this.Close()
        }
    }
}