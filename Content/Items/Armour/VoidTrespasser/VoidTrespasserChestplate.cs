using ChaoticUprising.Content.Items.Abstract;
using ChaoticUprising.Content.Items.Materials;
using ChaoticUprising.IDs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Armour.VoidTrespasser
{
    [AutoloadEquip(EquipType.Body)]
    public class VoidTrespasserChestplate : DarknessItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 40);
            Item.rare = CUItemRarityID.EarlyChaos;
            Item.defense = 37;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.14f;
            player.GetCritChance(DamageClass.Generic) += 0.09f;
            player.endurance += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<NightmareChitin>(35)
                .AddIngredient<TelekineBar>(15)
                .AddIngredient(ItemID.LunarBar, 18)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
