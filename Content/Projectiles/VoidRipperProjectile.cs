namespace ChaoticUprising.Content.Projectiles
{
    public class VoidRipperProjectile : AbstractSpearProjectile
    {
        protected override float HoldoutRangeMax => 240.0f;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Void Ripper");
        }
    }
}
