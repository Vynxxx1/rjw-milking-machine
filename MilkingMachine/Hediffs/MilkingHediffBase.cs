using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace MilkingMachine
{
    public class MilkingHediffBase : HediffWithComps
    {
        protected IMilker milker;

        public override void Tick()
        {
            if (milker?.CanMilk(pawn) ?? false)
                milker.Milk(pawn);
        }
    }
}
