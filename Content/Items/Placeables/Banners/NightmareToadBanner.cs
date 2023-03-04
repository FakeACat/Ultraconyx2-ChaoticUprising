using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Placeables.Banners
{
    public class NightmareToadBanner : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Nearby players get a bonus against: Nightmare Toad");
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.NightmareToadBanner>(), 0);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
            Item.width = 12;
            Item.height = 28;
        }
    }
}
