using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Rarities
{
    public class VeryEarlyChaos : ModRarity
    {
        public override Color RarityColor => new(Main.DiscoR, 0, 255 - Main.DiscoR);

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
