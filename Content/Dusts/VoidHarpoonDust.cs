using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Dusts
{
    public class VoidHarpoonDust : ModDust
    {
        public override void OnSpawn(Dust Dust)
        {
            Dust.frame = new Rectangle(0, 0, 10, 10);
            Dust.noGravity = true;
            Dust.rotation = Main.rand.NextFloat((float)Math.PI * 2);
        }
        public override bool Update(Dust Dust)
        {
            Dust.position += Dust.velocity;
            Dust.velocity *= 0.98f;
            Dust.rotation += 0.1f;
            Dust.alpha += 10;
            if (Dust.alpha >= 255)
            {
                Dust.active = false;
            }
            return false;
        }
    }
}
