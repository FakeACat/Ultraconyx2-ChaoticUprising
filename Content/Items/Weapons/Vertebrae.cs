using ChaoticUprising.Content.Rarities;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Weapons
{
    public class Vertebrae : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.Vertebrae>(), 250, 5f, 8f);
            Item.value = Item.sellPrice(0, 30);
            Item.rare = ModContent.RarityType<VeryEarlyChaos>();
        }
    }
}
