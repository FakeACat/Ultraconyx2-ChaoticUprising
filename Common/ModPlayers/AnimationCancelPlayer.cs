using Terraria;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace ChaoticUprising.Common.ModPlayers
{
    public class AnimationCancelPlayer : ModPlayer
    {
        private SetBonusPlayer SetBonusPlayer => Player.GetModPlayer<SetBonusPlayer>();
        public bool CanAnimationCancel => SetBonusPlayer.voidTrespasser;

        public static bool Scrolling => !Main.mapFullscreen && !CaptureManager.Instance.Active && !Main.hairWindow && !Main.playerInventory && PlayerInput.ScrollWheelDelta != 0;
        public static bool TryingToSwitchItem
        {
            get
            {
                bool bl = PlayerInput.Triggers.Current.Hotbar1 ||
                    PlayerInput.Triggers.Current.Hotbar2 ||
                    PlayerInput.Triggers.Current.Hotbar3 ||
                    PlayerInput.Triggers.Current.Hotbar4 ||
                    PlayerInput.Triggers.Current.Hotbar5 ||
                    PlayerInput.Triggers.Current.Hotbar6 ||
                    PlayerInput.Triggers.Current.Hotbar7 ||
                    PlayerInput.Triggers.Current.Hotbar8 ||
                    PlayerInput.Triggers.Current.Hotbar9 ||
                    PlayerInput.Triggers.Current.Hotbar10;

                if (Scrolling)
                {
                    bl = true;
                }

                return bl;
            }
        }
        private bool switched = false;
        private int currentSlot = 0;

        public override void PreUpdate()
        {
            if (Main.myPlayer == Player.whoAmI && CanAnimationCancel)
            {
                bool bl = TryingToSwitchItem;

                if (bl && !switched)
                {
                    Player.SetItemAnimation(0);
                    switched = true;
                }

                if (!bl && switched)
                {
                    switched = false;
                }
            }

            currentSlot = Player.selectedItem;
        }

        public override void PostUpdate()
        {
            if (Main.myPlayer == Player.whoAmI && CanAnimationCancel && Scrolling && currentSlot == Player.selectedItem)
            {
                HandleHotbar();
            }
        }

        private void HandleHotbar()
        {
            int num = PlayerInput.Triggers.Current.HotbarPlus.ToInt() - PlayerInput.Triggers.Current.HotbarMinus.ToInt();
            if (PlayerInput.CurrentProfile.HotbarAllowsRadial && num != 0 && PlayerInput.Triggers.Current.HotbarHoldTime > PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired && PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired != -1)
            {
                PlayerInput.MiscSettingsTEMP.HotbarRadialShouldBeUsed = true;
                PlayerInput.Triggers.Current.HotbarScrollCD = 2;
            }
            if (PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired != -1)
            {
                num = PlayerInput.Triggers.JustReleased.HotbarPlus.ToInt() - PlayerInput.Triggers.JustReleased.HotbarMinus.ToInt();
                if (PlayerInput.Triggers.Current.HotbarScrollCD == 1 && num != 0)
                {
                    num = 0;
                }
            }
            if (PlayerInput.Triggers.Current.HotbarScrollCD == 0 && num != 0)
            {
                Player.HotbarOffset += num;
                PlayerInput.Triggers.Current.HotbarScrollCD = 8;
            }
            if (!Main.inFancyUI && !Main.ingameOptionsWindow)
            {
                Player.HotbarOffset += PlayerInput.ScrollWheelDelta / -120;
            }
            Player.ScrollHotbar(Player.HotbarOffset);
            while (Player.selectedItem > 9)
            {
                Player.selectedItem -= 10;
            }
            while (Player.selectedItem < 0)
            {
                Player.selectedItem += 10;
            }
            Player.HotbarOffset = 0;
        }
    }
}
