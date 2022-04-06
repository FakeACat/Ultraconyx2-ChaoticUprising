using ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Consumables
{
    public class AbyssalChaosBossBag : AbstractBossBag
    {
        public override int BossBagNPC => ModContent.NPCType<AbyssalChaos>();

        public override string BossName() => "Abyssal Chaos";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 40;
            Item.height = 32;
        }

        public override void OpenBossBag(Player player)
		{

		}
	}
}
