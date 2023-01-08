using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

// using HarmonyLib;

namespace MilkingMachine
{
    /// <summary>
    /// Every tick, CanMilk is ran then, if it returned true, Milk.
    /// </summary>
    public interface IMilker
    {
        bool CanMilk(Pawn milkee);
        void Milk(Pawn milkee);
    }
}
