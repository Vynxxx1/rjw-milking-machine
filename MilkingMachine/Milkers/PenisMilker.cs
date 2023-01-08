using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using rjw;

// using HarmonyLib;

namespace MilkingMachine
{
    public class PenisMilker : IMilker
    {
        public bool CanMilk(Pawn milkee)
        {
            return milkee != null
                && (milkee.IsColonist || milkee.IsPrisoner || milkee.IsSlave)
                && milkee.needs.TryGetNeed<Need_Sex>().CurLevel < 0.4f; // i think it'd be better to have it be chance-based rather than a flat threshold
        }

        public void Milk(Pawn milkee)
        {
            float penisSizeMultiplier = 1;
            float traitMultiplier = 1;
            float quirkMultiplier = 1;
            float horse = 1;
            float dragon = 1;
            float dog = 1;
            float demon = 1;

            IEnumerable<Hediff> penises = milkee.GetGenitalsList().Where(HediffExtensions.IsPenis);
            if (penises.EnumerableNullOrEmpty())
                return;

            foreach (Hediff penis in penises)
            {
                if (penis.LabelBase.ToLower().Contains("peg")) // Wood can't cum
                    continue;
                CompHediffBodyPart rjwPenisHediff = penis.TryGetComp<CompHediffBodyPart>();
                if (rjwPenisHediff == null)
                    continue;

                // Humans can produce 1.25ml to 5ml of semen per day which averages at 3.125ml (1)
                // Horses produce about 50ml which is (16)
                // Dragons produce 3gal or 11356.2ml which is x3633.984 UsedCondoms (3663)
                // Dragon semen will be output into half-gallon jars (6)
                // Dogs produce 1ml to 30ml of semen, average 15ml (4)
                // Demons probably produce x666 that of a human (666)
                PartSizeExtension.TryGetLength(penis, out float penisLength);
                PartSizeExtension.TryGetGirth(penis, out float penisGirth);
                penisSizeMultiplier = penisLength / penisGirth;

                // Racial penis checks
                if (penis.LabelBase.ToLower().Contains("equine"))
                    horse = 16;
                if (penis.LabelBase.ToLower().Contains("canine"))
                    dog = 4;
                if (penis.LabelBase.ToLower().Contains("demon"))
                    demon = 6.66f;
                if (penis.LabelBase.ToLower().Contains("dragon"))
                {
                    Thing dragonPenisThing = ThingMaker.MakeThing(VariousDefOf.LM_DragonCum);
                    dragonPenisThing.stackCount = (int)(6);
                    GenPlace.TryPlaceThing(dragonPenisThing, milkee.Position, milkee.Map, ThingPlaceMode.Near);
                    return;
                }

                Thing penisThing = ThingMaker.MakeThing(VariousDefOf.UsedCondom);
                if (MMSettings.Sexperience)
                    penisThing = ThingMaker.MakeThing(VariousDefOf.GatheredCum);

                // Trait and quirk checks
                if (milkee.story.traits.HasTrait(VariousDefOf.LM_HighTestosterone) || milkee.story.traits.HasTrait(VariousDefOf.LM_NaturalCow))
                    traitMultiplier = 3;
                if (milkee.Has(Quirk.Messy))
                    quirkMultiplier = 2;

                penisThing.stackCount = (int)(milkee.BodySize * penisSizeMultiplier * traitMultiplier * quirkMultiplier * horse * dog * demon);
                if (penisThing.stackCount < 1)
                    penisThing.stackCount = 1;
                GenPlace.TryPlaceThing(penisThing, milkee.Position, milkee.Map, ThingPlaceMode.Near);

                Need sexNeed = milkee.needs?.TryGetNeed(VariousDefOf.Sex);
                if (sexNeed != null)
                    sexNeed.CurLevel = 1;
            }
        }
    }
}
