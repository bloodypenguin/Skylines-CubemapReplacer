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

        private static Cubemap vanillaNightCubemap;
        private static Cubemap customNightCubemap;

        private static Cubemap currentDayCubemap;
        private static Cubemap currentNightCubemap;

        public static void Initialize()
        {
            vanillaDayCubemap = Object.FindObjectOfType<RenderProperties>().m_cubemap;
            vanillaNightCubemap = Object.FindObjectOfType<DayNightProperties>().m_OuterSpaceCubemap;
            SetDayCubemap(OptionsWrapper<Options>.Options.CubemapDay);
            SetNightCubemap(OptionsWrapper<Options>.Options.CubemapNight);
        }

        public static void Revert()
        {
            SetDayCubemap(Vanilla);
            SetNightCubemap(Vanilla);
        }

        public static Cubemap GetDayCubemap() => currentDayCubemap;

        public static Cubemap GetNightCubemap() => currentNightCubemap;

        public static void SetDayCubemap(string code)
        {
            if (!LoadingExtension.inGame)
            {
                return;
            }

            if (Vanilla.Equals(code))
            {
                RevertDayCubemap();
                currentDayCubemap = vanillaDayCubemap;
                return;
            }
            currentDayCubemap = ReplaceCubemap(CubemapManager.GetDayReplacement(code));
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
            currentNightCubemap = ReplaceCubemap(CubemapManager.GetNightReplacement(code));
        }

        private static void RevertDayCubemap()
        {
            if (customDayCubemap == null)
            {
                return;
            }
            GameObject.Destroy(customDayCubemap);
            customDayCubemap = null;
        }

        private static void RevertNightCubemap()
        {
            if (customNightCubemap == null)
            {
                return;
            }
            GameObject.Destroy(customNightCubemap);
            customNightCubemap = null;
        }

        public static void ReloadSelectedCubemaps()
        {
            SetDayCubemap(OptionsWrapper<Options>.Options.CubemapDay);
            SetNightCubemap(OptionsWrapper<Options>.Options.CubemapNight);
        }

        private static Cubemap ReplaceCubemap(CubemapReplacement replacement)
        {
            if (replacement.IsNight)
            {
                RevertNightCubemap();
            }
            else
            {
                RevertDayCubemap();
            }
            var cubemap = new Cubemap(replacement.Size, TextureFormat.ARGB32, true)
            {
                name = "CubemapReplacerCubemap",
                wrapMode = TextureWrapMode.Clamp
            };
            var prefix = replacement.FilePrefix;
            if (replacement.SplitFormat)
            {
                var posx = Util.LoadTextureFromFile(Path.Combine(replacement.Directory, prefix + "posx.png"));
                SetCubemapFaceSolid(posx, CubemapFace.PositiveX, cubemap, 2, 0);
                Object.Destroy(posx);
                var posy = Util.LoadTextureFromFile(Path.Combine(replacement.Directory, prefix + "posy.png"));
                SetCubemapFaceSolid(posy, CubemapFace.PositiveY, cubemap, 2, 0);
                Object.Destroy(posy);
                var posz = Util.LoadTextureFromFile(Path.Combine(replacement.Directory, prefix + "posz.png"));
                SetCubemapFaceSolid(posz, CubemapFace.PositiveZ, cubemap, 2, 0);
                Object.Destroy(posz);
                var negx = Util.LoadTextureFromFile(Path.Combine(replacement.Directory, prefix + "negx.png"));
                SetCubemapFaceSolid(negx, CubemapFace.NegativeX, cubemap, 2, 0);
                Object.Destroy(negx);
                var negy = Util.LoadTextureFromFile(Path.Combine(replacement.Directory, prefix + "negy.png"));
                SetCubemapFaceSolid(negy, CubemapFace.NegativeY, cubemap, 2, 0);
                Object.Destroy(negy);
                var negz = Util.LoadTextureFromFile(Path.Combine(replacement.Directory, prefix + "negz.png"));
                SetCubemapFaceSolid(negz, CubemapFace.NegativeZ, cubemap, 2, 0);
                Object.Destroy(negz);
            }
            else
            {
                var texture = Util.LoadTextureFromFile(Path.Combine(replacement.Directory, prefix + "cubemap.png"));
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
            if (replacement.IsNight)
            {
                customNightCubemap = cubemap;
            }
            else
            {
                customDayCubemap = cubemap;
            }
            return cubemap;
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