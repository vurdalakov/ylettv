﻿namespace Vurdalakov.YleTtv
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

            Console.WriteLine("{0} {1} - YLE Texti TV text reader\n(c) 2013 Vurdalakov - http://ylettv.xxx.com\n", applicationName, applicationVersion);

            if (0 == args.Length)
            {
                Console.WriteLine("Usage:\n{0} api_key\n\nExample:\n{0} a1b2c3d4", applicationName);
                return;
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fi-FI");

            String apiKey = args[0];
            
            YleTtvCachingReader yleTtvReader = new YleTtvCachingReader(apiKey);

            YleTtvPage[] yleTtvPages;
            try
            {
                yleTtvPages = yleTtvReader.Read(100);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading teletext: {0}", ex.Message);
                return;
            }

            while (true)
            {
                if (yleTtvPages.Length != 1)
                {
                    Console.WriteLine("Page not found");
                    return;
                }

                YleTtvPage yleTtvPage = yleTtvPages[0];

                Int32 subpage = 0;
                Int32 action = 0;
                Int32 pageToGo = -1;

                while (true)
                {
                    YleTtvSubpage yleTtvSubpage = yleTtvPage.Subpages[subpage];

                    Console.Clear();
                    Console.WriteLine("=== Page {0}, subpage {1}/{2} (updated at {3}) | Press Esc to exit",
                        yleTtvPage.Number, yleTtvSubpage.Number, yleTtvPage.Subpages.Length, yleTtvSubpage.Timestamp);
                    Console.WriteLine("=== Type page number, change pages with ↑ and ↓, change subpages with ← and →");

                    for (int i = 0; i < yleTtvSubpage.TextLines.Length - 1; i++)
                    {
                        Console.WriteLine(yleTtvSubpage.TextLines[i]);
                    }
                    Console.Write(yleTtvSubpage.TextLines[22]);

                    Int32 digit1 = -1;
                    Int32 digit2 = -1;
                    pageToGo = -1;

                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);

                        action = 0;

                        if ((key.Key >= ConsoleKey.D0) && (key.Key <= ConsoleKey.D9))
                        {
                            int digit = key.Key - ConsoleKey.D0;
                            if (digit1 < 0)
                            {
                                digit1 = digit;
                                continue;
                            }
                            else if (digit2 < 0)
                            {
                                digit2 = digit;
                                continue;
                            }
                            else
                            {
                                pageToGo = digit1 * 100 + digit2 * 10 + digit;
                                break;
                            }
                        }

                        switch (key.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                subpage--;
                                if (subpage < 0)
                                {
                                    subpage = yleTtvPage.Subpages.Length - 1;
                                }
                                break;
                            case ConsoleKey.RightArrow:
                                subpage++;
                                if (subpage >= yleTtvPage.Subpages.Length)
                                {
                                    subpage = 0;
                                }
                                break;
                            case ConsoleKey.UpArrow:
                                action = -1;
                                break;
                            case ConsoleKey.DownArrow:
                                action = +1;
                                break;
                            case ConsoleKey.Escape:
                                Console.WriteLine("\n");
                                return;
                            default:
                                continue;
                        }

                        break;
                    }

                    if ((action != 0) || (pageToGo >= 0))
                    {
                        break;
                    }
                }

                try
                {
                    if (pageToGo >= 0)
                    {
                        yleTtvPages = yleTtvReader.Read(pageToGo);
                    }
                    else
                    {
                        yleTtvPages = action > 0 ? yleTtvReader.ReadNext(yleTtvPage.Number) : yleTtvReader.ReadPrev(yleTtvPage.Number);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading teletext: {0}", ex.Message);
                    return;
                }
                subpage = 0;
            }
        }
    }
}
