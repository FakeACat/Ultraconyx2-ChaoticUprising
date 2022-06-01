using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ChaoticUprising.Content.Tiles
{
    public class NightRock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            ItemDrop = ModContent.ItemType<Items.Placeables.NightRock>();
            HitSound = SoundID.Tink;
            MineResist = 3;
            AddMapEntry(new Color(100, 100, 120));
            DustType = DustID.Stone;
        }
    }
}
