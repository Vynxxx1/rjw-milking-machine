using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

// using HarmonyLib;

namespace MilkingMachine
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Returns <c>true</c> if at least one of the items in <c>enumerable</c> returns true when ran through <c>func</c>. <br/>
        /// Not guaranteed to run on all items.
        /// </summary>
        public static bool AggregateOr<T>(this IEnumerable<T> enumerable, Func<T, bool> func)
        {
            // return enumerable.Aggregate(false, (acc, t) => acc || func(t));

            foreach (T t in enumerable)
                if (func(t))
                    return true;
            return false;
        }

        /// <summary>
        /// Returns <c>true</c> if all of the items in <c>enumerable</c> return true when ran through <c>func</c>. <br/>
        /// Not guaranteed to run on all items.
        /// </summary>
        public static bool AggregateAnd<T>(this IEnumerable<T> enumerable, Func<T, bool> func)
        {
            // return enumerable.Aggregate(true, (acc, t) => acc && func(t));

            foreach (T t in enumerable)
                if (!func(t))
                    return false;
            return true;
        }
    }
}
