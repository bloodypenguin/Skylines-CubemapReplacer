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
        public bool IsNight = false;
        [XmlAttribute("code")]
        public string Code = "";
        [XmlAttribute("description")]
        public string Description = "";
        [XmlAttribute("file_prefix")]
        public string FilePrefix = "";

        [XmlIgnore]
        public string Directory;
    }
}