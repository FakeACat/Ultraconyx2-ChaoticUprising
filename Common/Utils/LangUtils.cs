using Terraria.Localization;
using Terraria.ModLoader;

namespace ChaoticUprising.Common.Utils
{
    public class LangUtils
    {
        public static LocalizedText GetReusable(string key)
        {
            return Language.GetOrRegister("Mods.ChaoticUprising.Reusable." + key, () => "");
        }

        public static LocalizedText GetFormattedReusable(string key, params object[] objects)
        {
            return Language.GetOrRegister("Mods.ChaoticUprising.Reusable." + key, () => "").WithFormatArgs(objects);
        }

        public static string GetNPCName(int id)
        {
            return ModContent.GetModNPC(id).DisplayName.Value;
        }
    }
}
