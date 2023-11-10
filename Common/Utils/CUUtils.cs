using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ChaoticUprising.Common.Utils
{
    public class CUUtils
    {
        public static bool Client => Main.netMode != NetmodeID.Server;
        public static bool Server => Main.netMode == NetmodeID.Server;
        public static bool Singleplayer => Main.netMode == NetmodeID.SinglePlayer;
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

        public static bool TryFindTarget(NPC npc)
        {
            if (InvalidTarget(npc.target))
            {
                npc.TargetClosest(false);
                if (InvalidTarget(npc.target))
                {
                    return false;
                }
            }
            return true;
        }

        public static float AngleTo(Vector2 destination, Vector2 centre)
        {
            return (float)Math.Atan2(destination.Y - centre.Y, destination.X - centre.X);
        }

        public static void DrawWormhole(Texture2D texture, SpriteBatch spriteBatch, Vector2 position, float scaleMultiplier = 1.0f, float alphaMultiplier = 0.5f, float rotationMultiplier = 1.0f, bool inWorld = true)
        {
            float layers = 20;
            float maxScale = 25;
            float rotation = Main.GlobalTimeWrappedHourly * -rotationMultiplier;
            for (int i = 1; i < layers; i++)
            {
                float scale = maxScale - i * maxScale / layers;
                Color colour = new(scale / maxScale, scale / maxScale, scale / maxScale);
                rotation *= 0.8f;
                spriteBatch.Draw(texture, position - (inWorld ? Main.screenPosition : Vector2.Zero), null, colour * (i / layers) * alphaMultiplier, rotation, new Vector2(32, 32), scale * scaleMultiplier, SpriteEffects.None, 0);
            }
        }

        public static void Text(string message, Color? colour = null)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(message, colour);
            else if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), (Color)(colour == null ? Color.White : colour));
        }

        public static void Ore(int tileID, int veinSize, int chanceDenominator)
        {
            for (int i = 0; i < Main.maxTilesX * Main.maxTilesY / chanceDenominator; i++)
            {
                int x = Main.rand.Next(1, Main.maxTilesX - 1);
                int y = Main.rand.Next((int)GenVars.rockLayerLow, Main.maxTilesY - 1);
                if (Main.tile[x, y].TileType == TileID.Stone ||
                    Main.tile[x, y].TileType == TileID.Dirt ||
                    Main.tile[x, y].TileType == TileID.Ebonstone ||
                    Main.tile[x, y].TileType == TileID.Crimstone ||
                    Main.tile[x, y].TileType == TileID.Pearlstone ||
                    Main.tile[x, y].TileType == TileID.Sand ||
                    Main.tile[x, y].TileType == TileID.Mud ||
                    Main.tile[x, y].TileType == TileID.SnowBlock ||
                    Main.tile[x, y].TileType == TileID.IceBlock)
                    WorldGen.TileRunner(x, y, veinSize, 15, tileID);
            }
        }

        public static void Succ(Vector2 position, int range, float strength, bool projectiles = true, bool magicOnly = false, bool npcs = true, bool items = true, float slow = 1.0f)
        {
            int rangeSquared = range * range;
            if (npcs)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active)
                    {
                        int dist = (int)Vector2.DistanceSquared(npc.Center, position);
                        if (dist < rangeSquared)
                        {
                            npc.velocity *= slow;
                            npc.velocity += Vector2.Normalize(position - npc.Center) * strength;
                            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame, 0, 0, 0, default);
                        }
                    }
                }
            }
            if (projectiles)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    if (projectile.active && (!magicOnly || projectile.DamageType == DamageClass.Magic && projectile.friendly && !projectile.hostile))
                    {
                        int dist = (int)Vector2.DistanceSquared(projectile.Center, position);
                        if (dist < rangeSquared)
                        {
                            projectile.velocity *= slow;
                            projectile.velocity += Vector2.Normalize(position - projectile.Center) * strength;
                            Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, 0, 0, 0, default);
                        }
                    }
                }
            }
            for (int i = 0; i < Main.maxDust; i++)
            {
                Dust dust = Main.dust[i];
                if (dust.active)
                {
                    int dist = (int)Vector2.DistanceSquared(dust.position, position);
                    if (dist < rangeSquared)
                    {
                        dust.velocity *= slow;
                        dust.velocity += Vector2.Normalize(position - dust.position) * strength;
                    }
                }
            }
            if (items)
            {
                for (int i = 0; i < Main.maxItems; i++)
                {
                    Item item = Main.item[i];
                    if (item.active)
                    {
                        int dist = (int)Vector2.DistanceSquared(item.Center, position);
                        if (dist < rangeSquared)
                        {
                            item.velocity *= slow;
                            item.velocity += Vector2.Normalize(position - item.Center) * strength;
                            Dust.NewDust(item.position, item.width, item.height, DustID.Shadowflame, 0, 0, 0, default);
                        }
                    }
                }
            }
        }
    }
}
