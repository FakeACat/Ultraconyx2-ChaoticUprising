using ChaoticUprising.Common.Systems;
using ChaoticUprising.Common.Utils;
using ChaoticUprising.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChaoticUprising.Content.NPCs.Darkness
{
    public class NightmareToad : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 8000;
            NPC.damage = 200;
            NPC.width = 56;
            NPC.height = 38;
            NPC.value = Item.buyPrice(0, 1, 20, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            Main.npcFrameCount[NPC.type] = 3;
            NPC.knockBackResist = 0;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Biomes.Darkness.Darkness>().Type };
            Banner = Type;
            BannerItem = ModContent.ItemType<NightmareToadBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
                new FlavorTextBestiaryInfoElement("These aggressive amphibians originate from another dimension and swarm the surface whenever the Wormhole becomes unstable.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (ChaosMode.chaosMode)
            {
                if (spawnInfo.Player.InModBiome<Biomes.Darkness.Darkness>())
                {
                    return ChaosMode.NormalSpawnMultiplier();
                }
                if (ChaosMode.difficulty >= ((int)Difficulty.Darkened) && !spawnInfo.PlayerSafe)
                    return SpawnCondition.Overworld.Chance;
            }
            return 0;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[1] == 0)
            {
                if (NPC.velocity.Y != 0)
                {
                    NPC.frame.Y = 38;
                }
                else
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                NPC.frame.Y = 76;
            }
        }

        public override void AI()
        {
            NPC.velocity *= 0.96f;
            if (CUUtils.InvalidTarget(NPC.target))
            {
                NPC.TargetClosest(false);
                if (CUUtils.InvalidTarget(NPC.target))
                {
                    return;
                }
            }
            Player player = Main.player[NPC.target];
            NPC.ai[1] = NPC.DistanceSQ(player.Center) > 409600 ? 0 : 1;
            if (NPC.ai[1] == 0)
            {
                NPC.ai[0]++;
                if (NPC.ai[0] > 120)
                {
                    NPC.ai[0] = 0;
                    NPC.velocity.Y = -8;
                    NPC.spriteDirection = player.Center.X > NPC.Center.X ? 1 : -1;
                }
                if (NPC.velocity.Y != 0)
                {
                    NPC.velocity.X = player.Center.X > NPC.Center.X ? 6 : -6;
                }
            }
            else
            {
                NPC.spriteDirection = player.Center.X > NPC.Center.X ? 1 : -1;
                if (tongues.Count == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ToadTongue t = new();
                        t.Setup();
                        tongues.Add(t);
                    }
                }
            }
            foreach (ToadTongue tongue in tongues)
            {
                tongue.AI(player, NPC);
                int d = (int)Vector2.DistanceSquared(tongue.position, Vector2.Zero);
                if (d > 2560000 || (tongue.returning && d < 1024))
                {
                    tongues.Remove(tongue);
                    return;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            foreach (ToadTongue tongue in tongues)
            {
                tongue.Draw(spriteBatch, NPC);
            }
            return true;
        }

        readonly List<ToadTongue> tongues = new();

        public class ToadTongue
        {
            internal Vector2 position;
            internal Vector2 velocity;
            internal bool returning;

            internal void Setup()
            {
                position = Vector2.Zero;
                velocity = Vector2.Zero;
                returning = true;
            }
            internal void AI(Player player, NPC npc)
            {
                position += velocity;
                Vector2 realPosition = position + npc.Center;
                if (npc.ai[1] == 0 || position.LengthSquared() > 1638400)
                    returning = true;
                else if (position.LengthSquared() < 900)
                    returning = false;

                if (returning)
                {
                    velocity = Vector2.Normalize(-position) * 16;
                }
                else if (position.LengthSquared() < 1024)
                {
                    velocity = Vector2.Normalize(player.Center - realPosition) * 20 + new Vector2((float)Main.rand.Next(-40, 41) / 40f, (float)Main.rand.Next(-40, 41) / 40f);
                }

                if (new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height).Intersects(new Rectangle((int)realPosition.X, (int)realPosition.Y, 0, 0)))
                {
                    player.Hurt(PlayerDeathReason.ByNPC(npc.whoAmI), npc.damage, 0);
                }
            }
            internal void Draw(SpriteBatch spriteBatch, NPC npc)
            {
                Vector2 realPosition = position + npc.Center;
                Vector2 attachPoint = npc.Center + new Vector2(npc.spriteDirection * 10, 8);
                int dist = (int)Vector2.Distance(realPosition, attachPoint);
                float r = (attachPoint - realPosition).ToRotation() - 1.57f;
                for (int i = 0; i < dist / 16; i++)
                {
                    Vector2 pos = Vector2.Normalize(attachPoint - realPosition) * 16 * i + realPosition;
                    Texture2D sprite = ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Darkness/NightmareToadTongueSegment").Value;
                    Color colour = Lighting.GetColor((int)(pos.X / 16f), (int)(pos.Y / 16f));
                    spriteBatch.Draw(sprite, pos - Main.screenPosition, null, colour, r, new Vector2(6, 8), 1, SpriteEffects.None, 0f);
                    if (position.Length() > 30)
                    {
                        sprite = ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Darkness/NightmareToadTongueTip").Value;
                        spriteBatch.Draw(sprite, realPosition - Main.screenPosition, null, colour, r, new Vector2(7, 8), 1, SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
