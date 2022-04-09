using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace ChaoticUprising.Content.NPCs.Bosses.AbyssalChaos
{
    public class AbyssalChaosSky : CustomSky
    {
        bool active;
        float intensity;
        float maxIntensity = 0.6f;
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
            if (NPC.AnyNPCs(ModContent.NPCType<AbyssalChaos>()))
            {
                if (intensity < maxIntensity)
                    intensity += 0.01f;
            }
            else
            {
                intensity -= 0.01f;
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
