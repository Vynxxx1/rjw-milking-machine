using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using rjw;

namespace MilkingMachine
{
    public class PenisMilkingHediff : MilkingHediffBase
    {
        public override void PostMake()
        {
            base.PostMake();
            milker = new PenisMilker();
        }
    }
}
