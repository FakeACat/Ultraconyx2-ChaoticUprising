using ChaoticUprising.Content.NPCs;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Tiles.Banners
{
    public class ImbuedEaterBanner : AbstractBanner
    {
        public override int ItemType() => ModContent.ItemType<Items.Placeables.Banners.ImbuedEaterBanner>();

        public override int NPCType() => ModContent.NPCType<ImbuedEater>();
    }
}
