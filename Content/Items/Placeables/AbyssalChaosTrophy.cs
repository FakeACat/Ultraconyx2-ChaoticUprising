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

            Item.width = 48;
            Item.height = 48;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Blue;
            Item.value = Terraria.Item.buyPrice(0, 1);
        }
    }
}
