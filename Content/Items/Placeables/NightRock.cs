using Terraria.ModLoader;
using Terraria.ID;

namespace ChaoticUprising.Content.Items.Placeables
{
    public class NightRock : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.NightRock>(), 0);

            Item.width = 16;
            Item.height = 16;
        }
    }
}
