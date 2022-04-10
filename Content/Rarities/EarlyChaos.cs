using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Rarities
{
    public class EarlyChaos : ModRarity
    {
        public override Color RarityColor => new(Main.DiscoR, 255 - Main.DiscoR, 155);

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
