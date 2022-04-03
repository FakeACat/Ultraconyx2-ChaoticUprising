using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos;

namespace ChaoticUprising.Content.Items.Summons
{
    public class CorruptedSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons the Abyssal Chaos\nRequires the world to be in Hardmode\nNot consumable");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 38;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Purple;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<AbyssalChaos>());
            Main.NewText("Music: Hellish Intent by ENNWAY", 0, 0, 255);
            SoundEngine.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
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
            .AddIngredient(ItemID.SoulofFlight, 3)
            .AddIngredient(ItemID.SoulofFright, 3)
            .AddIngredient(ItemID.SoulofLight, 3)
            .AddIngredient(ItemID.SoulofMight, 30)
            .AddIngredient(ItemID.SoulofNight, 30)
            .AddIngredient(ItemID.SoulofSight, 3)
            .AddIngredient(ItemID.Vertebrae, 10)
            .AddIngredient(ItemID.Ichor, 30)
            .AddIngredient(ItemID.DemoniteBar, 20)
            .AddIngredient(ItemID.DarkShard, 2)
            .AddIngredient(ItemID.LivingFireBlock, 40)
            .AddIngredient(ItemID.LunarBar, 5)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}
