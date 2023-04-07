﻿using System.Xml.Serialization;
using SkyboxReplacer.OptionsFramework.Attibutes;

namespace SkyboxReplacer
{
    [Options("CubemapReplacer")]
    public class Options
    {

        [XmlElement("cubemapDay")]
        [DynamicDropDown("Cubemap - day", nameof(CubemapManager), nameof(CubemapManager.GetDayCubemaps), null, nameof(SkyboxReplacer), nameof(SkyboxReplacer.SetDayCubemap))]
        public string CubemapDay { set; get; } = SkyboxReplacer.Vanilla;

        [XmlElement("cubemapNight")]
        [DynamicDropDown("Cubemap - night", nameof(CubemapManager), nameof(CubemapManager.GetNightCubemaps), null, nameof(SkyboxReplacer), nameof(SkyboxReplacer.SetNightCubemap))]
        public string CubemapNight { set; get; } = SkyboxReplacer.Vanilla;

        [XmlIgnore]
        [Button("Reload selected cubemaps", null, nameof(SkyboxReplacer), nameof(SkyboxReplacer.ReloadSelectedCubemaps))]
        public object ReplaceCubemap { set; get; } = null;
    }
}