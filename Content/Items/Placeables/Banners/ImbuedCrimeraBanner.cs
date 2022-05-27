using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Placeables.Banners
{
    public class ImbuedCrimeraBanner : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Nearby players get a bonus against: Imbued Crimera");
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.ImbuedCrimeraBanner>(), 0);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 99;
            Item.width = 12;
            Item.height = 28;
        }
    }
}
