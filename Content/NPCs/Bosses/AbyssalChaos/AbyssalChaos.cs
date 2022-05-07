using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using ChaoticUprising.Common.GlobalNPCs;
using ChaoticUprising.Content.Projectiles;
using Terraria.Graphics.Effects;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;
using ChaoticUprising.Common;
using ChaoticUprising.Content.Items.Consumables;
using ChaoticUprising.Content.Items.Vanity;
using ChaoticUprising.Content.Items.Weapons;

namespace ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos
{
    [AutoloadBossHead]
    public class AbyssalChaos : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = CUUtils.ConvenientBossHealth(160000, 240000);
            NPC.damage = CUUtils.ConvenientBossDamage(170, 220, false);
            NPC.defense = 80;
            NPC.width = 142;
            NPC.knockBackResist = 0f;
            NPC.height = 180;
            NPC.value = Item.buyPrice(2, 0, 0, 0);
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            Main.npcFrameCount[NPC.type] = 5;
            NPC.GetGlobalNPC<ChaosNPC>().shouldBeBuffedInChaosMode = false;
            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Hellish Intent");
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
				new FlavorTextBestiaryInfoElement("A replacement Guardian after the death of the Wall of Flesh. The transformation is incomplete and its destruction will have catastrophic implications on the world.")
            });
        }

        public override bool CheckActive()
        {
            return !Target().active || Target().dead;
        }
        int frame = 0;
        int frameChange = 0;
        int minion = 0;
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<AbyssalChaosBossBag>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeables.AbyssalChaosTrophy>(), 10));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeables.AbyssalChaosRelic>()));

            LeadingConditionRule notExpertLoot = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AbyssalChaosMask>(), 7));

            notExpertLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<RavenousBlaster>(), 3));
            notExpertLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<InfernalBlade>(), 3));
            notExpertLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BloodlustWand>(), 3));
            notExpertLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Vertebrae>(), 3));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0) 
                DeathEffects(NPC);
        }

        public override void FindFrame(int frameHeight)
        {
            frameChange++;
            if (frameChange >= 15)
            {
                if (frame < 4)
                {
                    frame++;
                }
                else
                {
                    frame = 0;
                }
                frameChange = 0;
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public void DeathEffects(NPC npc)
        {
            int bloodQuantity = 10;
            int dust = (NPC.localAI[0] == 0) ? DustID.FireworkFountain_Pink : DustID.Blood;
            for (int i = 0; i < bloodQuantity; i++)
            {
                Vector2 target = ((float)Math.PI * 2 / bloodQuantity * i + 1).ToRotationVector2() * 300;
                int bloodQuantity2 = 150;
                for (int a = 1; a <= bloodQuantity2; a++)
                {
                    Dust.NewDust(npc.Center, 0, 0, dust, target.X / a, target.Y / a, 0, default, 3);
                }
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.ai[0] == AI_MELEE;
        }

        private const int AI_RANGED = 0;
        private const int AI_MELEE = 1;
        private const int AI_MAGIC = 2;
        private const int AI_SUMMONER = 3;
        private const int AI_DESPAWN = 4;

        private bool ExpertSecondPhase()
        {
            return Main.expertMode && NPC.life < NPC.lifeMax / 2;
        }

        public Player Target()
        {
            return Main.player[NPC.target];
        }
        int age = 0;
        public override void AI()
        {
            age++;
            if (!SkyManager.Instance["ChaoticUprising:AbyssalChaos"].IsActive())
            {
                SkyManager.Instance.Activate("ChaoticUprising:AbyssalChaos");
            }

            if ((NPC.target < 0 || NPC.target >= 255 || Target().dead || !Target().active) && NPC.ai[0] != AI_DESPAWN) 
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
                if (NPC.target < 0 || NPC.target >= 255 || Target().dead || !Target().active)
                {
                    NPC.ai[0] = AI_DESPAWN;
                    NPC.ai[1] = NPC.position.Y + 3000;
                }
            }

            switch (NPC.ai[0])
            {
                case AI_RANGED:
                    AI_Ranged();
                    break;
                case AI_MELEE:
                    AI_Melee();
                    break;
                case AI_MAGIC:
                    AI_Magic();
                    break;
                case AI_SUMMONER:
                    AI_Summoner();
                    break;
                case AI_DESPAWN:
                    AI_Despawn();
                    break;
            }
        }

        private void SwitchAI()
        {
            NPC.ai[0]++;
            if (NPC.ai[0] >= AI_DESPAWN)
                NPC.ai[0] = AI_RANGED;
            NPC.ai[1] = 0;
            NPC.ai[2] = 0;
            if (ExpertSecondPhase())
            {
                NPC.ai[3]++;
                if (NPC.ai[3] > 2)
                    NPC.ai[3] = 0;
            }

            NPC.GetGlobalNPC<NPCEffects>().trail = NPC.ai[0] == AI_MELEE;
        }

        Vector2 offset = new Vector2(0, -400);
        public void AI_Ranged()
        {
            StayAbovePlayer(offset, ExpertSecondPhase() ? 0.7f : 0.6f, 0.96f, 120);
            NPC.rotation = NPC.velocity.X / 30;
            NPC.ai[1]++;
            if (NPC.ai[1] >= (ExpertSecondPhase() ? 50 : 70) && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[1] = 0;
                NPC.ai[2]++;
                int dmg = CUUtils.ConvenientBossDamage(100, 150, true);
                int speed = 12;
                int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.position.Y + NPC.height), Vector2.Normalize(Target().Center - new Vector2(NPC.Center.X, NPC.position.Y + NPC.height)) * speed, ModContent.ProjectileType<AbyssalFlamesBig>(), dmg, 1);
                Main.projectile[p].timeLeft = 320;
                Main.projectile[p].netUpdate = true;
            }
            if (NPC.ai[2] > 7)
            {
                if (ExpertSecondPhase())
                {
                    int nextOffsetX = ((int)NPC.ai[3] - 1) * 400;
                    int nextOffsetY = nextOffsetX == 0 ? -400 : 0;
                    offset = new Vector2(nextOffsetX, nextOffsetY);
                }
                SwitchAI();
            }
        }

        public void AI_Melee()
        {
            if (ExpertSecondPhase())
            {
                if (NPC.ai[3] == 1)
                {
                    NPC.ai[1]++;
                    if (NPC.ai[2] > 2)
                    {
                        NPC.ai[1]++;
                    }
                    if (NPC.ai[1] >= 75)
                    {
                        NPC.velocity *= 0.94f;
                        NPC.rotation = (Target().Center - NPC.Center).ToRotation() - (float)(Math.PI / 2);
                        if (NPC.ai[1] >= 100)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2]++;
                            NPC.velocity = Vector2.Normalize(Target().Center - NPC.Center) * 22;
                            SoundEngine.PlaySound(SoundID.ForceRoar, (int)NPC.Center.X, (int)NPC.Center.Y, 0);
                            if (NPC.ai[2] > 6)
                                SwitchAI();
                        }
                    }
                    return;
                }
                if (NPC.ai[3] == 2)
                {
                    NPC.ai[1]++;
                    if (NPC.ai[1] >= 40)
                    {
                        NPC.velocity *= 0.94f;
                        if (NPC.ai[1] >= 50)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2]++;
                            NPC.velocity.X = Target().Center.X > NPC.Center.X ? 30 : -30;
                            NPC.velocity.Y = Target().Center.Y > NPC.Center.Y ? 30 : -30;
                            NPC.rotation = NPC.velocity.ToRotation() - (float)(Math.PI / 2);
                            SoundEngine.PlaySound(SoundID.ForceRoar, (int)NPC.Center.X, (int)NPC.Center.Y, 0);
                            if (NPC.ai[2] > 8)
                                SwitchAI();
                        }
                    }
                    return;
                }
            }
            NPC.ai[1]++;
            if (NPC.ai[1] >= 75)
            {
                NPC.velocity *= 0.94f;
                NPC.rotation = (Target().Center - NPC.Center).ToRotation() - (float)(Math.PI / 2);
                if (NPC.ai[1] >= 100)
                {
                    NPC.ai[1] = 0;
                    NPC.ai[2]++;
                    NPC.velocity = Vector2.Normalize(Target().Center - NPC.Center) * 22;
                    SoundEngine.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 0);
                    if (NPC.ai[2] > 4)
                        SwitchAI();
                }
            }
        }

        public void AI_Magic()
        {
            StayAbovePlayer(new Vector2(0, -300), 0.5f, 0.97f, 200);
            NPC.rotation = NPC.velocity.X / 30;

            NPC.ai[1]++;
            if (NPC.ai[1] > 160 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[1] = 0;
                int numProj = 30;
                float pi = (float)Math.PI;
                float Speed = 7;
                if (ExpertSecondPhase())
                {
                    if (NPC.ai[3] == 1)
                    {
                        Speed = 2;
                        for (int I = 0; I < numProj; I++)
                        {
                            Vector2 pos = 2 * Target().Center - NPC.Center;
                            int type = ModContent.ProjectileType<LightRay>();
                            int damage = CUUtils.ConvenientBossDamage(120, 180, true);
                            float rotation = (pi * 2 / numProj * (I + 1)) + (float)Math.Atan2(pos.Y - (Target().position.Y + (Target().height * 0.5f)), pos.X - (Target().position.X + (Target().width * 0.5f)));
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), pos.X, pos.Y, (float)(Math.Cos(rotation) * Speed * -1) / 5, (float)(Math.Sin(rotation) * Speed * -1) / 5, type, damage, 1.0f);
                        }
                    }
                    else if (NPC.ai[3] == 2)
                    {
                        int dist = 96;
                        for (int y = (int)Target().Center.Y - 1000; y < Target().Center.Y + 1000; y += dist)
                        {
                            int m = NPC.ai[2] % 2 == 0 ? 1 : -1;
                            Vector2 pos = new Vector2(Target().Center.X + 1000 * m, y + Main.rand.Next(dist) - dist/2);
                            int damage = CUUtils.ConvenientBossDamage(100, 150, true);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), pos.X, pos.Y, -m, (Main.rand.NextFloat() - 0.5f) / 4, ModContent.ProjectileType<LightRay>(), damage, 1.0f);
                        }
                    }
                }
                if (NPC.ai[3] != 2 || !ExpertSecondPhase())
                {
                    for (int I = 0; I < numProj; I++)
                    {
                        int type = ModContent.ProjectileType<LightRay>();
                        int damage = CUUtils.ConvenientBossDamage(120, 180, true);
                        float rotation = (pi * 2 / numProj * (I + 1)) + (float)Math.Atan2(NPC.Center.Y - (Target().position.Y + (Target().height * 0.5f)), NPC.Center.X - (Target().position.X + (Target().width * 0.5f)));
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Cos(rotation) * Speed * -1) / 5, (float)(Math.Sin(rotation) * Speed * -1) / 5, type, damage, 1.0f);
                    }
                }
                NPC.ai[2]++;
                if (NPC.ai[2] > 2)
                    SwitchAI();
            }

        }

        public void AI_Summoner()
        {
            NPC.ai[1]++;
            NPC.velocity *= 0.94f;
            if (NPC.ai[1] > 200)
            {
                SwitchAI();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    return;
                int n = 0;
                if (NPC.AnyNPCs(ModContent.NPCType<AbyssalShade>()))
                {
                    n++;
                    if (minion == 0)
                        minion++;
                }
                if (NPC.AnyNPCs(ModContent.NPCType<BloodlustEye>()))
                {
                    n++;
                    if (minion == 1)
                        minion++;
                }
                if (NPC.AnyNPCs(ModContent.NPCType<RavenousEye>()))
                {
                    n++;
                    if (minion == 2)
                    {
                        if (NPC.AnyNPCs(ModContent.NPCType<AbyssalShade>()))
                            minion = 1;
                        else
                            minion = 0;
                    }
                }
                if (n < (NPC.localAI[0] == 1 ? 2 : 3))
                {
                    switch (minion)
                    {
                        case 0:
                            SpawnMiniboss(NPC.target, ModContent.NPCType<AbyssalShade>());
                            break;
                        case 1:
                            SpawnMiniboss(NPC.target, ModContent.NPCType<BloodlustEye>());
                            break;
                        case 2:
                            SpawnMiniboss(NPC.target, ModContent.NPCType<RavenousEye>());
                            break;
                    }
                    minion++;
                    if (minion > 2)
                        minion = 0;
                }
            }
        }

        private void SpawnMiniboss(int player, int type)
        {
            if (player == Main.myPlayer)
            {
                Player p = Main.player[player];
                SoundEngine.PlaySound(SoundID.Roar, p.position, 0);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(p.whoAmI, type);
                else
                    NetMessage.SendData(MessageID.SpawnBoss, number: p.whoAmI, number2: type);
            }
        }

        public void AI_Despawn()
        {
            NPC.velocity.Y += 1f;
            if (NPC.position.Y > NPC.ai[1])
            {
                NPC.active = false;
                NPC.life = -1;
            }
        }

        private void StayAbovePlayer(Vector2 offset, float speedMultiplier, float deceleration, float dist)
        {
            Vector2 targetPosition = Target().Center + offset;
            if (Vector2.Distance(targetPosition, NPC.Center) > dist)
            {
                NPC.velocity += Vector2.Normalize(targetPosition - NPC.Center) * speedMultiplier;
                NPC.velocity *= deceleration;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (ExpertSecondPhase())
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Bosses/AbyssalChaos/AbyssalChaosAura");

                Vector2 frameOrigin = NPC.frame.Size() / 2f;
                Vector2 offset = new Vector2(NPC.width / 2 - frameOrigin.X, NPC.height - NPC.frame.Height);
                Vector2 drawPos = NPC.position - Main.screenPosition + frameOrigin + offset;

                float time = Main.GlobalTimeWrappedHourly;
                float timer = age / 240f + time * 0.04f;

                time %= 4f;
                time /= 2f;

                if (time >= 1f)
                {
                    time = 2f - time;
                }

                time = time * 0.5f + 0.5f;

                float scale = NPC.scale * 1.1f;

                for (float i = 0f; i < 1f; i += 0.25f)
                {
                    float radians = (i + timer) * MathHelper.TwoPi;

                    spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, NPC.frame, new Color(90, 70, 255, 50) * 0.5f, NPC.rotation, frameOrigin, scale, SpriteEffects.None, 0);
                }

                for (float i = 0f; i < 1f; i += 0.34f)
                {
                    float radians = (i + timer) * MathHelper.TwoPi;

                    spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, NPC.frame, new Color(140, 120, 255, 77) * 0.5f, NPC.rotation, frameOrigin, scale, SpriteEffects.None, 0);
                }
            }

            NPC connected1 = NPC.AnyNPCs(ModContent.NPCType<AbyssalShade>()) ? Main.npc[NPC.FindFirstNPC(ModContent.NPCType<AbyssalShade>())] : null;
            if (connected1 != null)
                DrawConnector(connected1.Center, spriteBatch, connected1.alpha);
            NPC connected2 = NPC.AnyNPCs(ModContent.NPCType<BloodlustEye>()) ? Main.npc[NPC.FindFirstNPC(ModContent.NPCType<BloodlustEye>())] : null;
            if (connected2 != null)
                DrawConnector(connected2.Center, spriteBatch, connected2.alpha);
            NPC connected3 = NPC.AnyNPCs(ModContent.NPCType<RavenousEye>()) ? Main.npc[NPC.FindFirstNPC(ModContent.NPCType<RavenousEye>())] : null;
            if (connected3 != null)
                DrawConnector(connected3.Center, spriteBatch, connected3.alpha);
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public void DrawConnector(Vector2 connectorTarget, SpriteBatch spriteBatch, int alpha)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Bosses/AbyssalChaos/ConnectorTentacle");
            float a = (255f - alpha) / 255f;
            int l = texture.Height;
            Vector2 connectorOrigin = NPC.Center;
            bool lineComplete = false;
            while (!lineComplete)
            {
                Vector2 nextPosition = connectorTarget + Vector2.Normalize(Vector2.Normalize(connectorOrigin - connectorTarget) * l + new Vector2(0, l * 0.75f)) * l;
                float rotation = Vector2.Normalize(nextPosition - connectorTarget).ToRotation() + (float)(Math.PI / 2);
                spriteBatch.Draw(texture, connectorTarget - Main.screenPosition, null, Lighting.GetColor((int)connectorTarget.X / 16, (int)connectorTarget.Y / 16) * a, rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                connectorTarget = nextPosition;
                if (Vector2.Distance(connectorTarget, connectorOrigin) < NPC.width / 3)
                    lineComplete = true;
            }
        }
    }
}
