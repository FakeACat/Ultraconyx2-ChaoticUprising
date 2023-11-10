using ChaoticUprising.Common.ModPlayers;
using ChaoticUprising.Content.Items.Armour.VoidTrespasser;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Common.GlobalItems
{
    public class HealingItemModifications : GlobalItem
    {
        public override void OnConsumeItem(Item item, Player player)
        {
            if (item.healLife > 0)
            {
                OnUseHealingItem(item, player);
            }
        }

        private static void OnUseHealingItem(Item item, Player player)
        {
            if (player.GetModPlayer<SetBonusPlayer>().voidTrespasser)
            {
                player.SetImmuneTimeForAllTypes(240);
            }
        }
    }
}
