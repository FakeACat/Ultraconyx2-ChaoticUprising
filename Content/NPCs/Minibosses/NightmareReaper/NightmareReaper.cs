using ChaoticUprising.Common;
using ChaoticUprising.Common.Systems;
using ChaoticUprising.Content.Biomes.Darkness;
using ChaoticUprising.Content.Items.Materials;
using ChaoticUprising.Content.Items.Pets;
using ChaoticUprising.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.NPCs.Minibosses.NightmareReaper
{
    public class NightmareReaper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void-dropped Nightmare");
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.MustAlwaysDraw[Type] = true;

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = Texture + "Image",
                PortraitScale = 0.75f,
                PortraitPositionYOverride = 48f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }
        public override void SetDefaults()
        {
            NPC.width = 76;
            NPC.height = 76;
            NPC.damage = 80;
            NPC.lifeMax = 120000;
            NPC.defense = 45;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.knockBackResist = 0;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Darkness>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
                new FlavorTextBestiaryInfoElement("A highly territorial leviathan originating from the Void.")
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (ChaosMode.chaosMode && (int)ChaosMode.GetDifficulty() >= (int)Difficulty.Darkened && !NPC.AnyNPCs(ModContent.NPCType<NightmareReaper>()) && !spawnInfo.PlayerSafe && spawnInfo.Sky)
                return 0.0001f;
            return 0;
        }


        static readonly int segmentCount = 40;
        readonly Vector2[] segmentPos = new Vector2[segmentCount];
        readonly float[] segmentRot = new float[segmentCount];

        Player Target => Main.player[NPC.target];

        float jawRotation = 0;

        bool crawling = true;
        int deathrayCooldown = 600;

        private static SoundStyle[][] legacyArrayedStylesMapping;

        public override void AI()
        {
            if (NPC.ai[0] == 0)
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<NightmareRightHand>(), 0, NPC.whoAmI);
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<NightmareLeftHand>(), 0, NPC.whoAmI);
                NPC.ai[0] = 1;
            }
            if (CUUtils.InvalidTarget(NPC.target))
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
                if (CUUtils.InvalidTarget(NPC.target))
                {
                    NPC.active = false;
                    NPC.life = -1;
                    return;
                }
            }

            if (NPC.ai[1] == 0 || NPC.ai[1] == 3)
            {
                float d = NPC.Distance(Target.Center);
                NPC.rotation = NPC.rotation.AngleTowards(CUUtils.AngleTo(Target.Center, NPC.Center) + MathHelper.PiOver2, MathHelper.Pi / 30);
                if (crawling)
                {
                    NPC.velocity = NPC.velocity * 0.95f + Vector2.Normalize(Target.Center - NPC.Center) * (d - 750) / 20 * 0.05f;
                    if (d < 750)
                        crawling = false;
                }
                else
                {
                    NPC.velocity *= 0.98f;
                    if (d > 960 || d < 200)
                        crawling = true;
                }
                if (NPC.ai[1] == 3)
                {
                    if (jawRotation > 0)
                        jawRotation -= 0.05f;
                }
                else if (NPC.life < NPC.lifeMax * 0.45f)
                    NPC.ai[1] = 1;
            }
            else if (NPC.ai[1] == 1)
            {
                NPC.velocity *= 0.97f;
                if (jawRotation == 0)
                    SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                jawRotation += 0.0125f;
                if (Main.netMode != NetmodeID.Server)
                {
                    Vector2 dustStart = NPC.Center + (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * 96 + new Vector2(Main.rand.Next(-64, 65), Main.rand.Next(-64, 65));
                    for (int i = 0; i < jawRotation * 4; i++)
                    {
                        int d = Dust.NewDust(dustStart, 0, 0, DustID.BlueTorch, 0, 0, 0, default, 3.0f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity = (NPC.Center - Main.dust[d].position) / 30;
                    }
                }
                if (jawRotation >= 1.0f)
                {
                    SoundEngine.PlaySound(SoundID.Zombie104, NPC.position);
                    NPC.ai[1] = 2;
                    NPC.damage = NPC.defDamage * 2;
                }
            }
            else if (NPC.ai[1] == 2)
            {
                NPC.rotation = NPC.rotation.AngleTowards(CUUtils.AngleTo(Target.Center, NPC.Center) + MathHelper.PiOver2, MathHelper.Pi / 200);
                float d = NPC.DistanceSQ(Target.Center);
                if (d > DeathrayLengthInPixels * DeathrayLengthInPixels)
                    NPC.velocity = Vector2.Normalize(Target.Center - NPC.Center) * 16;
                else
                    NPC.velocity *= 0.975f;

                deathrayCooldown++;
                if (deathrayCooldown > 1000)
                {
                    deathrayStrength -= 0.05f;
                    if (deathrayStrength <= 0.0f)
                    {
                        //deathrayCooldown = 0;
                        NPC.ai[1] = 3;
                    }
                }
                else
                {
                    if (deathrayStrength < 1.0f && deathrayCooldown > 600)
                        deathrayStrength += 0.005f;
                    else if (deathrayStrength >= 1.0f)
                    {
                        Player player = Main.player[NPC.target];
                        if (Collision.CheckAABBvLineCollision(player.position, new Vector2(player.width, player.height), NPC.Center, NPC.Center + (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * DeathrayLengthInPixels))
                        {
                            player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), NPC.damage, 0);
                        }
                    }
                }

            }

            CUUtils.UpdateSpecialWormSegments(NPC, 56, 60, 80, 10, segmentCount, segmentPos, segmentRot, NPC.rotation - MathHelper.PiOver2);
        }

        private float deathrayStrength = 0.0f;
        private readonly int deathrayLength = 100;
        private int DeathrayLengthInPixels => deathrayLength * 26 + 50;

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBosses.downedNightmareReaper, -1);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<NightmareChitin>(), 1, 40, 60));
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<ReaperEssence>(), 4));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D head = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaper");
            Texture2D body = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaperBody");
            Texture2D tail = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaperTail");
            Texture2D torso = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaperTorso");
            Texture2D head2 = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaperHead2");
            Texture2D head3 = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaperHead3");
            Texture2D deathray = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaperDeathray");
            Texture2D head_glow = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaperGlowmask");
            Texture2D body_glow = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaperBodyGlowmask");
            Texture2D tail_glow = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaperTailGlowmask");
            Texture2D torso_glow = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaperTorsoGlowmask");

            CUUtils.DrawSpecialWorm(spriteBatch, NPC, head, body, tail, drawColor, segmentCount, segmentPos, segmentRot, torso, head_glow, body_glow, tail_glow, torso_glow);
            Vector2 offset = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2();
            Vector2 mouthPos = NPC.Center - Main.screenPosition + offset * 3;
            spriteBatch.Draw(head3, mouthPos, null, drawColor, NPC.rotation - jawRotation, new Vector2(head3.Width / 2, head3.Height / 2), NPC.scale, SpriteEffects.None, 0);
            spriteBatch.Draw(head2, mouthPos, null, drawColor, NPC.rotation + jawRotation, new Vector2(head2.Width / 2, head2.Height / 2), NPC.scale, SpriteEffects.None, 0);

            if (deathrayStrength > 0)
            {
                Color colour1 = deathrayStrength >= 1.0f ? Color.Red : Color.White;
                spriteBatch.Draw(deathray, mouthPos + offset * 50, new Rectangle(0, 56, 40, 26), colour1 * deathrayStrength, NPC.rotation, new Vector2(20, 13), NPC.scale, SpriteEffects.None, 0);
                for (int i = 1; i < deathrayLength; i++)
                {
                    float num1 = 1.0f - (float)(i / 100.0f);
                    float num2 = (float)(deathrayStrength / num1);
                    if (deathrayStrength >= num1)
                        colour1 = Color.Red;
                    spriteBatch.Draw(deathray, mouthPos + offset * (50 + 26 * i), new Rectangle(0, 28, 40, 26), colour1 * num2, NPC.rotation, new Vector2(20, 13), NPC.scale, SpriteEffects.None, 0);
                }
                spriteBatch.Draw(deathray, mouthPos + offset * (50 + 26 * deathrayLength), new Rectangle(0, 0, 40, 26), colour1, NPC.rotation, new Vector2(20, 13), NPC.scale, SpriteEffects.None, 0);
            }

            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return true;
        }

        public override bool CheckActive()
        {
            return false;
        }
    }

    public abstract class AbstractNightmareHand : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.lifeMax = 100000;
            NPC.defense = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.knockBackResist = 0;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            Main.npcFrameCount[NPC.type] = 3;
            NPC.dontTakeDamage = true;
        }

        public NPC Reaper => Main.npc[(int)NPC.ai[0]];

        public abstract Vector2 RestingOffset();

        public abstract Texture2D ArmTexture();

        public abstract Texture2D ArmGlowmask();
        public abstract Texture2D HandGlowmask();

        public abstract Vector2 Shoulder();

        public abstract int TimeToMove();

        public override bool PreAI()
        {
            if (Reaper == null || !Reaper.active)
            {
                NPC.life = -1;
                NPC.active = false;
                return false;
            }
            return true;
        }

        private int moveTime = 0;
        public const int CRAWL_CYCLE_LENGTH = 30;
        private const int HAND_MOVE_TIME = 12;
        public override void AI()
        {
            NPC.rotation = Reaper.rotation;

            NPC.ai[2]++;
            if (NPC.ai[2] > CRAWL_CYCLE_LENGTH)
                NPC.ai[2] = 0;

            if (NPC.ai[2] == TimeToMove())
            {
                moveTime = HAND_MOVE_TIME;
                NPC.velocity = (Reaper.Center + RestingOffset().RotatedBy(Reaper.rotation) - NPC.Center) / HAND_MOVE_TIME;
            }
            moveTime--;
            if (moveTime < 0)
                NPC.velocity = Vector2.Zero;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 wrist = NPC.Center - (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * 48;

            float r1 = (Reaper.Center + Shoulder() - wrist).ToRotation() - MathHelper.PiOver2;
            float r2 = NPC.rotation;
            float rotation1 = (r1.ToRotationVector2() + r2.ToRotationVector2()).ToRotation();

            spriteBatch.Draw(ArmTexture(), wrist - Main.screenPosition, null, Lighting.GetColor((int)wrist.X / 16, (int)wrist.Y / 16), rotation1, new Vector2(ArmTexture().Width / 2, 0), NPC.scale, SpriteEffects.None, 0);
            if (ArmGlowmask() != null)
                spriteBatch.Draw(ArmGlowmask(), wrist - Main.screenPosition, null, Color.White, rotation1, new Vector2(ArmGlowmask().Width / 2, 0), NPC.scale, SpriteEffects.None, 0);

            Vector2 elbow = wrist + (rotation1 + MathHelper.PiOver2).ToRotationVector2() * ArmTexture().Height;

            float rotation2 = (Reaper.Center + Shoulder().RotatedBy(r2) - elbow).ToRotation() - MathHelper.PiOver2;

            spriteBatch.Draw(ArmTexture(), elbow - Main.screenPosition, null, Lighting.GetColor((int)elbow.X / 16, (int)elbow.Y / 16), rotation2, new Vector2(ArmTexture().Width / 2, 0), NPC.scale, SpriteEffects.None, 0);
            if (ArmGlowmask() != null)
                spriteBatch.Draw(ArmGlowmask(), elbow - Main.screenPosition, null, Color.White, rotation2, new Vector2(ArmGlowmask().Width / 2, 0), NPC.scale, SpriteEffects.None, 0);

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Vector2 Vector = new(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + (NPC.width / 2) - texture.Width * NPC.scale / 2f + Vector.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - texture.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + Vector.Y * NPC.scale), new Rectangle?(NPC.frame), drawColor, NPC.rotation, Vector, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            if (HandGlowmask() != null)
                spriteBatch.Draw(HandGlowmask(), new Vector2(NPC.position.X - Main.screenPosition.X + (NPC.width / 2) - texture.Width * NPC.scale / 2f + Vector.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - texture.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + Vector.Y * NPC.scale), new Rectangle?(NPC.frame), Color.White, NPC.rotation, Vector, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            return false;
        }

        public override bool CheckActive()
        {
            return false;
        }
    }

    public class NightmareRightHand : AbstractNightmareHand
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 84;
            NPC.height = 88;
            NPC.damage = 100;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[1] > 600 && NPC.ai[1] < 900)
            {
                NPC.frame.Y = frameHeight * 2;
            }
            else if (NPC.ai[1] > 1100)
            {
                NPC.frame.Y = frameHeight;
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override Texture2D ArmTexture() => (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareRightArm");

        public override Texture2D ArmGlowmask() => (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareRightArmGlowmask");
        public override Texture2D HandGlowmask() => (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareRightHandGlowmask");

        public override Vector2 RestingOffset() => new(160, -160);

        public override Vector2 Shoulder() => new(40, 40);

        public override int TimeToMove() => 0;

        public override void AI()
        {
            if (!CUUtils.InvalidTarget(Reaper.target))
            {
                if (NPC.ai[3] > 0)
                    NPC.ai[3]--;

                NPC.ai[1]++;
                if (NPC.ai[1] > 600 && NPC.ai[1] < 900)
                {
                    Succ();
                    return;
                }
                else if (NPC.ai[1] > 1100 && NPC.ai[1] < 1200)
                {
                    Redirect();
                    return;
                }
                else if (NPC.ai[1] >= 1200)
                    NPC.ai[1] = 0;

                if (Main.expertMode)
                    NPC.ai[1] += 2;

            }
            base.AI();
        }
        readonly int tractorBeamLength = 3200;
        Vector2 aimPos = Vector2.Zero;
        private void Succ()
        {
            Player target = Main.player[Reaper.target];
            Vector2 targetPos = target.Center + target.velocity * 40;
            if (aimPos == Vector2.Zero)
            {
                aimPos = targetPos;
            }
            else if (NPC.ai[1] < 720)
            {
                float speed = 0.15f;
                aimPos = aimPos * (1 - speed) + targetPos * speed;
            }
            else
            {
                float speed = 0.02f;
                aimPos = aimPos * (1 - speed) + target.Center * speed;
                if (NPC.ai[3] <= 0 && !target.immune && Collision.CheckAABBvLineCollision(target.position, new Vector2(target.width, target.height), NPC.Center, NPC.Center + (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * tractorBeamLength))
                {
                    target.velocity = Vector2.Normalize(NPC.Center - target.Center) * 40;
                    if (target.DistanceSQ(NPC.Center) < 102400)
                        NPC.velocity = Vector2.Normalize(target.Center - NPC.Center) * 70;
                }
            }
            NPC.rotation = CUUtils.AngleTo(aimPos, NPC.Center) + MathHelper.PiOver2;
            NPC.velocity += Vector2.Normalize(Reaper.Center + RestingOffset().RotatedBy(Reaper.rotation) - NPC.Center);
            NPC.velocity *= 0.95f;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (NPC.ai[3] <= 0)
            {
                target.velocity += NPC.velocity;
                NPC.ai[3] = 120;
            }
        }

        private void Redirect()
        {
            Player target = Main.player[Reaper.target];
            NPC.rotation = (target.Center - NPC.Center).ToRotation() + MathHelper.PiOver2;
            NPC.velocity += Vector2.Normalize(Reaper.Center + RestingOffset().RotatedBy(Reaper.rotation) - NPC.Center);
            NPC.velocity *= 0.95f;

            if (NPC.ai[1] == 1199)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<DarkMatterEnergyBall>() && Main.projectile[i].active)
                    {
                        Main.projectile[i].ai[0] = 1;
                        Main.projectile[i].velocity = Vector2.Normalize(target.Center - Main.projectile[i].Center) * 9;
                        Main.projectile[i].timeLeft = 300;
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("ChaoticUprising/Assets/Textures/BlankTexture").Value;

            if (NPC.ai[1] > 1140 && !CUUtils.InvalidTarget(Reaper.target))
            {
                Vector2 fingertip = NPC.Center + new Vector2(-10, -35).RotatedBy(NPC.rotation);
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.type == ModContent.ProjectileType<DarkMatterEnergyBall>() && p.active)
                    {
                        spriteBatch.Draw(texture, new Rectangle((int)fingertip.X - (int)Main.screenPosition.X, (int)fingertip.Y - (int)Main.screenPosition.Y, (int)p.Distance(fingertip), 1), new Rectangle(0, 0, 16, 16), Color.Cyan * 0.5f, (p.Center - fingertip).ToRotation(), Vector2.Zero, SpriteEffects.None, 0);
                        spriteBatch.Draw(texture, new Rectangle((int)p.Center.X - (int)Main.screenPosition.X, (int)p.Center.Y - (int)Main.screenPosition.Y, 1600, 1), new Rectangle(0, 0, 16, 16), Color.Red * 0.5f, (Main.player[Reaper.target].Center - p.Center).ToRotation(), Vector2.Zero, SpriteEffects.None, 0);
                    }
                }
            }
            else if (NPC.ai[1] > 600 && NPC.ai[1] < 900)
            {
                float progress = Math.Clamp((NPC.ai[1] - 600.0f) / 120.0f, 0.0f, 1.0f);
                int beams = 32;
                float maxSpread = MathHelper.Pi / 8;
                int width = 2;
                for (int i = 0; i < beams; i++)
                {
                    float num1 = Main.GlobalTimeWrappedHourly + MathHelper.TwoPi * i / beams;
                    spriteBatch.Draw(texture, new Rectangle((int)NPC.Center.X - (int)Main.screenPosition.X, (int)NPC.Center.Y - (int)Main.screenPosition.Y, tractorBeamLength, width), new Rectangle(0, 0, 16, 16), Color.Cyan * progress * (float)((Math.Sin(num1) + 3.0f) / 4.0f), NPC.rotation - MathHelper.PiOver2 + (float)Math.Cos(num1) * (1.0f - progress) * maxSpread, new Vector2(0, width / 2), SpriteEffects.None, 0);
                }
                int crosshairLength = 32;
                int crosshairWidth = 4;
                spriteBatch.Draw(texture, new Rectangle((int)aimPos.X - (int)Main.screenPosition.X - crosshairLength / 2, (int)aimPos.Y - (int)Main.screenPosition.Y - crosshairWidth / 2, crosshairLength, crosshairWidth), new Rectangle(0, 0, 16, 16), Color.Red, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0);
                spriteBatch.Draw(texture, new Rectangle((int)aimPos.X - (int)Main.screenPosition.X - crosshairWidth / 2, (int)aimPos.Y - (int)Main.screenPosition.Y - crosshairLength / 2, crosshairWidth, crosshairLength), new Rectangle(0, 0, 16, 16), Color.Red, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
    }

    public class NightmareLeftHand : AbstractNightmareHand
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 88;
            NPC.height = 88;
            NPC.damage = 0;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[1] > 400 && NPC.ai[1] < 600)
            {
                NPC.frame.Y = frameHeight;
            }
            else if (NPC.ai[1] > 1000 && NPC.ai[1] < 1200)
            {
                NPC.frame.Y = frameHeight * 2;
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override Texture2D ArmTexture() => (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareLeftArm");

        public override Texture2D ArmGlowmask() => (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareLeftArmGlowmask");
        public override Texture2D HandGlowmask() => (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareLeftHandGlowmask");

        public override Vector2 RestingOffset() => new(-160, -160);

        public override Vector2 Shoulder() => new(-40, 40);

        public override int TimeToMove() => CRAWL_CYCLE_LENGTH / 2;

        public override void AI()
        {
            if (!CUUtils.InvalidTarget(Reaper.target))
            {
                NPC.ai[1]++;
                if (NPC.ai[1] > 400 && NPC.ai[1] < 600)
                {
                    VoidHarpoons();
                    return;
                }
                if (NPC.ai[1] > 1000 && NPC.ai[1] < 1200)
                {
                    Balls();
                    return;
                }
                if (NPC.ai[1] >= 1200)
                    NPC.ai[1] = 0;

                if (Main.expertMode)
                    NPC.ai[1] += 2;
            }
            base.AI();
        }

        private void VoidHarpoons()
        {
            NPC.rotation = (Reaper.Center - NPC.Center).ToRotation() - MathHelper.PiOver2;

            float a = ((NPC.ai[1] - 400) / 100) * (float)Math.PI;

            NPC.velocity += Vector2.Normalize(Reaper.Center + RestingOffset().RotatedBy(Reaper.rotation + Math.Abs(Math.Sin(a))) - NPC.Center);
            NPC.velocity *= 0.95f;

            if (NPC.ai[1] % 8 == 0 && (NPC.ai[1] < 475 || NPC.ai[1] > 525) && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * 6, ModContent.ProjectileType<VoidHarpoon>(), 50, 2);
            }
        }

        private void Balls()
        {
            Player target = Main.player[Reaper.target];
            NPC.velocity *= 0.975f;
            NPC.rotation = (target.Center - NPC.Center).ToRotation() + MathHelper.PiOver2;

            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[1] % 25 == 0 && NPC.ai[1] > 1060)
            {
                for (int i = 0; i < 7; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Normalize(target.Center - NPC.Center) * 10 + new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)) / 2, ModContent.ProjectileType<DarkMatterEnergyBall>(), 100, 2);
                }
            }
        }
    }
}
