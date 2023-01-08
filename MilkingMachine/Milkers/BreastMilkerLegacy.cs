using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using rjw;

namespace MilkingMachine
{
    public class BreastMilkerLegacy : IMilker
    {
        public bool CanMilk(Pawn milkee)
        {
            return !(milkee.IsColonist || milkee.IsPrisoner || milkee.IsSlave) 
                && milkee.IsHashIntervalTick(MMSettings.milkingInterval);
        }

        public void Milk(Pawn milkee)
        {
            int qty = milkee.GetMilkQuantity();
            if (qty == 0)
                return;
            Thing milkThing = ThingMaker.MakeThing(milkee.GetMilkType());
            milkThing.stackCount = qty;
            GenPlace.TryPlaceThing(milkThing, milkee.Position, milkee.Map, ThingPlaceMode.Near);

            Need_Sex sexNeed = milkee.needs.TryGetNeed<Need_Sex>();
            if (sexNeed != null)
                sexNeed.CurLevel -= 0.02f;
        }
    }
}
