using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace MilkingMachine
{
	public class CompProperties_ApparelHediffs : CompProperties
	{
		public List<string> hediffDefnames;
		public CompProperties_ApparelHediffs()
		{
			compClass = typeof(CompApparelHediffs);
		}
	}
}
