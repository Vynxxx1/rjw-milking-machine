using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace MilkingMachine
{
    public static class HediffSetExtensions
    {
        public static IEnumerable<Func<HediffSet, bool>> HugeBreasts = new List<Func<HediffSet, bool>>()
        {
            s => s.HasHediffSilentFail("HugeBreasts"),
            s => s.HasHediffSilentFail("BionicBreasts"),
            s => s.HasHediffSilentFail("SlimeBreasts"),
            s => s.HasHediffSilentFail("GR_MuffaloMammaries"),
        };
        public static IEnumerable<Func<HediffSet, bool>> SmallBreasts = new List<Func<HediffSet, bool>>()
        {
            s => s.HasHediffSilentFail("SmallBreasts"),
        };
        public static IEnumerable<Func<HediffSet, bool>> LargeBreasts = new List<Func<HediffSet, bool>>()
        {
            s => s.HasHediffSilentFail("LargeBreasts"),
            s => s.HasHediffSilentFail("ArchotechBreasts"),
        };
        public static IEnumerable<Func<HediffSet, bool>> FlatBreasts = new List<Func<HediffSet, bool>>()
        {
            s => s.HasHediffSilentFail("FlatBreasts"),
            s => s.HasHediffSilentFail("FeaturelessBreasts"),
            s => s.pawn.gender == Gender.Male,
        };

        private static bool RunCheckers(this HediffSet hediffSet, IEnumerable<Func<HediffSet, bool>> checkers, bool seed = false)
        {
            return checkers.Aggregate(seed, (foundSoFar, checker) => foundSoFar || checker(hediffSet));
        }

        private static bool HasHediffSilentFail(this HediffSet hediffSet, string defName)
        {
            return hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamedSilentFail(defName));
        }

        public static float GetBreastSize(this HediffSet hediffSet)
        {
            if (hediffSet == null)
                return 0.5f; // idk what default value to do

            if (hediffSet.RunCheckers(HugeBreasts))
                return 1.5f;
            if (hediffSet.RunCheckers(SmallBreasts))
                return 0.75f;
            if (hediffSet.RunCheckers(LargeBreasts))
                return 1.25f;
            if (hediffSet.RunCheckers(FlatBreasts))
                return 0.5f;

            return 1f;
        }
    }
}
