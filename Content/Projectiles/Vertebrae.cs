using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Projectiles
{
    public class Vertebrae : WhipProjectile
    {
        public override Color Colour()
        {
            return Color.Red;
        }

        public override float DamageDecreasePerHit()
        {
            return 0.1f;
        }

        public override float RangeMultiplier()
        {
            return 2.0f;
        }

        public override int Segments()
        {
            return 60;
        }

        public override int Tag()
        {
            return -1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.GetGlobalNPC<VertebraeDamageBonusNPC>().timesHit++;
        }
    }

    public class VertebraeDamageBonusNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int timesHit = 0;

        public const float PERCENT_INCREASE_PER_HIT = 0.5f;

        public override void SetDefaults(NPC npc)
        {
            timesHit = 0;
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.DamageType == DamageClass.Summon || projectile.DamageType == DamageClass.SummonMeleeSpeed || projectile.DamageType == DamageClass.MagicSummonHybrid)
            {
                modifiers.SourceDamage *= 1.0f + PERCENT_INCREASE_PER_HIT / 100.0f * timesHit;
            }
        }
    }
}
