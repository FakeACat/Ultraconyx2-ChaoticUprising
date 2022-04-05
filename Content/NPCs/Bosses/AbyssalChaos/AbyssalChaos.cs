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
            NPC.lifeMax = CUUtils.ConvenientBossHealthScaling(220000, 320000);
            NPC.damage = CUUtils.ConvenientBossDamageScaling(170, 220);
            NPC.defense = 50;
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
            //bossBag = mod.ItemType("AbyssalChaosTreasureBag");
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

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeables.AbyssalChaosTrophy>(), 10));
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

        public static void DeathEffects(NPC npc)
        {
            int bloodQuantity = 10;
            for (int i = 0; i < bloodQuantity; i++)
            {
                Vector2 target = ((float)Math.PI * 2 / bloodQuantity * i + 1).ToRotationVector2() * 300;
                int bloodQuantity2 = 150;
                for (int a = 1; a <= bloodQuantity2; a++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.Blood, target.X / a, target.Y / a, 0, default, 3);
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

        public Player Target()
        {
            return Main.player[NPC.target];
        }

        public int PercentHealth()
        {
            return (int)(100 * (float)NPC.life / NPC.lifeMax);
        }

        public override void AI()
        {
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
            NPC.ai[3] = 0;

            NPC.GetGlobalNPC<NPCEffects>().trail = NPC.ai[0] == AI_MELEE;
        }

        public void AI_Ranged()
        {
            StayAbovePlayer(new Vector2(0, -400), 0.6f, 0.96f, 120);
            NPC.rotation = NPC.velocity.X / 30;
            NPC.ai[1]++;
            if (NPC.ai[1] >= (PercentHealth() + 20) && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[1] = 0;
                NPC.ai[2]++;
                int dmg = CUUtils.ConvenientBossDamageScaling(100, 160);
                int speed = 12;
                int p = Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), new Vector2(NPC.Center.X, NPC.position.Y + NPC.height), Vector2.Normalize(Target().Center - new Vector2(NPC.Center.X, NPC.position.Y + NPC.height)) * speed, ModContent.ProjectileType<AbyssalFlamesBig>(), dmg, 1);
                Main.projectile[p].tileCollide = PercentHealth() > 80;
                Main.projectile[p].timeLeft = 320;
                Main.projectile[p].netUpdate = true;
            }
            if (NPC.ai[2] > 7)
                SwitchAI();
        }

        public void AI_Melee()
        {
            NPC.ai[1]++;
            if (NPC.ai[1] >= PercentHealth() + 20)
            {
                NPC.velocity *= 0.94f;
                NPC.rotation = (Target().Center - NPC.Center).ToRotation() - (float)(Math.PI / 2);
                if (NPC.ai[1] >= PercentHealth() + 40)
                {
                    NPC.ai[1] = 0;
                    NPC.ai[2]++;
                    NPC.velocity = Vector2.Normalize(Target().Center - NPC.Center) * (30 - PercentHealth() / 10);
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

            NPC.ai[1] += 1 + ((100 - PercentHealth()) / 50);
            if (NPC.ai[1] > 200 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[1] = 0;
                int numProj = 30;
                float pi = (float)Math.PI;
                for (int I = 0; I < numProj; I++)
                {
                    int type = ModContent.ProjectileType<LightRay>();
                    float Speed = 7;
                    int damage = CUUtils.ConvenientBossDamageScaling(120, 180);
                    float rotation = (pi * 2 / numProj * (I + 1)) + (float)Math.Atan2(NPC.Center.Y - (Target().position.Y + (Target().height * 0.5f)), NPC.Center.X - (Target().position.X + (Target().width * 0.5f)));
                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X, NPC.Center.Y, (float)(Math.Cos(rotation) * Speed * -1) / 5, (float)(Math.Sin(rotation) * Speed * -1) / 5, type, damage, 1.0f);
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
            if (NPC.ai[1] > (Main.expertMode ? 150 : 300))
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
                if (n < 2)
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
            NPC connected1 = NPC.AnyNPCs(ModContent.NPCType<AbyssalShade>()) ? Main.npc[NPC.FindFirstNPC(ModContent.NPCType<AbyssalShade>())] : null;
            if (connected1 != null)
                DrawConnector(connected1.Center, spriteBatch);
            NPC connected2 = NPC.AnyNPCs(ModContent.NPCType<BloodlustEye>()) ? Main.npc[NPC.FindFirstNPC(ModContent.NPCType<BloodlustEye>())] : null;
            if (connected2 != null)
                DrawConnector(connected2.Center, spriteBatch);
            NPC connected3 = NPC.AnyNPCs(ModContent.NPCType<RavenousEye>()) ? Main.npc[NPC.FindFirstNPC(ModContent.NPCType<RavenousEye>())] : null;
            if (connected3 != null)
                DrawConnector(connected3.Center, spriteBatch);
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public void DrawConnector(Vector2 connectorTarget, SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Bosses/AbyssalChaos/ConnectorTentacle");
            int l = texture.Height;
            Vector2 connectorOrigin = NPC.Center;
            bool lineComplete = false;
            while (!lineComplete)
            {
                Vector2 nextPosition = connectorTarget + Vector2.Normalize(Vector2.Normalize(connectorOrigin - connectorTarget) * l + new Vector2(0, l * 0.75f)) * l;
                float rotation = Vector2.Normalize(nextPosition - connectorTarget).ToRotation() + (float)(Math.PI / 2);
                spriteBatch.Draw(texture, connectorTarget - Main.screenPosition, null, Lighting.GetColor((int)connectorTarget.X / 16, (int)connectorTarget.Y / 16), rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                connectorTarget = nextPosition;
                if (Vector2.Distance(connectorTarget, connectorOrigin) < NPC.width / 3)
                    lineComplete = true;
            }
        }
    }
}
