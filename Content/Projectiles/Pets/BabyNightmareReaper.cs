using ChaoticUprising.Content.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using ReLogic.Content;
using ChaoticUprising.Common.Utils;

namespace ChaoticUprising.Content.Projectiles.Pets
{
    public class BabyNightmareReaper : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Baby Nightmare Reaper");
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = 42;
            Projectile.height = 44;
            Projectile.aiStyle = -1;
            Projectile.scale = 0.75f;
        }

        static int segmentCount = 20;
        Vector2[] segmentPos = new Vector2[segmentCount];
        float[] segmentRot = new float[segmentCount];

        public override void OnSpawn(IEntitySource source)
        {
            segmentPos = new Vector2[segmentCount];
            segmentRot = new float[segmentCount];
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 8;
            }
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<NightmareReaperBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            Projectile.velocity = Projectile.rotation.ToRotationVector2() * 10;
            if (player.DistanceSQ(Projectile.Center) > 65536)
            {
                Projectile.rotation = Projectile.rotation.AngleTowards(CUUtils.AngleTo(player.Center, Projectile.Center), MathHelper.Pi / 120);
            }
        }

        private static Asset<Texture2D> Head;
        private static Asset<Texture2D> HeadGlow;
        private static Asset<Texture2D> Body;
        private static Asset<Texture2D> BodyGlow;
        private static Asset<Texture2D> Tail;
        private static Asset<Texture2D> TailGlow;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                Head = ModContent.Request<Texture2D>(Texture);
                HeadGlow = ModContent.Request<Texture2D>(Texture + "Glow");
                Body = ModContent.Request<Texture2D>(Texture + "Body");
                BodyGlow = ModContent.Request<Texture2D>(Texture + "BodyGlow");
                Tail = ModContent.Request<Texture2D>(Texture + "Tail");
                TailGlow = ModContent.Request<Texture2D>(Texture + "TailGlow");
            }
        }

        public override void Unload()
        {
            Head = null;
            HeadGlow = null;
            Body = null;
            BodyGlow = null;
            Tail = null;
            TailGlow = null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = Head.Value;
            Texture2D glowmask = HeadGlow.Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.velocity.ToRotation() + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(glowmask, Projectile.Center - Main.screenPosition, null, new Color(Main.DiscoR, Main.DiscoR, Main.DiscoR), Projectile.velocity.ToRotation() + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            
            for (int i = 0; i < segmentCount; i++)
            {
                if (i == segmentCount - 1)
                {
                    texture = Tail.Value;
                    glowmask = TailGlow.Value;
                }
                else
                {
                    texture = Body.Value;
                    glowmask = BodyGlow.Value;
                }
                Vector2 previousSegment;
                float previousRot;
                if (!Main.gamePaused)
                {
                    if (i != 0)
                    {
                        previousSegment = segmentPos[i - 1];
                        previousRot = segmentRot[i - 1];
                    }
                    else
                    {
                        previousSegment = Projectile.Center;
                        previousRot = Projectile.velocity.ToRotation();
                    }
                    int gap = 23;
                    gap = (int)(gap * Projectile.scale);
                    segmentPos[i] += Vector2.Normalize(previousSegment - previousRot.ToRotationVector2() * gap * 2 - segmentPos[i]);
                    segmentPos[i] = -(Vector2.Normalize(previousSegment - segmentPos[i]) * gap) + previousSegment;
                    segmentRot[i] = (previousSegment - segmentPos[i]).ToRotation();
                }
                Main.EntitySpriteDraw(texture, segmentPos[i] - Main.screenPosition, null, lightColor, segmentRot[i] + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(glowmask, segmentPos[i] - Main.screenPosition, null, new Color(Main.DiscoR - (i + 1) * 4, Main.DiscoR - (i + 1) * 4, Main.DiscoR - (i + 1) * 4), segmentRot[i] + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            }
        }
    }
}
