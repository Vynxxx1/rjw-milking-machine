using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace MilkingMachine
{
    /// <summary>
    /// Doesn't actually have any settings at the moment, just boilerplate lol.
    /// </summary>
    public class MMPenisSettings : ModSettings
    {
        public static float equineCumMultiplier = 16f;
        public static float canineCumMultiplier = 4f;
        public static float demonCumMultiplier = 6.66f;
        public static float dragonCumMultiplier = 6f;

        public static float positiveTraitCumMultiplier = 3f;
        public static float positiveQuirkCumMultiplier = 2f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref equineCumMultiplier, "equineCumMultiplier", equineCumMultiplier, true);
            Scribe_Values.Look(ref canineCumMultiplier, "canineCumMultiplier", canineCumMultiplier, true);
            Scribe_Values.Look(ref demonCumMultiplier, "demonCumMultiplier", demonCumMultiplier, true);
            Scribe_Values.Look(ref dragonCumMultiplier, "dragonCumMultiplier", dragonCumMultiplier, true);

            Scribe_Values.Look(ref positiveTraitCumMultiplier, "positiveTraitCumMultiplier", positiveTraitCumMultiplier, true);
            Scribe_Values.Look(ref positiveQuirkCumMultiplier, "positiveQuirkCumMultiplier", positiveQuirkCumMultiplier, true);
        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            listingStandard.Label("Penis Type Cum Quantity Multipliers");
            listingStandard.SubLabel("Equine: " + equineCumMultiplier.ToString(), inRect.width);
            equineCumMultiplier = (float)Math.Round(listingStandard.Slider(equineCumMultiplier, 0f, 25f), 2);
            listingStandard.Gap(6);

            listingStandard.SubLabel("Canine: " + canineCumMultiplier.ToString(), inRect.width);
            canineCumMultiplier = (float)Math.Round(listingStandard.Slider(canineCumMultiplier, 0f, 25f), 2);
            listingStandard.Gap(6);

            listingStandard.SubLabel("Demon: " + demonCumMultiplier.ToString(), inRect.width);
            demonCumMultiplier = (float)Math.Round(listingStandard.Slider(demonCumMultiplier, 0f, 25f), 2);
            listingStandard.Gap(6);

            listingStandard.SubLabel("Dragon: " + dragonCumMultiplier.ToString(), inRect.width);
            dragonCumMultiplier = (float)Math.Round(listingStandard.Slider(dragonCumMultiplier, 0f, 25f), 2);
            listingStandard.Gap();

            listingStandard.Label("Positive Trait Multiplier: " + positiveTraitCumMultiplier.ToString());
            positiveTraitCumMultiplier = (float)Math.Round(listingStandard.Slider(positiveTraitCumMultiplier, 0f, 10f), 2);
            listingStandard.Gap();

            listingStandard.Label("Positive Quirk Multiplier: " + positiveQuirkCumMultiplier.ToString());
            positiveQuirkCumMultiplier = (float)Math.Round(listingStandard.Slider(positiveQuirkCumMultiplier, 0f, 10f), 2);
            listingStandard.Gap();

            listingStandard.End();
        }
    }
}
