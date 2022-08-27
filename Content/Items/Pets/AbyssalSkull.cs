using ChaoticUprising.Content.Buffs;
using ChaoticUprising.Content.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Pets
{
    public class AbyssalSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Skull");
            Tooltip.SetDefault("Summons a miniature Abyssal Shade");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<MiniShade>(), ModContent.BuffType<MiniShadeBuff>());
            Item.width = 34;
            Item.height = 42;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.value = Item.sellPrice(0, 5);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            return false;
        }
    }
}
