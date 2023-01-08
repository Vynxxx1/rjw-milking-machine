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
        public override void ExposeData()
        {
            base.ExposeData();
        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            listingStandard.End();
        }
    }
}
