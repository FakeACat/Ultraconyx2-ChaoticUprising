﻿using ChaoticUprising.Common.GlobalNPCs;
using ChaoticUprising.Common.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Projectiles
{
    public class VoidHarpoonFriendly : VoidHarpoon
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Void Harpoon");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 1)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 5;
                }
            }
            else
            {
                base.AI();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] == 1)
            {
                CUUtils.DrawWormhole(ModContent.Request<Texture2D>("ChaoticUprising/Assets/Textures/MiniBlackHole").Value, Main.spriteBatch, Projectile.Center, 0.1f, 0.5f * (255.0f - Projectile.alpha) / 255.0f, 6.0f);
                return false;
            }
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 1)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active)
                        Main.npc[i].GetGlobalNPC<NPCEffects>().blackHole = 0;
                }
                target.GetGlobalNPC<NPCEffects>().blackHole = 360;
            }
        }

        public override string Texture => "ChaoticUprising/Content/Projectiles/VoidHarpoon";
    }
}
