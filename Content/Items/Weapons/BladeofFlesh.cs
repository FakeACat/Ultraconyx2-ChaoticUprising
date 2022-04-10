using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using ChaoticUprising.Content.Rarities;
using ChaoticUprising.Content.Projectiles;

namespace ChaoticUprising.Content.Items.Weapons
{
    public class BladeofFlesh : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade of Flesh");
            Tooltip.SetDefault("Feels like it's alive...");
        }

        public override void SetDefaults()
        {
            Item.height = 80;
            Item.width = 70;
            Item.damage = 400;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 30);
            Item.rare = ModContent.RarityType<VeryEarlyChaos>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<IchorTentacle>();
        }
    }
}
