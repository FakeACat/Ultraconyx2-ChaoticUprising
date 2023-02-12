using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ChaoticUprising.Content.Items.Placeables.Ores;

namespace ChaoticUprising.Content.Items.Materials
{
    public class TelekineBar : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 50);
            Item.maxStack = 9999;
        }

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient(ModContent.ItemType<TelekineOreItem>(), 4)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}
