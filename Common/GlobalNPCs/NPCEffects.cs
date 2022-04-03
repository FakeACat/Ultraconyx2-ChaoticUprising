using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ChaoticUprising.Common.GlobalNPCs
{
    public class NPCEffects : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool trail = false;

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (trail)
            {
                Texture2D texture = (Texture2D)TextureAssets.Npc[npc.type];
                Vector2 Vector = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[npc.type] / 2);
                for (int i = 9; i >= 0; i--)
                {
                    Color Colour = npc.GetAlpha(drawColor);
                    Colour.R = (byte)(Colour.R * (10 - i) / 20);
                    Colour.G = (byte)(Colour.G * (10 - i) / 20);
                    Colour.B = (byte)(Colour.B * (10 - i) / 20);
                    Colour.A = (byte)(Colour.A * (10 - i) / 20);
                    Main.spriteBatch.Draw(texture, new Vector2(npc.oldPos[i].X - Main.screenPosition.X + (npc.width / 2) - texture.Width * npc.scale / 2f + Vector.X * npc.scale, npc.oldPos[i].Y - Main.screenPosition.Y + npc.height - texture.Height * npc.scale / Main.npcFrameCount[npc.type] + 4f + Vector.Y * npc.scale), new Microsoft.Xna.Framework.Rectangle?(npc.frame), Colour, npc.rotation, Vector, npc.scale, npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                }
            }
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
    }
}
