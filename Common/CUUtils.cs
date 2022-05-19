using Microsoft.Xna.Framework;
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

        public static NPC FindClosestNPC(Vector2 position, float maxDetectDistance)
        {
            NPC closestNPC = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                if (target.CanBeChasedBy())
                {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, position);
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }
            return closestNPC;
        }

        public static Color FadeBetweenColours(Color colour1, Color colour2)
        {
            colour1 *= Main.DiscoR / 255f;
            colour2 *= 1f - Main.DiscoR / 255f;
            return new Color(colour1.R + colour2.R, colour1.G + colour2.G, colour1.B + colour2.B);
        }
    }
}
