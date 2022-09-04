using ChaoticUprising.Common.Systems;
using ChaoticUprising.Content.NPCs;
using ChaoticUprising.Content.NPCs.Minibosses.NightmareReaper;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Biomes.Darkness
{
    public class Darkness : ModBiome
    {
        public virtual int SizeSQ => DarknessGeneration.outerDarknessSize * DarknessGeneration.outerDarknessSize * 256;
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/Absent");
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Darkness");
        }

        public override bool IsBiomeActive(Player player)
        {
            DarknessGeneration darknessGeneration = ModContent.GetInstance<DarknessGeneration>();
            Vector2 centre = new(darknessGeneration.darknessX * 16, darknessGeneration.darknessY * 16);

            return player.DistanceSQ(centre) < SizeSQ;
        }

        public override void OnEnter(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (!SkyManager.Instance["ChaoticUprising:Darkness"].IsActive())
                    SkyManager.Instance.Activate("ChaoticUprising:Darkness");

                if (!NPC.AnyNPCs(ModContent.NPCType<NightmareReaper>()) && !DownedBosses.downedNightmareReaper && ChaosMode.chaosMode)
                {
                    SoundEngine.PlaySound(SoundID.Roar, player.position);
                    int type = ModContent.NPCType<NightmareReaper>();
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        NPC.SpawnOnPlayer(player.whoAmI, type);
                    else
                        NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
                }
            }
        }

        public override void OnInBiome(Player player)
        {
            DarknessGeneration darknessGeneration = ModContent.GetInstance<DarknessGeneration>();
            if (!NPC.AnyNPCs(ModContent.NPCType<Wormhole>()) && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(Entity.GetSource_NaturalSpawn(), darknessGeneration.darknessX * 16, darknessGeneration.darknessY * 16, ModContent.NPCType<Wormhole>());
            }
        }
    }

    public class InnerDarkness : Darkness
    {
        public override int SizeSQ => DarknessGeneration.innerDarknessSize * DarknessGeneration.innerDarknessSize * 256;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/Devoid");
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
    }
}
