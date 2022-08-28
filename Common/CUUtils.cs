using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace ChaoticUprising.Common
{
    public class CUUtils
    {
        public static int ConvenientBossHealth(int normalHealth, int expertHealth)
        {
            if (Main.masterMode)
            {
                return (int)(expertHealth * 0.425f);
            }
            if (Main.expertMode)
            {
                return expertHealth / 2;
            }

            return normalHealth;
        }

        public static int ConvenientBossDamage(int normalDamage, int expertDamage, bool projectile)
        {
            int divideBy = projectile ? 2 : 1;
            if (Main.masterMode)
            {
                divideBy *= 3;
                return (int)(expertDamage * 1.5f) / divideBy;
            }
            if (Main.expertMode)
            {
                divideBy *= 2;
                return expertDamage / divideBy;
            }
            return normalDamage / divideBy;
        }

        public static NPC FindClosestNPC(Vector2 position, float maxDetectDistance)
        {
            NPC closestNPC = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                if (target.CanBeChasedBy())
                {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, position);
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }
            return closestNPC;
        }

        public static Color FadeBetweenColours(Color colour1, Color colour2)
        {
            colour1 *= Main.DiscoR / 255f;
            colour2 *= 1f - Main.DiscoR / 255f;
            return new Color(colour1.R + colour2.R, colour1.G + colour2.G, colour1.B + colour2.B);
        }

        public static void DrawSpecialWorm(SpriteBatch spriteBatch, 
            NPC npc, 
            Texture2D head, 
            Texture2D body, 
            Texture2D tail, 
            Color drawColor,
            int segmentCount,
            Vector2[] segmentPos,
            float[] segmentRot,
            Texture2D specialSecond = null,
            Texture2D glowmaskHead = null,
            Texture2D glowmaskBody = null,
            Texture2D glowmaskTail = null,
            Texture2D glowmaskSpecialSecond = null)
        {
            Texture2D texture = head;
            Texture2D glowmask = glowmaskHead;
            spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(texture.Width / 2, texture.Height / 2), npc.scale, SpriteEffects.None, 0);
            if (glowmask != null)
            {
                spriteBatch.Draw(glowmask, npc.Center - Main.screenPosition, null, Color.White, npc.rotation, new Vector2(glowmask.Width / 2, glowmask.Height / 2), npc.scale, SpriteEffects.None, 0);
            }
            for (int i = 0; i < segmentCount; i++)
            {
                if (i == segmentCount - 1)
                {
                    texture = tail;
                    glowmask = glowmaskTail;
                }
                else if (specialSecond != null && i == 0)
                {
                    texture = specialSecond;
                    glowmask = glowmaskSpecialSecond;
                }
                else
                {
                    texture = body;
                    glowmask = glowmaskBody;
                }
                spriteBatch.Draw(texture, segmentPos[i] - Main.screenPosition, null, Lighting.GetColor((int)segmentPos[i].X / 16, (int)segmentPos[i].Y / 16), segmentRot[i] + 1.57f, new Vector2(texture.Width / 2, texture.Height / 2), npc.scale, SpriteEffects.None, 0);
                if (glowmask != null)
                {
                    spriteBatch.Draw(glowmask, segmentPos[i] - Main.screenPosition, null, Color.White, segmentRot[i] + 1.57f, new Vector2(glowmask.Width / 2, glowmask.Height / 2), npc.scale, SpriteEffects.None, 0);
                }
            }
        }

        public static void UpdateSpecialWormSegments(NPC npc,
            int gap1,
            int gap2,
            int gap3,
            int speed,
            int segmentCount,
            Vector2[] segmentPos,
            float[] segmentRot,
            float headRot)
        {
            for (int i = 0; i < segmentCount; i++)
            {
                Vector2 previousSegment;
                float previousRot;
                if (i != 0)
                {
                    previousSegment = segmentPos[i - 1];
                    previousRot = segmentRot[i - 1];
                }
                else
                {
                    previousSegment = npc.Center;
                    previousRot = headRot;
                }

                int gap;
                if (i == 0)
                    gap = gap1;
                else if (i == segmentCount - 1)
                    gap = gap3;
                else
                    gap = gap2;
                if (i != 0 && i != segmentCount - 1)
                {
                    gap = gap2;
                }
                gap = (int)(gap * npc.scale);

                segmentPos[i] += Vector2.Normalize(previousSegment - previousRot.ToRotationVector2() * gap * 2 - segmentPos[i]) * speed;
                segmentPos[i] = -(Vector2.Normalize(previousSegment - segmentPos[i]) * gap) + previousSegment;
                segmentRot[i] = (previousSegment - segmentPos[i]).ToRotation();
            }
        }

        public static bool InvalidTarget(int target)
        {
            Player target2 = Main.player[target];
            return target < 0 || target >= 255 || target2.dead || !target2.active;
        }

        public static float AngleTo(Vector2 destination, Vector2 centre)
        {
            return (float)Math.Atan2(destination.Y - centre.Y, destination.X - centre.X);
        }
    }
}
