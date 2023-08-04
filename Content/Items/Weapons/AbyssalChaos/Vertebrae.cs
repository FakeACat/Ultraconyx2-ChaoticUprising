using ChaoticUprising.Content.Projectiles;
using ChaoticUprising.Content.Rarities;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Weapons.AbyssalChaos
{
    public class Vertebrae : ModItem
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(VertebraeDamageBonusNPC.PERCENT_INCREASE_PER_HIT);
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.Vertebrae>(), 170, 5f, 5f, 60);
            Item.value = Item.sellPrice(0, 30);
            Item.rare = ModContent.RarityType<VeryEarlyChaos>();
        }
    }
}
