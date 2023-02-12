using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Placeables
{
    public class AbyssalChaosTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.AbyssalChaosTrophy>());

            Item.width = 32;
            Item.height = 34;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
            Item.value = Terraria.Item.buyPrice(0, 1);
        }
    }
}
