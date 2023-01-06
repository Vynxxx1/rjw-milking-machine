using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using rjw;

namespace MilkingMachine
{
    [StaticConstructorOnStartup]
    public static class PawnExtensions
    {
        static PawnExtensions()
        {
            if (MMSettings.MilkableColonistsActive)
                getMilkTypeOf = milkableColonistsGetMilkType;
        }

        /* GENERAL */
        public static int GetMilkQuantity(this Pawn pawn)
        {
            IEnumerable<Hediff> breasts = pawn.GetBreastList().Where(Custom_Genital_Helper.is_breast);
            if (breasts.EnumerableNullOrEmpty())
                return 0;

            return (int)(breasts.Count() * pawn.BodySize * pawn.MilkMultiplier(breasts));
        }

        public static bool CanBeMilked(this Pawn pawn)
        {
            return pawn.IsHashIntervalTick(MMSettings.milkingInterval);
        }

        /* MILK TYPE */
        private static readonly ThingDef defaultMilk = VariousDefOf.Milk;
        private static Func<Pawn, ThingDef> getMilkTypeOf = p => defaultMilk;

        private static readonly Func<Pawn, ThingDef> milkableColonistsGetMilkType = p =>
        {
            return p?.TryGetComp<Milk.CompMilkableHuman>()?.Props?.milkDef ?? defaultMilk;
        };

        public static ThingDef GetMilkType(this Pawn pawn)
        {
            return getMilkTypeOf(pawn);
        }

        /* NATURAL (HU)COW TRAITS */
        public static float MilkMultiplierFromTraits(this Pawn pawn)
        {
            return pawn.story.traits.HasTrait(VariousDefOf.LM_NaturalCow) || pawn.story.traits.HasTrait(VariousDefOf.LM_NaturalHucow) ? 3 : 1;
        }

        /* BODY PARTS */
        public static bool HasUdders(this Pawn pawn)
        {
            return pawn?.GetBreastList()?
                .Where(Custom_Genital_Helper.is_breast)?
                .Aggregate(false, (agg, breast) => agg || breast.IsUdders()) ?? false;
        }

        public static bool IsUdders(this Hediff breast)
        {
            return breast.LabelBase.ToLower().Contains("udder");
        }

        public static float MilkMultiplier(this Pawn pawn, IEnumerable<Hediff> breasts)
        {
            if (breasts.EnumerableNullOrEmpty())
                return 0f; // or maybe 1f?

            float multiplier = pawn.MilkMultiplierFromTraits();

            if (MMSettings.MilkableColonistsActive
                && (pawn.health.hediffSet.HasHediff(VariousDefOf.Lactating_Drug) || pawn.health.hediffSet.HasHediff(VariousDefOf.Lactating_Permanent)))
                multiplier *= 2;
            if (ModsConfig.BiotechActive && pawn.health.hediffSet.HasHediff(VariousDefOf.Lactating))
                multiplier *= 2;

            return breasts.Aggregate(multiplier, MilkMultiplierAggregator);
        }

        private static float MilkMultiplierAggregator(float totalSoFar, Hediff breast)
        {
            totalSoFar *= breast.TryGetBreastSizeMultiplier();
            if (breast.IsUdders())
                totalSoFar *= breast.pawn.UdderMultiplier();
            return totalSoFar;
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

        /// <summary>
        /// If the pawn has udders, it is to be multiplied by this value.
        /// </summary>
        /// <returns>Value to multiply milk output by</returns>
        public static float UdderMultiplier(this Pawn _)
        {
            return 8;
        }
    }
}
