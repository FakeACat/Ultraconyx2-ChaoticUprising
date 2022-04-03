using Terraria.ModLoader;
using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ChaoticUprising.Common.GlobalProjectiles;
using Terraria.Audio;

namespace ChaoticUprising.Content.Projectiles
{
    public class LightRay : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.penetrate = 3;
            Projectile.light = 0.75f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 2;
            Projectile.scale = 1.8f;
            Projectile.timeLeft = 400;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.GetGlobalProjectile<ChaosProjectile>().shouldBeBuffedInChaosMode = false;
        }
        Vector2 spawnPos;
        Vector2[] prevPos = new Vector2[30];
        float[] prevRot = new float[30];
        float num1 = 0;
        float num2 = 120;
        public override void AI()
        {
            if (Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item33, Projectile.position);
                spawnPos = Projectile.Center;
            }
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            prevPos[0] = Projectile.Center;
            prevRot[0] = Projectile.rotation;
            for (int i = 29; i > 0; i--)
            {
                prevPos[i] = prevPos[i - 1];
                prevRot[i] = prevRot[i - 1];
            }
            Projectile.velocity *= 1.01f;
            if (Projectile.ai[0] == 0)
                num1++;
            else
                num1--;
            if (num1 > num2)
                Projectile.ai[0] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (num1 > 0)
            {
                int length = 200;
                for (int segment = 0; segment < length; segment++)
                {
                    float a = 0.4f;
                    a *= num1 / num2;
                    a *= segment < 25 ? (float)segment / 25 : 1f;
                    Vector2 offset = (Projectile.rotation + 1.57f).ToRotationVector2() * segment * 16;
                    Vector2 drawPos = spawnPos - offset - Main.screenPosition;
                    Main.EntitySpriteDraw((Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Assets/Textures/BlankTexture"), drawPos, new Rectangle(0, 0, 4, 16), new Color(75, 100, 0) * a, Projectile.rotation, new Vector2(2, 8), 1, SpriteEffects.None, 0);
                }
            }

            float scale = Projectile.scale;
            float alpha = 0.8f;
            for (int i = 0; i < 30; i++)
            {
                scale *= 0.95f;
                alpha *= 0.96f;
                Vector2 pos = prevPos[i] - Main.screenPosition;
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/Projectiles/LightRay");
                Main.EntitySpriteDraw(texture, pos, null, Color.White * alpha, prevRot[i], new Vector2(texture.Width, texture.Height) / 2, scale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
