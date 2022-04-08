using Terraria.ModLoader;
using Terraria;
using ChaoticUprising.Content.Dusts;
using ChaoticUprising.Common.GlobalProjectiles;
using ChaoticUprising.Content.Buffs;

namespace ChaoticUprising.Content.Projectiles
{
    public class AbyssalFlames : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 28;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.light = 1f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.GetGlobalProjectile<ChaosProjectile>().shouldBeBuffedInChaosMode = false;
        }
        public override void AI()
        {
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AbyssDust>(), Projectile.velocity.X, Projectile.velocity.Y, 0, default, 1);
            Main.dust[d].noGravity = true;
            Projectile.rotation += 0.3f * Projectile.direction;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<AbyssalFlamesDebuff>(), 120);
        }
    }
}
