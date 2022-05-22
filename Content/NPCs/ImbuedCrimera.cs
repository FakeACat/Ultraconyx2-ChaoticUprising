using ChaoticUprising.Common.Systems;
using ChaoticUprising.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChaoticUprising.Content.NPCs
{
    public class ImbuedCrimera : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Ichor
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.EaterofSouls);
            AIType = NPCID.Crimera;
            NPC.lifeMax = 15000;
            NPC.damage = 100;
            NPC.defense = 30;
            NPC.width = 42;
            NPC.height = 66;
            NPC.value = Item.buyPrice(0, 1, 0, 0);
            Main.npcFrameCount[NPC.type] = 2;
            AnimationType = NPCID.Crimera;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.Add(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson);
            bestiaryEntry.Info.Add(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson);
            bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement("Pure, unfiltered crimson."));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (ChaosMode.chaosMode)
                return SpawnCondition.Crimson.Chance * ChaosMode.NormalSpawnMultiplier() / 4;
            return 0;
        }

        int projectileTimer = 0;
        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && Main.player[NPC.target] != null)
            {
                projectileTimer++;
                if (projectileTimer > 100)
                {
                    projectileTimer = 0;
                    for (int i = 0; i < 3; i++)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 6 + new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), ProjectileID.GoldenShowerHostile, 60, 1);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Ichor, 1, 0, 3));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<BladeofFlesh>(), 200, 100));
        }
    }
}
