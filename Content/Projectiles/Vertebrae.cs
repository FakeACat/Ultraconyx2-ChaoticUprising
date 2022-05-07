using Microsoft.Xna.Framework;

namespace ChaoticUprising.Content.Projectiles
{
    public class Vertebrae : AbstractWhipProjectile
    {
        public override Color Colour()
        {
            return Color.Red;
        }

        public override float DamageDecreasePerHit()
        {
            return 0.1f;
        }

        public override float RangeMultiplier()
        {
            return 2.0f;
        }

        public override int Segments()
        {
            return 60;
        }

        public override int Tag()
        {
            return -1;
        }
    }
}
