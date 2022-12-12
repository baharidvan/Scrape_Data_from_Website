using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using SanctionScannerCaseStudy;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string mainUrl = "https://www.sahibinden.com";
            HtmlDocument doc = GetDocument(mainUrl);
            var nodes = doc.DocumentNode.SelectNodes("//*[@id=\"container\"]/div[3]/div/div[3]/div[3]/ul/li/a"); //get the list of <a> tags that have details of homepage products as HtmlNode


            List<HomeProduct> homeProducts = new List<HomeProduct>();
            //int counter = 0;
            foreach (var node in nodes)
            {
                HomeProduct homeProduct = new HomeProduct();
                homeProduct.Url = mainUrl + node.Attributes["href"].Value; //Get the href value for navigating of that page and take the price

                var title = node.GetAttributeValue("title", "");
                if (title == "") //In HomePage products there are adds. We dont want to add them to list.
                    continue;
                homeProduct.Title = title;

                homeProduct.Price = GetPrice(homeProduct.Url); //Send the product Url, scrap the website and take the price value

                homeProducts.Add(homeProduct);

                //counter++;
                //if (counter == 10)
                //    break;
            }
            Print(homeProducts);
            Console.WriteLine("\nAverage of the prices: " + homeProducts.Average(item => item.Price)); // Display the average of price
        }

        public static int GetPrice(string url)
        {
            HtmlDocument doc = GetDocument(url);
            System.Threading.Thread.Sleep(3000); 
            string fiyatString = doc.DocumentNode.SelectSingleNode("//*[@class=\"classifiedInfo \"]/h3/text()")?.InnerText;

            int price;
            if (fiyatString == null)
                price = 0;
            else
            {
                string onlyDigits = new String(fiyatString.Where(Char.IsDigit).ToArray()); // Filter only digit characters
                price = Int32.Parse(onlyDigits); // Converting price to integer
            }
            
            return price;
        }
        public static void Print(List<HomeProduct> list)
        {
            StreamWriter writer = new StreamWriter("data.txt");

            writer.Write("Title of Product".PadRight(100)); //title of the first column in txt
            writer.Write("Price"); //title of the second column in txt
            writer.WriteLine();//newline to move to the next row

            foreach (var product in list)
            {
                writer.Write(product.Title.PadRight(100)); //write the column of the row with a fixed width of 100 characters
                Console.Write(product.Title.PadRight(100));
                writer.Write(product.Price);  //write the column of price  
                Console.Write(product.Price);  
                writer.WriteLine(); //newline to move to the next row
                Console.WriteLine();

            }
            writer.Close();
        }

        public static HtmlDocument GetDocument(string url)
        {
            ChromeOptions options = new ChromeOptions(); // I got proxy adress from https://free-proxy-list.net/ for protection against ip bans
            var proxy = new Proxy();
            proxy.Kind = ProxyKind.Manual;
            proxy.IsAutoDetect = false;
            proxy.HttpProxy = "https://" + "130.193.56.73" + ":" + "5002";
            //proxy.SslProxy = "127.0.0.1:3330";
            options.Proxy = proxy;
            options.AddArgument("ignore-certificate-errors");
            IWebDriver driver = new ChromeDriver(options); //Create an instance of the Chrome driver with proxy options

            //IWebDriver driver = new ChromeDriver(); 
            driver.Navigate().GoToUrl(url); //Navigate to the website desired to scrape
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); //Wait for the page to load and 
            string html = driver.PageSource; //Extract the HTML source code using the driver's PageSource property

            HtmlDocument doc = new HtmlDocument();//Use the HTML agility pack to load the HTML and extract the data needed.
            doc.LoadHtml(html);

            driver.Close(); //After load the data, close the Chrome driver to release the resources

            return doc;
        }
    }
}