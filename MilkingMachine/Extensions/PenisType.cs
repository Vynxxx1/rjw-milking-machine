using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace MilkingMachine
{
    public enum PenisType
    {
        Default,
        Equine,
        Canine,
        Demon,
        Dragon,
    }

    public static class PenisTypeExtensions
    {
        public static float MilkingQuantity(this PenisType penisType)
        {
            switch (penisType)
            {
                case PenisType.Equine:
                    return 16f;
                case PenisType.Canine:
                    return 4f;
                case PenisType.Demon:
                    return 6.66f;
                case PenisType.Dragon:
                    return 6f;
                default:
                    return 1f;
            }
        }

        public static ThingDef MilkingType(this PenisType penisType)
        {
            if (penisType == PenisType.Dragon)
                return VariousDefOf.LM_DragonCum;

            if (MMSettings.Sexperience)
                return VariousDefOf.GatheredCum;

            return VariousDefOf.UsedCondom;
        }
    }
}
