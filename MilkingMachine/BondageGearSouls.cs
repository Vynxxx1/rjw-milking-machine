/*
 * Duplicated code and could probably be done using XML. I don't know how, though.
 * Might be possible with XML or Harmony patches idk.
 */

using RimWorld;
using Verse;
using rjw;

namespace MilkingMachine
{
    public class BreastMilkerSoul : bondage_gear_soul
    {
        public override void on_wear(Pawn wearer, Apparel gear)
        {
            base.on_wear(wearer, gear);
            wearer?.health?.AddHediff(VariousDefOf.LM_BreastMilkingHediff);
        }

        public override void on_remove(Apparel gear, Pawn former_wearer)
        {
            base.on_remove(gear, former_wearer);
            former_wearer?.RemoveHediffs(VariousDefOf.LM_BreastMilkingHediff);
        }
    }

    public class PenisMilkerSoul : bondage_gear_soul
    {
        public override void on_wear(Pawn wearer, Apparel gear)
        {
            base.on_wear(wearer, gear);
            wearer?.health?.AddHediff(VariousDefOf.LM_PenisMilkingHediff);
        }

        public override void on_remove(Apparel gear, Pawn former_wearer)
        {
            base.on_remove(gear, former_wearer);
            former_wearer?.RemoveHediffs(VariousDefOf.LM_PenisMilkingHediff);
        }
    }
}
