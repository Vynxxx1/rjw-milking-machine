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
                    return MMPenisSettings.equineCumMultiplier;
                case PenisType.Canine:
                    return MMPenisSettings.canineCumMultiplier;
                case PenisType.Demon:
                    return MMPenisSettings.demonCumMultiplier;
                case PenisType.Dragon:
                    return MMPenisSettings.dragonCumMultiplier;
                default:
                    return 1f;
            }
        }

        public static ThingDef MilkingType(this PenisType penisType)
        {
            if (penisType == PenisType.Dragon)
                return VariousDefOf.LM_DragonCum;

            if (MMSettings.SexperienceActive)
                return VariousDefOf.GatheredCum;

            return VariousDefOf.UsedCondom;
        }
    }
}
