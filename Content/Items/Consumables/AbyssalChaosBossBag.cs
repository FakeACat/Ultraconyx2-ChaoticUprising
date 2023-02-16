using ChaoticUprising.Content.Items.Accessories;
using ChaoticUprising.Content.Items.Vanity;
using ChaoticUprising.Content.Items.Weapons.AbyssalChaos;
using ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Consumables
{
    public class AbyssalChaosBossBag : AbstractBossBag
    {
        public override int BossBagNPC => ModContent.NPCType<AbyssalChaos>();

        public override string BossName() => "Abyssal Chaos";

        public override void OpenBossBag(Player player)
        {
            var source = player.GetSource_OpenItem(Type);
            if (Main.rand.NextBool(7))
                player.QuickSpawnItem(source, ModContent.ItemType<AbyssalChaosMask>());

            if (Main.rand.NextBool(3))
                player.QuickSpawnItem(source, ModContent.ItemType<RavenousBlaster>());
            if (Main.rand.NextBool(3))
                player.QuickSpawnItem(source, ModContent.ItemType<InfernalBlade>());
            if (Main.rand.NextBool(3))
                player.QuickSpawnItem(source, ModContent.ItemType<BloodlustWand>());
            if (Main.rand.NextBool(3))
                player.QuickSpawnItem(source, ModContent.ItemType<Vertebrae>());

            player.QuickSpawnItem(source, ModContent.ItemType<MiracleMind>());
        }
	}
}
