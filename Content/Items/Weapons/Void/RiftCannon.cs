using ChaoticUprising.Content.Items.Abstract;
using ChaoticUprising.Content.Items.Materials;
using ChaoticUprising.Content.Projectiles;
using ChaoticUprising.Content.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Weapons.Void
{
    public class RiftCannon : DarknessItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Launches a spread of void harpoons\nRight-click to launch a black hole\nBlack holes stick to enemies for 6 seconds, pulling in all magic projectiles");
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 20;
            Item.rare = ModContent.RarityType<EarlyChaos>();
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.mana = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item11;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 420;
            Item.knockBack = 3.0f;
            Item.noMelee = true;
            Item.shootSpeed = 10.0f;
            Item.shoot = ModContent.ProjectileType<VoidHarpoonFriendly>();
            Item.value = Item.sellPrice(0, 45);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 1);
                return false;
            }
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(source, position, velocity + new Vector2(Main.rand.Next(-50, 51), Main.rand.Next(-50, 51)) / 10.0f, type, damage, knockback, player.whoAmI);
            }
            return true;
        }

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient(ModContent.ItemType<NightmareChitin>(), 18)
            .AddIngredient(ModContent.ItemType<TelekineBar>(), 12)
            .AddIngredient(ItemID.LunarBar, 8)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}
