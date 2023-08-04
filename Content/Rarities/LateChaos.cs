using ChaoticUprising.Common.Utils;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Rarities
{
    public class LateChaos : ModRarity
    {
        public override Color RarityColor => CUUtils.FadeBetweenColours(new Color(0, 0, 255), new Color(0, 155, 155));

        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset == -2)
                return ModContent.RarityType<PostFirstChaosBoss>();
            if (offset == -1)
                return ModContent.RarityType<MidChaos>();
            if (offset > 0)
                return ModContent.RarityType<Ultraconyx>();
            return Type;
        }
    }
}
