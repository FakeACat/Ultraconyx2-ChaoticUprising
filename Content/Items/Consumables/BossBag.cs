using ChaoticUprising.Common.Utils;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Consumables
{
    public abstract class BossBag : ModItem
    {
		public abstract int BossID();
        public abstract bool PreHardmode();
		public abstract int Mask();
        public abstract int ExpertItem();
		public abstract IItemDropRule[] ExtraDrops();

        public override LocalizedText DisplayName => LangUtils.GetFormattedReusable("TreasureBag.DisplayName", LangUtils.GetNPCName(BossID()));
        public override LocalizedText Tooltip => LangUtils.GetReusable("TreasureBag.Tooltip");

        public override void SetStaticDefaults()
		{
			ItemID.Sets.BossBag[Type] = true;
			Item.ResearchUnlockCount = 3;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = PreHardmode();
		}

		public override void SetDefaults()
        {
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Purple;
			Item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(Mask(), 7));
			foreach (IItemDropRule rule in ExtraDrops())
			{
				itemLoot.Add(rule);
			}
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(BossID()));
        }
    }
}
