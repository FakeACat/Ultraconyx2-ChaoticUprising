using ChaoticUprising.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Placeables.Banners
{
    public abstract class Banner : ModItem
    {
        public override LocalizedText DisplayName => LangUtils.GetFormattedReusable("Banner.DisplayName", LangUtils.GetNPCName(NPCID()));
        public override LocalizedText Tooltip => LangUtils.GetFormattedReusable("Banner.Tooltip", LangUtils.GetNPCName(NPCID()));
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(TileID(), 0);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = Item.CommonMaxStack;
            Item.width = 12;
            Item.height = 28;
        }

        public abstract int TileID();
        public abstract int NPCID();
    }
}
