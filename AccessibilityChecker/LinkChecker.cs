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
                if (Link.InnerText.Contains("click"))
                {
                    results.Add("Link " + Link.InnerText + " should not contain 'click'");
                } else if (Link.InnerText.Contains("link"))
                {
                    results.Add("Link " + Link.InnerText + " should not contain 'link'");
                } else if (Link.InnerText.Contains("http") || Link.InnerText.Contains("www."))
                {
                    results.Add("Link " + Link.InnerText + " should not contain URL");
                }
            }

            return results;
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
