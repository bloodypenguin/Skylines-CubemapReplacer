using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SkyboxReplacer
{
    public class CubemapMonitor : MonoBehaviour
    {
        private Cubemap cachedSelectedDayCubemap;
        private Cubemap cachedSelectedNightCubemap;
        private Cubemap cachedSelectedOuterSpaceCubemap;

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
                cachedSelectedNightCubemap = selectedNightCubemap;
            }

            var selectedOuterSpaceCubemap = SkyboxReplacer.GetOuterSpaceCubemap();
            if (selectedOuterSpaceCubemap != cachedSelectedOuterSpaceCubemap)
            {
                Object.FindObjectOfType<DayNightProperties>().m_OuterSpaceCubemap = selectedNightCubemap;
                cachedSelectedOuterSpaceCubemap = selectedOuterSpaceCubemap;
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
            Object.FindObjectOfType<DayNightProperties>().m_OuterSpaceCubemap = cachedSelectedOuterSpaceCubemap;
            Shader.SetGlobalTexture("_EnvironmentCubemap", cachedSelectedDayCubemap);
        }
    }
}