using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

// using HarmonyLib;

namespace MilkingMachine
{
    /// <summary>
    /// Based on https://github.com/Vanilla-Expanded/VanillaExpandedFramework/blob/5f103810a74f0a26b3fdadd8c9604af5a44a22f2/Source/VFECore/VFECore/Comps/ThingComps/CompApparelHediffs.cs
    /// </summary>
    public class CompApparelHediffs : ThingComp
    {
        public Pawn wearer = null;

        public List<Hediff> wearerHediffs = new List<Hediff>();
        public CompProperties_ApparelHediffs Props => (CompProperties_ApparelHediffs)props;

        public override void CompTick()
        {
            if (!(parent is Apparel apparel) 
                || apparel.Wearer == wearer
                || Props.hediffDefnames.NullOrEmpty())
                return;

            // Remove hediffs from previous owner
            if (wearer != null)
                wearer.RemoveHediffs(wearerHediffs);

            // Add hediffs to new owner
            if (apparel.Wearer != null)
                RemakeHediffsAndAddToWearer();

            wearer = apparel.Wearer;
        }

        public override void PostExposeData()
        {
            Scribe_References.Look(ref wearer, "wearer");
            Scribe_Collections.Look(ref wearerHediffs, "wearerHediffs", LookMode.Reference);
        }

        public IEnumerable<Hediff> MakeHediffs()
        {
            if (!(parent is Apparel apparel))
                return new List<Hediff>();

            return Props.hediffDefnames
                .Select(defName => HediffMaker.MakeHediff(HediffDef.Named(defName), apparel.Wearer))
                .Where(x => x != null)
                .ToList();
        }

        public void RemakeHediffsAndAddToWearer()
        {
            if (!(parent is Apparel apparel))
                return;

            wearerHediffs = MakeHediffs().ToList();
            apparel.Wearer?.AddHediffs(wearerHediffs);
        }
    }
}
