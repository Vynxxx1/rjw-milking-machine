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
        public static float fullnessToMilkAt = 0.5f;
        public static float positiveTraitMultiplier = 3f;

        public override void ExposeData() 
        {
            base.ExposeData();
            Scribe_Values.Look(ref uddersMultiplier, "uddersMultiplier", uddersMultiplier, true);
            Scribe_Values.Look(ref fullnessToMilkAt, "fullnessToMilkAt", fullnessToMilkAt, true);
            Scribe_Values.Look(ref positiveTraitMultiplier, "positiveTraitMultiplier", positiveTraitMultiplier, true);
        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            listingStandard.Label("Udders Multiplier: " + uddersMultiplier.ToString(), -1, "Multiplier for milk quantity when the breast type is udders.");
            uddersMultiplier = (float)Math.Round(listingStandard.Slider(uddersMultiplier, 0f, 100f), 2);
            listingStandard.Gap();

            listingStandard.Label("Fullness to Milk At: " + fullnessToMilkAt.ToString(), -1, "The fullness over which should breasts be milked by milking machines.");
            fullnessToMilkAt = (float)Math.Round(listingStandard.Slider(fullnessToMilkAt, 0f, 1f), 2);
            listingStandard.Gap();

            listingStandard.Label("Positive Trait Multiplier: " + positiveTraitMultiplier.ToString(), -1, "Value to multiply breast milk quantity by when the pawn has a trait that would increase milk output.");
            positiveTraitMultiplier = (float)Math.Round(listingStandard.Slider(positiveTraitMultiplier, 1f, 10f), 2);
            listingStandard.Gap();

            listingStandard.End();
        }
    }
}
