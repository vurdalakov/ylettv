namespace Vurdalakov.YleTtv
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json.Linq;

    public class YleTtvReader
    {
        private String apiKey;

        public YleTtvReader(String apiKey)
        {
            if (String.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException();
            }

            this.apiKey = apiKey;
        }

        public YleTtvPage[] Read(Int32 page)
        {
            String url = String.Format("http://beta.yle.fi/api/ttvcontent/?a={0}&c=true&p={1}", this.apiKey, page);

            return this.DownloadAndParseString(url);
        }

        public YleTtvPage[] ReadNext(Int32 page)
        {
            String url = String.Format("http://beta.yle.fi/api/ttvcontent/?a={0}&c=true&p={1}&s=next", this.apiKey, page);

            return this.DownloadAndParseString(url);
        }

        public YleTtvPage[] ReadPrev(Int32 page)
        {
            String url = String.Format("http://beta.yle.fi/api/ttvcontent/?a={0}&c=true&p={1}&s=prev", this.apiKey, page);

            return this.DownloadAndParseString(url);
        }

        public YleTtvPage[] Read(Int32[] pages)
        {
            if (null == pages)
            {
                throw new ArgumentNullException();
            }
            
            if (0 == pages.Length)
            {
                throw new ArgumentException();
            }

            StringBuilder list = new StringBuilder();
            list.Append(pages[0]);
            for (int i = 1; i < pages.Length; i++)
            {
                list.Append(',');
                list.Append(pages[i]);
            }

            String url = String.Format("http://beta.yle.fi/api/ttvcontent/?a={0}&c=true&p={1}", this.apiKey, list);

            return this.DownloadAndParseString(url);
        }

        private YleTtvPage[] DownloadAndParseString(String url)
        {
            String jsonText = this.DownloadString(url);
            return this.Parse(jsonText);
        }

        protected virtual String DownloadString(String url)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            return webClient.DownloadString(url);
        }

        public YleTtvPage[] Parse(String jsonText)
        {
            JObject root = JObject.Parse(jsonText);

            Int32 status = root.GetInt32("status", -1);
            String message = root.GetString("message", "");

            if (status != 200)
            {
                throw new Exception(String.Format("Unknown status {0} '{1}'", status, message));
            }

            List<YleTtvPage> yleTekstiTvPages = new List<YleTtvPage>();
            
            JArray pages = root.GetArray("pages");
            
            foreach (JObject page in pages)
            {
                List<YleTtvSubpage> yleTekstiTvSubpages = new List<YleTtvSubpage>();

                JArray subpages = page.GetArray("subpages");
                
                foreach (JObject subpage in subpages)
                {
                    String content = subpage.GetString("content", "");

                    String[] rawLines = content.Split(new Char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

                    List<String> lines = new List<String>();
                    foreach (String rawLine in rawLines)
                    {
                        lines.Add(this.ToPlainText(rawLine));
                    }

                    YleTtvSubpage yleTekstiTvSubpage = new YleTtvSubpage();
                    yleTekstiTvSubpage.Number = subpage.GetInt32("number", -1);
                    yleTekstiTvSubpage.Timestamp = subpage.GetDate("timestamp", DateTime.MinValue);
                    yleTekstiTvSubpage.TextLines = lines.ToArray();

                    yleTekstiTvSubpages.Add(yleTekstiTvSubpage);
                }

                YleTtvPage yleTekstiTvPage = new YleTtvPage();
                yleTekstiTvPage.Number = page.GetInt32("number", -1);
                yleTekstiTvPage.Subpages = yleTekstiTvSubpages.ToArray();
            
                yleTekstiTvPages.Add(yleTekstiTvPage);
            }

            return yleTekstiTvPages.ToArray();
        }

        private String ToPlainText(String text)
        {
            while (true)
            {
                int i1 = text.IndexOf("[g");
                if (i1 < 0)
                {
                    break;
                }

                int i2 = text.IndexOf("[t", i1 + 2);
                int i3 = text.IndexOf("[g", i1 + 2);

                if ((i2 < 0) && (i3 < 0))
                {
                    text = text.Remove(i1);
                }
                else
                {
                    int i4 = Math.Min(i2, i3);
                    if (i2 < 0)
                    {
                        i4 = i3;
                    }
                    else if (i3 < 0)
                    {
                        i4 = i2;
                    }

                    text = text.Remove(i1, i4 - i1);
                    text = text.Insert(i1, new String(' ', i4 - i1 - 5));
                }
            }

            return RemoveCodes(text).PadRight(40);
        }

        private String RemoveCodes(String text)
        {
            String[] codes = { "[null]", "[tred]", "[tgre]", "[tyel]", "[tblu]", "[tmag]", "[tcya]", "[twhi]", "[flas]", "[stea]", "[ebox]", "[sbox]", "[nhei]", "[dhei]", "[so]", "[si]", "[dle]", "[gred]", "[ggre]", "[gyel]", "[gblu]", "[gmag]", "[gcya]", "[gwhi]", "[cdis]", "[cgra]", "[sgra]", "[esc]", "[bbgr]", "[nbgr]", "[hgra]", "[rgra]", "[tbox]" };

            foreach (String code in codes)
            {
                text = text.Replace(code, " ");
            }

            return text;
        }

        //private String FixString(String text)
        //{
        //    StringBuilder stringBuilder = new StringBuilder(text.Length);

        //    foreach (Char c in text)
        //    {
        //        switch (c)
        //        {
        //            case '$':
        //                stringBuilder.Append('¤');
        //                break;
        //            case '@':
        //                stringBuilder.Append('É');
        //                break;
        //            case '[':
        //                stringBuilder.Append('Ä');
        //                break;
        //            case '\\':
        //                stringBuilder.Append('Ö');
        //                break;
        //            case ']':
        //                stringBuilder.Append('Å');
        //                break;
        //            case '^':
        //                stringBuilder.Append('Ü');
        //                break;
        //            case '`':
        //                stringBuilder.Append('é');
        //                break;
        //            case '{':
        //                stringBuilder.Append('ä');
        //                break;
        //            case '|':
        //                stringBuilder.Append('ö');
        //                break;
        //            case '}':
        //                stringBuilder.Append('å');
        //                break;
        //            case '~':
        //                stringBuilder.Append('ü');
        //                break;
        //            case '\x7F':
        //                stringBuilder.Append('█');
        //                break;
        //            default:
        //                stringBuilder.Append(c);
        //                break;
        //        }
        //    }

        //    return stringBuilder.ToString();
        //}
    }
}
