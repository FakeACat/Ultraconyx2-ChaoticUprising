using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using ChaoticUprising.Common.GlobalNPCs;
using ChaoticUprising.Common;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;

namespace ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos
{
    [AutoloadBossHead]
    public class AbyssalShade : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.dontTakeDamage = false;
            NPC.aiStyle = -1;
            NPC.lifeMax = CUUtils.ConvenientBossHealthScaling(22500, 27500);
            NPC.damage = CUUtils.ConvenientBossDamageScaling(100, 150);
            NPC.defense = 60;
            NPC.width = 94;
            NPC.knockBackResist = 0f;
            NPC.height = 102;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.npcSlots = 1000000;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Hellish Intent");
            NPC.GetGlobalNPC<ChaosNPC>().shouldBeBuffedInChaosMode = false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
                new FlavorTextBestiaryInfoElement("An extension of the Abyssal Chaos. Given hundreds of years it may mature into a distinct entity.")
            });
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public Player Target()
        {
            return Main.player[NPC.target];
        }

        private const int AI_CONTACT = 0;
        private const int AI_CHARGE = 1;
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
                case AI_CONTACT:
                    AI_Contact();
                    break;
                case AI_CHARGE:
                    AI_Charge();
                    break;
                case AI_DESPAWN:
                    AI_Despawn();
                    break;
            }
        }

        private void AI_Contact()
        {
            NPC.rotation = NPC.velocity.X / 30;
            NPC.velocity += Vector2.Normalize(Target().Center - NPC.Center) * 0.6f;
            NPC.velocity *= 0.96f;
            NPC.ai[1]++;
            if (NPC.ai[1] > 480)
                SwitchAI();
        }

        private void AI_Charge()
        {
            NPC.ai[1]++;
            if (NPC.ai[1] >= 90)
            {
                NPC.velocity *= 0.94f;
                NPC.rotation = (Target().Center - NPC.Center).ToRotation() - (float)(Math.PI / 2);
                if (NPC.ai[1] >= 120)
                {
                    NPC.ai[1] = 0;
                    NPC.ai[2]++;
                    NPC.velocity = Vector2.Normalize(Target().Center - NPC.Center) * 30;
                    SoundEngine.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 0);
                    if (NPC.ai[2] > 6)
                        SwitchAI();
                }
            }
        }

        public void AI_Despawn()
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
                NPC.ai[0] = AI_CONTACT;
            NPC.ai[1] = 0;
            NPC.ai[2] = 0;
            NPC.ai[3] = 0;
        }
    }
}
