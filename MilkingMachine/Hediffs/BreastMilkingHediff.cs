using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using rjw;

namespace MilkingMachine
{
    public class BreastMilkingHediff : MilkingHediffBase
    {
        public override void PostMake()
        {
            base.PostMake();

            if (MMSettings.MilkableColonistsActive)
                milker = new BreastMilkerMilkableColonists();
            else
                milker = new BreastMilkerLegacy();
        }
    }
}
