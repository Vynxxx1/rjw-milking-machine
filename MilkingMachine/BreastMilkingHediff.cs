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
            Pawn pawn = this.pawn;
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
        }
	}
}
