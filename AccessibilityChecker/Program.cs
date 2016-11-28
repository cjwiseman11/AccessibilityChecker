using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AccessibilityChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            ColourChecker ColourChecker = new ColourChecker();
            Results Results = new Results();

            var incorrect = true;
            var html = "";
            while (incorrect)
            {
                Console.WriteLine("Please type in the URL you want to check:");
                var url = Console.ReadLine();
                if (!url.StartsWith("http"))
                {
                    url = "http://" + url;
                }

                Console.WriteLine("Running Accessify!");
                try
                {
                    WebClient client = new WebClient();
                    //client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    client.DownloadString(url);
                    html = loadChrome(url, ColourChecker);
                    incorrect = false;
                } catch (WebException e)
                {
                    Console.WriteLine("Cannot Connect. Please put in valid URL: " + e);
                }
                Results.UrlToCheck = url;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            Console.WriteLine("\n###Running Heading Checker");
            HeadingChecker HeadingChecker = new HeadingChecker();
            var HeadingOneExists = HeadingChecker.DoesHeadingOneExist(doc);
            Results.DoesHeadingOneExist = HeadingOneExists;
            if (HeadingOneExists)
            {
                Results.HeadingResult = HeadingChecker.HeadingOneCheck(doc);
                Console.WriteLine(Results.HeadingResult);
            }
            Results.PageHeadings = HeadingChecker.PageHeadings(doc);

            Console.WriteLine("\n###Running Image Checker");
            ImageChecker ImageChecker = new ImageChecker();
            Results.AltTagsResult = ImageChecker.AltTagsCheck(doc);
            Results.AltTagsFound = ImageChecker.AltTagsFoundList;

            Console.WriteLine("\n###Running Form Checker");
            FormChecker FormChecker = new FormChecker();
            Results.FormLabelResult = FormChecker.LabelCheck(doc);

            Console.WriteLine("\n###Running Colour Check");
            Results.ColourContrastResult = ColourChecker.GetColourDifference();

            Console.WriteLine("\n###Running Link Check");
            LinkChecker LinkChecker = new LinkChecker();
            Results.ContextlessLinkCheckResult = LinkChecker.ContextlessLinkCheck(doc);

            Reporter reporter = new Reporter();
            reporter.WriteToTextFile(Results);
        }

        public static string loadChrome(string url, ColourChecker ColourBlindChecker)
        {
            Console.WriteLine("Loading Selenium to get full HTML (JS, Ajax etc)");
            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);

            var body = driver.FindElement(By.CssSelector("html"));
            var html = body.GetAttribute("innerHTML");

            ColourBlindChecker.GetColours(driver);

            Console.WriteLine("Got HTML. Closing browser and continuing.");
            driver.Close();
            return html;
        }
    }
}
