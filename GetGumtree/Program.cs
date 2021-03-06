﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net;
using System.Data;
using GetGumtree;

namespace GetWillow
{
    class Program
    {
        public const string ConnectionString = "Server=PLEX-PC;Database=nopCommerceDB_willowcrest;User Id=sa;Password=sa;";
        //Server=tcp:willowcrest.database.windows.net,1433;Integrated Security = False; Initial Catalog = nopCommerceDB_willowcrest; Persist Security Info=False;User ID =-admin-pdk@willowcrest;Password=sLpIQEFR%3k!k5&I8f1TvblFR;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;
        private static void Main(string[] args)
        {
            //var arg0 = @"C:\temp\Dropbox\jnk\WeSellCars\"; // file path
            //var arg1 = @"C:\git\apdekock.github.io\"; //repo path
            //var arg2 = @"_posts\2015-08-04-weMineData.markdown"; //post path
            //var arg3 = @"C:\Program Files\Git\cmd\git.exe"; //git path

            try
            {                
                List<LineItem> listOfLines = new List<LineItem>();

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

                    var firstLine = ScrapeLink(driver);
                    Console.WriteLine(string.Join(Environment.NewLine, firstLine.Select(x => x.ToString())));
                    listOfLines.AddRange(firstLine);

                    foreach (var link in Urls)
                    {
                        driver.Navigate().GoToUrl(link);
                        var subsequentLines = ScrapeLink(driver);
                        Console.WriteLine(string.Join(Environment.NewLine, subsequentLines.Select(x => x.ToString())));
                        listOfLines.AddRange(subsequentLines);

                    }

                    foreach (var item in listOfLines)
                    {
                        driver.Navigate().GoToUrl(item.URL);
                        Console.WriteLine(item.URL);
                        try
                        {

                            var price = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(1) > td > table > tbody > tr > td.bodyfont")).Text;
                            var description = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(1) > td")).Text;
                            var features = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(2) > td")).Text;
                            var stockNo = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(3) > td:nth-child(2)")).Text;
                            var manufacturer = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(4) > td:nth-child(2)")).Text;
                            var Model = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(5) > td:nth-child(2)")).Text;
                            var Fuel = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(6) > td:nth-child(2)")).Text;
                            var km = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(7) > td:nth-child(2)")).Text;
                            var Category = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(8) > td:nth-child(2)")).Text;
                            var Year = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(9) > td:nth-child(2)")).Text;
                            var Doors = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(11) > td:nth-child(2)")).Text;
                            var Colour = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(12) > td:nth-child(2)")).Text;
                            var EngineCC = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(1) > table > tbody > tr:nth-child(13) > td:nth-child(2)")).Text;
                            var detailItem = new LineItemDetail();
                            detailItem.Description = description;
                            detailItem.Features = features;
                            detailItem.StockNo = stockNo;
                            detailItem.Manufacturer = manufacturer;
                            detailItem.Model = Model;
                            detailItem.Fuel = Fuel;
                            detailItem.km = km;
                            detailItem.Category = Category;
                            detailItem.Year = Year;
                            detailItem.Doors = Doors;
                            detailItem.Colour = Colour;
                            detailItem.EngineCC = EngineCC;
                            var imagesRow = driver.FindElement(By.CssSelector("body > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td > center > table > tbody > tr:nth-child(3) > td:nth-child(2) > form > table > tbody > tr:nth-child(2) > td"));
                            Pictures pictures = new Pictures();
                            pictures.PictureStreams = new List<byte[]>();
                            foreach (var image in imagesRow.FindElements(By.CssSelector("img")))
                            {
                                var link = image.GetAttribute("src");
                                using (WebClient webClient = new WebClient())
                                {
                                    byte[] data = webClient.DownloadData(link);
                                    pictures.PictureStreams.Add(data);
                                }
                                Console.WriteLine(link);
                            }
                            item.Pictures = pictures;
                            item.Detail = detailItem;
                            item.Price = price;

                        }
                        catch (Exception ex)
                        {
                            item.ExceptionEncountered = true;
                            Console.WriteLine(string.Join(Environment.NewLine, getException(ex)));
                        }
                    }


                    driver.Quit();
                }

                StoreData(listOfLines);

                //var cTempCarsTxt = @"c:\temp\willow_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".csv";
                //var fileStream = File.Create(cTempCarsTxt);
                //fileStream.Close();
                //File.WriteAllText(cTempCarsTxt, string.Join(Environment.NewLine, listOfLines));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Join(Environment.NewLine, getException(e)));
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        public static string[] getException(Exception e)
        {
            List<string> result = new List<string>();
            result.Add(e.Message);
            if (e.InnerException != null)
            {
                result.AddRange(getException(e.InnerException));
            }
            return result.ToArray();
        }


        private static void StoreData(List<LineItem> listOfLines)
        {
            var entities = new nopCommerceDB_willowcrestEntities();

            foreach (var lineItem in listOfLines)
            {
                if (lineItem.ExceptionEncountered)
                    continue;
                var product = createProduct(lineItem);
                entities.Products.Add(product);

                var category = entities.Categories.SingleOrDefault(c => c.Name == lineItem.Detail.Category);
                if (category == null)
                {
                    category = createCategory(lineItem);
                    entities.Categories.Add(category);
                }

                entities.Product_Category_Mapping.Add(new Product_Category_Mapping() { Product = product, IsFeaturedProduct = false, DisplayOrder = 0, Category = category });

                int displayOrder = 0;
                foreach (var property in lineItem.Detail.GetType().GetProperties())
                {
                    var specificationAttribute = entities.SpecificationAttributes.SingleOrDefault(c => c.Name == property.Name);
                    if (specificationAttribute == null)
                    {
                        specificationAttribute = createAttribute(property.Name, displayOrder++);
                        entities.SpecificationAttributes.Add(specificationAttribute);
                    }

                    string val = GetPropValue(lineItem.Detail, property.Name).ToString();
                    var specificationAttributeOption = new SpecificationAttributeOption() { Name = val.ToString() };
                    specificationAttribute.SpecificationAttributeOptions.Add(specificationAttributeOption);
                    entities.Product_SpecificationAttribute_Mapping.Add(new Product_SpecificationAttribute_Mapping() { ShowOnProductPage = true, Product = product, CustomValue = val, SpecificationAttributeOption = specificationAttributeOption });
                }
                var manufacturer = entities.Manufacturers.SingleOrDefault(c => c.Name == lineItem.Detail.Manufacturer);
                if (manufacturer == null)
                {
                    manufacturer = new Manufacturer() { Published = true, PageSize = 5, Name = lineItem.Detail.Manufacturer, CreatedOnUtc = DateTime.Now, UpdatedOnUtc = DateTime.Now };
                }
                entities.Product_Manufacturer_Mapping.Add(new Product_Manufacturer_Mapping() { Product = product, Manufacturer = manufacturer });
                for (int i = 0; i < lineItem.Pictures.PictureStreams.Count; i++)
                {
                    product.Product_Picture_Mapping.Add(new Product_Picture_Mapping() { DisplayOrder = i, Product = product, Picture = new Picture() { IsNew = true, MimeType = "image/jpeg", AltAttribute = lineItem.Description, PictureBinary = lineItem.Pictures.PictureStreams[i], TitleAttribute = lineItem.Description, SeoFilename = lineItem.Description } });
                }
                Console.WriteLine(lineItem.Description);
                entities.SaveChanges();

            }
             
            foreach (var product in entities.Products)
            {
                if (entities.UrlRecords.FirstOrDefault(x => x.EntityId == product.Id && x.EntityName == "Product") == null)
                {
                    var slug = string.Join("", product.Name.Replace(" ", "-").Where(c => Char.IsLetterOrDigit(c) || c == '-'));
                    entities.UrlRecords.Add(new UrlRecord() { EntityId = product.Id, EntityName = "Product", IsActive = true, LanguageId = 0, Slug = slug });
                }

            }
            foreach (var category in entities.Categories)
            {
                if (entities.UrlRecords.FirstOrDefault(x => x.EntityId == category.Id && x.EntityName == "Category") == null)
                {
                    var slug = string.Join("", category.Name);
                    entities.UrlRecords.Add(new UrlRecord() { EntityId = category.Id, EntityName = "Category", IsActive = true, LanguageId = 0, Slug = slug });
                }

            }
            foreach (var manufacturer in entities.Manufacturers)
            {
                if (entities.UrlRecords.FirstOrDefault(x => x.EntityId == manufacturer.Id && x.EntityName == "Manufacturer") == null)
                {
                    var slug = string.Join("", manufacturer.Name);
                    entities.UrlRecords.Add(new UrlRecord() { EntityId = manufacturer.Id, EntityName = "Manufacturer", IsActive = true, LanguageId = 0, Slug = slug });
                }

            }
            entities.SaveChanges();

        }



        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        private static SpecificationAttribute createAttribute(string name, int displayOrder)
        {
            return new SpecificationAttribute() { Name = name, DisplayOrder = displayOrder };
        }

        private static Category createCategory(LineItem item)
        {
            return new Category()
            {
                Name = item.Detail.Category,
                CategoryTemplateId = 1,
                ParentCategoryId = 17,
                PictureId = 1,
                PageSize = 9,
                AllowCustomersToSelectPageSize = true,
                PageSizeOptions = "6, 3, 9",
                ShowOnHomePage = true,
                IncludeInTopMenu = true,
                SubjectToAcl = false,
                LimitedToStores = false,
                Published = true,
                Deleted = false,
                DisplayOrder = 0,
                CreatedOnUtc = DateTime.Now,
                UpdatedOnUtc = DateTime.Now
            };
        }

        private static Product createProduct(LineItem item)
        {
            return new Product()
            {
                ProductTypeId = 5,
                ParentGroupedProductId = 0,
                VisibleIndividually = true,
                Name = item.Description,
                ProductTemplateId = 1,
                VendorId = 0,
                ShowOnHomePage = false,
                AllowCustomerReviews = false,
                ApprovedRatingSum = 0,
                NotApprovedRatingSum = 0,
                ApprovedTotalReviews = 0,
                NotApprovedTotalReviews = 0,
                SubjectToAcl = false,
                LimitedToStores = false,
                IsGiftCard = false,
                GiftCardTypeId = 0,
                RequireOtherProducts = false,
                AutomaticallyAddRequiredProducts = false,
                IsDownload = false,
                DownloadId = 0,
                UnlimitedDownloads = false,
                MaxNumberOfDownloads = 0,
                DownloadActivationTypeId = 1,
                HasSampleDownload = false,
                SampleDownloadId = 0,
                HasUserAgreement = false,
                IsRecurring = false,
                RecurringCycleLength = 0,
                RecurringCyclePeriodId = 0,
                RecurringTotalCycles = 0,
                IsRental = false,
                RentalPriceLength = 0,
                RentalPricePeriodId = 0,
                IsShipEnabled = false,
                IsFreeShipping = false,
                ShipSeparately = false,
                AdditionalShippingCharge = 0,
                DeliveryDateId = 0,
                IsTaxExempt = false,
                TaxCategoryId = 0,
                IsTelecommunicationsOrBroadcastingOrElectronicServices = false,
                ManageInventoryMethodId = 0,
                UseMultipleWarehouses = false,
                WarehouseId = 0,
                StockQuantity = 1,
                DisplayStockAvailability = false,
                DisplayStockQuantity = false,
                MinStockQuantity = 1,
                LowStockActivityId = 0,
                NotifyAdminForQuantityBelow = 1,
                BackorderModeId = 0,
                AllowBackInStockSubscriptions = false,
                OrderMinimumQuantity = 1,
                OrderMaximumQuantity = 1,
                AllowAddingOnlyExistingAttributeCombinations = false,
                NotReturnable = false,
                DisableBuyButton = false,
                DisableWishlistButton = false,
                AvailableForPreOrder = false,
                CallForPrice = false,
                Price = Convert.ToDecimal(string.Join("", item.Price.ToCharArray().Where(char.IsDigit))),
                OldPrice = 0,
                ProductCost = 0,
                CustomerEntersPrice = false,
                MinimumCustomerEnteredPrice = 0,
                MaximumCustomerEnteredPrice = 0,
                BasepriceEnabled = false,
                BasepriceAmount = 0,
                BasepriceUnitId = 1,
                BasepriceBaseAmount = 0,
                BasepriceBaseUnitId = 1,
                MarkAsNew = false,
                HasTierPrices = false,
                HasDiscountsApplied = false,
                Weight = 0,
                Length = 0,
                Width = 0,
                Height = 0,
                DisplayOrder = 0,
                Published = true,
                Deleted = false,
                CreatedOnUtc = DateTime.Now,
                UpdatedOnUtc = DateTime.Now
            };
        }

        private class Pictures
        {
            public List<byte[]> PictureStreams { get; set; }
        }
        private class LineItemDetail
        {
            public string Description { get; set; }
            public string Features { get; set; }

            public string StockNo { get; set; }
            public string Manufacturer { get; set; }
            public string Model { get; set; }
            public string Fuel { get; set; }
            public string km { get; set; }
            public string Category { get; set; }
            public string Year { get; set; }
            public string Doors { get; set; }
            public string Colour { get; set; }
            public string EngineCC { get; set; }
        }
        private class LineItem
        {
            public LineItem(string description, string url)
            {
                Description = description;
                URL = url;
            }

            public bool ExceptionEncountered { get; set; }
            public string Description { get; set; }
            public string URL { get; set; }
            public LineItemDetail Detail
            { get; set; }
            public Pictures Pictures { get; set; }

            public string Price { get; set; }
            public override string ToString()
            {
                return Description + ";" + URL;
            }
        }
        private static LineItem[] ScrapeLink(IWebDriver driver)
        {
            List<LineItem> listOfLines = new List<LineItem>();
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

                        listOfLines.Add(new LineItem(description.Text, src));

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
