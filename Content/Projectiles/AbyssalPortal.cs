using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Projectiles
{
    public class AbyssalPortal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Flames");
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
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, (i * MathHelper.TwoPi / numProj).ToRotationVector2() * 8f, ModContent.ProjectileType<WandFlames>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }
}
