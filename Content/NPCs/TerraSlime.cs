using ChaoticUprising.Common;
using ChaoticUprising.Common.Systems;
using ChaoticUprising.Common.Utils;
using ChaoticUprising.Content.Items.Materials;
using ChaoticUprising.Content.Items.Placeables.Banners;
using ChaoticUprising.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChaoticUprising.Content.NPCs
{
    public class TerraSlime : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 20000;
            NPC.damage = 80;
            NPC.defense = 30;
            NPC.width = 100;
            NPC.height = 66;
            NPC.value = Item.buyPrice(0, 0, 50, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Main.npcFrameCount[NPC.type] = 2;
            AnimationType = NPCID.BlueSlime;
            NPC.knockBackResist = 0.3f;
            NPC.alpha = 50;
            Banner = Type;
            BannerItem = ModContent.ItemType<TerraSlimeBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.Add(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
            bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement("A servant of the Slime Progenitor, reawoken by the death of the Abyssal Chaos."));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (ChaosMode.chaosMode)
                return SpawnCondition.OverworldDay.Chance * ChaosMode.NormalSpawnMultiplier() / 4;
            return 0;
        }
        private const int ShieldSizeInTiles = 12;
        private const int ShieldSizeSq = (ShieldSizeInTiles * 16) * (ShieldSizeInTiles * 16);
        private const int ShieldTime = 600;
        private const int ProjectileTime = 300;
        private bool ShieldUp => NPC.ai[0] < ShieldTime;

        public override void AI()
        {
            if (CUUtils.Client)
            {
                var shader = Effects.GetOrCreate("TerraSlimeShield", NPC.Center, out bool justCreated);
                if (justCreated)
                {
                    shader.UseColor(10, 5, 20).UseProgress(1).UseOpacity(100);
                }
            }

            if (!CUUtils.TryFindTarget(NPC)) return;

            NPC.ai[0]++;
            if (NPC.ai[0] > ShieldTime + ProjectileTime)
            {
                NPC.ai[0] = 0;
            }

            if (NPC.velocity.Y == 0)
            {
                NPC.velocity.Y = -16;
                NPC.velocity.X = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center).X * 12;

                if (!ShieldUp)
                {
                    int numProj = 6;
                    float speed = 30;
                    for (int i = 0; i < numProj; i++)
                    {
                        int type = ModContent.ProjectileType<TerraTentacle>();
                        int damage = 50;
                        float rotation = (MathHelper.PiOver4 / (numProj - 1) * i) - MathHelper.Pi / 8 + (float)Math.Atan2(NPC.Center.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), NPC.Center.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Cos(rotation) * speed * -1) / 5, (float)(Math.Sin(rotation) * speed * -1) / 5, type, damage, 1.0f);
                    }
                }
            }

            NPC.velocity.X += Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center).X / 8;
            NPC.velocity *= 0.97f;

            if (ShieldUp)
            {
                if (CUUtils.Client)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Dust.NewDust(NPC.Center + Main.rand.NextFloat((float)Math.PI * 2).ToRotationVector2() * ShieldSizeInTiles * 16, 0, 0, DustID.Terra);
                    }
                }

                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && 
                        proj.friendly && 
                        proj.DistanceSQ(NPC.Center) < ShieldSizeSq && 
                        proj.GetGlobalProjectile<TerraSlimeProjectile>().Origin.DistanceSQ(NPC.Center) > ShieldSizeSq)
                    {
                        proj.Kill();
                    }
                }
            }
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (!ShieldUp) return null;

            Player owner = Main.player[projectile.owner];
            if (owner == null)
            {
                return false;
            }
            return owner.DistanceSQ(NPC.Center) < ShieldSizeSq;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Terragel>()));
        }
    }

    public class TerraSlimeProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int originalX;
        public int originalY;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            originalX = (int)projectile.Center.X;
            originalY = (int)projectile.Center.Y;
        }

        public Vector2 Origin => new(originalX, originalY);
    }
}
