using Terraria.ModLoader;

namespace ChaoticUprising.Content.Tiles.Furniture
{
    public class AbyssalChaosTrophy : AbstractTrophy
    {
        public override int Drop()
        {
            return ModContent.ItemType<Items.Placeables.AbyssalChaosTrophy>();
        }
    }
}
