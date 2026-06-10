using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace IWantThis.UI
{
    internal class Selector : Window
    {
        private Action<Def> onSelect;
        private List<Def> all;
        private Vector2 scrollPosition;
        private QuickSearchWidget searchWidget = new QuickSearchWidget();

        public override Vector2 InitialSize => new Vector2(450f, 600f);

        public Selector(Action<Def> onSelect, string option)
        {
            this.doCloseX = true;
            this.closeOnClickedOutside = true;
            this.absorbInputAroundWindow = true;
            this.onSelect = onSelect;

            if(option== "ItemsTab".Translate()) this.all = DefDatabase<ThingDef>.AllDefs
                .Where(d => d.PlayerAcquirable && (d.IsPleasureDrug || d.IsNonMedicalDrug || d.IsMedicine || d.IsNaturalOrgan /*|| d.isTechHediff*/ || d.IsWeapon /*|| d.IsApparel*/)).OrderBy(d => d.label).Cast<Def>().ToList();
            if (option == "TabPenAnimals".Translate()) this.all = DefDatabase<ThingDef>.AllDefs
                .Where(d => d.race != null && d.race.Animal && !d.IsCorpse && d.race.animalType != AnimalType.Dryad).OrderBy(d => d.label).Cast<Def>().ToList();
            if (option == "Xenotype".Translate() && ModsConfig.BiotechActive) this.all = DefDatabase<XenotypeDef>.AllDefs.OrderBy(d => d.label).Cast<Def>().ToList();

        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            Rect searchRect = new Rect(0f, 10f, inRect.width, 30f);
            this.searchWidget.OnGUI(searchRect, null, null);

            var filtered = this.all.Where(a => this.searchWidget.filter.Matches(a.label)).ToList();

            Rect outRect = new Rect(0f, searchRect.yMax + 10f, inRect.width, inRect.height - searchRect.height - 10f);
            float rowHeight = 35f;
            Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, filtered.Count * rowHeight);

            Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect);

            float yPosition = 0f;
            foreach (Def thing in filtered)
            {
                Rect rowRect = new Rect(0f, yPosition, viewRect.width, rowHeight);

                Widgets.DrawHighlightIfMouseover(rowRect);

                Rect iconRect = new Rect(rowRect.x + 5f, rowRect.y + 2f, 30f, 30f);

                Texture2D icon = null;
                if (thing is ThingDef thingDef) icon = thingDef.uiIcon;
                if (thing is XenotypeDef xenoDef) icon = xenoDef.Icon;

                if (icon != null) GUI.DrawTexture(iconRect, icon);

                Rect labelRect = new Rect(iconRect.xMax + 10f, rowRect.y, rowRect.width - 50f, rowHeight);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(labelRect, thing.LabelCap);
                Text.Anchor = TextAnchor.UpperLeft;

                if (Widgets.ButtonInvisible(rowRect))
                {
                    this.onSelect?.Invoke(thing);
                    this.Close();
                }

                yPosition += rowHeight;
            }

            Widgets.EndScrollView();
        }
    }
}
