using ChaoticUprising.Common.Utils;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Rarities
{
    public class EarlyChaos : ModRarity
    {
        public override Color RarityColor => CUUtils.FadeBetweenColours(new Color(255, 0, 0), new Color(0, 0, 255));

        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset == -2)
                return 11;
            if (offset == -1)
                return ModContent.RarityType<VeryEarlyChaos>();
            if (offset == 1)
                return ModContent.RarityType<PostFirstChaosBoss>();
            if (offset == 2)
                return ModContent.RarityType<MidChaos>();
            return Type;
        }
    }
}
