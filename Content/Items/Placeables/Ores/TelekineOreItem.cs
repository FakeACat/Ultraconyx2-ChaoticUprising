using ChaoticUprising.Content.Rarities;
using ChaoticUprising.Content.Tiles.Ores;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Placeables.Ores
{
    public class TelekineOreItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Telekine Ore");
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TelekineOre>(), 0);

            Item.width = 20;
            Item.height = 16;

            Item.rare = ItemRarityID.Orange;
        }
    }
}
