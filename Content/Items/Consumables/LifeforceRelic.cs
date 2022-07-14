using ChaoticUprising.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChaoticUprising.Content.Items.Consumables
{
    public class LifeforceRelic : ModItem
    {
        public const int Life = 25;
        public const int Max = 20;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Permanently increases maximum life by " + Life);
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LifeFruit);
            Item.rare = ModContent.RarityType<EarlyChaos>();
            Item.width = 42;
            Item.height = 40;
        }

        public override bool CanUseItem(Player player)
        {
            return player.statLifeMax >= 500 && player.GetModPlayer<LifeforceRelicPlayer>().lifeforceRelics < Max;
        }

        public override bool? UseItem(Player player)
        {
            player.statLifeMax2 += Life;
            player.statLife += Life;
            if (Main.myPlayer == player.whoAmI)
                player.HealEffect(Life);
            player.GetModPlayer<LifeforceRelicPlayer>().lifeforceRelics++;
            return true;
        }
    }
    public class LifeforceRelicPlayer : ModPlayer
    {
        public int lifeforceRelics;

        public override void ResetEffects()
        {
            Player.statLifeMax2 += lifeforceRelics * LifeforceRelic.Life;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)ChaoticUprising.PacketType.LifeforceRelicsSync);
            packet.Write((byte)Player.whoAmI);
            packet.Write(lifeforceRelics);
            packet.Send(toWho, fromWho);
        }

        public override void SaveData(TagCompound tag)
        {
            tag["lifeforceRelics"] = lifeforceRelics;
        }

        public override void LoadData(TagCompound tag)
        {
            lifeforceRelics = (int)tag["lifeforceRelics"];
        }
    }
}
