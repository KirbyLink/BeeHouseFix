using Harmony;
using System;
using StardewModdingAPI;
using StardewValley;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;

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
            var original = typeof(Utility).GetMethod("findCloseFlower");
            var prefix = helper.Reflection.GetMethod(typeof(FixBeeHouses), "Prefix").MethodInfo;
            harmony.Patch(original, new HarmonyMethod(prefix), null);

        }
    }

    public static class FixBeeHouses
    {
        /* Check if Event.isFestival is true before going into exitEven() */
        static bool Prefix(GameLocation location, Vector2 startTileLocation, Crop __result)
        {
            
            Queue<Vector2> vector2Queue = new Queue<Vector2>();
            HashSet<Vector2> vector2Set = new HashSet<Vector2>();
            var terrainFeatures = location.terrainFeatures;

            vector2Queue.Enqueue(startTileLocation);
            for (int index1 = 0; index1 <= 150 && vector2Queue.Count > 0; ++index1)
            {
                Vector2 index2 = vector2Queue.Dequeue();
                bool containsHoeDirt = terrainFeatures.ContainsKey(index2) && terrainFeatures[index2] is HoeDirt;
                if (!containsHoeDirt)
                {
                    goto CheckAdjacent;
                }

                HoeDirt dirt = terrainFeatures[index2] as HoeDirt;
                bool isFlower = dirt.crop != null && dirt.crop.programColored.Value;
                if (!isFlower)
                {
                    goto CheckAdjacent;
                }

                bool isGrownAndNotDead = dirt.crop.currentPhase.Value >= dirt.crop.phaseDays.Count - 1 && !dirt.crop.dead.Value;
                if (isGrownAndNotDead)
                {
                    __result = dirt.crop;
                    return false;
                }

                CheckAdjacent:
                foreach (Vector2 adjacentTileLocation in Utility.getAdjacentTileLocations(index2))
                {
                    if (!vector2Set.Contains(adjacentTileLocation))
                        vector2Queue.Enqueue(adjacentTileLocation);
                }
                vector2Set.Add(index2);
            }

            __result = (Crop)null;
            return false;
            
        }
    }
}
