using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace MilkingMachine
{
    class MMBreastSettings : ModSettings
    {
        /// <summary>
        /// Multiplier for milk quantity when the breast type is udders.
        /// </summary>
        public static float uddersMultiplier = 8f;

        public override void ExposeData() 
        {
            base.ExposeData();
            Scribe_Values.Look(ref uddersMultiplier, "uddersMultiplier", uddersMultiplier, true);
        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            listingStandard.Label("Udders Multiplier", -1, "Multiplier for milk quantity when the breast type is udders.");
            uddersMultiplier = listingStandard.Slider(uddersMultiplier, 0f, 100f);
            listingStandard.Gap();

            listingStandard.End();
        }
    }
}
