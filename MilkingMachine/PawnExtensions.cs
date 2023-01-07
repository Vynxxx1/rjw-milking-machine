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
            canBeMilked = canBeMilkedMilkableColonists;
            milk = milkMilkableColonists;
        }

        /* GENERAL */
        public static int GetMilkQuantity(this Pawn pawn)
        {
            IEnumerable<Hediff> breasts = pawn.GetBreastList().Where(IsBreast);
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

        private static Action<Pawn> milk = milkLegacy;
        public static void MilkPawn(this Pawn pawn)
        {
            milk(pawn);
        }
        private static void milkLegacy(Pawn pawn) 
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
        private static void milkMilkableColonists(Pawn pawn)
        {
            if (!pawn.CanBeMilked())
                return;
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

            float BreastSize = 1f;

            // this can almost certainly be refactored massively; also doesn't account for multiple sets
            if (pawn.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamedSilentFail("HugeBreasts")) 
                || pawn.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamedSilentFail("BionicBreasts")) 
                || pawn.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamedSilentFail("SlimeBreasts")) 
                || pawn.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamedSilentFail("GR_MuffaloMammaries")))
            {
                BreastSize = 1.5f;
            }
            /*else if (pawn.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamedSilentFail("Breasts")) 
                || pawn.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamedSilentFail("HydraulicBreasts")) 
                || (pawn.gender == Gender.Female && DefDatabase<HediffDef>.GetNamedSilentFail("Breasts") == null))
            {
                BreastSize = 1f;
            }*/
            else if (pawn.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamedSilentFail("SmallBreasts")))
            {
                BreastSize = 0.75f;
            }
            else if (pawn.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamedSilentFail("LargeBreasts"))
                || pawn.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamedSilentFail("ArchotechBreasts")))
            {
                BreastSize = 1.25f;
            }
            else if (pawn.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamedSilentFail("FlatBreasts")) 
                || pawn.gender == Gender.Male)
            {
                BreastSize = 0.5f;
            }

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
                .Where(IsBreast)?
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
