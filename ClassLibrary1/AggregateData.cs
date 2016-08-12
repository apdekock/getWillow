using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Aggregator
{
    public class AggregateData
    {
        private readonly IDataRepository _dataRepository;

        public AggregateData(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public Dictionary<string, SortedItem> Aggregate()
        {
            var dataItems = _dataRepository.GetDataItems();

            var cars = new Dictionary<string, SortedItem>();

            foreach (var dataItem in dataItems)
            {
                foreach (var lineItem in dataItem.LineItems)
                {
                    if (!cars.ContainsKey(lineItem.Description))
                    {
                        var p = new SortedDictionary<DateTime, double> { { dataItem.DateTime, lineItem.Price } };
                        cars.Add(lineItem.Description, new SortedItem { Prices = p, Link = lineItem.Link });
                    }
                    else
                    {
                        var dictionary = cars[lineItem.Description];
                        dictionary.Prices[dataItem.DateTime] = lineItem.Price;
                        dictionary.Link = lineItem.Link;
                    }
                }
            }

            var monthAgo = DateTime.Now.AddDays(-30);

            var listedAtLeastMonthAgo = cars.Where(c => c.Value.Prices.Keys.Max() > monthAgo).OrderByDescending(f => 1 - (f.Value.Prices.Values.Last() / f.Value.Prices.Values.First()));

            return listedAtLeastMonthAgo.ToDictionary(d => d.Key, y => new SortedItem() { Prices = y.Value.Prices, Link = y.Value.Link });
        }

        public string GetHTML(Dictionary<string, SortedItem> aggregate)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<ol>");
            foreach (var car in aggregate)
            {
                string Template = "<li><div><a Href='{3}'> {0} (R{1})</a> <span class=\"sparklines\">{2}</span></div></li>";
                var link = string.IsNullOrWhiteSpace(car.Value.Link) ? "http://www.wesellcars.co.za" : car.Value.Link;
                sb.AppendLine(string.Format(Template, car.Key, car.Value.Prices.Last().Value, string.Join(",", car.Value.Prices.Select(c => c.Value)), link));
            }
            sb.AppendLine("</ol>");
            sb.AppendLine("<script type=\"text/javascript\"> $('.sparklines').sparkline('html'); </script>");
            return sb.ToString();
        }
    }

    public class SortedItem
    {
        public string Link { get; set; }
        public SortedDictionary<DateTime, double> Prices { get; set; }
    }

    public class DataPoint
    {
        public DataPoint(DateTime dateTime)
        {
            DateTime = dateTime;
            LineItems = new List<LineItem>();
        }

        public DateTime DateTime { get; set; }
        public List<LineItem> LineItems { get; }

        public void AddLineItem(LineItem item)
        {
            LineItems.Add(item);
        }
    }

    public class FileSystemLocation : IDataRepository
    {
        private readonly string _path;

        public FileSystemLocation(string path)
        {
            _path = path;
        }


        public IEnumerable<DataPoint> GetDataItems()
        {
            var dataPoints = new List<DataPoint>();
            var directoryInfo = new DirectoryInfo(_path);
            foreach (var file in directoryInfo.GetFiles())
            {
                var datePart = file.Name.Substring(11);
                var day = Convert.ToInt32(datePart.Substring(0, 2));
                var month = Convert.ToInt32(datePart.Substring(3, 2));
                var year = Convert.ToInt32(datePart.Substring(6, 4));
                DataPoint dPoint = new DataPoint(new DateTime(year, month, day));
                try
                {
                    var streamReader = file.OpenText();
                    var fileContent = streamReader.ReadToEnd();
                    var lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        try
                        {
                            dPoint.LineItems.Add(new LineItem(line));
                        }
                        catch (Exception)
                        {
                            // skip line
                        }
                    }
                }
                catch (Exception)
                {
                    // skip file
                }
                dataPoints.Add(dPoint);
            }
            return dataPoints;
        }
    }

    public class LineItem
    {
        public LineItem(string lineContent)
        {
            var lastFiledSeperator = lineContent.LastIndexOf(',');
            var lastSection = lineContent.Substring(lastFiledSeperator);

            if (lastSection.StartsWith(",http"))
            {
                Link = lineContent.Substring(lastFiledSeperator + 1);
                var last = lineContent.Remove(lastFiledSeperator);
                lastFiledSeperator = last.LastIndexOf(',');
                lastSection = last.Substring(lastFiledSeperator);
            }
            Price = Double.Parse(string.Concat(lastSection.Where(Char.IsDigit)));
            Description = lineContent.Remove(lastFiledSeperator);
        }

        public string Description { get; set; }
        public double Price { get; set; }
        public string Link { get; set; }
    }
    public interface IDataRepository
    {
        IEnumerable<DataPoint> GetDataItems();
    }
}
