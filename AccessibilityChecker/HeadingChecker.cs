using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AccessibilityChecker
{

    class HeadingChecker 
    {
        //Store these for future use
        HtmlNodeCollection HeadingOneList = null;

        public bool DoesHeadingOneExist(HtmlDocument doc)
        {
            HeadingOneList = doc.DocumentNode.SelectNodes("//h1");
            if (HeadingOneList == null)
            {
                Console.WriteLine("No H1 Detected!");
                return false;
            } else
            {
                return true;
            }
        }

        public string HeadingOneCheck(HtmlDocument doc)
        {
            // If there is more than a single H1 element, check to see if the others are semantic.
            if (HeadingOneList.Count > 1)
            {
                for (var i = 1; i < HeadingOneList.Count; i++)
                {   
                    HtmlNode HeadingOne = doc.DocumentNode.SelectSingleNode(HeadingOneList[i].XPath + "/parent::node()");

                    // This can be expanded to check if 'header' is used in the correct context - i.e inside of an article.
                    if (HeadingOne.Name.Contains("article") || HeadingOne.Name.Contains("section") || HeadingOne.Name.Contains("header"))
                    {
                        Console.WriteLine(HeadingOneList[i].InnerText + " - H1 is semantic.");
                    }
                    else
                    {
                        Console.WriteLine(HeadingOneList[i].InnerText + " - H1 is not semantic.");
                    }
                }
                return "Multiple H1 elements found & checked for semantics.";
            }
            else
            {
                Console.WriteLine("Exactly 1 Heading One, checking if first...");
                HtmlNode ChickenDipper = doc.DocumentNode.SelectSingleNode(HeadingOneList[0].XPath + "/preceding::*[starts-with(name(),'h')][1]");
                if (ChickenDipper.Name.Contains("head"))
                {
                    return "##HeadingOne: Pass";
                }
                else
                {
                    return "##HeadingOne: Fail";
                }
            }
        }

        public static List<string> PageHeadings(HtmlDocument doc)
        {
            List<string> allPageHeadings = new List<string>();
            int h1HeadingCount = 0;
            int h2HeadingCount = 0;
            int h3HeadingCount = 0;
            int h4HeadingCount = 0;

            foreach (var item in GetAllHeadings(doc))
            {
                allPageHeadings.Add(item);

                switch (item.Substring(0, 2))
                {
                    case "h1":
                        h1HeadingCount++;
                        break;

                    case "h2":
                        h2HeadingCount++;
                        break;

                    case "h3":
                        h3HeadingCount++;
                        break;

                    case "h4":
                        h4HeadingCount++;
                        break;

                    default:
                        break;

                }
            }

            allPageHeadings.Add("Number of H1 headings: " + h1HeadingCount);
            allPageHeadings.Add("Number of H2 headings: " + h2HeadingCount);
            allPageHeadings.Add("Number of H3 headings: " + h3HeadingCount);
            allPageHeadings.Add("Number of H4 headings: " + h4HeadingCount);
            allPageHeadings.Add("Total number of headings: " + (h1HeadingCount + h2HeadingCount + h3HeadingCount + h4HeadingCount));

            return allPageHeadings;
        }

        public static List<string> GetAllHeadings(HtmlDocument html)
        {

            var xpath = "//*[self::h1 or self::h2 or self::h3 or self::h4 or self::h5]";

            return html
                    .DocumentNode
                    .SelectNodes(xpath)
                    .Select(node => node.Name + " " + node.InnerText)
                    .ToList();
        }

    }
}
