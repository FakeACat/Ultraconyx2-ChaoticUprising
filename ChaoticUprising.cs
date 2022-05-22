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

namespace ChaoticUprising
{
	public class ChaoticUprising : Mod
	{
        public override void Load()
        {
            if (!Main.dedServ)
            {
                SkyManager.Instance["ChaoticUprising:AbyssalChaos"] = new AbyssalChaosSky();
            }
        }

        public override void PostSetupContent()
        {
            Mod checklist = ModLoader.GetMod("BossChecklist");
            if (checklist != null)
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
    }
}