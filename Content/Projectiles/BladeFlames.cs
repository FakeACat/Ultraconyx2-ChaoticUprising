﻿using ChaoticUprising.Content.Buffs;
using ChaoticUprising.Content.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Projectiles
{
    public class BladeFlames : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.timeLeft = 40;
            Projectile.light = 1f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
        }

        Vector2 realPosition;
        float size = 0;

        public override void AI()
        {
            Projectile.tileCollide = size > Projectile.ai[1] / 8;
            if (size < Projectile.ai[1])
            {
                size += Projectile.ai[1] / 120;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 5;
            }
            if (Projectile.localAI[0] == 0f)
            {
                realPosition = Projectile.Center;
                Projectile.localAI[0] = 1f;
            }
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AbyssDust>(), Projectile.velocity.X, Projectile.velocity.Y);
            Main.dust[d].noGravity = true;
            realPosition += Projectile.velocity;
            Projectile.ai[0] += 0.1f / (size / 48);
            Vector2 offset = Projectile.ai[0].ToRotationVector2() * size;
            offset.X /= 2;
            Vector2 targetPos = realPosition + offset;
            Vector2 realVeloicity = Projectile.velocity + (targetPos - Projectile.Center);
            Projectile.rotation = (float)Math.Atan2(realVeloicity.Y, realVeloicity.X) - 1.57f;
            Projectile.position = targetPos - new Vector2(Projectile.width, Projectile.height) / 2;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(ModContent.BuffType<AbyssalFlamesDebuff>(), 180);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AbyssalFlamesDebuff>(), 180);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AbyssDust>(), Projectile.velocity.X, Projectile.velocity.Y);
                Main.dust[d].noGravity = true;
            }
        }

        public override string Texture => "ChaoticUprising/Content/Projectiles/AbyssalFlamesBig";
    }
}
