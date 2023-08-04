using ChaoticUprising.Common.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Common.GlobalProjectiles
{
    public class ProjectileFeatures : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool hostileHoming = false;
        public bool friendlyHoming = false;
        public float featureSpeed = 0.0f;

        public override void AI(Projectile projectile)
        {
            if (hostileHoming) 
                projectile.velocity = Vector2.Normalize(Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)].Center - projectile.Center) * featureSpeed;
            if (friendlyHoming)
            {
                NPC nearest = CUUtils.FindClosestNPC(projectile.Center, 640);
                if (nearest != null)
                    projectile.velocity = Vector2.Normalize(nearest.Center - projectile.Center) * featureSpeed;
            }
        }
    }
}
