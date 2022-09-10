using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using ChaoticUprising.Content.Rarities;
using ChaoticUprising.Content.Projectiles;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace ChaoticUprising.Content.Items.Weapons
{
    public class BladeofFlesh : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade of Flesh");
            Tooltip.SetDefault("Ichor tentacles briefly reduce their target's defense and dramatically increase the amount of damage it takes from all attacks\n'Feels like it's alive...'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 80;
            Item.width = 70;
            Item.damage = 300;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 45);
            Item.rare = ModContent.RarityType<EarlyChaos>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<IchorTentacle>();
        }
    }
}
