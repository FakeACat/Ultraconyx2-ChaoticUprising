﻿using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
//using SubworldLibrary;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticUprising.Content.NPCs
{
    public class Wormhole : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.dontTakeDamage = true;
            NPC.aiStyle = -1;
            NPC.lifeMax = 1;
            NPC.damage = 0;
            NPC.width = 64;
            NPC.height = 64;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
        }
        Vector2 pos = Vector2.Zero;
        public override void AI()
        {
            if (pos == Vector2.Zero)
            {
                pos = NPC.Center;
            }
            NPC.rotation -= 0.1f;
            for(int i = 0; i < 255; i++)
            {
                Player player = Main.player[i];
                if(player.active && !player.dead)
                {
                    if(Vector2.Distance(NPC.Center, player.Center) < 512)
                    {
                        player.velocity += Vector2.Normalize(NPC.Center - player.Center) / 2;
                        if (Vector2.Distance(NPC.Center, player.Center) < 32 && Main.netMode != NetmodeID.Server && player.whoAmI == Main.myPlayer)
                        {
                            //Subworld.Enter("ChaoticUprising_Abyss", true);
                            Main.NewText("Dimensions are not added yet.", 255, 255, 255);
                        }
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float layers = 20;
            float maxScale = 25;
            float rotation = NPC.rotation;
            for (int i = 1; i < layers; i++)
            {
                float scale = maxScale - i * maxScale / layers;
                Color colour = new Color(scale / maxScale, scale / maxScale, scale / maxScale);
                rotation *= 0.8f;
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/NPCs/Wormhole"), NPC.Center - Main.screenPosition, null, colour * ((float)i / layers) * 0.5f, rotation, new Vector2(32, 32), scale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
