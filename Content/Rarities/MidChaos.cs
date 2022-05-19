using ChaoticUprising.Common;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Rarities
{
    public class MidChaos : ModRarity
    {
        public override Color RarityColor => CUUtils.FadeBetweenColours(new Color(255, 0, 0), new Color(0, 255, 0));

        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset == -2)
                return ModContent.RarityType<EarlyChaos>();
            if (offset == -1)
                return ModContent.RarityType<PostFirstChaosBoss>();
            if (offset == 1)
                return ModContent.RarityType<LateChaos>();
            if (offset == 2)
                return ModContent.RarityType<Ultraconyx>();
            return Type;
        }
    }
}
