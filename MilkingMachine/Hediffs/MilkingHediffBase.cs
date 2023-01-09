using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace MilkingMachine
{
    public class MilkingHediffBase : HediffWithComps
    {
        protected MilkerBase milker;

        public override void Tick()
        {
            if (milker?.CanMilk(pawn) ?? false)
                milker.Milk(pawn);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref milker, "milker");
        }
    }
}
