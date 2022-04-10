using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Rarities
{
    public class Ultraconyx : ModRarity
    {
        public override Color RarityColor => new(255, 255, Main.DiscoR);

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
