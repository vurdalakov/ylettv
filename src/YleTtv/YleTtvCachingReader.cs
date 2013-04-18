#region License
// YleTtv is a C# class library that simplifies work with YLE teletext (teksti-TV) API
// https://github.com/vurdalakov/ylettv
//
// Copyright (c) 2013 Vurdalakov
// email: vurdalakov@gmail.com

/*
    This file is part of YleTtv class library.

    YleTtv is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    YleTtv is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with YleTtv.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

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
