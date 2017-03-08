using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Aggregator;
using GitSharp;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using File = System.IO.File;

namespace GetGumtree
{
    class Program
    {
        private static void Main(string[] args)
        {
            //var arg0 = @"C:\temp\Dropbox\jnk\WeSellCars\"; // file path
            //var arg1 = @"C:\git\apdekock.github.io\"; //repo path
            //var arg2 = @"_posts\2015-08-04-weMineData.markdown"; //post path
            //var arg3 = @"C:\Program Files\Git\cmd\git.exe"; //git path

            try
            {
                List<string> listOfLines = new List<string>();
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("-incognito");
                using (IWebDriver driver = new ChromeDriver(Path.Combine(Directory.GetCurrentDirectory(), "WebDriverServer"), chromeOptions))
                {

                    driver.Navigate().GoToUrl("http://www.willowcrestmotors.co.za/results.php?submit=Search&start=&sort=_motor.price%20desc");
                    var links = driver.FindElements(By.ClassName("links"));
                    List<string> Urls = new List<string>();
                    foreach (var link in links)
                    {
                        if (link.Text == "Next") break;
                        Urls.Add(link.GetAttribute("href"));
                    }

                    var lines1 = ScrapeLink(driver);
                    Console.WriteLine(string.Join(Environment.NewLine, lines1));
                    listOfLines.AddRange(lines1);

                    foreach (var link in Urls)
                    {
                        driver.Navigate().GoToUrl(link);
                        var lines2 = ScrapeLink(driver);
                        Console.WriteLine(string.Join(Environment.NewLine, lines2));
                        listOfLines.AddRange(lines2);

                    }
                    // var link = "http://www.willowcrestmotors.co.za/results.php?start=" + pageNumber + "&submit=1";


                    driver.Quit();
                }

                var cTempCarsTxt = @"c:\temp\willow_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".csv";
                var fileStream = File.Create(cTempCarsTxt);
                fileStream.Close();
                File.WriteAllText(cTempCarsTxt, string.Join(Environment.NewLine, listOfLines));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static string[] ScrapeLink(IWebDriver driver)
        {
            List<string> listOfLines = new List<string>();
            var findElement = driver.FindElements(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > table > tbody"));

            foreach (var item in findElement)
            {
                var rows = item.FindElements(By.CssSelector("tr"));
                for (int i = 3; i < rows.Count; i++)
                {
                    var row = rows[i];

                    try
                    {
                        //find car
                        var image = row.FindElement(By.CssSelector("td > table > tbody > tr:nth-child(2) > td > table > tbody > tr > td > a"));

                        //scrape

                        var src = image.GetAttribute("href");
                        var description = row.FindElement(By.ClassName("bodyfontdkgrey"));

                        listOfLines.Add(description.Text + ";" + src);

                        //find back link and click

                    }
                    catch (Exception)
                    {

                        //throw;
                    }
                }

            }
            return listOfLines.ToArray();
        }
    }
}
