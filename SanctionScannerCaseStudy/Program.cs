using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using SanctionScannerCaseStudy;

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
                //if (counter == 5)
                //    break;
            }

            PrintToTxt(homeProducts);

            #region MyRegion

            //HtmlDocument document = new HtmlDocument();
            //document.Load(@"C:\Users\Samsman\Desktop\Sahibinden.html");

            //var nodes = document.DocumentNode.SelectNodes("//*[@id=\"container\"]/div[3]/div/div[3]/div[3]/ul/li/a");
            //List<string> links = new List<string>();
            //foreach (var node in nodes)
            //{
            //    var link = node.Attributes["href"].Value;
            //    links.Add(link);
            //    Console.WriteLine(link);
            //}

            //HtmlDocument document2 = new HtmlDocument();
            //document2.Load(@"C:\Users\Samsman\Desktop\araba.html");
            //var fiyat = document2.DocumentNode.SelectSingleNode("//*[@id=\"classifiedDetail\"]/div/div[2]/div[2]/h3/text()").InnerText;
            //Console.WriteLine(fiyat);

            //HtmlDocument document3 = new HtmlDocument();
            //document3.Load(@"C:\Users\Samsman\Desktop\araba2.html");
            //var fiyat3 = document3.DocumentNode.SelectSingleNode("//*[@id=\"classifiedDetail\"]/div/div[2]/div[2]/h3/text()").InnerText;
            //string temp = new String(fiyat3.Where(Char.IsDigit).ToArray());
            //int fiyat4 = Int32.Parse(temp);
            //Console.WriteLine(fiyat4);

            #endregion
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
                string onlyDigits = new String(fiyatString.Where(Char.IsDigit).ToArray());
                price = Int32.Parse(onlyDigits);
            }
            
            return price;
        }
        public static void PrintToTxt(List<HomeProduct> list)
        {
            StreamWriter writer = new StreamWriter("data.txt");

            writer.Write("Title of Product".PadRight(100)); //title of the first column in txt
            writer.Write("Price"); //title of the second column in txt
            writer.WriteLine();//newline to move to the next row

            foreach (var product in list)
            {
                writer.Write(product.Title.PadRight(100)); //write the column of the row with a fixed width of 100 characters
                writer.Write(product.Price);  //write the column of price  
                writer.WriteLine(); //newline to move to the next row
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