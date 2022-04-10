using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Rarities
{
    public class MidChaos : ModRarity
    {
        public override Color RarityColor => new(Main.DiscoR, 255, 255);

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
