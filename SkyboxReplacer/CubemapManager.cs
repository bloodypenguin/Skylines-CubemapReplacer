using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ColossalFramework;
using ColossalFramework.Plugins;
using SkyboxReplacer.Configuration;
using SkyboxReplacer.OptionsFramework.Attibutes;

namespace SkyboxReplacer
{
    public static class CubemapManager
    {
        private const string ModConfigName = "CubemapReplacements.xml";
        private static bool imported;

        private static readonly Dictionary<string, CubemapReplacement> DayCubemaps =
            new Dictionary<string, CubemapReplacement>();

        private static readonly Dictionary<string, CubemapReplacement> NightCubemaps =
            new Dictionary<string, CubemapReplacement>();
        
        private static readonly Dictionary<string, CubemapReplacement> OuterSpaceCubemaps =
            new Dictionary<string, CubemapReplacement>();


        public static DropDownEntry<string>[] GetDayCubemaps()
        {
            ImportFromMods();
            var entries = new List<DropDownEntry<string>>()
            {
                new DropDownEntry<string>(SkyboxReplacer.Vanilla, "Vanilla"),
            };
            //TODO(earalov): load custom cubemaps
            entries.AddRange(DayCubemaps.Select(kvp => new DropDownEntry<string>(kvp.Key, kvp.Value.Description))
                .ToArray());
            return entries.ToArray();
        }

        public static DropDownEntry<string>[] GetNightCubemaps()
        {
            ImportFromMods();
            var entries = new List<DropDownEntry<string>>()
            {
                new DropDownEntry<string>(SkyboxReplacer.Vanilla, "Vanilla"),
            };
            //TODO(earalov): load custom cubemaps
            entries.AddRange(NightCubemaps.Select(kvp => new DropDownEntry<string>(kvp.Key, kvp.Value.Description))
                .ToArray());
            return entries.ToArray();
        }
        
        public static DropDownEntry<string>[] GetOuterSpaceCubemaps()
        {
            ImportFromMods();
            var entries = new List<DropDownEntry<string>>()
            {
                new DropDownEntry<string>(SkyboxReplacer.Vanilla, "Vanilla"),
            };
            //TODO(earalov): load custom cubemaps
            entries.AddRange(OuterSpaceCubemaps.Select(kvp => new DropDownEntry<string>(kvp.Key, kvp.Value.Description))
                .ToArray());
            return entries.ToArray();
        }

        public static CubemapReplacement GetDayReplacement(string code)
        {
            return DayCubemaps[code];
        }

        public static CubemapReplacement GetNightReplacement(string code)
        {
            return NightCubemaps[code];
        }
        
        public static CubemapReplacement GetOuterSpaceReplacement(string code)
        {
            return OuterSpaceCubemaps[code];
        }

        public static void ImportFromMods()
        {
            if (imported)
            {
                return;
            }
            imported = true;
            foreach (var pluginInfo in Singleton<PluginManager>.instance.GetPluginsInfo()
                .Where(pluginInfo => pluginInfo.isEnabled))
            {
                try
                {
                    var config = CubemapReplacementsConfig.Deserialize(Path.Combine(pluginInfo.modPath, ModConfigName));
                    if (config == null)
                    {
                        continue;
                    }
                    foreach (var replacement in config.Replacements)
                    {
                        if (replacement.Code.IsNullOrWhiteSpace())
                        {
                            UnityEngine.Debug.LogError("Invalid CubemapReplacements.xml of mod " + pluginInfo.name + ": replacement code is empty!");
                            continue;
                        }
                        if (replacement.Description.IsNullOrWhiteSpace())
                        {
                            UnityEngine.Debug.LogError("Invalid CubemapReplacements.xml of mod " + pluginInfo.name + ": replacement description is empty!");
                            continue;
                        }
                        replacement.Directory = pluginInfo.modPath;
                        if (replacement.IsOuterSpace)
                        {
                            if (OuterSpaceCubemaps.ContainsKey(replacement.Code))
                            {
                                UnityEngine.Debug.LogError("Invalid CubemapReplacements.xml of mod " + pluginInfo.name + ": outer space replacement code is already present!");
                                continue;
                            }
                            OuterSpaceCubemaps.Add(replacement.Code, replacement);
                        }
                        else if (replacement.TimePeriod == "day" || !replacement.IsNight)
                        {
                            if (DayCubemaps.ContainsKey(replacement.Code))
                            {
                                UnityEngine.Debug.LogError("Invalid CubemapReplacements.xml of mod " + pluginInfo.name + ": day replacement code is already present!");
                                continue;
                            }
                            DayCubemaps.Add(replacement.Code, replacement);
                        }
                        else if (replacement.TimePeriod == "night" || replacement.IsNight)
                        {
                            if (NightCubemaps.ContainsKey(replacement.Code))
                            {
                                UnityEngine.Debug.LogError("Invalid CubemapReplacements.xml of mod " + pluginInfo.name + ": night replacement code is already present!");
                                continue;
                            }
                            NightCubemaps.Add(replacement.Code, replacement);
                        }
                        else
                        {
                            throw new Exception("Unknown type of replacement!"); //TODO improve
                        }
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.Log("Error while parsing CubemapReplacements.xml of mod " + pluginInfo.name);
                    UnityEngine.Debug.LogException(e);
                }
            }
        }
    }
}