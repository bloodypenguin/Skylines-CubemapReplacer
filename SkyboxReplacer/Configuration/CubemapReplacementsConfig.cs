using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace SkyboxReplacer.Configuration
{
    public class CubemapReplacementsConfig
    {
        [XmlArray(ElementName = "CubemapReplacements")]
        [XmlArrayItem(ElementName = "CubemapReplacement")]
        public CubemapReplacement[] Replacements = {};

        public static CubemapReplacementsConfig Deserialize(string filename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }
            var xmlSerializer = new XmlSerializer(typeof(CubemapReplacementsConfig));
            try
            {
                using (var streamReader = new StreamReader(filename))
                {
                    return (CubemapReplacementsConfig)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch
            {
                UnityEngine.Debug.LogError("Couldn't load cubemap replacement configuration (XML malformed?)");
                throw;
            }
        }
    }
}