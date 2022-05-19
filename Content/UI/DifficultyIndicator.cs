using ChaoticUprising.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace ChaoticUprising.Content.UI
{
    public class DifficultyIndicator : UIState
    {
        private UIImage outline;
        float outlineX = 24;
        float outlineY = 256;
        float outlineWidth = 58;
        float outlineHeight = 260;
        float outlineThickness = 16;

        private Bar bar = new Bar();

        private UIText text;

        public override void OnInitialize()
        {
            bar.Left.Set(outlineX + outlineThickness, 0f);
            bar.Top.Set(outlineY + outlineThickness, 0f);
            bar.Width.Set(outlineWidth - 2 * outlineThickness, 0f);
            bar.Height.Set(outlineHeight - 2 * outlineThickness, 0f);
            Append(bar);

            text = new UIText("Current Difficulty: "); 
            text.Width.Set(138, 0f);
            text.Height.Set(34, 0f);
            text.Left.Set(outlineX + outlineWidth, 0f);
            text.Top.Set(outlineY + outlineHeight / 2, 0f);
            Append(text);

            outline = new(ModContent.Request<Texture2D>("ChaoticUprising/Content/UI/DifficultyIndicator"));
            outline.Left.Set(outlineX, 0f);
            outline.Top.Set(outlineY, 0f);
            outline.Width.Set(outlineWidth, 0f);
            outline.Height.Set(outlineHeight, 0f);
            Append(outline);
        }

        public override void Update(GameTime gameTime)
        {
            text.SetText("Current Difficulty: " + ChaosMode.GetDifficulty().ToString());
            base.Update(gameTime);
        }
    }

    public class Bar : UIElement
    {
        float barLengthInDifficulty = 2.0f;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            DrawDif(spriteBatch, -1.0f, new Color(40, 40, 40));
            DrawDif(spriteBatch, 0.0f, new Color(0, 255, 0));
            DrawDif(spriteBatch, 1.0f, new Color(255, 255, 0));
            DrawDif(spriteBatch, 2.0f, new Color(255, 0, 0));
            DrawDif(spriteBatch, 3.0f, new Color(40, 40, 40));
        }

        protected void DrawDif(SpriteBatch spriteBatch, float min, Color colour)
        {
            Rectangle rect = GetInnerDimensions().ToRectangle();
            rect.Height /= 2;
            rect.Y += (int)(ChaosMode.difficulty / barLengthInDifficulty * GetInnerDimensions().Height - (GetInnerDimensions().Height / barLengthInDifficulty * min));
            int num1 = rect.Bottom - GetInnerDimensions().ToRectangle().Bottom;
            if (num1 > 0)
                rect.Height -= num1;
            int num2 = rect.Top - GetInnerDimensions().ToRectangle().Top;
            if (num2 < 0)
            {
                rect.Height += num2;
                rect.Y -= num2;
            }
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Content/UI/Bar"), rect, colour);
        }
    }
}
