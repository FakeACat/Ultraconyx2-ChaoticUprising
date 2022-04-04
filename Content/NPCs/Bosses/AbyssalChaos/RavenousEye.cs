using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using ChaoticUprising.Content.Projectiles;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;

namespace ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos
{
    [AutoloadBossHead]
    public class RavenousEye : AbstractTorturedEye
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 52;
            NPC.height = 94;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
                new FlavorTextBestiaryInfoElement("A Wandering Eye augmented by the Abyssal Chaos to incinerate any threats.")
            });
        }

        public Player Target()
        {
            return Main.player[NPC.target];
        }

        private const int AI_FLAMESPIT = 0;
        private const int AI_FLAMETHROWER = 1;
        private const int AI_DESPAWN = 2;

        public override void AI()
        {
            if ((NPC.target < 0 || NPC.target >= 255 || Target().dead || !Target().active || !NPC.AnyNPCs(ModContent.NPCType<AbyssalChaos>())) && NPC.ai[0] != AI_DESPAWN)
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
                if (NPC.target < 0 || NPC.target >= 255 || Target().dead || !Target().active || !NPC.AnyNPCs(ModContent.NPCType<AbyssalChaos>()))
                {
                    NPC.ai[0] = AI_DESPAWN;
                    NPC.ai[1] = NPC.position.Y + 3000;
                }
            }

            switch (NPC.ai[0])
            {
                case AI_FLAMETHROWER:
                    AI_Flamethrower();
                    break;
                case AI_FLAMESPIT:
                    AI_Flamespit();
                    break;
                case AI_DESPAWN:
                    AI_Despawn();
                    break;
            }
        }
        private void AI_Flamespit()
        {
            NPC.velocity += Vector2.Normalize(Target().Center + new Vector2(Target().Center.X > NPC.Center.X ? -500 : 500, 0) - NPC.Center) * 0.65f;
            NPC.velocity *= 0.96f;
            NPC.rotation = (Target().Center - NPC.Center).ToRotation() - (float)(Math.PI / 2);
            NPC.ai[1]++;
            if (NPC.ai[1] > 60 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[1] = 0;
                int dmg = 100;
                if (Main.expertMode)
                {
                    dmg /= 2;
                }
                for (int i = 0; i < 3; i++)
                {
                    int fireball = Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center, new Vector2(Main.rand.Next(-40, 41) / 20f, Main.rand.Next(-40, 41) / 20f) + Vector2.Normalize(Target().Center - new Vector2(NPC.Center.X, NPC.position.Y + NPC.height)) * 12, ModContent.ProjectileType<Fireball>(), dmg, 1);
                    Main.projectile[fireball].timeLeft = 360;
                }
                NPC.ai[2]++;
                if (NPC.ai[2] > 8)
                    SwitchAI();
            }
        }

        private void AI_Flamethrower()
        {
            NPC.velocity += Vector2.Normalize(Target().Center + new Vector2(Target().Center.X > NPC.Center.X ? -500 : 500, -600) - NPC.Center) * 0.55f;
            NPC.velocity *= 0.96f;
            NPC.rotation = (Target().Center - NPC.Center).ToRotation() - (float)(Math.PI / 2);
            NPC.ai[1]++;
            if (NPC.ai[1] > 4 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[1] = 0;
                int dmg = 110;
                if (Main.expertMode)
                {
                    dmg /= 2;
                }
                int fireball = Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center, Vector2.Normalize(Target().Center - new Vector2(NPC.Center.X, NPC.position.Y + NPC.height)) * 10, ModContent.ProjectileType<Fireball>(), dmg, 1);
                Main.projectile[fireball].timeLeft = 90;
                NPC.ai[2]++;
                if (NPC.ai[2] > 80)
                    SwitchAI();
            }
        }

        private void AI_Despawn()
        {
            NPC.velocity.Y += 1f;
            if (NPC.position.Y > NPC.ai[1] || !NPC.AnyNPCs(ModContent.NPCType<AbyssalChaos>()))
            {
                NPC.active = false;
                NPC.life = -1;
            }
        }

        private void SwitchAI()
        {
            NPC.ai[0]++;
            if (NPC.ai[0] >= AI_DESPAWN)
                NPC.ai[0] = AI_FLAMESPIT;
            NPC.ai[1] = 0;
            NPC.ai[2] = 0;
            NPC.ai[3] = 0;
        }
    }
}
