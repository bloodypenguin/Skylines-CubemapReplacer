using System.Xml.Serialization;
using SkyboxReplacer.OptionsFramework.Attibutes;

namespace SkyboxReplacer
{
    [Options("CubemapReplacer")]
    public class Options
    {

        [XmlElement("cubemapDay")]
        [DynamicDropDown("Cubemap - day", nameof(CubemapManager), nameof(CubemapManager.GetDayCubemaps))]
        public string CubemapDay { set; get; } = CubemapManager.Vanilla;

        [XmlElement("cubemapNight")]
        [DynamicDropDown("Cubemap - night", nameof(CubemapManager), nameof(CubemapManager.GetNightCubemaps))]
        public string CubemapNight { set; get; } = CubemapManager.Vanilla;

        [XmlElement("cubemapSize")]
        [Textfield("Cubemap size")]
        public int CubemapSize { set; get; } = 1024;

        [XmlElement("splitFormat")]
        [Checkbox("Use 6-part cubemap format")]
        public bool SplitFormat { set; get; } = false;

        [XmlElement("minimizeHorizon")]
        [Checkbox("Minimize horizon", null, nameof(SkyboxReplacer), nameof(SkyboxReplacer.MinimizeHorizon))]
        public bool MinimizeHorizon { set; get; } = false;

        [XmlIgnore]
        [Button("Reload custom cubemap", null, nameof(SkyboxReplacer), nameof(SkyboxReplacer.ReplaceCubemap))]
        public object ReplaceCubemap { set; get; } = null;

        [XmlIgnore]
        [Button("Revert to vanilla cubemap", null, nameof(SkyboxReplacer), nameof(SkyboxReplacer.RevertCubemap))]
        public object RevertCubemap { set; get; } = null;
    }
}