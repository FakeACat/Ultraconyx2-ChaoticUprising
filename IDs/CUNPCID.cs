using ChaoticUprising.Content.NPCs;
using ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos;
using ChaoticUprising.Content.NPCs.Darkness;
using ChaoticUprising.Content.NPCs.Minibosses.NightmareReaper;
using Terraria.ModLoader;

namespace ChaoticUprising.IDs
{
    public static class CUNPCID
    {
        // Abyssal Chaos
        public static readonly int AbyssalChaos = ModContent.NPCType<AbyssalChaos>();
        public static readonly int AbyssalShade = ModContent.NPCType<AbyssalShade>();
        public static readonly int BloodlustEye = ModContent.NPCType<BloodlustEye>();
        public static readonly int RavenousEye = ModContent.NPCType<RavenousEye>();

        // Nightmare/Darkness
        public static readonly int NightmareToad = ModContent.NPCType<NightmareToad>();
        public static readonly int NightmareReaper = ModContent.NPCType<NightmareReaper>();

        // World Evil
        public static readonly int ImbuedCrimera = ModContent.NPCType<ImbuedCrimera>();
        public static readonly int ImbuedEater = ModContent.NPCType<ImbuedEater>();

        // Slimes
        public static readonly int SlimePraetorian = ModContent.NPCType<SlimePraetorian>();
        public static readonly int TerraSlime = ModContent.NPCType<TerraSlime>();

        // Misc
        public static readonly int Wormhole = ModContent.NPCType<Wormhole>();
    }
}
