using ChaoticUprising.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Biomes.Darkness
{
    public class OuterDarkness : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/Absent");
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Outer Darkness");
        }

        public override bool IsBiomeActive(Player player)
        {
            DarknessGeneration darknessGeneration = ModContent.GetInstance<DarknessGeneration>();
            Vector2 centre = new(darknessGeneration.darknessX * 16, darknessGeneration.darknessY * 16);

            return player.DistanceSQ(centre) < 36000000 && player.DistanceSQ(centre) >= 2560000;
        }

        public override void OnEnter(Player player)
        {
            if (!SkyManager.Instance["ChaoticUprising:Darkness"].IsActive() && player.whoAmI == Main.myPlayer)
                SkyManager.Instance.Activate("ChaoticUprising:Darkness");
        }
    }
}
