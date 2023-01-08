using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using rjw;
using UnityEngine;
using System.Reflection;

namespace MilkingMachine
{
    [StaticConstructorOnStartup]
    public static class PawnExtensions
    {
        static PawnExtensions()
        {
            if (MMSettings.MilkableColonistsActive)
                SetupMilkableColonists();
        }

        private static void SetupMilkableColonists()
        {
            getMilkTypeOf = getMilkTypeMilkableColonists;
        }

        /* GENERAL */
        public static int GetMilkQuantity(this Pawn pawn)
        {
            IEnumerable<Hediff> breasts = pawn.GetBreastList().Where(HediffExtensions.IsBreast);
            if (breasts.EnumerableNullOrEmpty())
                return 0;

            return (int)(breasts.Count() * pawn.BodySize * pawn.MilkMultiplier(breasts));
        }

        /* MILK TYPE */
        private static Func<Pawn, ThingDef> getMilkTypeOf = defaultMilk;
        public static ThingDef GetMilkType(this Pawn pawn)
        {
            return getMilkTypeOf(pawn);
        }

        private static ThingDef defaultMilk(this Pawn p)
        {
            return VariousDefOf.Milk;
        } 
        private static ThingDef getMilkTypeMilkableColonists(this Pawn pawn)
        {
            return pawn?.TryGetComp<Milk.CompMilkableHuman>()?.Props?.milkDef ?? pawn.defaultMilk();
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
                .Where(HediffExtensions.IsBreast)?
                .Aggregate(false, (agg, breast) => agg || breast.IsUdders()) ?? false;
        }

        public static float MilkMultiplier(this Pawn pawn)
        {
            return pawn.MilkMultiplier(pawn.GetBreastList().Where(HediffExtensions.IsBreast));
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
            bool ok = breast.TryGetBreastSizeMultiplier(out float size);
            if (!ok)
                return totalSoFar;

            totalSoFar *= size;
            if (breast.IsUdders())
                totalSoFar *= breast.pawn.UdderMultiplier();
            return totalSoFar;
        }

        /// <summary>
        /// If the pawn has udders, it is to be multiplied by this value. <br/>
        /// Doesn't check if the pawn has udders or not; check yourself elsewhere.
        /// </summary>
        /// <returns>Value to multiply milk output by if udders</returns>
        public static float UdderMultiplier(this Pawn _)
        {
            return MMBreastSettings.uddersMultiplier;
        }

        public static float CumMultiplierFromTraits(this Pawn pawn)
        {
            float multiplier = 1f;

            if (pawn.story.traits.HasTrait(VariousDefOf.LM_HighTestosterone) 
                || pawn.story.traits.HasTrait(VariousDefOf.LM_NaturalCow))
                multiplier *= 3f;

            return multiplier;
        }

        public static float CumMultiplierFromQuirks(this Pawn pawn)
        {
            float multiplier = 1f;

            if (pawn.Has(Quirk.Messy))
                multiplier *= 2;

            return multiplier;
        }

        public static bool TryGetPenises(this Pawn pawn, out IEnumerable<Hediff> penises)
        {
            penises = pawn?.GetGenitalsList()?.Where(HediffExtensions.IsPenis);
            return !penises.EnumerableNullOrEmpty();
        }
    }
}
