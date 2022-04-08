using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using ChaoticUprising.Content.Dusts;
using ChaoticUprising.Common.GlobalProjectiles;
using ChaoticUprising.Content.Buffs;
using System;

namespace ChaoticUprising.Content.Projectiles
{
    public class AbyssalFlamesBig : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Flames");
            Main.projFrames[Type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 80;
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
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 5;
            }
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - 1.57f;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            }
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AbyssDust>(), Projectile.velocity.X, Projectile.velocity.Y);
            Main.dust[d].noGravity = true;
        }


        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<AbyssalFlamesDebuff>(), 180);
        }
    }
}
