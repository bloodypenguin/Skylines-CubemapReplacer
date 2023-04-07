using ICities;
using SkyboxReplacer.OptionsFramework;
using UnityEngine;

namespace SkyboxReplacer
{
    public class LoadingExtension : LoadingExtensionBase
    {

        private GameObject _gameObject;
        public static bool inGame = false;
        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            inGame = true;
            SkyboxReplacer.Initialize();
            _gameObject = new GameObject("CubemapReplacerRedux");
            _gameObject.AddComponent<CubemapMonitor>();
        }

        public override void OnLevelUnloading()
        {
            inGame = false;
            SkyboxReplacer.Revert();
            var dayNightCycleMonitor = _gameObject.GetComponent<CubemapMonitor>();
            dayNightCycleMonitor.Update(); //let's make sure it runs at least once to set the cached values
            GameObject.Destroy(_gameObject);
        }
    }
}