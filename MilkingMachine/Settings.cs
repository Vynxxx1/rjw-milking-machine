using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;

// using HarmonyLib;

namespace MilkingMachine
{
    public class MMSettings
    {
        public static bool MilkableColonistsActive = ModsConfig.IsActive("mlie.milkablecolonists") || ModsConfig.IsActive("rjw.milk.humanoid");

        public static int milkingInterval = 3000;
    }
}
