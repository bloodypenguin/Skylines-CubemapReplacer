using System.IO;

namespace SkyboxReplacer.Configuration
{
    public class CubemapReplacement
    {
        public int size;
        public bool splitFormat;
        public bool isNight;
        public string code;
        public string description;
        public string filePrefix;
        public string directory;

        public CubemapReplacement(int size, bool splitFormat, bool isNight, string code, string description, string filePrefix, string directory)
        {
            this.size = size;
            this.splitFormat = splitFormat;
            this.isNight = isNight;
            this.code = code;
            this.description = description;
            this.filePrefix = filePrefix;
            this.directory = directory;
        }
    }
}