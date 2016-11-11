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
            ColourChecker ColourBlindChecker = new ColourChecker();

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
                    client.DownloadString(url);
                    html = loadChrome(url, ColourBlindChecker);
                    incorrect = false;
                } catch (WebException)
                {
                    Console.WriteLine("Cannot Connect. Please put in valid URL");
                }
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            Console.WriteLine("\n###Running Heading Checker");
            HeadingChecker HeadingChecker = new HeadingChecker();
            var HeadingOneExists = HeadingChecker.HeadingAmount(doc);
            if (HeadingOneExists)
            {
                HeadingChecker.HeadingOneCheck(doc);
            }

            Console.WriteLine("\n###Running Image Checker");
            ImageChecker ImageChecker = new ImageChecker();
            ImageChecker.AltTagsCheck(doc);

            Console.WriteLine("\n###Running Form Checker");
            FormChecker FormChecker = new FormChecker();
            FormChecker.LabelCheck(doc);

            Console.WriteLine("\n###Running Colour Check");
            ColourBlindChecker.GetColourDifference();
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
