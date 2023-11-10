using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ChaoticUprising.Content
{
    public class Effects
    {
        public static void Load()
        {
            // I did not create the shockwave effect, all credit goes to Kazzymodus ( https://forums.terraria.org/index.php?threads/tutorial-shockwave-effect-for-tmodloader.81685/ )
            Ref<Effect> screenRef = new(ModContent.Request<Effect>("ChaoticUprising/Assets/Effects/Shockwave", AssetRequestMode.ImmediateLoad).Value);

            LoadFilter("TerraSlimeShield", screenRef);
        }

        private static void LoadFilter(string name, Ref<Effect> screenRef)
        {
            Filters.Scene[name] = new Filter(new ScreenShaderData(screenRef, name), EffectPriority.VeryHigh);
            Filters.Scene[name].Load();
        }

        public static ScreenShaderData GetOrCreate(string effect, Vector2 location, out bool justCreated)
        {
            if (Filters.Scene[effect].IsActive())
            {
                justCreated = false;
                return Filters.Scene[effect].GetShader().UseTargetPosition(location);
            }
            else
            {
                justCreated = true;
                return Filters.Scene.Activate(effect, location).GetShader();
            }
        }
    }
}
