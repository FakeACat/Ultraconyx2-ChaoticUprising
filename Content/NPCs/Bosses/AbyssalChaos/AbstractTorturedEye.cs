using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using ChaoticUprising.Common.GlobalNPCs;

namespace ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos
{
    public abstract class AbstractTorturedEye : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 15000;
            NPC.damage = 100;
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
            NPC.GetGlobalNPC<ChaosNPC>().shouldBeBuffedInChaosMode = false;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(20000 * bossLifeScale);
            NPC.damage = 150;
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
