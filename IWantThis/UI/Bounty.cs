using RimWorld;
using RimWorld.QuestGen;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace IWantThis.UI
{
    public class Bounty : Window
    {
        public Map Map;
        private readonly string gold = "IWantThis.Misc".Translate(ThingDefOf.Gold.LabelCap);
        private readonly string silver = "IWantThis.Misc".Translate(ThingDefOf.Silver.LabelCap);
        private readonly string option1 = "ItemsTab".Translate();
        private readonly string option2 = "TabPenAnimals".Translate();
        private readonly string option3 = "Xenotype".Translate();
        private string selectedOption = "ItemsTab".Translate();
        private bool Open = false;
        private Def bountyTarget = null;
        private float bountyPrice = 0;
        private int goldC = 0;
        private int silverC = 0;
        private float wealth = 0;

        public Bounty(Map map)
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

        public override Vector2 InitialSize => new Vector2(Screen.width * 0.3f, Screen.height - 200);

        public override void PostOpen()
        {
            base.PostOpen();
            Map.listerThings.ThingsOfDef(ThingDefOf.Silver).ForEach(t => silverC += t.stackCount);
            Map.listerThings.ThingsOfDef(ThingDefOf.Gold).ForEach(t => goldC += t.stackCount);

            wealth = ThingDefOf.Silver.BaseMarketValue * silverC + ThingDefOf.Gold.BaseMarketValue * goldC;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Medium;
            Rect labelRect = listing.GetRect(40f);
            Widgets.Label(labelRect, "IWantThis.PlaceBounty".Translate(""));
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

            Rect labelRect3 = listing.GetRect(35f);
            if (Widgets.ButtonText(labelRect3, "IWantThis.Select".Translate(selectedOption)))
            {
                Open = true;
            }
            listing.Gap();

            if (Open)
            {
                Open = false;
                Find.WindowStack.Add(new Selector(delegate (Def chosenThing)
                {
                    bountyTarget = chosenThing;
                    if (bountyTarget is ThingDef thingDef2) bountyPrice = thingDef2.BaseMarketValue * 1.75f;
                    if (bountyTarget is XenotypeDef xenoDef2)
                    {
                        bountyPrice = 700 * xenoDef2.combatPowerFactor + 20 * xenoDef2.AllGenes.Count + 300 * (xenoDef2.Archite ? 1 : 0)
                            - 100 * xenoDef2.generateWithXenogermReplicatingHediffChance - 5 * xenoDef2.factionlessGenerationWeight;

                    }
                }, selectedOption));
            }

            float size = (inRect.width + inRect.height) * 0.25f;
            float posX = inRect.width * 0.5f - size / 2;
            Rect iconRect = new Rect(posX, listing.GetRect(0).y, size, size);
            GUI.DrawTexture(iconRect, ThingDef.Named("RelicInertTablet").uiIcon);

            Texture2D icon = null;
            if (bountyTarget is ThingDef thingDef) icon = thingDef.uiIcon;
            if (bountyTarget is XenotypeDef xenoDef) icon = xenoDef.Icon;
            float size2 = size * 0.35f; float posX2 = inRect.width * 0.5f - size2 / 2;
            Rect iconRect2 = new Rect(posX2, listing.GetRect(0).y + size * 0.45f, size2, size2);
            GUI.DrawTexture(iconRect2, icon);

            if (bountyTarget != null)
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                Text.Font = GameFont.Medium;
                Rect infoRect = new Rect(0, inRect.height - 80 - 35 - 12, inRect.width, 80f);
                Widgets.Label(infoRect, "IWantThis.InfoBounty".Translate(bountyPrice, wealth, silver, gold));
                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.UpperLeft;
            }

            Rect confirmRect = new Rect(0, inRect.height - 35 - 12, inRect.width, 35f);
            if (Widgets.ButtonText(confirmRect, "IWantThis.ConfirmBounty".Translate()) && bountyTarget != null)
            {
                var slate = new Slate();
                slate.Set("delayTicks", IWantThisMod.IntervalArrival.RandomInRange);
                slate.Set("availableTime", Mathf.RoundToInt(IWantThisMod.IntervalArrival.max * GenDate.TicksPerDay));
                //var things = new List<ThingDef>();
                slate.Set("bountyTarget", bountyTarget.LabelCap);
                QuestUtility.GenerateQuestAndMakeAvailable(IWantThis_DefOf.IWantThis_BountyQuest, slate);
                WorldComponent_IWantThis.Instance.ActiveBounty=true;
                this.Close();
            }

            listing.End();
        }
    }
}