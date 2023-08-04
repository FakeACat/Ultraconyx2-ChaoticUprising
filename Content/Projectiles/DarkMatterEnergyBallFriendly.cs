using Microsoft.Xna.Framework;
using Terraria;

namespace ChaoticUprising.Content.Projectiles
{
    public class DarkMatterEnergyBallFriendly : DarkMatterEnergyBall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Dark Matter Energy Ball");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.friendly = true;
            Projectile.hostile = false;
        }
        public override void AI()
        {
            base.AI();
            if (Projectile.ai[0] == 1)
            {
                Player player = Main.player[Projectile.owner];
                if (player != null)
                {
                    Projectile.ai[1] += 0.01f;
                    Projectile.velocity = Vector2.Normalize(player.Center - Projectile.Center) * Projectile.ai[1];
                    if (Projectile.DistanceSQ(player.Center) > 1024)
                    {
                        Projectile.timeLeft = 2;
                    }
                }
            }
        }
        public override string Texture => "ChaoticUprising/Content/Projectiles/DarkMatterEnergyBall";
    }
}
