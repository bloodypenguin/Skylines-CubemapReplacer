using System.Collections.Generic;
using SkyboxReplacer.OptionsFramework.Attibutes;

namespace SkyboxReplacer
{
    public static class CubemapManager
    {
        public const string Vanilla = "vanilla";

        public static List<DropDownEntry<string>> GetDayCubemaps()
        {
            return new List<DropDownEntry<string>>()
            {
                new DropDownEntry<string>(Vanilla, "Vanilla"),
                new DropDownEntry<string>("example1piece", "Example - 1 piece"),
                new DropDownEntry<string>("example6pieces", "Example - 6 pieces"),
                //TODO(earalov): load custom cubemaps
            };
        }

        public static List<DropDownEntry<string>> GetNightCubemaps()
        {
            return new List<DropDownEntry<string>>()
            {
                new DropDownEntry<string>(Vanilla, "Vanilla"),
                //TODO(earalov): load custom cubemaps
            };
        }

        public static void setDayCubemap(string code)
        {
            //TODO(earalov): implement
        }

        public static void setNightCubemap(string code)
        {
            //TODO(earalov): implement
        }
    }
}