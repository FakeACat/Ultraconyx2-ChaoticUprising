using ChaoticUprising.Common.Systems;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Common.GlobalNPCs
{
    public class ChaosModeChanges : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (ChaosMode.chaosMode)
            {
                int difficulty = (int)ChaosMode.GetDifficulty();
                spawnRate = (int)(spawnRate * Math.Pow(0.8d, difficulty));
                maxSpawns = (int)(maxSpawns * Math.Pow(1.2d, difficulty));
            }
        }
    }
}
