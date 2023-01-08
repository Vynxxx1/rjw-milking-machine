using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;

namespace MilkingMachine
{
    public class MMSettings : ModSettings
    {
        // active mods
        public static bool MilkableColonistsActive = ModsConfig.IsActive("mlie.milkablecolonists") || ModsConfig.IsActive("rjw.milk.humanoid");
        public static bool SexperienceActive = ModsConfig.IsActive("rjw.sexperience");
        public static bool CriaActive = ModsConfig.IsActive("c0ffee.rjw.IdeologyAddons");

        // scribe exposed
        public static int milkingInterval = 3000;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref milkingInterval, "milkingInterval", milkingInterval, true);
        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            listingStandard.Label("Milking Interval: " + milkingInterval.ToString(), -1, "Used in breast milking without Milkable Colonists enabled and in all penis milking");
            milkingInterval = (int)listingStandard.Slider(milkingInterval, 1f, 100000f);
            listingStandard.Gap();

            listingStandard.End();
        }
    }
}
