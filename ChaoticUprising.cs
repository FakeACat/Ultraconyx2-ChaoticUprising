using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using ChaoticUprising.Content.Items.Consumables;
using System.IO;
using ChaoticUprising.Content.Skies;

namespace ChaoticUprising
{
	public class ChaoticUprising : Mod
	{
        public override void Load()
        {
            if (!Main.dedServ)
            {
                SkyManager.Instance["ChaoticUprising:AbyssalChaos"] = new AbyssalChaosSky();
                SkyManager.Instance["ChaoticUprising:Darkness"] = new DarknessSky();
                SkyManager.Instance["ChaoticUprising:ChaosMode"] = new ChaosModeSky();
            }
        }

        internal enum PacketType : byte
        {
            LifeforceRelicsSync
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            PacketType type = (PacketType)reader.ReadByte();

            switch (type)
            {
                case PacketType.LifeforceRelicsSync:
                    byte playernumber = reader.ReadByte();
                    LifeforceRelicPlayer player = Main.player[playernumber].GetModPlayer<LifeforceRelicPlayer>();
                    player.lifeforceRelics = reader.ReadInt32();
                    break;
            }
        }
    }
}