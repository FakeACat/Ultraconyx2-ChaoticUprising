using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;

namespace ChaoticUprising.Content.Walls
{
    public class NightRockWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(80, 80, 100));
        }
    }
}
