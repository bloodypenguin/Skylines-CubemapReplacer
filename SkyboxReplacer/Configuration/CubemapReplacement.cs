using System.IO;
using System.Xml.Serialization;

namespace SkyboxReplacer.Configuration
{
    public class CubemapReplacement
    {
        [XmlAttribute("size")]
        public int Size = 1024;
        [XmlAttribute("is_split_format")]
        public bool SplitFormat = false ;
        [XmlAttribute("is_night")]
        public bool IsOuterSpace = false;
        [XmlAttribute("code")]
        public string Code = "";
        [XmlAttribute("description")]
        public string Description = "";
        [XmlAttribute("file_prefix")]
        public string FilePrefix = "";
        [XmlAttribute("time_period")]
        public string TimePeriod;
        [XmlAttribute("weather_type")]
        public string WeatherType;

        [XmlIgnore]
        public string Directory;
    }
}