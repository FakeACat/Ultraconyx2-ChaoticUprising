using ChaoticUprising.Content.Tiles.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Placeables.Ores
{
    public class TelekineOreItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TelekineOre>(), 0);
            Item.maxStack = 9999;
            Item.width = 20;
            Item.height = 16;
            Item.value = Item.sellPrice(0, 0, 12);
            Item.rare = ItemRarityID.Orange;
        }
    }
}
