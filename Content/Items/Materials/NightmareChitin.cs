using ChaoticUprising.Content.Items.Abstract;
using ChaoticUprising.Content.Rarities;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Materials
{
    public class NightmareChitin : DarknessItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.rare = ModContent.RarityType<EarlyChaos>();
            Item.value = Item.sellPrice(0, 0, 75);
            Item.maxStack = 9999;
        }
    }
}
