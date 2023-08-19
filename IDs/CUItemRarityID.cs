using ChaoticUprising.Content.Rarities;
using Terraria.ModLoader;

namespace ChaoticUprising.IDs
{
    public static class CUItemRarityID
    {
        // Items that can be obtained as soon as you enter Chaos Mode
        public static readonly int VeryEarlyChaos = ModContent.RarityType<VeryEarlyChaos>();

        // Items that can be obtained after a little progression in Chaos Mode
        public static readonly int EarlyChaos = ModContent.RarityType<EarlyChaos>();

        // Items that can be obtained after defeating a boss in Chaos Mode
        public static readonly int PostFirstChaosBoss = ModContent.RarityType<PostFirstChaosBoss>();

        public static readonly int MidChaos = ModContent.RarityType<MidChaos>();
        public static readonly int LateChaos = ModContent.RarityType<LateChaos>();
        public static readonly int Ultraconyx = ModContent.RarityType<Ultraconyx>();
    }
}
