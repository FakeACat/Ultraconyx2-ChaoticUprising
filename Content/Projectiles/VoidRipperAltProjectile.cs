using ChaoticUprising.Content.Items.Weapons.Void;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Projectiles
{
    public class VoidRipperAltProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 136;
            Projectile.height = 136;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.position = player.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
            int duration = (int)(player.itemAnimationMax * VoidRipper.ALT_USE_TIME_MULTIPLIER);
            float progress = Projectile.timeLeft / (float)duration;
            Projectile.rotation = MathHelper.TwoPi * progress * progress + MathHelper.PiOver4;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<DarkMatterEnergyBallFriendly>() && Main.projectile[i].active && Main.projectile[i].ai[0] == 0)
                {
                    Main.projectile[i].ai[0] = 1;
                    Main.projectile[i].ai[1] = 8;
                    Main.projectile[i].penetrate = -1;
                    Main.projectile[i].timeLeft = 300;
                    Main.projectile[i].usesLocalNPCImmunity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 tip = Projectile.Center + new Vector2(-60, 60).RotatedBy(Projectile.rotation + MathHelper.PiOver2);
            Texture2D texture = ModContent.Request<Texture2D>("ChaoticUprising/Assets/Textures/BlankTexture").Value;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<DarkMatterEnergyBallFriendly>() && Main.projectile[i].active && Main.projectile[i].ai[0] == 0)
                {
                    Projectile p = Main.projectile[i];
                    Main.spriteBatch.Draw(texture, new Rectangle((int)tip.X - (int)Main.screenPosition.X, (int)tip.Y - (int)Main.screenPosition.Y, (int)p.Distance(tip), 1), new Rectangle(0, 0, 16, 16), Color.Cyan * 0.5f, (p.Center - tip).ToRotation(), Vector2.Zero, SpriteEffects.None, 0);
                }
            }
            return true;
        }

        public override string Texture => "ChaoticUprising/Content/Projectiles/VoidRipperProjectile";
    }
}
