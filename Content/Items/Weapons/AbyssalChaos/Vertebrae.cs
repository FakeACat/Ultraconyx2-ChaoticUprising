using ChaoticUprising.Content.Projectiles;
using ChaoticUprising.Content.Rarities;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Weapons.AbyssalChaos
{
    public class Vertebrae : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Your summons will focus struck enemies\nEach hit permanently increases the damage dealt by minions and whips to the struck enemy by " + VertebraeDamageBonusNPC.PERCENT_INCREASE_PER_HIT + "%");
        }
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.Vertebrae>(), 170, 5f, 5f, 60);
            Item.value = Item.sellPrice(0, 30);
            Item.rare = ModContent.RarityType<VeryEarlyChaos>();
        }
    }
}
