using ChaoticUprising.Content.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Buffs
{
    public class MiniShadeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Miniature Abyssal Shade");
            Description.SetDefault("A small Abyssal Shad is following you.");

            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            int projType = ModContent.ProjectileType<MiniShade>();
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] <= 0)
            {
                var entitySource = player.GetSource_Buff(buffIndex);
                Projectile.NewProjectile(entitySource, player.Center, Vector2.Zero, projType, 0, 0f, player.whoAmI);
            }
        }
    }
}
