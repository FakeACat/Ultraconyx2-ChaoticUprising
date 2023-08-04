using ChaoticUprising.Content.Items.Accessories;
using ChaoticUprising.Content.Items.Vanity;
using ChaoticUprising.Content.Items.Weapons.AbyssalChaos;
using ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Consumables
{
    public class AbyssalChaosBossBag : BossBag
    {
        public override int BossID()
        {
            return ModContent.NPCType<AbyssalChaos>();
        }

        public override int ExpertItem()
        {
            return ModContent.ItemType<MiracleMind>();
        }

        public override IItemDropRule[] ExtraDrops() => new IItemDropRule[]
        {
            ItemDropRule.Common(ModContent.ItemType<RavenousBlaster>(), 3),
            ItemDropRule.Common(ModContent.ItemType<InfernalBlade>(), 3),
            ItemDropRule.Common(ModContent.ItemType<BloodlustWand>(), 3),
            ItemDropRule.Common(ModContent.ItemType<Vertebrae>(), 3)
        };

        public override int Mask()
        {
            return ModContent.ItemType<AbyssalChaosMask>();
        }

        public override bool PreHardmode()
        {
            return false;
        }
    }
}
