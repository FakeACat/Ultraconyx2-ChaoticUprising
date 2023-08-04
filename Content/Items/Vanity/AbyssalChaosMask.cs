using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChaoticUprising.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class AbyssalChaosMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 28;
            Item.value = Terraria.Item.sellPrice(0, 0, 75);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
            Item.maxStack = 1;
        }
    }
}
