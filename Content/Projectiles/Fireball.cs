using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using ChaoticUprising.Common.GlobalProjectiles;

namespace ChaoticUprising.Content.Projectiles
{
    public class Fireball : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
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
            if (Projectile.ai[0] == 1)
            {
                Projectile.velocity.Y += 0.05f;
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

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 600);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float scale = Projectile.scale;
            float alpha = 0.4f;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/Projectiles/Fireball");
            for (int i = 0; i < 30; i++)
            {
                scale *= 1.05f;
                alpha *= 0.9f;
                Vector2 pos = prevPos[i] - Main.screenPosition;
                Main.EntitySpriteDraw(texture, pos, null, Color.White * alpha, prevRot[i], new Vector2(texture.Width, texture.Height) / 2, scale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
