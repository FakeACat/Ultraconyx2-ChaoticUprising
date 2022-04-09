using Terraria;

namespace ChaoticUprising.Common
{
    public class CUUtils
    {
        public static int ConvenientBossHealth(int normalHealth, int expertHealth)
        {
            if (Main.masterMode)
            {
                return (int)(expertHealth * 0.425f);
            }
            if (Main.expertMode)
            {
                return expertHealth / 2;
            }
            return normalHealth;
        }

        public static int ConvenientBossDamage(int normalDamage, int expertDamage, bool projectile)
        {
            int divideBy = projectile ? 2 : 1;
            if (Main.masterMode)
            {
                divideBy *= 3;
                return (int)(expertDamage * 1.5f) / divideBy;
            }
            if (Main.expertMode)
            {
                divideBy *= 2;
                return expertDamage / divideBy;
            }
            return normalDamage / divideBy;
        }
    }
}
