using ChaoticUprising.Content.Items.Abstract;
using ChaoticUprising.Content.Items.Materials;
using ChaoticUprising.IDs;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Armour.VoidTrespasser
{
    [AutoloadEquip(EquipType.Head)]
    public class VoidTrespasserHelmet : DarknessItem
    {
        public static LocalizedText SetBonusText { get; private set; }
        public override void SetStaticDefaults()
        {
            SetBonusText = this.GetLocalization("SetBonus");
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = CUItemRarityID.EarlyChaos;
            Item.defense = 20;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == CUItemID.VoidTrespasserChestplate && legs.type == CUItemID.VoidTrespasserGreaves;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = SetBonusText.Value;

        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.14f;
            player.GetCritChance(DamageClass.Generic) += 0.12f;
            player.nightVision = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<NightmareChitin>(20)
                .AddIngredient<TelekineBar>(15)
                .AddIngredient(ItemID.LunarBar, 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    internal class VoidTrespasserPlayer : ModPlayer
    {

    }
}
