using Terraria;

namespace ChaoticUprising.Common
{
    public class CUUtils
    {
        public static int ConvenientBossHealthScaling(int normalHealth, int expertHealth)
        {
            if (Main.masterMode)
            {
                return (int)(expertHealth * 1.275f) / 3;
            }
            if (Main.expertMode)
            {
                return expertHealth / 2;
            }
            return normalHealth;
        }

        public static int ConvenientBossDamageScaling(int normalDamage, int expertDamage)
        {
            if (Main.masterMode)
            {
                return (int)(expertDamage * 1.5f) / 3;
            }
            if (Main.expertMode)
            {
                return expertDamage / 2;
            }
            return normalDamage;
        }
    }
}
