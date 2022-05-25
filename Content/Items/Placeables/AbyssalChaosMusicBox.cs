using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Placeables
{
    public class AbyssalChaosMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (Abyssal Chaos)");
            Tooltip.SetDefault("Hellish Intent by ENNWAY");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/Hellish Intent"), ModContent.ItemType<AbyssalChaosMusicBox>(), ModContent.TileType<Tiles.Furniture.AbyssalChaosMusicBox>());
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.AbyssalChaosMusicBox>(), 0);
            Item.width = 24;
            Item.height = 26;
            Item.rare = ItemRarityID.Orange;
            Item.value = 110000;
            Item.accessory = true;
            Item.maxStack = 1;
        }
    }
}
