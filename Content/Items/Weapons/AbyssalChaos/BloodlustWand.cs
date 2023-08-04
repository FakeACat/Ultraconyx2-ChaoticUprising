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
    public class BloodlustWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.DamageType = DamageClass.Magic;
            Item.width = 64;
            Item.height = 56;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 30);
            Item.rare = ModContent.RarityType<VeryEarlyChaos>();
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<WandFlames>();
            Item.shootSpeed = 13f;
            Item.crit = 6;
            Item.mana = 35;
            Item.staff[Type] = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    Projectile.NewProjectile(source, new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, ModContent.ProjectileType<AbyssalPortal>(), damage, knockback, player.whoAmI);
                }
                return false;
            }

            float angleBetweenProjectiles = MathHelper.Pi / 12;
            int numProjectiles = 5;
            for (int i = 0; i < numProjectiles; i++)
            {
                Vector2 v = velocity.RotatedBy(angleBetweenProjectiles * (i - numProjectiles / 2));
                int p = Projectile.NewProjectile(source, position, v, type, damage, knockback, player.whoAmI);
                Main.projectile[p].GetGlobalProjectile<ProjectileFeatures>().featureSpeed = Item.shootSpeed;
            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
