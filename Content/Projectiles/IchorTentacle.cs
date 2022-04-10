using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChaoticUprising.Content.Projectiles
{
    public class IchorTentacle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0)
                Projectile.ai[1] = Projectile.timeLeft;
            Projectile.ai[0]++;
            if (Projectile.ai[0] > Projectile.ai[1])
                Projectile.Kill();
            if (Main.myPlayer == Projectile.owner)
            {
                float r = Projectile.ai[0] / Projectile.ai[1] * MathHelper.Pi;
                Vector2 player = Main.player[Main.myPlayer].Center;
                Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) - new Vector2(Main.screenWidth, Main.screenHeight) / 2;
                Projectile.position = player + (mouse * (float)Math.Sin(r)) + (mouse.RotatedBy(Math.PI / 2) * (float)Math.Sin(2 * r)) * 0.2f;
            }

            for (int num114 = 0; num114 < 3; num114++)
            {
                float num115 = Projectile.velocity.X / 3f * num114;
                float num116 = Projectile.velocity.Y / 3f * num114;
                int num117 = 14;
                int num118 = Dust.NewDust(new Vector2(Projectile.position.X + num117, Projectile.position.Y + num117), Projectile.width - num117 * 2, Projectile.height - num117 * 2, DustID.Ichor, 0f, 0f, 100);
                Main.dust[num118].noGravity = true;
                Dust dust2 = Main.dust[num118];
                dust2.velocity *= 0.1f;
                dust2 = Main.dust[num118];
                dust2.velocity += Projectile.velocity * 0.5f;
                Main.dust[num118].position.X -= num115;
                Main.dust[num118].position.Y -= num116;
            }
            if (Main.rand.Next(8) == 0)
            {
                int num119 = 16;
                int num120 = Dust.NewDust(new Vector2(Projectile.position.X + num119, Projectile.position.Y + num119), Projectile.width - num119 * 2, Projectile.height - num119 * 2, DustID.Ichor, 0f, 0f, 100, default, 0.5f);
                Dust dust2 = Main.dust[num120];
                dust2.velocity *= 0.25f;
                dust2 = Main.dust[num120];
                dust2.velocity += Projectile.velocity * 0.5f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Ichor, 300);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Ichor, 300);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/Projectiles/IchorTentacleSegment");
            Texture2D tipTexture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/Projectiles/IchorTentacle");
            float a = (255f - Projectile.alpha) / 255f;
            int l = texture.Height;
            Vector2 connectorOrigin = Main.player[Projectile.owner].Center;
            Vector2 connectorTarget = Projectile.Center;
            bool lineComplete = false;
            bool first = true;
            while (!lineComplete)
            {
                Vector2 nextPosition = connectorTarget + Vector2.Normalize(Vector2.Normalize(connectorOrigin - connectorTarget) * l - new Vector2(0, l * 0.5f)) * l;
                float rotation = Vector2.Normalize(nextPosition - connectorTarget).ToRotation() - (float)(Math.PI / 2);
                Main.spriteBatch.Draw(first ? tipTexture : texture, connectorTarget - Main.screenPosition, null, Color.White * a, rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                connectorTarget = nextPosition;
                first = false;
                if (Vector2.Distance(connectorTarget, connectorOrigin) <= 16)
                    lineComplete = true;
            }
            return false;
        }
    }
}
