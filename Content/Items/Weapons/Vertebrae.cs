using ChaoticUprising.Content.Rarities;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Weapons
{
    public class Vertebrae : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToWhip(915, 250, 5f, 8f, 20);
            Item.value = Item.sellPrice(0, 30);
            Item.rare = ModContent.RarityType<VeryEarlyChaos>();
        }
    }
}
