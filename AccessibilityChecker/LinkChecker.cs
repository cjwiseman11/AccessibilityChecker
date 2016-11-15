using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessibilityChecker
{
    class LinkChecker
    {
        public void TitleCheck(HtmlDocument doc)
        {
            var Passes = 0;
            var Fails = 0;
            var LinkList = doc.DocumentNode.Descendants("a");

            Console.WriteLine("Links Found: " + LinkList.Count());

            LinkChecker LinkChecker = new LinkChecker();

            foreach (var link in LinkList)
            {
                if(link.Attributes["title"] == null || link.Attributes["title"].Value == "")
                {
                    Fails++;
                    Console.WriteLine("\nThis link requires a Title tag: " + link.InnerText);
                } else
                {
                    Passes++;
                }
            }

            Console.WriteLine("Fails: " + Fails + ", Passes: " + Passes);
        }
    }
}
