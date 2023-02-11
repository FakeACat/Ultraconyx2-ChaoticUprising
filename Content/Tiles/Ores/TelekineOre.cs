using ChaoticUprising.Content.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Tiles.Ores
{
    public class TelekineOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            ItemDrop = ModContent.ItemType<TelekineOreItem>();
            HitSound = SoundID.Tink;
            MineResist = 5;
            MinPick = 110;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Telekine");
            AddMapEntry(new Color(15, 149, 183), name);
            DustType = DustID.Electric;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float offset = i + (float)Math.Sin(j / 20.0f + Main.GlobalTimeWrappedHourly / 15.0f) * 20;
            float value = (float)Math.Sin(offset / 2.0f + Main.GlobalTimeWrappedHourly);
            value *= value;

            r = value / 2;
            g = value; 
            b = value;
        }
    }
}
