using ChaoticUprising.Content.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ChaoticUprising.Common.Systems
{
    public class MonolithGeneration : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int CleanupIndex = tasks.FindIndex(genPass => genPass.Name.Equals("Life Crystals"));
            tasks.Insert(CleanupIndex + 1, new PassLegacy("Chaotic Uprising Monoliths", Monoliths));
        }

        private void Monoliths(GenerationProgress progress, GameConfiguration _)
        {
            progress.Message = "Making the world more alien";
            for (int i = 0; i < Main.maxTilesX * Main.maxTilesY / 100; i++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX - 3);
                int y = WorldGen.genRand.Next(0, Main.maxTilesY - 8);
                Monolith(x, y);
            }
        }

        private static void Monolith(int x, int y)
        {
            int width = 4;
            int height = 9;

            for (int X = x; X < x + width; X++)
            {
                if (WorldGen.TileEmpty(X, y + 1))
                    return;
                for (int Y = y; Y > y - height; Y--)
                    if (!WorldGen.TileEmpty(X, Y))
                        return;
            }

            for (int X = x; X < x + width; X++)
                for (int Y = y; Y > y - height; Y--)
                    WorldGen.PlaceTile(X, Y, ModContent.TileType<Monolith>(), true, true);
        }
    }
}
