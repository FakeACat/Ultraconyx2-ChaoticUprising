using ChaoticUprising.Content.Rarities;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Materials
{
    public class Terragel : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.rare = ModContent.RarityType<VeryEarlyChaos>();
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 25);
        }
    }
}
