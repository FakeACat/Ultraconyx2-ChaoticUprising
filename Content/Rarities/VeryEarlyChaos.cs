using ChaoticUprising.Common.Utils;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Rarities
{
    public class VeryEarlyChaos : ModRarity
    {
        public override Color RarityColor => CUUtils.FadeBetweenColours(new Color(0, 255, 0), Color.Orange);

        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset == -2)
                return 10;
            if (offset == -1)
                return 11;
            if (offset == 1)
                return ModContent.RarityType<EarlyChaos>();
            if (offset == 2)
                return ModContent.RarityType<PostFirstChaosBoss>();
            return Type;
        }
    }
}
