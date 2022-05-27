using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ChaoticUprising.Content.Projectiles;
using ChaoticUprising.Common.Systems;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework.Graphics;
using ChaoticUprising.Content.Items.Placeables.Banners;

namespace ChaoticUprising.Content.NPCs
{
    public class SlimePraetorian : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers(0) { PortraitScale = 0.75f });
        }

        public override void SetDefaults()
        {
            NPC.width = 174;
            NPC.height = 120;
            NPC.aiStyle = -1;
            NPC.damage = 150;
            NPC.defense = 60;
            NPC.lifeMax = 70000;
            NPC.knockBackResist = 0.3f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.alpha = 30;
            NPC.value = Item.buyPrice(0, 25, 0, 0);
            NPC.scale = 1.25f;
            NPC.lavaImmune = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            AnimationType = NPCID.KingSlime;
            Main.npcFrameCount[NPC.type] = 6;
            Banner = Type;
            BannerItem = ModContent.ItemType<SlimePraetorianBanner>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.Add(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
            bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement("A specialist slime created to protect the Slime Progenitor from the abominations brought into this world by the Abyssal Chaos' death."));
        }

        int fireballTimer = 0;
        int age = 0;
        public override void AI()
        {
            age++;
            fireballTimer++;
            if (fireballTimer > 60 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                float speed = 12;
                int damage = 60;
                int type = ModContent.ProjectileType<Fireball>();
                Vector2 velocity = (Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * speed) + (NPC.velocity / 2);
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, velocity.X, velocity.Y, type, damage, 0f, 0);
                Main.projectile[proj].timeLeft = 200;
                Main.projectile[proj].tileCollide = true;
                fireballTimer = 0;
            }

            float realScale = 0.75f;
            float num818 = 1f;
            bool flag177 = false;
            bool flag178 = false;
            NPC.aiAction = 0;
            if (NPC.ai[3] == 0f && NPC.life > 0)
            {
                NPC.ai[3] = NPC.lifeMax;
            }
            if (NPC.localAI[3] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[0] = -100f;
                NPC.localAI[3] = 1f;
                NPC.TargetClosest();
                NPC.netUpdate = true;
            }
            Dust dust46;
            if (NPC.ai[1] == 5f)
            {
                flag177 = true;
                NPC.aiAction = 1;
                NPC.ai[0] += 1f;
                num818 = MathHelper.Clamp((60f - NPC.ai[0]) / 60f, 0f, 1f);
                num818 = 0.5f + num818 * 0.5f;
                if (NPC.ai[0] >= 60f)
                {
                    flag178 = true;
                }
                if (NPC.ai[0] == 60f)
                {
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-40f, (0f - (float)NPC.height) / 2f), NPC.velocity, 734);
                }
                if (NPC.ai[0] >= 60f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.Bottom = new Vector2(NPC.localAI[1], NPC.localAI[2]);
                    NPC.ai[1] = 6f;
                    NPC.ai[0] = 0f;
                    NPC.netUpdate = true;
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && NPC.ai[0] >= 120f)
                {
                    NPC.ai[1] = 6f;
                    NPC.ai[0] = 0f;
                }
                if (!flag178)
                {
                    for (int num859 = 0; num859 < 10; num859++)
                    {
                        int num860 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, DustID.TintableDust, NPC.velocity.X, NPC.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                        Main.dust[num860].noGravity = true;
                        dust46 = Main.dust[num860];
                        dust46.velocity *= 0.5f;
                    }
                }
            }
            else if (NPC.ai[1] == 6f)
            {
                flag177 = true;
                NPC.aiAction = 0;
                NPC.ai[0] += 1f;
                num818 = MathHelper.Clamp(NPC.ai[0] / 30f, 0f, 1f);
                num818 = 0.5f + num818 * 0.5f;
                if (NPC.ai[0] >= 30f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 0f;
                    NPC.netUpdate = true;
                    NPC.TargetClosest();
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && NPC.ai[0] >= 60f)
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 0f;
                    NPC.TargetClosest();
                }
                for (int num865 = 0; num865 < 10; num865++)
                {
                    int num869 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, DustID.TintableDust, NPC.velocity.X, NPC.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                    Main.dust[num869].noGravity = true;
                    dust46 = Main.dust[num869];
                    dust46.velocity *= 2f;
                }
            }
            NPC.dontTakeDamage = (NPC.hide = flag178);
            if (NPC.velocity.Y == 0f)
            {
                NPC.velocity.X *= 0.8f;
                if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                {
                    NPC.velocity.X = 0f;
                }
                if (!flag177)
                {
                    NPC.ai[0] += 2f;
                    if ((double)NPC.life < (double)NPC.lifeMax * 0.8)
                    {
                        NPC.ai[0] += 1f;
                    }
                    if ((double)NPC.life < (double)NPC.lifeMax * 0.6)
                    {
                        NPC.ai[0] += 1f;
                    }
                    if ((double)NPC.life < (double)NPC.lifeMax * 0.4)
                    {
                        NPC.ai[0] += 2f;
                    }
                    if ((double)NPC.life < (double)NPC.lifeMax * 0.2)
                    {
                        NPC.ai[0] += 3f;
                    }
                    if ((double)NPC.life < (double)NPC.lifeMax * 0.1)
                    {
                        NPC.ai[0] += 4f;
                    }
                    if (NPC.ai[0] >= 0f)
                    {
                        NPC.netUpdate = true;
                        NPC.TargetClosest();
                        if (NPC.ai[1] == 3f)
                        {
                            NPC.velocity.Y = -13f;
                            NPC.velocity.X += 7f * NPC.direction;
                            NPC.ai[0] = -200f;
                            NPC.ai[1] = 0f;
                        }
                        else if (NPC.ai[1] == 2f)
                        {
                            NPC.velocity.Y = -6f;
                            NPC.velocity.X += 9f * NPC.direction;
                            NPC.ai[0] = -120f;
                            NPC.ai[1] += 1f;
                        }
                        else
                        {
                            NPC.velocity.Y = -8f;
                            NPC.velocity.X += 8f * NPC.direction;
                            NPC.ai[0] = -120f;
                            NPC.ai[1] += 1f;
                        }
                    }
                    else if (NPC.ai[0] >= -30f)
                    {
                        NPC.aiAction = 1;
                    }
                }
            }
            else if (NPC.target < 255 && ((NPC.direction == 1 && NPC.velocity.X < 3f) || (NPC.direction == -1 && NPC.velocity.X > -3f)))
            {
                if ((NPC.direction == -1 && (double)NPC.velocity.X < 0.1) || (NPC.direction == 1 && (double)NPC.velocity.X > -0.1))
                {
                    NPC.velocity.X += 0.2f * (float)NPC.direction;
                }
                else
                {
                    NPC.velocity.X *= 0.93f;
                }
            }
            int num871 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.TintableDust, NPC.velocity.X, NPC.velocity.Y, 255, new Color(0, 80, 255, 80), NPC.scale * 1.2f);
            Main.dust[num871].noGravity = true;
            dust46 = Main.dust[num871];
            dust46.velocity *= 0.5f;
            if (NPC.life <= 0)
            {
                return;
            }
            float num885 = (float)NPC.life / (float)NPC.lifeMax;
            num885 = num885 * 0.5f + 0.75f;
            num885 *= num818 * realScale;
            if (num885 != NPC.scale)
            {
                NPC.position.X += (float)(NPC.width / 2);
                NPC.position.Y += (float)NPC.height;
                NPC.scale = num885;
                NPC.width = (int)(98f * NPC.scale);
                NPC.height = (int)(92f * NPC.scale);
                NPC.position.X -= (float)(NPC.width / 2);
                NPC.position.Y -= (float)NPC.height;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            int num888 = (int)((double)NPC.lifeMax * 0.15f);
            if (!((float)(NPC.life + num888) < NPC.ai[3]))
            {
                return;
            }
            NPC.ai[3] = NPC.life;
            int r = Main.rand.Next(10, 25);
            for (int i = 0; i < r; i++)
            {
                int d = 100;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity + new Vector2(Main.rand.Next(-8, 9), Main.rand.Next(-8, 0)), ProjectileID.GoldenShowerHostile, d, 1);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (ChaosMode.chaosMode)
                return SpawnCondition.Overworld.Chance * ChaosMode.EliteSpawnMultiplier() / 15;
            else
                return 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/SlimePraetorianAura");

            Vector2 frameOrigin = NPC.frame.Size() / 2f;
            Vector2 drawPos = NPC.Center - Main.screenPosition - new Vector2(0, 10);

            float time = Main.GlobalTimeWrappedHourly;
            float timer = age / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;

            for (float i = 0f; i < 1f; i += 0.25f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, NPC.frame, new Color(155, 100, 0, 50) * 0.5f, NPC.rotation, frameOrigin, NPC.scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, NPC.frame, new Color(155, 100, 0, 77) * 0.5f, NPC.rotation, frameOrigin, NPC.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
