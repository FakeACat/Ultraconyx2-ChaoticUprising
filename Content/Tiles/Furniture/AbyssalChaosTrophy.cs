using Terraria.ModLoader;

namespace ChaoticUprising.Content.Tiles.Furniture
{
    public class AbyssalChaosTrophy : AbstractTrophy
    {
        public override int Drop()/* tModPorter Note: Removed. Use CanDrop to decide if an item should drop. Use GetItemDrops to decide which item drops. Item drops based on placeStyle are handled automatically now, so this method might be able to be removed altogether. */
        {
            return ModContent.ItemType<Items.Placeables.AbyssalChaosTrophy>();
        }
    }
}
