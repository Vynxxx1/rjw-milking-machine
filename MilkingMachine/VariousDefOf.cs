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

        // sexperience
        [MayRequire("rjw.sexperience")]
        public static ThingDef GatheredCum;
        // Todo: use these stats to calculate milk amount when available
        // https://gitgud.io/c0ffeeeeeeee/coffees-rjw-ideology-addons/-/blob/master/CRIALactation/Defs/StatDefs/Stats_Milk_Production.xml
        [MayRequire("rjw.sexperience")]
        public static StatDef MilkProductionSpeed;
        [MayRequire("rjw.sexperience")]
        public static StatDef MilkProductionYield;

        // milkable colonists
        [MayRequire("rjw.milk.humanoid")]
        public static HediffDef Lactating_Drug;
        [MayRequire("rjw.milk.humanoid")]
        public static HediffDef Lactating_Permanent;
        [MayRequire("rjw.milk.humanoid")]
        public static HediffDef Lactating_Permanent_Heavily;

        // cria
        [MayRequire("c0ffee.rjw.IdeologyAddons")]
        public static Hediff Hucow;

        // Humanoid Slimes
        [MayRequire("Akiya82.SlimeGirls")]
        public static HediffDef MucusBreasts; // https://gitgud.io/Akiya82/slime-girls/-/blob/470715f7c2d5a08aa1f20cb9fa6a6c4dbd902a2d/1.4/Defs/HediffDefs/Hediffs_PrivateParts/Hediffs_PrivateParts_Mucus.xml#L61
    }
}
