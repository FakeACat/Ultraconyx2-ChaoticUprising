using ChaoticUprising.Content.Items.Consumables;
using ChaoticUprising.Content.Items.Weapons;
using ChaoticUprising.Content.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace ChaoticUprising.Common.Systems
{
    public class ChaosMode : ModSystem
    {
        public static ModKeybind UIToggleKeybind { get; private set; }

        private UserInterface difficultyIndicatorInterface;
        internal DifficultyIndicator difficultyIndicator;

        public static bool chaosMode = false;
        public static float difficulty = 0.0f;
        public const float MAXIMUM_DIFFICULTY = 4.0f;

        public override void Load()
        {
            UIToggleKeybind = KeybindLoader.RegisterKeybind(Mod, "Chaos Mode Difficulty UI", "C");
            if (!Main.dedServ)
            {
                difficultyIndicator = new DifficultyIndicator();
                difficultyIndicator.Activate();
                difficultyIndicatorInterface = new UserInterface();
                difficultyIndicatorInterface.SetState(difficultyIndicator);
            }
        }

        public static Difficulty GetDifficulty()
        {
            switch (difficulty)
            {
                case <= 1.0f:
                    return Difficulty.Easy;
                case <= 2.0f:
                    return Difficulty.Medium;
                case <= 3.0f:
                    return Difficulty.Hard;
                case <= 4.0f:
                    return Difficulty.Chaotic;
                case <= 5.0f:
                    return Difficulty.Darkened;
                case <= 6.0f:
                    return Difficulty.Infested;
                case <= 7.0f:
                    return Difficulty.Abyssal;
            }

            return Difficulty.Easy;
        }

        public static float NormalSpawnMultiplier()
        {
            switch (GetDifficulty())
            {
                case Difficulty.Easy:
                    return 0.5f;
                case Difficulty.Medium:
                    return 1.0f;
                case Difficulty.Hard:
                    return 2.0f;
                default:
                    return 4.0f;

            }
        }

        public static float EliteSpawnMultiplier()
        {
            switch (GetDifficulty())
            {
                case Difficulty.Easy:
                    return 0.0625f;
                case Difficulty.Medium:
                    return 0.25f;
                case Difficulty.Hard:
                    return 1.0f;
                default:
                    return 4.0f;

            }
        }

        public override void PostUpdatePlayers()
        {
            if (chaosMode)
            {
                int n = 0;
                float totalDif = 0;
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player != null && player.active && !player.DeadOrGhost)
                    {
                        n++;
                        totalDif += playerDif(player);
                    }
                }
                if (n > 0) 
                    difficulty = Math.Clamp(difficulty + (totalDif / n), 0.0f, MAXIMUM_DIFFICULTY);
            }
        }

        private float playerDif(Player player)
        {
            if (player.townNPCs >= 1)
                return 0;
            float val = 0.00003f;
            if (player.ZoneCorrupt || player.ZoneCrimson || player.ZoneUnderworldHeight)
                val += 0.000015f;
            return val;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (chaosMode && Main.LocalPlayer.GetModPlayer<ChaosModePlayer>().showUI) 
                difficultyIndicatorInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (chaosMode && Main.LocalPlayer.GetModPlayer<ChaosModePlayer>().showUI)
            {
                int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
                if (mouseTextIndex != -1)
                {
                    layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                        "ChaoticUprising: Difficulty Indicator",
                        delegate
                        {
                            difficultyIndicatorInterface.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }

        public static void GenerateChaosMode()
        {
            GenerateFleshChests();
        }

        private static void GenerateFleshChests()
        {
            for (int i = 0; i < Main.maxTilesX * Main.maxTilesY / 300; i++)
            {
                int x = WorldGen.genRand.Next(10, Main.maxTilesX - 9);
                int y = WorldGen.genRand.Next(10, Main.maxTilesY - 9);
                int c = WorldGen.PlaceChest(x, y, 21, false, 43);
                if (c != -1)
                {
                    int item = 0;
                    switch (WorldGen.genRand.Next(5))
                    {
                        case 0:
                            item = ModContent.ItemType<BladeofFlesh>();
                            break;
                        case 1:
                            item = ModContent.ItemType<RavenousBlaster>();
                            break;
                        case 2:
                            item = ModContent.ItemType<InfernalBlade>();
                            break;
                        case 3:
                            item = ModContent.ItemType<BloodlustWand>();
                            break;
                        case 4:
                            item = ModContent.ItemType<Vertebrae>();
                            break;
                    }
                    Main.chest[c].item[0].SetDefaults(item);

                    Main.chest[c].item[1].SetDefaults(ModContent.ItemType<LifeforceRelic>());

                    if (WorldGen.genRand.NextBool())
                    {
                        Main.chest[c].item[2].SetDefaults(ItemID.LunarBar);
                        Main.chest[c].item[2].stack = WorldGen.genRand.Next(8, 17);
                    }
                    else
                    {
                        int item2 = WorldGen.genRand.Next(ItemID.FragmentVortex, ItemID.FragmentVortex + 4);

                        Main.chest[c].item[2].SetDefaults(item2);
                        Main.chest[c].item[2].stack = WorldGen.genRand.Next(16, 33);
                    }
                }
            }
        }

        public override void OnWorldLoad()
        {
            chaosMode = false;
        }

        public override void OnWorldUnload()
        {
            chaosMode = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (chaosMode)
            {
                tag["chaosMode"] = true;
                tag["chaosDifficulty"] = difficulty;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            chaosMode = tag.ContainsKey("chaosMode");
            if (tag.ContainsKey("chaosDifficulty"))
                difficulty = tag.GetFloat("chaosDifficulty");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = chaosMode;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            chaosMode = flags[0];
        }

        public override void Unload()
        {
            UIToggleKeybind = null;
        }
    }

    public class ChaosModePlayer : ModPlayer
    {
        public bool showUI = true;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (ChaosMode.UIToggleKeybind.JustPressed)
                showUI = !showUI;
        }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Chaotic,
        Darkened,
        Infested,
        Abyssal
    }
}
