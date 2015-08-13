using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace GetGumtree
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                List<VehicleAdd> items = new List<VehicleAdd>();
                GetItems(items);
                var cTempCarsTxt = @"c:\temp\GumtreeCars" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".txt";
                var fileStream = File.Create(cTempCarsTxt);
                fileStream.Close();
                var stringBuilder = new StringBuilder();
                foreach (var vehicleAdd in items)
                {
                    stringBuilder.Append(vehicleAdd.Year);
                    stringBuilder.Append(";");
                    stringBuilder.Append(vehicleAdd.Title);
                    stringBuilder.Append(";");
                    stringBuilder.Append(vehicleAdd.PriceRaw);
                    stringBuilder.AppendLine();
                }
                File.WriteAllText(cTempCarsTxt, stringBuilder.ToString());
                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static void GetItems(List<VehicleAdd> items)
        {

            try
            {
                IWebDriver driver = new ChromeDriver(Path.Combine(Directory.GetCurrentDirectory(), "WebDriverServer"));
                driver.Navigate().GoToUrl("http://www.gumtree.co.za/");
                driver.FindElement(By.LinkText("Cars & Bakkies")).Click();
                do
                {
                    items.AddRange(GetData(driver));
                    IWebElement nextElement = driver.FindElement(By.ClassName("pagination")).FindElement(By.ClassName("next"));
                    if (nextElement != null)
                    {
                        driver.Navigate().GoToUrl(nextElement.GetAttribute("href"));
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                driver.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static List<VehicleAdd> GetData(IWebDriver driver)
        {
            var result = new List<VehicleAdd>();
            try
            {
                var resultsView = driver.FindElement(By.ClassName("results"));

                var view = resultsView.FindElements(By.ClassName("view")).Last();

                foreach (var item in view.FindElements(By.ClassName("result")))
                {
                    try
                    {
                        var vehicle = item.FindElement(By.ClassName("title")).Text;
                        var price = item.FindElement(By.ClassName("price")).Text;
                        var vehicleAdd = new VehicleAdd(vehicle, price);
                        result.Add(vehicleAdd);
                        Console.WriteLine(vehicleAdd);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        private class VehicleAdd
        {
            private readonly string _title;
            private readonly string _price;

            public string Year
            {
                get
                {
                    string pattern = @"\b\d{4}\b";
                    var r = new System.Text.RegularExpressions.Regex(pattern);

                    var year = r.Match(_title);
                    if (year.Success)
                    {
                        return r.Match(_title).ToString();
                    }
                    return string.Empty;
                }
            }

            public VehicleAdd(string title, string price)
            {
                _title = title;
                _price = price;
            }

            public string Title
            {
                get { return _title; }
            }

            public string Price
            {
                get { return _price; }
            }

            public string PriceRaw
            {
                get { return string.Join("", Price.ToCharArray().Where(Char.IsDigit)); }
            }

            public override string ToString()
            {
                return string.Format("{0} - {1} - {2}", Year, PriceRaw, Title);
            }
        }
    }
}
