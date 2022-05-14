using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos;

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
    }
}