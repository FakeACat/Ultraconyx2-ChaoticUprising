using ChaoticUprising.Content.Biomes.Darkness;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.Skies
{
    public class DarknessSky : CustomSky
    {
        bool active;
        float intensity;
        public override void Activate(Vector2 position, params object[] args)
        {
            active = true;
            intensity = 0.01f;
        }

        public override void Deactivate(params object[] args)
        {
            active = false;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("ChaoticUprising/Assets/Textures/BlankTexture"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
        }

        public override bool IsActive()
        {
            return active;
        }

        public override void Reset()
        {
            active = false;
            intensity = 0.01f;
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.InModBiome(ModContent.GetInstance<Darkness>()))
            {
                if (intensity < 1.0f)
                    intensity += 0.05f;
            }
            else
            {
                intensity -= 0.05f;
                if (intensity < 0)
                    Deactivate();
            }
        }

        public override Color OnTileColor(Color inColor)
        {
            return inColor * (1f - intensity);
        }
    }
}
