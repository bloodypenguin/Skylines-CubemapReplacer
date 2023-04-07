
using ICities;
using SkyboxReplacer.OptionsFramework.Extensions;

namespace SkyboxReplacer
{
    public class Mod : IUserMod
    {
        public string Name => "Reflection Texture Changer";
        public string Description => "Replaces the cubemap & reflections";

        public void OnSettingsUI(UIHelperBase helper)
        {
            helper.AddOptionsGroup<Options>();
        }
    }
}
