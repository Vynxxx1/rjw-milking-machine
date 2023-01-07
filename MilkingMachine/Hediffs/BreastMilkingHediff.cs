using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using rjw;

namespace MilkingMachine
{
    public class BreastMilkingHediff : HediffWithComps
    {
        public override void Tick()
        {
            pawn.MilkPawn();
        }
	}
}
