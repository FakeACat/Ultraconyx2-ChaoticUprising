using ChaoticUprising.Content.Items.Accessories;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Buffs
{
    public class MiracleDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MiraclePlayer>().miracle = true;
        }
    }
}
