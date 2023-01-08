using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using rjw;
using UnityEngine;
using System.Reflection;

namespace MilkingMachine
{
    class BreastMilkerMilkableColonists : IMilker
    {
        public bool CanMilk(Pawn pawn)
        {
            Milk.HumanCompHasGatherableBodyResource milkee = pawn.TryGetComp<Milk.HumanCompHasGatherableBodyResource>();
            if (milkee == null || !milkee.Active)
                return false;
            return milkee.Fullness > MMBreastSettings.fullnessToMilkAt;
        }

        /// <summary>
        /// Largely based on https://github.com/emipa606/MilkableColonists/blob/main/Source/Milk/HumanCompHasGatherableBodyResource.cs#L90
        /// </summary>
        public void Milk(Pawn pawn)
        {
            Milk.CompMilkableHuman milkee = pawn?.TryGetComp<Milk.CompMilkableHuman>();
            if ((!milkee?.Active) ?? false)
                return;

            float BreastSize = pawn.health.hediffSet.GetBreastSize();

            ThingDef ResourceDef = pawn.GetMilkType();
            var ResourceAmount = milkee.Props.milkAmount;

            var i = GenMath.RoundRandom(ResourceAmount * BreastSize * milkee.Fullness);
            while (i > 0)
            {
                var num = Mathf.Clamp(i, 1, ResourceDef.stackLimit);
                i -= num;
                var thing = ThingMaker.MakeThing(ResourceDef);
                thing.stackCount = num;
                GenPlace.TryPlaceThing(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near);
            }

            typeof(Milk.CompMilkableHuman).GetField("fullness", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(milkee, 0f);
        }
    }
}
