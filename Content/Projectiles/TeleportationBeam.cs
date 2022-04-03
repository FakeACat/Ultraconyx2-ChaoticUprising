using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ChaoticUprising.Projectiles
{
    public class TeleportationBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.timeLeft = 1000;
            Projectile.light = 1f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            int aa = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color));
            Main.dust[aa].noGravity = true;
            Projectile.damage = 0;
            Projectile.velocity.X *= 0.99f;
            Projectile.velocity.Y += 0.025f;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 100; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Main.rand.Next(-400, 401) / 10, Main.rand.Next(-400, 401) / 10, 0, default(Color));
                Main.dust[d].noGravity = true;
                int d2 = Dust.NewDust(Main.npc[(int)Projectile.ai[1]].position, Main.npc[(int)Projectile.ai[1]].width, Main.npc[(int)Projectile.ai[1]].height, DustID.Electric, Main.rand.Next(-400, 401) / 10, Main.rand.Next(-400, 401) / 10, 0, default(Color));
                Main.dust[d2].noGravity = true;
            }
            Main.npc[(int)Projectile.ai[1]].position = new Vector2(Projectile.position.X - (Main.npc[(int)Projectile.ai[1]].width / 2), Projectile.position.Y - (Main.npc[(int)Projectile.ai[1]].height / 2));
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
