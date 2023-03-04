using ChaoticUprising.Content.NPCs.Darkness;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Tiles.Banners
{
    public class NightmareToadBanner : AbstractBanner
    {
        public override int ItemType() => ModContent.ItemType<Items.Placeables.Banners.NightmareToadBanner>();

        public override int NPCType() => ModContent.NPCType<NightmareToad>();
    }
}
