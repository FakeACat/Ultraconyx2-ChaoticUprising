using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using ChaoticUprising.Content.Dusts;
using Terraria.ID;

namespace ChaoticUprising.Content.Buffs
{
    public class AbyssalFlamesDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AbyssalFlamesDebuffPlayer>().abyssalFlames = true;
            int d = Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<AbyssDust>(), 0, 0, 0, default, 2);
            Main.dust[d].noGravity = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<AbyssalFlamesDebuffNPC>().abyssalFlames = true;
            int d = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<AbyssDust>(), 0, 0, 0, default, 2);
            Main.dust[d].noGravity = true;
        }
    }

    public class AbyssalFlamesDebuffPlayer : ModPlayer
    {
        public bool abyssalFlames;

        public override void ResetEffects()
        {
            abyssalFlames = false;
        }

        public override void UpdateBadLifeRegen()
        {
            if (abyssalFlames)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 16;
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (abyssalFlames)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " disintegrated.");
            }
            return true;
        }
    }

    public class AbyssalFlamesDebuffNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool abyssalFlames;

        public override void ResetEffects(NPC npc)
        {
            abyssalFlames = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (abyssalFlames)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 32;
                if (damage < 1)
                {
                    damage = 1;
                }
            }
        }
    }
}
