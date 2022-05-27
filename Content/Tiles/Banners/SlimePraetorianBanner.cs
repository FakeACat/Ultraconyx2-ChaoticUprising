using ChaoticUprising.Content.NPCs;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Tiles.Banners
{
    public class SlimePraetorianBanner : AbstractBanner
    {
        public override int ItemType() => ModContent.ItemType<Items.Placeables.Banners.SlimePraetorianBanner>();

        public override int NPCType() => ModContent.NPCType<SlimePraetorian>();
    }
}
