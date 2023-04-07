using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SkyboxReplacer
{
    public class CubemapMonitor : MonoBehaviour
    {
        private Cubemap cachedSelectedDayCubemap;
        private Cubemap cachedSelectedNightCubemap;

        public void Update()
        {
            var selectedDayCubemap = SkyboxReplacer.GetDayCubemap();
            if (selectedDayCubemap != cachedSelectedDayCubemap)
            {
                cachedSelectedDayCubemap = selectedDayCubemap;
            }
            
            var selectedNightCubemap = SkyboxReplacer.GetNightCubemap();
            if (selectedNightCubemap != cachedSelectedNightCubemap)
            {
                Object.FindObjectOfType<DayNightProperties>().m_OuterSpaceCubemap = selectedNightCubemap;
                cachedSelectedNightCubemap = selectedNightCubemap;
            }

            if (SimulationManager.instance.m_isNightTime)
            {
                Shader.SetGlobalTexture("_EnvironmentCubemap", cachedSelectedNightCubemap);
            }
            else
            {
                Shader.SetGlobalTexture("_EnvironmentCubemap", cachedSelectedDayCubemap);
            }
        }

        public void OnDestroy()
        {
            Object.FindObjectOfType<DayNightProperties>().m_OuterSpaceCubemap = cachedSelectedNightCubemap;
            Shader.SetGlobalTexture("_EnvironmentCubemap", cachedSelectedDayCubemap);
        }
    }
}