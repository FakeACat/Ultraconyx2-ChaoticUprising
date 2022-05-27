using ChaoticUprising.Content.Tiles;
using ChaoticUprising.Content.Walls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace ChaoticUprising.Common.Systems
{
    public class DarknessGeneration : ModSystem
    {

        public static int darknessX;
        public static int darknessY;

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int CleanupIndex = tasks.FindIndex(genPass => genPass.Name.Equals("Final Cleanup"));
            tasks.Insert(CleanupIndex + 1, new PassLegacy("Chaotic Uprising Darkness Biome", Darkness));
        }

        private void Darkness(GenerationProgress progress, GameConfiguration _)
        {
            int x;
            if (Main.dungeonX < Main.maxTilesX / 2)
            {
                x = Main.maxTilesX - Main.dungeonX;
            }
            else
            {
                x = -(Main.dungeonX - Main.maxTilesX);
            }
            Vector2 vector = new(x, 200);
            darknessX = (int)vector.X;
            darknessY = (int)vector.Y;

            for (int X = darknessX - 120; X < darknessX + 120; X++)
            {
                for (int Y = darknessY - 120; Y < darknessY + 120; Y++)
                {
                    WorldGen.KillTile(X, Y, false, false, true);
                }
            }

            for (int X = darknessX - 400; X < darknessX + 400; X++)
            {
                for (int Y = darknessY - 400; Y < darknessY + 400; Y++)
                {
                    int dist = (int)Vector2.Distance(new Vector2(X, Y), vector);
                    if (dist < 400 && dist > 120 && Y < Main.worldSurface)
                        if (WorldGen.genRand.NextBool(dist * 20))
                            MiniIsland(X, Y, WorldGen.genRand.Next(5, 11), (ushort)ModContent.TileType<NightRock>());

                    if (dist < 120 && dist > 100)
                    {
                        WorldGen.PlaceTile(X, Y, ModContent.TileType<NightRock>());
                        Main.tile[X, Y].WallType = (ushort)ModContent.WallType<NightRockWall>();

                        if (WorldGen.genRand.NextBool(40))
                            GenDarknessStalagtite(new Vector2(X, Y), new Vector2(darknessX, darknessY));
                    }
                }
            }
        }

        private static void GenDarknessStalagtite(Vector2 pos, Vector2 target)
        {
            bool bg = WorldGen.genRand.NextBool(2);
            Vector2 velocity = ((target - pos).ToRotation() + 1.57f).ToRotationVector2();
            float size = 3;
            while (size > 0.25)
            {
                for (float x = pos.X - size; x < pos.X + size; x++)
                {
                    for (float y = pos.Y - size; y < pos.Y + size; y++)
                    {
                        if (bg)
                        {
                            WorldGen.PlaceWall((int)x, (int)y, ModContent.WallType<NightRockWall>());
                        }
                        else
                        {
                            WorldGen.PlaceTile((int)x, (int)y, ModContent.TileType<NightRock>());
                        }
                    }
                }
                size *= bg ? 0.97f : 0.95f;
                velocity += Vector2.Normalize(target - pos) / 16;
                pos += velocity;
                velocity.Normalize();
            }
        }

        private static void MiniIsland(int X, int Y, int size, ushort tile)
        {
            for (int x = X - size; x < X + size; x++)
            {
                for (int y = Y - size; y < Y + size; y++)
                {
                    if (y >= Y)
                    {
                        if (x > 0 && x < Main.maxTilesX && y > 0 && y < Main.maxTilesY)
                        {
                            float distance = Vector2.Distance(new Vector2(x, y), new Vector2(X, Y));
                            if (WorldGen.genRand.NextFloat(distance / (size / 5)) < 1 && distance < size)
                            {
                                WorldGen.PlaceTile(x, y, tile, true, true);
                            }
                        }
                    }
                }
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["darknessX"] = darknessX;
            tag["darknessY"] = darknessY;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            darknessX = tag.GetInt("darknessX");
            darknessY = tag.GetInt("darknessY");
        }
    }
}
