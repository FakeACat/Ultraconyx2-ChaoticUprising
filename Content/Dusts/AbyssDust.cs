using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace ChaoticUprising.Content.Dusts
{
    public class AbyssDust : ModDust
    {
        public override void OnSpawn(Dust Dust)
        {
            Dust.frame = new Rectangle(0, 0, 8, 8);
            if (Main.rand.Next(2) == 0)
            {
                Dust.frame = new Rectangle(0, 8, 8, 8);
            }
            Dust.noGravity = true;
            Dust.rotation = Main.rand.NextFloat((float)(Math.PI) * 2);
        }
        public override bool Update(Dust Dust)
        {
            Dust.position += Dust.velocity;
            Dust.velocity *= 1.01f;
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
