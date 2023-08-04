using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Projectiles
{
    public class BlasterFireball : ModProjectile
    {
        public override string Texture => "ChaoticUprising/Content/Projectiles/Fireball";

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;
        }
        private Vector2[] prevPos;
        private float[] prevRot;
        public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
        {
            prevPos = new Vector2[30];
            prevRot = new float[30];
        }
        public override void AI()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Lighting.AddLight(Projectile.Center, new Vector3(1, 0.5f, 0) * Projectile.scale);
            }
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            }
            prevPos[0] = Projectile.Center;
            prevRot[0] = Projectile.rotation;
            for (int i = 29; i > 0; i--)
            {
                prevPos[i] = prevPos[i - 1];
                prevRot[i] = prevRot[i - 1];
            }
            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 600);
            Expand();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(BuffID.OnFire, 600);
                Expand();
            }
        }

        void Expand()
        {
            Projectile.timeLeft = 3;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.position = Projectile.Center;
            Projectile.width = 250;
            Projectile.height = 250;
            Projectile.Center = Projectile.position;
        }

        public override void Kill(int timeLeft)
        {
            Expand();
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int num924 = 0; num924 < 50; num924++)
            {
                int num925 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                Dust dust = Main.dust[num925];
                dust.velocity *= 1.4f;
            }
            for (int num926 = 0; num926 < 80; num926++)
            {
                int num927 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                Main.dust[num927].noGravity = true;
                Dust dust = Main.dust[num927];
                dust.velocity *= 5f;
                num927 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                dust = Main.dust[num927];
                dust.velocity *= 3f;
            }
            for (int i = 0; i < 30; i++)
            {
                for (int n = 0; n < 7; n++)
                {
                    Vector2 pos = prevPos[i];
                    int num927 = Dust.NewDust(pos, 0, 0, DustID.Torch, 0f, 0f, 100, default, 5f * Projectile.scale * 0.75f);
                    Main.dust[num927].noGravity = true;
                    Dust dust = Main.dust[num927];
                    dust.velocity *= 25f;
                }
            }
            var source = Projectile.GetSource_FromThis();
            for (int num928 = 0; num928 < 2; num928++)
            {
                int num929 = Gore.NewGore(source, new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + (Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64));
                Main.gore[num929].scale = 1.5f;
                Main.gore[num929].velocity.X += 1.5f;
                Main.gore[num929].velocity.Y += 1.5f;
                num929 = Gore.NewGore(source, new Vector2(Projectile.position.X + (Projectile.width / 2) - 24f, Projectile.position.Y + (Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64));
                Main.gore[num929].scale = 1.5f;
                Main.gore[num929].velocity.X -= 1.5f;
                Main.gore[num929].velocity.Y += 1.5f;
                num929 = Gore.NewGore(source, new Vector2(Projectile.position.X + (Projectile.width / 2) - 24f, Projectile.position.Y + (Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64));
                Main.gore[num929].scale = 1.5f;
                Main.gore[num929].velocity.X += 1.5f;
                Main.gore[num929].velocity.Y -= 1.5f;
                num929 = Gore.NewGore(source, new Vector2(Projectile.position.X + (Projectile.width / 2) - 24f, Projectile.position.Y + (Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64));
                Main.gore[num929].scale = 1.5f;
                Main.gore[num929].velocity.X -= 1.5f;
                Main.gore[num929].velocity.Y -= 1.5f;
            }
            Projectile.position.X += Projectile.width / 2;
            Projectile.position.Y += Projectile.height / 2;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.position.X -= Projectile.width / 2;
            Projectile.position.Y -= Projectile.height / 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float scale = Projectile.scale;
            float alpha = 0.6f;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/Projectiles/Fireball");
            for (int i = 0; i < 30; i++)
            {
                scale *= 1.05f;
                alpha *= 0.9f;
                Vector2 pos = prevPos[i] - Main.screenPosition;
                Main.EntitySpriteDraw(texture, pos, null, new Color(255, 255, 255, 1) * alpha, prevRot[i], new Vector2(texture.Width, texture.Height) / 2, scale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
