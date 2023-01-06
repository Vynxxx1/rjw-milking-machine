using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace MilkingMachine
{
    [StaticConstructorOnStartup]
    public static class PawnExtensions
    {
        static PawnExtensions()
        {
            if (ModLister.GetActiveModWithIdentifier("rjw.milk.humanoid") != null)
                getMilkTypeOf = milkableColonistsGetMilkType;
        }

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
    }
}
