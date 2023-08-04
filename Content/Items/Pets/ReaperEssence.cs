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
    public class ReaperEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 5));
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<BabyNightmareReaper>(), ModContent.BuffType<NightmareReaperBuff>());
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
