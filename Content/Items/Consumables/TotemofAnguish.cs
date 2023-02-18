using ChaoticUprising.Common.Systems;
using ChaoticUprising.Content.Rarities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Consumables
{
    public class TotemofAnguish : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases the difficulty");
            DisplayName.SetDefault("Totem of Anguish");
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 42;
            Item.maxStack = 9999;
            Item.rare = ModContent.RarityType<EarlyChaos>();
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override bool? UseItem(Player player)
        {
            ChaosMode.difficulty = Math.Clamp(ChaosMode.difficulty + 0.5f, 0.0f, ChaosMode.MAXIMUM_DIFFICULTY);
            return true;
        }
    }
}
