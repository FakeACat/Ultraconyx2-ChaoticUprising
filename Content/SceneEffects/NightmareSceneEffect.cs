using ChaoticUprising.Common.Systems;
using ChaoticUprising.Content.NPCs.Minibosses.NightmareReaper;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.SceneEffects
{
    public class NightmareSceneEffect : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override int Music => Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<NightmareReaper>()) ? MusicLoader.GetMusicSlot(Mod, "Assets/Music/Terrible Day") : MusicLoader.GetMusicSlot(Mod, "Assets/Music/Absent");

        public override bool IsSceneEffectActive(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<NightmareReaper>()) || 
                ((int)ChaosMode.GetDifficulty()) >= ((int)Difficulty.Darkened))
            {
                return true;
            }
            return false;
        }

        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (!SkyManager.Instance["ChaoticUprising:NightmareReaper"].IsActive())
            {
                SkyManager.Instance.Activate("ChaoticUprising:NightmareReaper");
            }
        }
    }
}
