using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using rjw;

// using HarmonyLib;

namespace MilkingMachine
{
    public static class HediffExtensions
    {
        public static bool IsPenis(this Hediff hediff)
        {
            if (hediff == null)
                return false;

            string defNameLower = hediff.def.defName.ToLower();

            if (!(defNameLower.Contains("penis") || defNameLower.Contains("ovipositor") || defNameLower.Contains("pegdick")))
            {
                if (defNameLower.Contains("tentacle"))
                    return !defNameLower.Contains("penis");
                return false;
            }

            return true;
        }

        public static bool IsBreast(this Hediff hediff)
        {
            string defNameLower = hediff.def.defName.ToLower();
            return hediff != null && (defNameLower.Contains("breast") || defNameLower.Contains("chest"));
        }

        public static float TryGetBreastSizeMultiplier(this Hediff breast)
        {
            // Cows in real life produce 8gal daily while cows in RW produce 14u daily
            // 1u of milk = 1.75gal or 6624ml
            // Humans produce at min 216ml max 3031ml or just under 1u for both average 1623.5ml
            PartSizeExtension.TryGetBreastWeight(breast, out float breastWeight);
            PartSizeExtension.TryGetCupSize(breast, out float cupSize);
            return cupSize / breastWeight;
        }
    }
}
