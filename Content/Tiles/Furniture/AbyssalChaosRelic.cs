using System;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Tiles.Furniture
{
    internal class AbyssalChaosRelic : AbstractRelic
    {
        public override int ItemType()
        {
            return ModContent.ItemType<Items.Placeables.AbyssalChaosRelic>(); ;
        }

        public override string RelicTextureName()
        {
            return "ChaoticUprising/Content/Tiles/Furniture/AbyssalChaosRelic";
        }
    }
}
