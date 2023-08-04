using ChaoticUprising.Common.GlobalProjectiles;
using ChaoticUprising.Content.Projectiles;
using ChaoticUprising.Content.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Weapons.AbyssalChaos
{
    public class RavenousBlaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 30;
            Item.rare = ModContent.RarityType<VeryEarlyChaos>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item11;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 1000;
            Item.knockBack = 5f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.RocketI;
            Item.shootSpeed = 24f;
            Item.useAmmo = AmmoID.Rocket;
            Item.value = Item.sellPrice(0, 30);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float scale = Main.rand.NextFloat(0.75f, 1.25f);
            bool alt = player.altFunctionUse == 2;
            if (alt)
                velocity /= 4;
            type = ModContent.ProjectileType<BlasterFireball>();
            int p = Projectile.NewProjectile(source, position, velocity, type, (int)(damage * scale), knockback, player.whoAmI);
            Main.projectile[p].scale = scale;
            if (alt)
            {
                Main.projectile[p].GetGlobalProjectile<ProjectileFeatures>().friendlyHoming = true;
                Main.projectile[p].GetGlobalProjectile<ProjectileFeatures>().featureSpeed = Item.shootSpeed / 4;
            }
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, -2f);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
