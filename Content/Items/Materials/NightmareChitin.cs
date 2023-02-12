using ChaoticUprising.Common;
using ChaoticUprising.Content.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Items.Materials
{
    public class NightmareChitin : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.rare = ModContent.RarityType<EarlyChaos>();
            Item.value = Item.sellPrice(0, 0, 75);
            Item.maxStack = 9999;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            CUUtils.DrawWormhole(ModContent.Request<Texture2D>("ChaoticUprising/Assets/Textures/MiniBlackHole").Value, spriteBatch, Item.Center, 0.1f, 0.2f, 6.0f);
            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            CUUtils.DrawWormhole(ModContent.Request<Texture2D>("ChaoticUprising/Assets/Textures/MiniBlackHole").Value, spriteBatch, position + new Vector2(frame.Width / 2, frame.Width / 2) * scale, 0.045f, 0.5f, 6.0f, false);
            return true;
        }
    }
}
