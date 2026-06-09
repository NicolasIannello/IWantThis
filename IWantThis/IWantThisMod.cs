using UnityEngine;
using Verse;

namespace IWantThis
{
    public class IWantThisMod : Mod
    {
        public static IWantThisMod Instance;
        private IWantThisSettings Settings;
        private string buf;

        public IWantThisMod(ModContentPack content) : base(content)
        {
            Instance = this;
            this.Settings = GetSettings<IWantThisSettings>();
        }

        public static IntRange IntervalArrival => Instance.Settings.IntervalArrival;
        public static bool EnableCap => Instance.Settings.EnableCap;
        public static int Cap => Instance.Settings.Cap;

        public override string SettingsCategory()
        {
            return "IWantThis";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);

            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.CheckboxLabeled("IWantThis.EnableCap".Translate("30.000"), ref Settings.EnableCap);
            if (Settings.EnableCap)
            {
                listing.Label("IWantThis.Cap".Translate());
                listing.IntEntry(ref Settings.Cap, ref buf, 100);
            }
            listing.Gap();

            listing.Label("IWantThis.IntervalArrival".Translate(2, 10));
            listing.IntRange(ref Settings.IntervalArrival, 0, 30);

            listing.Gap(12f);
            if (listing.ButtonText("IWantThis.Reset".Translate()))
            {
                //Log.Message(Settings.EnableCap);
                Settings.EnableCap = false;
                Settings.Cap = 30000;
                Settings.IntervalArrival = new IntRange(2, 10);
                buf = "30000";
            }

            listing.End();
        }
    }

    public class IWantThisSettings : ModSettings
    {
        public IntRange IntervalArrival = new IntRange(2, 10);
        public bool EnableCap = false;
        public int Cap = 30000;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref IntervalArrival, nameof(IntervalArrival), new IntRange(2, 10));
            Scribe_Values.Look(ref EnableCap, nameof(EnableCap), false);
            Scribe_Values.Look(ref Cap, nameof(Cap), 30000);
        }
    }
}