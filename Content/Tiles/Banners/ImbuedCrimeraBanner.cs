using ChaoticUprising.Content.NPCs;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Tiles.Banners
{
    public class ImbuedCrimeraBanner : AbstractBanner
    {
        public override int ItemType() => ModContent.ItemType<Items.Placeables.Banners.ImbuedCrimeraBanner>();

        public override int NPCType() => ModContent.NPCType<ImbuedCrimera>();
    }
}
