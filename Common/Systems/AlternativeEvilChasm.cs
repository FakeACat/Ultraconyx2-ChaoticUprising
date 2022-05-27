using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ChaoticUprising.Common.Systems
{
    public class AlternativeEvilChasm : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int CleanupIndex = tasks.FindIndex(genPass => genPass.Name.Equals("Final Cleanup"));
            tasks.Insert(CleanupIndex + 1, new PassLegacy("Chaotic Uprising Chasm", Blessing));
        }

        private void Blessing(GenerationProgress progress, GameConfiguration _) // I wonder if anyone will get this reference
        {
            progress.Message = "Making the world eviler";

            var spikes = new List<Vector2>();
            ushort tile;
            ushort ore;
            ushort wall;
            int tileDensity = -1;
            int maxTileDensity = 100;
            int X = Main.dungeonX > Main.maxTilesX / 2 ? Main.maxTilesX / 2 + 400 : Main.maxTilesX / 2 - 400;
            if (WorldGen.crimson)
            {
                tile = TileID.Ebonstone;
                ore = TileID.Demonite;
                wall = WallID.EbonstoneUnsafe;
            }
            else
            {
                tile = TileID.Crimstone;
                ore = TileID.Crimtane;
                wall = WallID.CrimstoneUnsafe;
            }
            int width = 32;
            int width3 = width;
            for (int y = (int)Main.worldSurface - 400 > 0 ? (int)Main.worldSurface - 400 : 0; y < Main.maxTilesY - 150; y++)
            {
                if (!WorldGen.TileEmpty(X, y))
                    tileDensity++;
                if (tileDensity > maxTileDensity)
                    tileDensity = maxTileDensity;
                if (WorldGen.genRand.NextBool(2))
                    width3++;
                else
                    width3--;
                if (width3 < width / 2)
                    width3 = width / 2;
                else if (width3 > width * 1.5f)
                    width3 = (int)(width * 1.5f);
                int width2 = (int)(width3 * 0.75f);

                for (int x = X - width3; x <= X + width3; x++)
                {
                    if (((x == X - width3 || x == X + width3) && WorldGen.genRand.NextBool(2)) || (x > X - width3 && x < X + width3))
                    {
                        if (tileDensity > -1 && WorldGen.genRand.Next(maxTileDensity) <= tileDensity && (y < Main.maxTilesY - 200 || y == Main.maxTilesY - 200 && WorldGen.genRand.NextBool(2)))
                        {
                            WorldGen.PlaceTile(x, y, WorldGen.genRand.NextBool(120)? ore : tile, false, true);
                            Main.tile[x, y].WallType = wall;
                            if (WorldGen.genRand.NextBool(128)&& Math.Abs(X - x) > width2)
                            {
                                spikes.Add(new Vector2(x, y));
                            }
                        }
                    }
                    if ((((x == X - width2 || x == X + width2) && WorldGen.genRand.NextBool(2)) || (x > X - width2 && x < X + width2)) && (y <= Main.maxTilesY - 200 || Main.tile[x, y].TileType == TileID.Ash || Main.tile[x, y].TileType == TileID.Stone))
                    {
                        WorldGen.KillTile(x, y, false, false, false);
                    }
                }
            }

            foreach (Vector2 spike in spikes) {
                WorldGen.TileRunner((int)spike.X, (int)spike.Y, 7, 60, tile, true);
            }
        }
    }
}
