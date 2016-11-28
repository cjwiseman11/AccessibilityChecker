using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessibilityChecker
{
    class LinkChecker
    {
        public List<string> ContextlessLinkCheck(HtmlDocument doc)
        {
            var LinkList = doc.DocumentNode.Descendants("a");
            List<string> results = new List<string>();

            foreach(var Link in LinkList)
            {
                var LinkText = Link.InnerText.ToLower();
                if (LinkText.Contains("click"))
                {
                    results.Add("Link " + Link + " should not contain 'click'");
                } else if (LinkText.Contains("link"))
                {
                    results.Add("Link " + Link.InnerText + " should not contain 'link'");
                } else if (LinkText.Contains("http") || LinkText.Contains("www."))
                {
                    results.Add("Link " + Link.InnerText + " should not contain URL");
                } else if (LinkText.Contains("download here") || LinkText.Contains("download now"))
                {
                    results.Add("Link " + Link.InnerText + " should not contain 'download' out of context");
                }

            }
            return results;
        }

        // Returns all links & checks the title attribute.
        public static List<string> PageLinks(HtmlDocument doc)
        {
            var LinkList = doc.DocumentNode.Descendants("a");
            List<string> links = new List<string>();

            foreach(var Link in LinkList)
            {
                string hrefValue = Link.GetAttributeValue("href", string.Empty);
                string titleValue = Link.GetAttributeValue("title", string.Empty);

                if (titleValue == null || titleValue == "")
                {
                    titleValue = "No title text found";
                }

                links.Add(hrefValue + " *** " + titleValue);
            }

            return links;
        }

        /*public List<string> TitleCheck(HtmlDocument doc)
        {
            var Passes = 0;
            var Fails = 0;
            var LinkList = doc.DocumentNode.Descendants("a");
            List<string> results = new List<string>();

            Console.WriteLine("Links Found: " + LinkList.Count());

            LinkChecker LinkChecker = new LinkChecker();

            foreach (var link in LinkList)
            {
                if(link.Attributes["title"] == null || link.Attributes["title"].Value == "")
                {
                    Fails++;
                    Console.WriteLine("\nThis link requires a Title tag: " + link.InnerText);
                    results.Add("This link requires a Title tag: " + link.InnerText);
                } else
                {
                    Passes++;
                }
            }

            Console.WriteLine("Fails: " + Fails + ", Passes: " + Passes);
            return results;
            }
            */
    }
}
