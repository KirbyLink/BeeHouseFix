using Harmony;
using System;
using StardewModdingAPI;
using StardewValley;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Reflection.Emit;

namespace FestivalEndTimeTweak
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            //Harmony patcher
            //https://github.com/kirbylink/FestivalEndTimeTweak.git
            var harmony = HarmonyInstance.Create("com.github.kirbylink.beehousefix");
            var original = typeof(Event).GetMethod("exitEvent");
            var prefix = helper.Reflection.GetMethod(typeof(FestivalEndTimeTweak.ChangeFestivalEndTime), "Prefix").MethodInfo;
            var postfix = helper.Reflection.GetMethod(typeof(FestivalEndTimeTweak.ChangeFestivalEndTime), "Postfix").MethodInfo;
            var transpiler = helper.Reflection.GetMethod(typeof(FestivalEndTimeTweak.ChangeFestivalEndTime), "Transpiler").MethodInfo;
            harmony.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix), new HarmonyMethod(transpiler));

        }
    }

    public static class ChangeFestivalEndTime
    {
        /* Check if Event.isFestival is true before going into exitEven() */
        static void Prefix(Event __instance, ref bool __state)
        {

        }

        static void Postfix(Event __instance, ref bool __state)
        {
           
        }
    }
}
