using ChaoticUprising.Common.Systems;
using ChaoticUprising.Content.NPCs.Minibosses.NightmareReaper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChaoticUprising.Content.Skies
{
    public class ChaosModeSky : CustomSky
    {
        bool active;
        public float intensity;
        public float smogIntensity;
        private static float MaxIntensity => ChaosMode.GetDifficulty() == Difficulty.Abyssal ? 0.95f : 0.75f;
        public override void Activate(Vector2 position, params object[] args)
        {
            active = true;
            intensity = 0.01f;
            smogIntensity = 0.0f;
        }

        public override void Deactivate(params object[] args)
        {
            active = false;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0f && minDepth < 0f)
            {
                DrawInfestationSmog(spriteBatch);
                spriteBatch.Draw(ModContent.Request<Texture2D>("ChaoticUprising/Assets/Textures/BlankTexture").Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
            }
        }

        public override bool IsActive()
        {
            return active;
        }

        public override void Reset()
        {
            active = false;
            intensity = 0.01f;
            smogIntensity = 0.0f;
        }

        public override void Update(GameTime gameTime)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<NightmareReaper>()) ||
                ((int)ChaosMode.GetDifficulty()) >= ((int)Difficulty.Darkened))
            {
                if (intensity < MaxIntensity)
                    intensity += 0.01f;
                else if (intensity > MaxIntensity)
                    intensity -= 0.01f;
            }
            else
            {
                intensity -= 0.01f;
                if (intensity < 0)
                    Deactivate();
            }
            if (((int)ChaosMode.GetDifficulty()) >= ((int)Difficulty.Infested))
            {
                UpdateInfestationSmog();
                if (smogIntensity < 1.0f)
                    smogIntensity += 0.01f;
            }
            else
            {
                if (smogIntensity > 0)
                    smogIntensity -= 0.01f;
            }
        }

        public override Color OnTileColor(Color inColor)
        {
            return inColor * (1f - intensity);
        }

        List<SmogParticle> particles = new();
        class SmogParticle
        {
            internal Vector2 position = Vector2.Zero;
            internal Vector2 realStartingPosition;
            internal Texture2D texture = ModContent.Request<Texture2D>("ChaoticUprising/Content/Skies/AlienPollution").Value;
            internal int alpha;
            internal bool fade;
            internal int colour;
            internal float distance;
            internal void Setup()
            {
                alpha = 255;
                fade = false;
                colour = Main.rand.Next(155, 256);
                distance = Main.rand.Next(20, 41) / 10;
            }
            internal void Update()
            {
                position.Y -= 1 / distance;
                position.X += Main.windSpeedCurrent / distance;
                alpha += fade ? 1 : -1;
                if (alpha <= 0)
                    fade = true;
            }
            internal void Draw(SpriteBatch spriteBatch, float smogIntensity)
            {
                Vector2 drawPos = position - new Vector2(64) - new Vector2(Main.player[Main.myPlayer].Center.X - realStartingPosition.X, Main.player[Main.myPlayer].Center.Y - realStartingPosition.Y) / distance;
                spriteBatch.Draw(texture, new Rectangle((int)drawPos.X, (int)drawPos.Y, (int)(128 / distance) * 3, (int)(128 / distance) * 3), new Color(colour, colour, colour) * (1 - ((float)alpha / 255)) * 0.5f * smogIntensity);
            }
        }

        private void UpdateInfestationSmog()
        {
            foreach (SmogParticle smog in particles)
            {
                smog.Update();
            }
            if (Main.netMode != NetmodeID.Server)
            {
                SmogParticle spawnedSmog = new SmogParticle();
                spawnedSmog.Setup();
                spawnedSmog.position = new Vector2(Main.rand.Next(-320, Main.screenWidth + 320), Main.rand.Next(-320, Main.screenHeight + 320));
                spawnedSmog.realStartingPosition = Main.player[Main.myPlayer].Center;

                particles.Add(spawnedSmog);
            }

            if (particles.Count > 0)
                while (particles[0].alpha >= 255 && particles[0].fade == true)
                    particles.RemoveAt(0);
        }

        private void DrawInfestationSmog(SpriteBatch spriteBatch)
        {
            foreach (SmogParticle s in particles)
            {
                s.Draw(spriteBatch, smogIntensity);
            }
        }
    }
}
