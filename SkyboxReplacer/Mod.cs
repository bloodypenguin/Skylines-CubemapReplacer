
using ICities;
using SkyboxReplacer.OptionsFramework.Extensions;

namespace SkyboxReplacer
{
    public class Mod : IUserMod
    {
        public string Name => "Cubemap Replacer";
        public string Description => "Replaces skybox & window reflections";

        public void OnSettingsUI(UIHelperBase helper)
        {
            helper.AddOptionsGroup<Options>();
        }
    }
}
