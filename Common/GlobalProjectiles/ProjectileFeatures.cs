using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Common.GlobalProjectiles
{
    public class ProjectileFeatures : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool homing = false;
        public float featureSpeed = 0.0f;

        public override void AI(Projectile projectile)
        {
            if (homing) 
                projectile.velocity = Vector2.Normalize(Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)].Center - projectile.Center) * featureSpeed;
        }
    }
}
