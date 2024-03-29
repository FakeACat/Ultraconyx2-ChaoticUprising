﻿using ChaoticUprising.Common.GlobalProjectiles;
using ChaoticUprising.Content.Buffs;
using ChaoticUprising.Content.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Projectiles
{
    public class WandFlames : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 300;
            Projectile.light = 1f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.GetGlobalProjectile<ProjectileFeatures>().featureSpeed = 8f;
        }

        public override void AI()
        {
            Projectile.GetGlobalProjectile<ProjectileFeatures>().friendlyHoming = Projectile.timeLeft < 270;
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AbyssDust>(), Projectile.velocity.X, Projectile.velocity.Y, 0, default, 1);
            Main.dust[d].noGravity = true;
            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(ModContent.BuffType<AbyssalFlamesDebuff>(), 120);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AbyssalFlamesDebuff>(), 120);
        }

        public override string Texture => "ChaoticUprising/Content/Projectiles/AbyssalFlames";
    }
}
