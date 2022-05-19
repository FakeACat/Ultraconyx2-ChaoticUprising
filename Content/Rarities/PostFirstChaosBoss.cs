using ChaoticUprising.Common;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Rarities
{
    public class PostFirstChaosBoss : ModRarity
    {
        public override Color RarityColor => CUUtils.FadeBetweenColours(new Color(255, 255, 0), new Color(105, 0, 0));

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
