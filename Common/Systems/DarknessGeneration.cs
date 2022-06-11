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

        public int darknessX;
        public int darknessY;

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int CleanupIndex = tasks.FindIndex(genPass => genPass.Name.Equals("Final Cleanup"));
            tasks.Insert(CleanupIndex + 1, new PassLegacy("Chaotic Uprising Darkness Biome", Darkness));
        }

        private void Darkness(GenerationProgress progress, GameConfiguration _)
        {
            progress.Message = "Making the world darker";

            int x;
            if (Main.dungeonX < Main.maxTilesX / 2)
            {
                x = Main.maxTilesX - Main.dungeonX;
            }
            else
            {
                x = -(Main.dungeonX - Main.maxTilesX);
            }
            darknessX = x;
            darknessY = 200;

            for (int X = darknessX - 300; X < darknessX + 300; X++)
            {
                for (int Y = darknessY - 300; Y < darknessY + 300; Y++)
                {
                    int dist = (int)Vector2.DistanceSquared(new Vector2(X, Y), new Vector2(darknessX, darknessY));
                    if (dist < 90000 && dist > 14400 && Y < Main.worldSurface)
                        if (WorldGen.genRand.NextBool(dist / 30))
                            GenDarknessStalagtite(new Vector2(X, Y), new Vector2(darknessX, darknessY));

                    if (dist < 14400 && dist > 10000 && WorldGen.genRand.NextBool(300))
                    {
                        GenDarknessStalagtite(new Vector2(X, Y), new Vector2(darknessX, darknessY), true);
                    }
                }
            }
        }

        private void GenDarknessStalagtite(Vector2 pos, Vector2 target, bool b = false)
        {
            bool bg = b || WorldGen.genRand.NextBool(2);
            Vector2 velocity = ((target - pos).ToRotation() + 1.57f).ToRotationVector2();
            float size = WorldGen.genRand.Next(3, 7);
            while (size > 0.25)
            {
                for (float x = pos.X - size; x < pos.X + size; x++)
                {
                    for (float y = pos.Y - size; y < pos.Y + size; y++)
                    {
                        if (WorldGen.genRand.NextBool(1 + (int)size))
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
                }
                size *= bg ? 0.98f : 0.97f;
                velocity += Vector2.Normalize(target - pos) / (pos.DistanceSQ(new Vector2(darknessX, darknessY)) / 450);
                pos += velocity;
                velocity.Normalize();
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
