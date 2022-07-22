using ChaoticUprising.Content.Projectiles;
using ChaoticUprising.Content.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Weapons
{
    public class InfernalBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Launches abyssal fireballs\nRight-click to launch a slow disc of penetrating abyssal fireballs");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 50;
            Item.width = 50;
            Item.damage = 500;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 30;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 30);
            Item.rare = ModContent.RarityType<VeryEarlyChaos>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<BladeFlames>();
            Item.shootSpeed = 20;
            Item.scale = 1.5f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item20, position);
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < 6; i++)
                {
                    int p = Projectile.NewProjectile(source, position, velocity / 3, type, damage, knockback, player.whoAmI, MathHelper.Pi * 2 * i / 6, 256);
                    Main.projectile[p].timeLeft = 100;
                }
            }
            else
            {
                int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 192);
                Main.projectile[p].penetrate = 1;
                p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, MathHelper.Pi, 192);
                Main.projectile[p].penetrate = 1;
            }

            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
