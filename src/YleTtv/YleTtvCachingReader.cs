namespace Vurdalakov.YleTtv
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public class YleTtvCachingReader : YleTtvReader
    {
        private Int32 cachingTimeInMinutes;
        private String cachingDirectory;

        public YleTtvCachingReader(String apiKey, Int32 cachingTimeInMinutes = 5, String cachingBaseDirectory = null)
            : base(apiKey)
        {
            this.cachingTimeInMinutes = cachingTimeInMinutes;

            if (String.IsNullOrEmpty(cachingBaseDirectory))
            {
                cachingBaseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }

            this.cachingDirectory = Path.Combine(cachingBaseDirectory, "YleTtvCache");

            if (!Directory.Exists(this.cachingDirectory))
            {
                Directory.CreateDirectory(this.cachingDirectory);
            }
        }

        protected override String DownloadString(String url)
        {
            Int32 pos = url.IndexOf("&p=");
            if (pos < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            String fileName = url.Substring(pos + 1);
            fileName = Path.Combine(this.cachingDirectory, fileName) + ".json";

            if (File.Exists(fileName))
            {
                DateTime dateTime = File.GetLastWriteTime(fileName);
                if (dateTime.AddMinutes(this.cachingTimeInMinutes) > DateTime.Now)
                {
                    return File.ReadAllText(fileName, Encoding.UTF8);
                }
            }

            String jsonText = base.DownloadString(url);

            File.WriteAllText(fileName, jsonText, Encoding.UTF8);

            return jsonText;
        }
    }
}
