using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using ChaoticUprising.Common.Utils;

namespace ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos
{
    public abstract class AbstractTorturedEye : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = CUUtils.ConvenientBossHealth(15000, 20000);
            NPC.damage = CUUtils.ConvenientBossDamage(100, 150, false);
            NPC.defense = 50;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 1;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Hellish Intent");
            Main.npcFrameCount[NPC.type] = 4;
            NPC.boss = true;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0 && NPC.AnyNPCs(ModContent.NPCType<AbyssalChaos>()))
            {
                NPC boss = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<AbyssalChaos>())];
                boss.localAI[0] = 1;
            }
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }
        int frame = 0;
        int frameChange = 0;
        public override void FindFrame(int frameHeight)
        {
            frameChange++;
            if (frameChange >= 8)
            {
                if (frame < 3)
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
    }
}
