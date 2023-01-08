using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

// using HarmonyLib;

namespace MilkingMachine
{
    public class MMSettingsController : Mod
    {
        public MMSettingsController(ModContentPack content) : base(content)
        {
            GetSettings<MMSettings>();
        }

        public override string SettingsCategory()
        {
            return "Milking Machine - Main Settings";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            MMSettings.DoWindowContents(inRect);
        }
    }

    public class MMBreastSettingsController : Mod
    {
        public MMBreastSettingsController(ModContentPack content) : base(content)
        {
            GetSettings<MMBreastSettings>();
        }

        public override string SettingsCategory()
        {
            return "Milking Machine - Breast Milking";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            MMBreastSettings.DoWindowContents(inRect);
        }
    }

    public class MMPenisSettingsController : Mod
    {
        public MMPenisSettingsController(ModContentPack content) : base(content)
        {
            GetSettings<MMPenisSettings>();
        }

        public override string SettingsCategory()
        {
            return "Milking Machine - Penis Milking";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            MMPenisSettings.DoWindowContents(inRect);
        }
    }
}
