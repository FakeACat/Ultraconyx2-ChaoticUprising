using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChaoticUprising.Common.Systems
{
    public class DownedBosses : ModSystem
    {
        public static bool downedNightmareReaper = false;

        public override void OnWorldLoad()
        {
            downedNightmareReaper = false;
        }

        public override void OnWorldUnload()
        {
            downedNightmareReaper = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (downedNightmareReaper)
            {
                tag["downedNightmareReaper"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedNightmareReaper = tag.ContainsKey("downedNightmareReaper");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();

            flags[0] = downedNightmareReaper;

            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedNightmareReaper = flags[0];
        }
    }
}
