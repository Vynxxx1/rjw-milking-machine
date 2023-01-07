using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace MilkingMachine
{
    [DefOf]
    public static class VariousDefOf
    {
        // core
        public static ThingDef Milk;

        [MayRequireBiotech]
        public static HediffDef Lactating;

        // this mod
        public static ThingDef LM_DragonCum;
        public static TraitDef LM_NaturalHucow = DefDatabase<TraitDef>.GetNamed("LM_NaturalHucow");
        public static TraitDef LM_HighTestosterone = DefDatabase<TraitDef>.GetNamed("LM_HighTestosterone");
        public static TraitDef LM_NaturalCow = DefDatabase<TraitDef>.GetNamed("LM_NaturalCow");

        // rjw
        public static ThingDef UsedCondom;
        public static NeedDef Sex;

        [MayRequire("rjw.sexperience")]
        public static ThingDef GatheredCum;
        [MayRequire("rjw.sexperience")]
        public static HediffDef Lactating_Drug;
        [MayRequire("rjw.sexperience")]
        public static HediffDef Lactating_Permanent;

        [MayRequire("c0ffee.rjw.IdeologyAddons")]
        public static Hediff Hucow;
    }
}
