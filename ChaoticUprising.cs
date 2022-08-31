using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos;
using System.Collections.Generic;
using ChaoticUprising.Common.Systems;
using System;
using ChaoticUprising.Content.Items.Placeables;
using ChaoticUprising.Content.Items.Vanity;
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
                SkyManager.Instance["ChaoticUprising:NightmareReaper"] = new NightmareReaperSky();
            }
        }

        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("BossChecklist", out Mod checklist))
                checklist.Call("AddBoss", 
                    this, 
                    "Abyssal Chaos",
                    new List<int>() { ModContent.NPCType<AbyssalChaos>(), ModContent.NPCType<AbyssalShade>(), ModContent.NPCType<BloodlustEye>(), ModContent.NPCType<RavenousEye>() },
                    18f,
                    (Func<bool>)(() => ChaosMode.chaosMode),
                    true,
                    new List<int>() { ModContent.ItemType<AbyssalChaosRelic>(), ModContent.ItemType<AbyssalChaosTrophy>(), ModContent.ItemType<AbyssalChaosMask>() },
                    ModContent.ItemType<CorruptedSkull>(),
                    string.Format("Use a [i:{0}]\n[c/63445c:Activates Chaos Mode]", ModContent.ItemType<CorruptedSkull>())
                    );
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