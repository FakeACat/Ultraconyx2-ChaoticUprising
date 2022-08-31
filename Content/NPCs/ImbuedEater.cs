using ChaoticUprising.Common.Systems;
using ChaoticUprising.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChaoticUprising.Content.NPCs
{
    public class ImbuedEater : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.CursedInferno
                }
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.EaterofSouls);
            AIType = NPCID.EaterofSouls;
            NPC.lifeMax = 15000;
            NPC.damage = 100;
            NPC.defense = 30;
            NPC.width = 54;
            NPC.height = 94;
            NPC.value = Item.buyPrice(0, 1, 0, 0);
            Main.npcFrameCount[NPC.type] = 2;
            AnimationType = NPCID.EaterofSouls;
            Banner = Type;
            BannerItem = ModContent.ItemType<ImbuedEaterBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.Add(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption);
            bestiaryEntry.Info.Add(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption);
            bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement("Pure, unfiltered corruption."));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (ChaosMode.chaosMode)
                return SpawnCondition.Corruption.Chance * ChaosMode.NormalSpawnMultiplier() / 4;
            return 0;
        }

        int projectileTimer = 0;
        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && Main.player[NPC.target] != null)
            {
                projectileTimer++;
                if (projectileTimer > 120)
                {
                    projectileTimer = 0;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 16, ProjectileID.CursedFlameHostile, 60, 1);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.CursedFlame, 1, 0, 3));
        }

        private static Asset<Texture2D> glowmask;

        public override void Load()
        {
            if (!Main.dedServ)
                glowmask = ModContent.Request<Texture2D>(Texture + "Glow");
        }

        public override void Unload()
        {
            glowmask = null;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(glowmask.Value, NPC.Center - screenPos + new Vector2(0, 2), NPC.frame, Color.White, NPC.rotation, new Vector2(NPC.width / 2, NPC.height / 2), NPC.scale, SpriteEffects.None, 0);
        }
    }
}
