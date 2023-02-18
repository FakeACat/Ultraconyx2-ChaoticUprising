using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChaoticUprising.Content.Projectiles
{
    public class TerraTentacle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.light = 1f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }
        bool setCurve = false;
        public override void AI()
        {
            if (!setCurve)
            {
                setCurve = true;
                Projectile.ai[0] = Main.rand.Next(-20, 21) / 1500f;
                Projectile.ai[1] = Main.rand.Next(-20, 21) / 1500f;
            }
            Vector2 center10 = Projectile.Center;
            Projectile.scale = 1f - Projectile.localAI[0];
            Projectile.width = (int)(20f * Projectile.scale);
            Projectile.height = Projectile.width;
            Projectile.position.X = center10.X - Projectile.width / 2;
            Projectile.position.Y = center10.Y - Projectile.height / 2;
            Projectile.localAI[0] += 0.01f;
            if (Projectile.localAI[0] >= 0.95f)
            {
                Projectile.Kill();
            }
            Projectile.velocity.X = Projectile.velocity.X + Projectile.ai[0] * 1.5f;
            Projectile.velocity.Y = Projectile.velocity.Y + Projectile.ai[1] * 1.5f;
            if (Projectile.velocity.Length() > 16f)
            {
                Projectile.velocity.Normalize();
                Projectile.velocity *= 16f;
            }
            Projectile.ai[0] *= 1.04f;
            Projectile.ai[1] *= 1.04f;
            if (Projectile.scale < 1f)
            {
                int num891 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.TerraBlade, Projectile.velocity.X, Projectile.velocity.Y);
                Main.dust[num891].position = (Main.dust[num891].position + Projectile.Center) / 2f;
                Main.dust[num891].noGravity = true;
                Dust dust3 = Main.dust[num891];
                dust3.velocity *= 0.1f;
                dust3 = Main.dust[num891];
                dust3.velocity -= Projectile.velocity * (1.3f - Projectile.scale);
                Main.dust[num891].fadeIn = 100 + Projectile.owner;
                dust3 = Main.dust[num891];
                dust3.scale = 0.5f + Projectile.scale / 2.0f;
            }
        }
        public override string Texture
        {
            get
            {
                return "ChaoticUprising/Assets/Textures/BlankTexture";
            }
        }
    }
}
