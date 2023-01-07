﻿using RimWorld;
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
            canBeMilked = canBeMilkedMilkableColonists;
            milkPawn = milkPawnMilkableColonists;
        }

        /* GENERAL */
        public static int GetMilkQuantity(this Pawn pawn)
        {
            IEnumerable<Hediff> breasts = pawn.GetBreastList().Where(HediffExtensions.IsBreast);
            if (breasts.EnumerableNullOrEmpty())
                return 0;

            return (int)(breasts.Count() * pawn.BodySize * pawn.MilkMultiplier(breasts));
        }

        private static Func<Pawn, bool> canBeMilked = canBeMilkedLegacy;
        public static bool CanBeMilked(this Pawn pawn)
        {
            return canBeMilked(pawn);
        }
        private static bool canBeMilkedLegacy(this Pawn pawn)
        {
            return pawn.IsHashIntervalTick(MMSettings.milkingInterval);
        }
        private static bool canBeMilkedMilkableColonists(this Pawn pawn)
        {
            Milk.HumanCompHasGatherableBodyResource milkee =  pawn.TryGetComp<Milk.HumanCompHasGatherableBodyResource>();
            if (milkee == null || !milkee.Active)
                return false;
            Log.Message(milkee.Fullness.ToString());
            return milkee.Fullness > 0.5f;
        }

        public static bool CanPenisBeMilked(this Pawn pawn)
        {
            return pawn.needs.TryGetNeed<Need_Sex>().CurLevel < 0.4f;
        }

        private static Action<Pawn> milkPawn = milkPawnLegacy;
        public static void MilkPawn(this Pawn pawn)
        {
            milkPawn(pawn);
        }
        private static void milkPawnLegacy(Pawn pawn) 
        {
            if (!pawn.CanBeMilked())
                return;
            if (!(pawn.IsColonist || pawn.IsPrisoner || pawn.IsSlave))
                return;

            int qty = pawn.GetMilkQuantity();
            if (qty == 0)
                return;
            Thing milkThing = ThingMaker.MakeThing(pawn.GetMilkType());
            milkThing.stackCount = qty;
            GenPlace.TryPlaceThing(milkThing, pawn.Position, pawn.Map, ThingPlaceMode.Near);

            Need_Sex sexNeed = pawn.needs.TryGetNeed<Need_Sex>();
            if (sexNeed != null)
                sexNeed.CurLevel -= 0.02f;
        }
        private static void milkPawnMilkableColonists(Pawn pawn)
        {
            if (pawn.CanBeMilked())
                gatherMilk(pawn);
        }
        /// <summary>
        /// Based on https://github.com/emipa606/MilkableColonists/blob/main/Source/Milk/HumanCompHasGatherableBodyResource.cs#L90
        /// </summary>
        /// <param name="pawn"></param>
        private static void gatherMilk(Pawn pawn)
        {
            if (pawn == null)
                return;

            Milk.CompMilkableHuman milkee = pawn.TryGetComp<Milk.CompMilkableHuman>();
            if (milkee == null || !milkee.Active) // i think maybe fallthrough to default if null and return doing nothing if nonnull but inactive
                return;

            float BreastSize = pawn.health.hediffSet.GetBreastSize();

            ThingDef ResourceDef = milkee.Props.milkDef;
            var ResourceAmount = milkee.Props.milkAmount;

            var i = GenMath.RoundRandom(ResourceAmount * BreastSize * milkee.Fullness);
            while (i > 0)
            {
                var num = Mathf.Clamp(i, 1, ResourceDef.stackLimit);
                i -= num;
                var thing = ThingMaker.MakeThing(ResourceDef);
                thing.stackCount = num;
                GenPlace.TryPlaceThing(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near);
            }

            typeof(Milk.CompMilkableHuman).GetField("fullness", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(milkee, 0f);
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

        public static float MilkMultiplier(this Pawn pawn)
        {
            return pawn.MilkMultiplier(pawn.GetBreastList().Where(HediffExtensions.IsBreast));
        }

        private static float MilkMultiplierAggregator(float totalSoFar, Hediff breast)
        {
            totalSoFar *= breast.TryGetBreastSizeMultiplier();
            if (breast.IsUdders())
                totalSoFar *= breast.pawn.UdderMultiplier();
            return totalSoFar;
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
