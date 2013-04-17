namespace Vurdalakov.YleTtv
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            String[] parts = Assembly.GetExecutingAssembly().FullName.Split(',');
            String applicationName = parts[0];
            String applicationVersion = parts[1].Split('=')[1];

            Console.WriteLine("{0} {1} - YLE Texti TV text tester\n(c) 2013 Vurdalakov - https://github.com/vurdalakov/ylettv\n", applicationName, applicationVersion);

            if (0 == args.Length)
            {
                Console.WriteLine("Usage:\n{0} api_key [page1 [page2 [...]]]\n\nExample:\n{0} a1b2c3d4 867 868", applicationName);
                return;
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fi-FI");

            String apiKey = args[0];

            List<Int32> pages = new List<Int32>();
            if (1 == args.Length)
            {
                pages.Add(100);
            }
            else
            {
                for (int i = 1; i < args.Length; i++)
                {
                    Int32 page;
                    if (!Int32.TryParse(args[i], out page))
                    {
                        Console.WriteLine("Wrong page number: {0}", args[i]);
                        return;
                    }

                    pages.Add(page);
                }
            }

            YleTtvPage[] yleTtvPages;
            try
            {
                YleTtvCachingReader yleTtvReader = new YleTtvCachingReader(apiKey);
                yleTtvPages = yleTtvReader.Read(pages.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading teletext: {0}", ex.Message);
                return;
            }

            foreach (YleTtvPage yleTtvPage in yleTtvPages)
            {
                Console.WriteLine("\n{1}\n=== Page {0}\n{1}", yleTtvPage.Number, new String('=', 46));

                foreach (YleTtvSubpage yleTtvSubpage in yleTtvPage.Subpages)
                {
                    Console.WriteLine("\n{2}\n=== Subpage {3}/{0} at {1}\n{2}\n", yleTtvSubpage.Number, yleTtvSubpage.Timestamp, new String('=', 46), yleTtvPage.Number);

                    int line = 1;
                    foreach (String textLine in yleTtvSubpage.TextLines)
                    {
                        Console.WriteLine("{0:D2} {1:D2} {2}", line, textLine.Length, textLine);
                        line++;
                    }
                }
            }
        }
    }
}
