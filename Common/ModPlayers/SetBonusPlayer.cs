using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Common.ModPlayers
{
    public class SetBonusPlayer : ModPlayer
    {
        public bool voidTrespasser = false;

        private bool activatedSetBonus;
        public override void ResetEffects()
        {
            voidTrespasser = false;

            if ((Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[0] < 15) || 
                (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[1] < 15))
            {
                activatedSetBonus = true;
            }
            else
            {
                activatedSetBonus = false;
            }
        }

        public override void PostUpdate()
        {
            if (activatedSetBonus)
            {
                SetBonus();
            }
        }

        public virtual void SetBonus() { }
    }
}
