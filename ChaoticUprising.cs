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
using ChaoticUprising.Content.NPCs.Minibosses.NightmareReaper;
using ChaoticUprising.Content.Items.Pets;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
            {
                checklist.Call("AddBoss",
                    this,
                    "Abyssal Chaos",
                    new List<int>() { ModContent.NPCType<AbyssalChaos>(), ModContent.NPCType<AbyssalShade>(), ModContent.NPCType<BloodlustEye>(), ModContent.NPCType<RavenousEye>() },
                    19f,
                    () => ChaosMode.chaosMode,
                    true,
                    new List<int>() { ModContent.ItemType<AbyssalChaosRelic>(), ModContent.ItemType<AbyssalChaosTrophy>(), ModContent.ItemType<AbyssalChaosMask>(), ModContent.ItemType<AbyssalSkull>(), ModContent.ItemType<AbyssalChaosMusicBox>() },
                    ModContent.ItemType<CorruptedSkull>(),
                    string.Format("Use a [i:{0}].\n[c/63445c:Activates Chaos Mode.]", ModContent.ItemType<CorruptedSkull>())
                    );

                checklist.Call("AddMiniBoss",
                    this,
                    "Void-dropped Nightmare",
                    ModContent.NPCType<NightmareReaper>(),
                    19.5f,
                    () => DownedBosses.downedNightmareReaper,
                    true,
                    new List<int>() { ModContent.ItemType<ReaperEssence>() },
                    null,
                    "Enter the Darkness biome in Chaos Mode.",
                    null,
                    (SpriteBatch sb, Rectangle rect, Color color) => {
                        Texture2D texture = ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Minibosses/NightmareReaper/NightmareReaperImage").Value;
                        Vector2 centered = new(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                    );
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