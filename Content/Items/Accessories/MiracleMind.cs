using ChaoticUprising.Content.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Accessories
{
    public class MiracleMind : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Revives the user from death\nInflicts the Miracle debuff on revive\nPlayers that take damage while affected by the Miracle debuff will die instantly after it runs out\nPlayers that survive the Miracle debuff cannot be revived for 25 seconds");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.consumable = true;
            Item.width = 30;
            Item.height = 28;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
            Item.accessory = true;
            Item.value = Item.sellPrice(0, 45);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MiraclePlayer>().revivable = true;
        }
    }

    public class MiraclePlayer : ModPlayer
    {
        public bool revivable;
        public bool miracle;
        public bool miracleSickness;
        public bool diedDuringMiracle = false;

        public override void ResetEffects()
        {
            revivable = false;
            miracle = false;
            miracleSickness = false;
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (revivable && (!miracleSickness || miracle))
            {
                if (!miracleSickness)
                {
                    diedDuringMiracle = false;
                    Player.AddBuff(ModContent.BuffType<MiracleDebuff>(), 600);
                    Player.AddBuff(ModContent.BuffType<MiracleSickness>(), 2100);
                }
                Player.statLife = 100;
                return false;
            }
            return true;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (miracle && !diedDuringMiracle)
                diedDuringMiracle = true;
            return true;
        }

        public override void PreUpdate()
        {
            if (diedDuringMiracle && !miracle)
            {
                Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " did not survive the Miracle."), 1, 0);
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            diedDuringMiracle = false;
        }
    }
}
