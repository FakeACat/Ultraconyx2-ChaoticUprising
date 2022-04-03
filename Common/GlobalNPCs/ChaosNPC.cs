using Terraria.ModLoader;

namespace ChaoticUprising.Common.GlobalNPCs
{
    public class ChaosNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool shouldBeBuffedInChaosMode = true;
    }
}
