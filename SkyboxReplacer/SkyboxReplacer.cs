using System;
using System.IO;
using SkyboxReplacer.Configuration;
using SkyboxReplacer.OptionsFramework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SkyboxReplacer
{
    public class SkyboxReplacer
    {
        public const string Vanilla = "vanilla";

        private static Cubemap vanillaDayCubemap;
        private static Cubemap customDayCubemap;
        private static float defaultHorizon;
        private static float defaultFog;

        public static void Initialize()
        {
            vanillaDayCubemap = Object.FindObjectOfType<RenderProperties>().m_cubemap;
            defaultFog = Object.FindObjectOfType<RenderProperties>().m_fogHeight;
            defaultHorizon = Object.FindObjectOfType<FogProperties>().m_HorizonHeight;
            MinimizeHorizon(OptionsWrapper<Options>.Options.MinimizeHorizon);
            SetDayCubemap(OptionsWrapper<Options>.Options.CubemapDay);
            SetNightCubemap(OptionsWrapper<Options>.Options.CubemapNight);
        }

        public static void Revert()
        {
            RevertDayCubemap();
            RevertNightCubemap();
        }

        public static void SetDayCubemap(string code)
        {
            if (!LoadingExtension.inGame)
            {
                return;
            }
            if (Vanilla.Equals(code))
            {
                RevertDayCubemap();
                return;
            }
            ReplaceCubemap(CubemapManager.GetDayReplacement(code));
        }

        public static void SetNightCubemap(string code)
        {
            if (!LoadingExtension.inGame)
            {
                return;
            }
            if (Vanilla.Equals(code))
            {
                RevertNightCubemap();
                return;
            }
            ReplaceCubemap(CubemapManager.GetNightReplacement(code));
        }

        private static void RevertDayCubemap()
        {
            if (customDayCubemap == null)
            {
                return;
            }
            GameObject.Destroy(customDayCubemap);
            customDayCubemap = null;
            Shader.SetGlobalTexture("_EnvironmentCubemap", vanillaDayCubemap);
        }

        private static void RevertNightCubemap()
        {
            //TODO(earalov): implement
        }

        public static void ReloadSelectedCubemaps()
        {
            SetDayCubemap(OptionsWrapper<Options>.Options.CubemapDay);
            SetNightCubemap(OptionsWrapper<Options>.Options.CubemapNight);
        }

        private static void ReplaceCubemap(CubemapReplacement replacement)
        {
            if (replacement.isNight)
            {
                RevertNightCubemap();
            }
            else
            {
                RevertDayCubemap();
            }
            var cubemap = new Cubemap(replacement.size, TextureFormat.ARGB32, true)
            {
                name = "CubemapReplacerCubemap",
                wrapMode = TextureWrapMode.Clamp
            };
            var prefix = replacement.filePrefix;
            if (replacement.splitFormat)
            {
                var posx = Util.LoadTextureFromFile(Path.Combine(replacement.directory, prefix + "posx.png"));
                SetCubemapFaceSolid(posx, CubemapFace.PositiveX, cubemap, 2, 0);
                Object.Destroy(posx);
                var posy = Util.LoadTextureFromFile(Path.Combine(replacement.directory, prefix + "posy.png"));
                SetCubemapFaceSolid(posy, CubemapFace.PositiveY, cubemap, 2, 0);
                Object.Destroy(posy);
                var posz = Util.LoadTextureFromFile(Path.Combine(replacement.directory, prefix + "posz.png"));
                SetCubemapFaceSolid(posz, CubemapFace.PositiveZ, cubemap, 2, 0);
                Object.Destroy(posz);
                var negx = Util.LoadTextureFromFile(Path.Combine(replacement.directory, prefix + "negx.png"));
                SetCubemapFaceSolid(negx, CubemapFace.NegativeX, cubemap, 2, 0);
                Object.Destroy(negx);
                var negy = Util.LoadTextureFromFile(Path.Combine(replacement.directory, prefix + "negy.png"));
                SetCubemapFaceSolid(negy, CubemapFace.NegativeY, cubemap, 2, 0);
                Object.Destroy(negy);
                var negz = Util.LoadTextureFromFile(Path.Combine(replacement.directory, prefix + "negz.png"));
                SetCubemapFaceSolid(negz, CubemapFace.NegativeZ, cubemap, 2, 0);
                Object.Destroy(negz);
            }
            else
            {
                var texture = Util.LoadTextureFromFile(Path.Combine(replacement.directory, prefix + "cubemap.png"));
                SetCubemapFaceSolid(texture, CubemapFace.PositiveX, cubemap, 1, 2);
                SetCubemapFaceSolid(texture, CubemapFace.PositiveY, cubemap, 0, 1);
                SetCubemapFaceSolid(texture, CubemapFace.PositiveZ, cubemap, 1, 1);
                SetCubemapFaceSolid(texture, CubemapFace.NegativeX, cubemap, 1, 0);
                SetCubemapFaceSolid(texture, CubemapFace.NegativeY, cubemap, 2, 1);
                SetCubemapFaceSolid(texture, CubemapFace.NegativeZ, cubemap, 1, 3);
                Object.Destroy(texture);
            }
            cubemap.anisoLevel = 9;
            cubemap.filterMode = FilterMode.Trilinear;
            cubemap.SmoothEdges();
            cubemap.Apply();
            if (replacement.isNight)
            {
                //TODO(earalov): implement
                Object.Destroy(cubemap);
            }
            else
            {
                Shader.SetGlobalTexture("_EnvironmentCubemap", cubemap);
                customDayCubemap = cubemap;
            }
        }

        public static void MinimizeHorizon(bool minimize)
        {
            if (!LoadingExtension.inGame)
            {
                return;
            }
            Object.FindObjectOfType<FogProperties>().m_HorizonHeight = minimize ? Single.Epsilon : defaultHorizon;
            Object.FindObjectOfType<RenderProperties>().m_fogHeight = minimize ? Single.Epsilon : defaultFog;
        }

        private static void SetCubemapFaceSolid(Texture2D texture, CubemapFace face, Cubemap cubemap, int positionY, int positionX)
        {
            for (var x = 0; x < cubemap.width; x++)
            {
                for (var y = 0; y < cubemap.height; y++)
                {
                    var sourceX = positionX * cubemap.width + x;
                    var sourceY = (2 - positionY) * cubemap.height + (cubemap.height - y - 1);
                    var color = texture.GetPixel(sourceX, sourceY);
                    cubemap.SetPixel(face, x, y, color);
                }
            }
        }
    }
}