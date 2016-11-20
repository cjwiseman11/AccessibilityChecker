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

        public bool HeadingAmount(HtmlDocument doc)
        {
            //Check for Headings
            HtmlNodeCollection HeadingTwoList = doc.DocumentNode.SelectNodes("//h2");
            HtmlNodeCollection HeadingThreeList = doc.DocumentNode.SelectNodes("//h3");
            HtmlNodeCollection HeadingFourList = doc.DocumentNode.SelectNodes("//h4");
            if (HeadingOneList != null)
            {
                Console.WriteLine("H1: " + HeadingOneList.Count);
            } else if(HeadingOneList == null)
            {
                Console.WriteLine("No H1 Detected!");
                return false;
            }
            if (HeadingTwoList != null)
            {
                Console.WriteLine("H2: " + HeadingTwoList.Count);
            }
            if (HeadingThreeList != null)
            {
                Console.WriteLine("H3: " + HeadingThreeList.Count);
            }
            if (HeadingFourList != null)
            {
                Console.WriteLine("H4 " + HeadingFourList.Count);
            }
            return true;
        }

        public string HeadingOneCheck(HtmlDocument doc)
        {
            //Check if Heading1 is first
            if (HeadingOneList.Count > 1)
            {
                return "Please Fix, There Should be 1 HeadingOne";
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
    }
}
