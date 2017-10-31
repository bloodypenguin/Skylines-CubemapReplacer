using System.Collections.Generic;
using System.Linq;
using SkyboxReplacer.Configuration;
using SkyboxReplacer.OptionsFramework.Attibutes;

namespace SkyboxReplacer
{
    public static class CubemapManager
    {
        private static readonly Dictionary<string, CubemapReplacement> DayCubemaps = new Dictionary<string, CubemapReplacement>
        {
            { "example1piece", new CubemapReplacement(1024, false, false, "example1piece", "Example - 1 piece", "", Util.AssemblyDirectory)},
            { "example6piece", new CubemapReplacement(1024, true, false, "example6piece", "Example - 6 piece", "", Util.AssemblyDirectory)}
        };

        private static readonly Dictionary<string, CubemapReplacement> NightCubemaps = new Dictionary<string, CubemapReplacement>();


        public static DropDownEntry<string>[] GetDayCubemaps()
        {
            var entries = new List<DropDownEntry<string>>()
            {
                new DropDownEntry<string>(SkyboxReplacer.Vanilla, "Vanilla"),
            };
            //TODO(earalov): load custom cubemaps
            entries.AddRange(DayCubemaps.Select(kvp => new DropDownEntry<string>(kvp.Key, kvp.Value.description)).ToArray());
            return entries.ToArray();
        }

        public static DropDownEntry<string>[] GetNightCubemaps()
        {
            return new List<DropDownEntry<string>>
            {
                new DropDownEntry<string>(SkyboxReplacer.Vanilla, "Vanilla"),
            }.ToArray();
            //TODO(earalov): load custom cubemaps
        }

        public static CubemapReplacement GetDayReplacement(string code)
        {
            return DayCubemaps[code];
        }

        public static CubemapReplacement GetNightReplacement(string code)
        {
            return NightCubemaps[code];
        }
    }
}