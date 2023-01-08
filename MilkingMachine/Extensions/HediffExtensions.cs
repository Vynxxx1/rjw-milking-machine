using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using rjw;

namespace MilkingMachine
{
    public static class HediffExtensions
    {
        public static bool IsPenis(this Hediff hediff)
        {
            // pretty sure this can be done w/out any if statements but my brain is fried...
            string defNameLower = hediff?.def.defName.ToLower() ?? "";
            if (!(defNameLower.Contains("penis") || defNameLower.Contains("ovipositorm") || defNameLower.Contains("pegdick"))) // rjw defname for male ovipositor is "ovipositorm"; not typo
            {
                if (defNameLower.Contains("tentacle"))
                    return !defNameLower.Contains("penis");
                return false;
            }
            return true;
        }

        public static bool IsBreast(this Hediff hediff)
        {
            string defNameLower = hediff?.def.defName.ToLower() ?? "";
            return defNameLower.Contains("breast") || defNameLower.Contains("chest");
        }

        public static bool IsUdders(this Hediff breast)
        {
            return breast.LabelBase.ToLower().Contains("udder");
        }

        public static bool TryGetBreastSizeMultiplier(this Hediff breast, out float sizeMultiplier)
        {
            // Cows in real life produce 8gal daily while cows in RW produce 14u daily
            // 1u of milk = 1.75gal or 6624ml
            // Humans produce at min 216ml max 3031ml or just under 1u for both average 1623.5ml
            if (PartSizeExtension.TryGetBreastWeight(breast, out float breastWeight)
                && PartSizeExtension.TryGetCupSize(breast, out float cupSize))
            {
                sizeMultiplier = cupSize / breastWeight;
                return true;
            } else
            {
                sizeMultiplier = 1;
                return false;
            }
        }

        public static bool TryGetPenisSizeMultiplier(this Hediff penis, out float sizeMultiplier)
        {
            // Humans can produce 1.25ml to 5ml of semen per day which averages at 3.125ml (1)
            // Horses produce about 50ml which is (16)
            // Dragons produce 3gal or 11356.2ml which is x3633.984 UsedCondoms (3663)
            // Dragon semen will be output into half-gallon jars (6)
            // Dogs produce 1ml to 30ml of semen, average 15ml (4)
            // Demons probably produce x666 that of a human (666)
            if (PartSizeExtension.TryGetLength(penis, out float penisLength)
                && PartSizeExtension.TryGetGirth(penis, out float penisGirth))
            {
                sizeMultiplier = penisLength / penisGirth;
                return true;
            } else
            {
                sizeMultiplier = 1;
                return false;
            }
        }

        /// <summary>
        /// Assumes the given Hediff is a penis.
        /// </summary>
        public static PenisType GetPenisType(this Hediff penis)
        {
            switch (penis.LabelBase.ToLower())
            {
                case "equine":
                    return PenisType.Equine;
                case "canine":
                    return PenisType.Canine;
                case "demon":
                    return PenisType.Demon;
                case "dragon":
                    return PenisType.Dragon;
                default:
                    return PenisType.Default;
            }
        }
    }
}
