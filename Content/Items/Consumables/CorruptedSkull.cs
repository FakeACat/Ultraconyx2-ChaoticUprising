using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos;
using Terraria.GameContent.Creative;

namespace ChaoticUprising.Content.Items.Consumables
{
    public class CorruptedSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 30;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Purple;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.value = Item.buyPrice(0, 20);
        }

        public override bool? UseItem(Player player)
        {
            Main.NewText("Music: Hellish Intent by ENNWAY", 0, 0, 255);
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                int type = ModContent.NPCType<AbyssalChaos>();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
            }

            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (!Main.hardMode)
                return false;
            if (NPC.AnyNPCs(ModContent.NPCType<AbyssalChaos>()))
                return false;

            return true;
        }

        public override void AddRecipes() => CreateRecipe()
            .AddIngredient(ItemID.SoulofNight, 45)
            .AddIngredient(ItemID.Vertebrae, 15)
            .AddIngredient(ItemID.Ichor, 30)
            .AddIngredient(ItemID.LunarBar, 15)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}
