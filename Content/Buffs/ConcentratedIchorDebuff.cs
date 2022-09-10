using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Buffs
{
    public class ConcentratedIchorDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Concentrated Ichor");
            Description.SetDefault("You are defenseless");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ConcentratedIchorDebuffNPC>().concentratedIchor = true;
            npc.ichor = true;
        }
    }

    public class ConcentratedIchorDebuffNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool concentratedIchor;

        public override void ResetEffects(NPC npc)
        {
            concentratedIchor = false;
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (concentratedIchor)
                damage = (int)(damage * 1.75f);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (concentratedIchor)
                damage = (int)(damage * 1.75f);
        }
    }
}
