using ChaoticUprising.Common;
using ChaoticUprising.Common.Systems;
using ChaoticUprising.Content.Items.Materials;
using ChaoticUprising.Content.Items.Placeables.Banners;
using ChaoticUprising.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
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
            NPC.aiStyle = 1;
            NPC.lifeMax = 10000;
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

        public override void AI()
        {
            if ((!Main.player[NPC.target].active || Main.player[NPC.target].dead) && NPC.aiStyle != 1)
                NPC.aiStyle = 1;
            else if (NPC.life < NPC.lifeMax)
            {
                NPC.aiStyle = -1;

                if (NPC.velocity.Y == 0)
                {
                    NPC.velocity.Y = -16;
                    NPC.velocity.X = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center).X * 12;

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
                NPC.velocity.X += Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center).X / 8;
                NPC.velocity *= 0.97f;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Terragel>()));
        }
    }
}
