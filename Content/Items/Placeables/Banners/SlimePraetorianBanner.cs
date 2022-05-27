using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Placeables.Banners
{
    public class SlimePraetorianBanner : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Nearby players get a bonus against: Slime Praetorian");
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.SlimePraetorianBanner>(), 0);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 99;
            Item.width = 12;
            Item.height = 28;
        }
    }
}
