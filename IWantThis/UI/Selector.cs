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
        private Action<ThingDef> onSelect;
        private List<ThingDef> all;
        private Vector2 scrollPosition;
        private QuickSearchWidget searchWidget = new QuickSearchWidget();

        public override Vector2 InitialSize => new Vector2(450f, 600f);

        public Selector(Action<ThingDef> onSelect, string option)
        {
            this.doCloseX = true;
            this.closeOnClickedOutside = true;
            this.absorbInputAroundWindow = true;
            this.onSelect = onSelect;

            if(option== "ItemsTab".Translate()) this.all = DefDatabase<ThingDef>.AllDefs
                .Where(d => d.PlayerAcquirable && (d.IsPleasureDrug || d.IsNonMedicalDrug || d.IsMedicine || d.IsNaturalOrgan /*|| d.isTechHediff*/ || d.IsWeapon /*|| d.IsApparel*/)).OrderBy(d => d.label).ToList();
            if (option == "TabPenAnimals".Translate()) this.all = DefDatabase<ThingDef>.AllDefs
                .Where(d => d.race != null && d.race.Animal && !d.IsCorpse && d.race.animalType != AnimalType.Dryad).OrderBy(d => d.label).ToList();
            //if (option == "Xenotype".Translate()) this.all = DefDatabase<ThingDef>.AllDefs
                //.Where(d => d).OrderBy(d => d.label).ToList();

        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            Rect searchRect = new Rect(0f, 10f, inRect.width, 30f);
            this.searchWidget.OnGUI(searchRect, null, null);

            var filteredAnimals = this.all.Where(a => this.searchWidget.filter.Matches(a.label)).ToList();

            Rect outRect = new Rect(0f, searchRect.yMax + 10f, inRect.width, inRect.height - searchRect.height - 10f);
            float rowHeight = 35f;
            Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, filteredAnimals.Count * rowHeight);

            Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect);

            float yPosition = 0f;
            foreach (ThingDef animal in filteredAnimals)
            {
                Rect rowRect = new Rect(0f, yPosition, viewRect.width, rowHeight);

                Widgets.DrawHighlightIfMouseover(rowRect);

                Rect iconRect = new Rect(rowRect.x + 5f, rowRect.y + 2f, 30f, 30f);
                if (animal.uiIcon != null)
                {
                    GUI.DrawTexture(iconRect, animal.uiIcon);
                }

                Rect labelRect = new Rect(iconRect.xMax + 10f, rowRect.y, rowRect.width - 50f, rowHeight);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(labelRect, animal.LabelCap);
                Text.Anchor = TextAnchor.UpperLeft;

                if (Widgets.ButtonInvisible(rowRect))
                {
                    this.onSelect?.Invoke(animal);
                    this.Close();
                }

                yPosition += rowHeight;
            }

            Widgets.EndScrollView();
        }
    }
}
