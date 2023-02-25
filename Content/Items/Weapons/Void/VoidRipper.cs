using ChaoticUprising.Content.Items.Abstract;
using ChaoticUprising.Content.Items.Materials;
using ChaoticUprising.Content.Projectiles;
using ChaoticUprising.Content.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Weapons.Void
{
    public class VoidRipper : DarknessItem
    {
        public const float ALT_USE_TIME_MULTIPLIER = 1.5f;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right-click to redirect dark energy");

            ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
            ItemID.Sets.Spears[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ModContent.RarityType<EarlyChaos>();
            Item.value = Item.sellPrice(0, 45);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.damage = 500;
            Item.knockBack = 3.0f;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.shootSpeed = 10.0f;
            Item.width = 70;
            Item.height = 70;
            Item.shoot = ModContent.ProjectileType<VoidRipperProjectile>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1 && player.ownedProjectileCounts[ModContent.ProjectileType<VoidRipperAltProjectile>()] < 1;
        }

        public override bool? UseItem(Player player)
        {
            if (!Main.dedServ && Item.UseSound.HasValue)
            {
                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
            }

            return null;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                int p = Projectile.NewProjectile(source, position + velocity, velocity, ModContent.ProjectileType<VoidRipperAltProjectile>(), damage, knockback, player.whoAmI);
                Main.projectile[p].ai[0] = 1;
                Main.projectile[p].timeLeft = (int)(player.itemAnimationMax * ALT_USE_TIME_MULTIPLIER);
                return false;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    Projectile.NewProjectile(source, position, velocity + new Vector2(Main.rand.Next(-10, 11) / 2, Main.rand.Next(-10, 11) / 2), ModContent.ProjectileType<DarkMatterEnergyBallFriendly>(), damage, knockback, player.whoAmI);
                }
                return true;
            }
            
        }

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient(ModContent.ItemType<NightmareChitin>(), 18)
            .AddIngredient(ModContent.ItemType<TelekineBar>(), 12)
            .AddIngredient(ItemID.LunarBar, 8)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}
