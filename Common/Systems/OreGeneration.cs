using ChaoticUprising.Content.Tiles.Ores;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ChaoticUprising.Common.Systems
{
    public class OreGeneration : ModSystem
    {
        //public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        //{
            //int CleanupIndex = tasks.FindIndex(genPass => genPass.Name.Equals("Shinies"));
            //tasks.Insert(CleanupIndex + 1, new PassLegacy("Chaotic Uprising Shinies", Shinies));
        //}

        private void Shinies(GenerationProgress progress, GameConfiguration _)
        {
            progress.Message = "Making the world more shiny";
            //CUUtils.Ore(ModContent.TileType<TelekineOre>(), 25, 500000);
        }
    }
}
