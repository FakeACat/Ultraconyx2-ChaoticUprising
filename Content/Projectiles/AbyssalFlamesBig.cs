using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using ChaoticUprising.Content.Dusts;
using ChaoticUprising.Common.GlobalProjectiles;
using ChaoticUprising.Content.Buffs;

namespace ChaoticUprising.Content.Projectiles
{
    public class AbyssalFlamesBig : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Flames");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.light = 1f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.5f;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.GetGlobalProjectile<ChaosProjectile>().shouldBeBuffedInChaosMode = false;
        }
        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            }
            Projectile.rotation += 0.3f * Projectile.direction;
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AbyssDust>(), Projectile.velocity.X, Projectile.velocity.Y, 0, default, 2);
            Main.dust[d].noGravity = true;
        }


        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<AbyssalFlamesDebuff>(), 180);
        }
        public override string Texture
        {
            get
            {
                return "ChaoticUprising/Content/Projectiles/AbyssalFlames";
            }
        }
    }
}
