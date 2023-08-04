using ChaoticUprising.Common;
using ChaoticUprising.Common.Systems;
using ChaoticUprising.Content.Tiles.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Tiles
{
    public class Monolith : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSolid[Type] = false;
            HitSound = SoundID.Tink;
            MineResist = 10;
            MinPick = 225;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(17, 30, 57), name);
            DustType = DustID.Obsidian;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!ChaosMode.chaosMode)
                fail = true;
            else if (!fail)
            {
                ChaosMode.brokenMonolithBlocks++;
                /*if (ChaosMode.brokenMonolithBlocks % 30 == 0)
                {
                    int multiplier = ChaosMode.brokenMonolithBlocks / 30;
                    CUUtils.Ore(ModContent.TileType<TelekineOre>(), 8, 240000 * multiplier);
                    CUUtils.Text("Your world has been blessed with Telekine!", new Color(15, 149, 183));
                }*/
            }
        }
    }
}
