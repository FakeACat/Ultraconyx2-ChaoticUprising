using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Projectiles
{
    public class AbyssalPortal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Abyssal Flames");
            Main.projFrames[Type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 66;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.timeLeft = 300;
            Projectile.light = 3f;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 6;
            }

            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha = (int)(255f * (1f - (Projectile.timeLeft / 60f)));
            }

            Projectile.ai[0]++;
            if (Main.netMode != NetmodeID.MultiplayerClient && Projectile.ai[0] > 120)
            {
                Projectile.ai[0] = 0;
                int numProj = 7;
                for (int i = 0; i < numProj; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (i * MathHelper.TwoPi / numProj).ToRotationVector2() * 8f, ModContent.ProjectileType<WandFlames>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Rectangle frame = new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height);

            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new Vector2(Projectile.width / 2 - frameOrigin.X, Projectile.height - frame.Height);
            Vector2 drawPos = Projectile.position - Main.screenPosition + frameOrigin + offset;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = Projectile.timeLeft / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;

            float realAlpha = 1 - (Projectile.alpha / 255f);

            for (float i = 0f; i < 1f; i += 0.25f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                Main.EntitySpriteDraw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(90, 70, 255) * 0.2f * realAlpha, Projectile.rotation, frameOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                Main.EntitySpriteDraw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(140, 120, 255, 77) * 0.25f * realAlpha, Projectile.rotation, frameOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
