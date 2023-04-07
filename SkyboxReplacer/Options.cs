using System.Xml.Serialization;
using SkyboxReplacer.OptionsFramework.Attibutes;

namespace SkyboxReplacer
{
    [Options("ReflectionTextureChanger")]
    public class Options
    {

        [XmlElement("cubemapDay")]
        [DynamicDropDown("Reflections - day", nameof(CubemapManager), nameof(CubemapManager.GetDayCubemaps), null, nameof(SkyboxReplacer), nameof(SkyboxReplacer.SetDayCubemap))]
        public string CubemapDay { set; get; } = SkyboxReplacer.Vanilla;

        [XmlElement("cubemapNight")]
        [DynamicDropDown("Reflections - night", nameof(CubemapManager), nameof(CubemapManager.GetNightCubemaps), null, nameof(SkyboxReplacer), nameof(SkyboxReplacer.SetNightCubemap))]
        public string CubemapNight { set; get; } = SkyboxReplacer.Vanilla;
        
        [XmlElement("cubemapOuterSpace")]
        [DynamicDropDown("Skybox - outer space at night", nameof(CubemapManager), nameof(CubemapManager.GetOuterSpaceCubemaps), null, nameof(SkyboxReplacer), nameof(SkyboxReplacer.SetOuterSpaceCubemap))]
        public string CubemapOuterSpace { set; get; } = SkyboxReplacer.Vanilla;

        [XmlIgnore]
        [Button("Reload selected cubemaps", null, nameof(SkyboxReplacer), nameof(SkyboxReplacer.ReloadSelectedCubemaps))]
        public object ReplaceCubemap { set; get; } = null;
    }
}