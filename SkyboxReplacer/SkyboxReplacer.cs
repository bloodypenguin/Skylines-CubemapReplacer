using System.IO;
using SkyboxReplacer.OptionsFramework;
using UnityEngine;

namespace SkyboxReplacer
{
    public class SkyboxReplacer
    {
        private static Cubemap vanillaCubemap;
        private static Cubemap customCubemap;
        private static float defaultHorizon;
        private static float defaultFog;

        public static void Initialize()
        {
            vanillaCubemap = Object.FindObjectOfType<RenderProperties>().m_cubemap;
            defaultFog = Object.FindObjectOfType<RenderProperties>().m_fogHeight;
            defaultHorizon = Object.FindObjectOfType<FogProperties>().m_HorizonHeight;
            MinimizeHorizon(OptionsWrapper<Options>.Options.MinimizeHorizon);
            ReplaceCubemap();
        }

        public static void RevertCubemap()
        {
            if (!LoadingExtension.inGame)
            {
                return;
            }
            if (customCubemap == null)
            {
                return;
            }
            GameObject.Destroy(customCubemap);
            customCubemap = null;
            Shader.SetGlobalTexture("_EnvironmentCubemap", vanillaCubemap);
        }

        public static void ReplaceCubemap()
        {
            if (!LoadingExtension.inGame)
            {
                return;
            }
            RevertCubemap();
            var cubemap = new Cubemap(OptionsWrapper<Options>.Options.CubemapSize, TextureFormat.ARGB32, true)
            {
                name = "CubemapReplacerCubemap",
                wrapMode = TextureWrapMode.Clamp
            };
            if (OptionsWrapper<Options>.Options.SplitFormat)
            {
                var posx = Util.LoadTextureFromFile(Path.Combine(Util.AssemblyDirectory, "posx.png"));
                SetCubemapFaceSolid(posx, CubemapFace.PositiveX, cubemap, 2, 0);
                Object.Destroy(posx);
                var posy = Util.LoadTextureFromFile(Path.Combine(Util.AssemblyDirectory, "posy.png"));
                SetCubemapFaceSolid(posy, CubemapFace.PositiveY, cubemap, 2, 0);
                Object.Destroy(posy);
                var posz = Util.LoadTextureFromFile(Path.Combine(Util.AssemblyDirectory, "posz.png"));
                SetCubemapFaceSolid(posz, CubemapFace.PositiveZ, cubemap, 2, 0);
                Object.Destroy(posz);
                var negx = Util.LoadTextureFromFile(Path.Combine(Util.AssemblyDirectory, "negx.png"));
                SetCubemapFaceSolid(negx, CubemapFace.NegativeX, cubemap, 2, 0);
                Object.Destroy(negx);
                var negy = Util.LoadTextureFromFile(Path.Combine(Util.AssemblyDirectory, "negy.png"));
                SetCubemapFaceSolid(negy, CubemapFace.NegativeY, cubemap, 2, 0);
                Object.Destroy(negy);
                var negz = Util.LoadTextureFromFile(Path.Combine(Util.AssemblyDirectory, "negz.png"));
                SetCubemapFaceSolid(negz, CubemapFace.NegativeZ, cubemap, 2, 0);
                Object.Destroy(negz);
            }
            else
            {
                var texture = Util.LoadTextureFromFile(Path.Combine(Util.AssemblyDirectory, "cubemap.png"));
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
            Shader.SetGlobalTexture("_EnvironmentCubemap", cubemap);
            customCubemap = cubemap;
        }

        public static void MinimizeHorizon(bool minimize)
        {
            if (!LoadingExtension.inGame)
            {
                return;
            }
            Object.FindObjectOfType<FogProperties>().m_HorizonHeight = minimize ? float.Epsilon : defaultHorizon;
            Object.FindObjectOfType<RenderProperties>().m_fogHeight = minimize ? float.Epsilon : defaultFog;
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