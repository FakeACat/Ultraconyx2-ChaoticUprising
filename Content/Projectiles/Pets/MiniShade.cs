using ChaoticUprising.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Projectiles.Pets
{
    public class MiniShade : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Miniature Abyssal Shade");
            Main.projFrames[Type] = 8;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = 30;
            Projectile.height = 42;
            Projectile.aiStyle = -1;
        }
        int age = 0;
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 8;
            }
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<MiniShadeBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;

            int distancesq = (int)player.DistanceSQ(Projectile.Center);

            if (distancesq > 4096)
            {
                Projectile.velocity += Vector2.Normalize(player.Center - Projectile.Center) / 3;
                Projectile.velocity *= 0.975f;
            }
            age++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.player[Projectile.owner].statLife < Main.player[Projectile.owner].statLifeMax2 / 2)
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/Projectiles/Pets/MiniShade");

                Vector2 frameOrigin = new Vector2(Projectile.width, Projectile.height) * 0.5f;
                Vector2 offset = new Vector2(Projectile.width / 2 - frameOrigin.X, Projectile.height - Projectile.height);
                Vector2 drawPos = Projectile.position - Main.screenPosition + frameOrigin + offset;

                float time = Main.GlobalTimeWrappedHourly;
                float timer = age / 240f + time * 0.04f;

                time %= 4f;
                time /= 2f;

                if (time >= 1f)
                {
                    time = 2f - time;
                }

                time = time * 0.5f + 0.5f;

                float scale = Projectile.scale * 1.1f;

                for (float i = 0f; i < 1f; i += 0.25f)
                {
                    float radians = (i + timer) * MathHelper.TwoPi;

                    Main.spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), new Color(90, 70, 255, 50) * 0.5f, Projectile.rotation, frameOrigin, scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }

                for (float i = 0f; i < 1f; i += 0.34f)
                {
                    float radians = (i + timer) * MathHelper.TwoPi;

                    Main.spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), new Color(140, 120, 255, 77) * 0.5f, Projectile.rotation, frameOrigin, scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }
            return true;
        }
    }
}
