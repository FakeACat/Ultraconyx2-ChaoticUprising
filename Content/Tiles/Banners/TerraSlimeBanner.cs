using ChaoticUprising.Content.NPCs;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Tiles.Banners
{
    public class TerraSlimeBanner : AbstractBanner
    {
        public override int ItemType() => ModContent.ItemType<Items.Placeables.Banners.TerraSlimeBanner>();
        public override int NPCType() => ModContent.NPCType<TerraSlime>();
    }
}
