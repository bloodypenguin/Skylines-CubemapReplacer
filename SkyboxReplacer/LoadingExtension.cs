using ICities;
using SkyboxReplacer.OptionsFramework;

namespace SkyboxReplacer
{
    public class LoadingExtension : LoadingExtensionBase
    {

        public static bool inGame = false;
        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            inGame = true;
            SkyboxReplacer.Initialize();

        }

        public override void OnLevelUnloading()
        {
            inGame = false;
            SkyboxReplacer.Revert();
        }
    }
}