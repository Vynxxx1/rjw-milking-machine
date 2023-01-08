using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using rjw;
using static UnityEngine.Random;

namespace MilkingMachine
{
    public class PenisMilker : IMilker
    {
        public bool CanMilk(Pawn milkee)
        {
            if (milkee == null
                || !(milkee.IsColonist || milkee.IsPrisoner || milkee.IsSlave)
                || !milkee.IsHashIntervalTick(MMSettings.milkingInterval))
                return false;

            float? sexNeed = milkee.needs?.TryGetNeed<Need_Sex>()?.CurLevel;
            if (sexNeed == null) // no sex need :(
                return false;

            return sexNeed < Range(0f, 1f);
        }

        public void Milk(Pawn milkee)
        {
            if (!milkee.TryGetPenises(out IEnumerable<Hediff> penises))
                return;

            foreach (Hediff penis in penises)
            {
                if (penis.LabelBase.ToLower().Contains("peg")) // Wood can't cum
                    continue;

                CompHediffBodyPart rjwPenisHediff = penis.TryGetComp<CompHediffBodyPart>();
                if (rjwPenisHediff == null)
                    continue;

                if (!penis.TryGetPenisSizeMultiplier(out float penisSizeMultiplier))
                    continue;

                // Penis type handling
                PenisType penisType = penis.GetPenisType();
                float penisTypeMultiplier = penisType.MilkingQuantity();

                // Trait and quirk checks
                float traitMultiplier = milkee.CumMultiplierFromTraits();
                float quirkMultiplier = milkee.CumMultiplierFromQuirks();

                // Create and place the Thing
                Thing penisThing = ThingMaker.MakeThing(penisType.MilkingType());
                penisThing.stackCount = (int)(milkee.BodySize * penisSizeMultiplier * traitMultiplier * quirkMultiplier * penisTypeMultiplier);
                if (penisThing.stackCount < 1)
                    penisThing.stackCount = 1;
                GenPlace.TryPlaceThing(penisThing, milkee.Position, milkee.Map, ThingPlaceMode.Near);
            }

            Need sexNeed = milkee.needs?.TryGetNeed(VariousDefOf.Sex);
            if (sexNeed != null)
                sexNeed.CurLevel = 1;
        }
    }
}
