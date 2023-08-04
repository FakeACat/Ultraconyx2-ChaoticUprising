using ChaoticUprising.Common.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Abstract
{
    public abstract class DarknessItem : ModItem
    {
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            CUUtils.DrawWormhole(ModContent.Request<Texture2D>("ChaoticUprising/Assets/Textures/MiniBlackHole").Value, spriteBatch, Item.Center, 0.1f, 0.2f, 6.0f);
            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            CUUtils.DrawWormhole(ModContent.Request<Texture2D>("ChaoticUprising/Assets/Textures/MiniBlackHole").Value, spriteBatch, position, 0.045f, 0.5f, 6.0f, false);
            return true;
        }
    }
}
