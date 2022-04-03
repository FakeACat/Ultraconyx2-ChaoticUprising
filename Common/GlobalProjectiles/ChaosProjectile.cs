using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Common.GlobalProjectiles
{
    public class ChaosProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool shouldBeBuffedInChaosMode = true;

        public override void SetDefaults(Projectile projectile)
        {
            shouldBeBuffedInChaosMode = true;
        }
    }
}
