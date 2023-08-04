using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ChaoticUprising.Content.Projectiles;
using ChaoticUprising.Common.GlobalProjectiles;
using ChaoticUprising.Projectiles;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using ChaoticUprising.Common.Utils;

namespace ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos
{
    [AutoloadBossHead]
    public class BloodlustEye : AbstractTorturedEye
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
                new FlavorTextBestiaryInfoElement("A Wandering Eye augmented by the Abyssal Chaos to disintegrate any threats.")
            });
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 58;
            NPC.height = 94;
        }
        public Player Target()
        {
            return Main.player[NPC.target];
        }

        private const int AI_HOMINGFIREBALL = 0;
        private const int AI_WARP = 1;
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
                case AI_HOMINGFIREBALL:
                    AI_HomingFireball();
                    break;
                case AI_WARP:
                    AI_Warp();
                    break;
                case AI_DESPAWN:
                    AI_Despawn();
                    break;
            }
        }

        private void AI_HomingFireball()
        {
            NPC.velocity += Vector2.Normalize(Target().Center + new Vector2(Target().Center.X > NPC.Center.X ? -200 : 200, 0) - NPC.Center) * 0.5f;
            NPC.velocity *= 0.96f;
            NPC.rotation = (Target().Center - NPC.Center).ToRotation() - (float)(Math.PI / 2);
            NPC.ai[1]++;
            if (NPC.ai[1] > 60 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[1] = 0;
                int dmg = CUUtils.ConvenientBossDamage(90, 150, true);
                int fireball = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AbyssalFlames>(), dmg, 1);
                Main.projectile[fireball].timeLeft = 400;
                Main.projectile[fireball].GetGlobalProjectile<ProjectileFeatures>().hostileHoming = true;
                Main.projectile[fireball].GetGlobalProjectile<ProjectileFeatures>().featureSpeed = 5.5f;
                NPC.ai[2]++;
                if (NPC.ai[2] > 6)
                    SwitchAI();
            }
        }

        private void AI_Warp()
        {
            NPC.rotation = (Target().Center - NPC.Center).ToRotation() - (float)(Math.PI / 2);
            NPC.velocity *= 0.96f;
            NPC.ai[1]++;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.ai[1] == 25)
                {
                    int numProj = 15;
                    float pi = (float)Math.PI;
                    for (int I = 0; I < numProj; I++)
                    {
                        int type = ModContent.ProjectileType<AbyssalFlames>();
                        float Speed = 25;
                        int damage = CUUtils.ConvenientBossDamage(150, 200, true);
                        float rotation = (pi * 2 / numProj * (I + 1)) + (float)Math.Atan2(NPC.Center.Y - (Target().position.Y + (Target().height * 0.5f)), NPC.Center.X - (Target().position.X + (Target().width * 0.5f)));
                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Cos(rotation) * Speed * -1) / 5, (float)(Math.Sin(rotation) * Speed * -1) / 5, type, damage, 1.0f);
                        Main.projectile[p].timeLeft = 120;
                    }
                }
                if (NPC.ai[1] > 75)
                {
                    NPC.ai[1] = 0;
                    int teleportationProjectile = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<TeleportationBeam>(), 0, 0);
                    Main.projectile[teleportationProjectile].ai[1] = NPC.whoAmI;
                    Main.projectile[teleportationProjectile].timeLeft = 60;
                    Main.projectile[teleportationProjectile].GetGlobalProjectile<ProjectileFeatures>().hostileHoming = true;
                    Main.projectile[teleportationProjectile].GetGlobalProjectile<ProjectileFeatures>().featureSpeed = 12f;
                    NPC.ai[2]++;
                    if (NPC.ai[2] > 6)
                        SwitchAI();
                }
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
                NPC.ai[0] = AI_HOMINGFIREBALL;
            NPC.ai[1] = 0;
            NPC.ai[2] = 0;
        }
    }
}
