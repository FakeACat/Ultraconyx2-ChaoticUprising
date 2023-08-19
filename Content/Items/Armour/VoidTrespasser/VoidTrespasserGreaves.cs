using ChaoticUprising.Content.Items.Abstract;
using ChaoticUprising.Content.Items.Materials;
using ChaoticUprising.IDs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Armour.VoidTrespasser
{
    [AutoloadEquip(EquipType.Legs)]
    public class VoidTrespasserGreaves : DarknessItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 25);
            Item.rare = CUItemRarityID.EarlyChaos;
            Item.defense = 29;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.14f;
            player.GetCritChance(DamageClass.Generic) += 0.09f;
            player.runAcceleration *= 1.2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<NightmareChitin>(25)
                .AddIngredient<TelekineBar>(15)
                .AddIngredient(ItemID.LunarBar, 15)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
