using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Rarities
{
    public class PostFirstChaosBoss : ModRarity
    {
        public override Color RarityColor => new(255, Main.DiscoR, 255 - Main.DiscoR);

        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset == -2)
                return ModContent.RarityType<VeryEarlyChaos>();
            if (offset == -1)
                return ModContent.RarityType<EarlyChaos>();
            if (offset == 1)
                return ModContent.RarityType<MidChaos>();
            if (offset == 2)
                return ModContent.RarityType<LateChaos>();
            return Type;
        }
    }
}
