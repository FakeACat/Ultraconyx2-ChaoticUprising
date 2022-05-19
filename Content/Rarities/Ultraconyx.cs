using ChaoticUprising.Common;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Rarities
{
    public class Ultraconyx : ModRarity
    {
        public override Color RarityColor => CUUtils.FadeBetweenColours(Color.White, Color.Black);

        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset == -2)
                return ModContent.RarityType<MidChaos>();
            if (offset == -1)
                return ModContent.RarityType<LateChaos>();
            return Type;
        }
    }
}
