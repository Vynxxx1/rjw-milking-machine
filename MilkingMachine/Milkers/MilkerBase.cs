using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace MilkingMachine
{
    /// <summary>
    /// Every tick, CanMilk is ran then, if it returned true, Milk.
    /// </summary>
    public abstract class MilkerBase : IExposable
    {
        public abstract bool CanMilk(Pawn milkee);

        public abstract void Milk(Pawn milkee);

        public void ExposeData() {}
    }
}
