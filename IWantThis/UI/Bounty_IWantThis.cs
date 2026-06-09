using RimWorld;
using System.Text;
using UnityEngine;
using Verse;

namespace IWantThis.UI
{
    public class Bounty_IWantThis : Window
    {
        public Map Map;

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

        public override Vector2 InitialSize => new Vector2(1000, 750);

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
            //var deserters = WorldComponent_IWantThis.Instance;
            var left = inRect.LeftPart(inRect.width / 5 * 2);
            var factionName = Faction.OfPlayer.Name;
            var text = "VFED.Visibility";
            var width = Text.CalcSize(text).x + 35;
            Text.Font = GameFont.Medium;
            var title = left.TopPart(Text.CalcHeight(factionName, left.width - width) + 8);
            var visibilityRect = title.RightPart(width).TopPartPixels(30);
            Widgets.Label(title, factionName);
            Text.Font = GameFont.Small;
            visibilityRect = visibilityRect.ContractedBy(2f, 4.5f);
            Widgets.DrawHighlight(visibilityRect);
            //if (Mouse.IsOver(visibilityRect))
            //{
                Widgets.DrawHighlight(visibilityRect);
                var builder = new StringBuilder();
                builder.Append("VFED.CurrentVisibility".Translate().Colorize(ColoredText.TipSectionTitleColor));
                builder.AppendLine(text.ToString()
                   .Colorize(Color.Lerp(ColorLibrary.BrightGreen, ColorLibrary.RedReadable, Mathf.InverseLerp(0, 100, 11))));
                builder.AppendLine();
                builder.AppendLine("visibilityLevel.description");
                builder.AppendLine();
                builder.AppendLine("VFED.Effects".Translate().Colorize(ColoredText.TipSectionTitleColor));
                builder.Append("VFED.IntelCostModifier".Translate());
                builder.Append("VFED.ContrabandRecieveTimeModifier".Translate());
                builder.Append("VFED.ContrabandSiteTimeModifier".Translate());
                builder.Append("VFED.ImperialResponseTime".Translate());
                builder.Append("VFED.ImperialResponseType".Translate());
                builder.Append("VFED.SpecialEffects".Translate().Colorize(ColoredText.TipSectionTitleColor));
                TooltipHandler.TipRegion(visibilityRect, builder.ToString());
            //}

            //GUI.DrawTexture(visibilityRect.LeftPart(20), visibilityLevel.Icon);
            visibilityRect.LeftPart(3);
            using (new TextBlock(TextAnchor.MiddleCenter))
                Widgets.Label(visibilityRect, text);

            Widgets.DrawLineHorizontal(left.x, left.yMin, left.width);

            Text.Font = GameFont.Small;

            var intelRect = left.TopPart(30);
            Widgets.DrawLightHighlight(intelRect);
            if (Mouse.IsOver(intelRect)) Widgets.DrawHighlight(intelRect);
            TooltipHandler.TipRegion(intelRect, "VFED.IntelAmount".Translate("VFED_DefOf.VFED_Intel.label"));
            //Widgets.DefIcon(intelRect.LeftPart(30).ContractedBy(1.5f), VFED_DefOf.VFED_Intel);
            //Widgets.InfoCardButton(intelRect.LeftPart(30).ContractedBy(3), VFED_DefOf.VFED_Intel);
            using (new TextBlock(TextAnchor.MiddleLeft))
            {
                Widgets.Label(intelRect.RightPart(80), "TotalIntel.ToString()");
                intelRect.LeftPart(20);
                //Widgets.Label(intelRect, VFED_DefOf.VFED_Intel.LabelCap);
            }

            intelRect = left.TopPart(30);
            if (Mouse.IsOver(intelRect)) Widgets.DrawHighlight(intelRect);
            //TooltipHandler.TipRegion(intelRect, "VFED.IntelAmount".Translate(VFED_DefOf.VFED_CriticalIntel.label));
            //Widgets.DefIcon(intelRect.LeftPart(30).ContractedBy(1.5f), VFED_DefOf.VFED_CriticalIntel);
            //Widgets.InfoCardButton(intelRect.LeftPart(30).ContractedBy(3), VFED_DefOf.VFED_CriticalIntel);
            //using (new TextBlock(TextAnchor.MiddleLeft))
            //{
            //    Widgets.Label(intelRect.RightPart(80), TotalCriticalIntel.ToString());
            //    intelRect.LeftPart(20);
            //    Widgets.Label(intelRect, VFED_DefOf.VFED_CriticalIntel.LabelCap);
            //}

            left.TopPart(40);
            Widgets.DrawMenuSection(left);
            //TabDrawer.DrawTabs(left, tabs);
            //curTab.Worker.DoLeftPart(left.ContractedBy(2));
            inRect.LeftPart(3);
            //curTab.Worker.DoMainPart(inRect.ContractedBy(2));
        }
    }
}