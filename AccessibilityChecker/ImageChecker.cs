using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessibilityChecker
{
    class ImageChecker
    {
        public void AltTagsCheck(HtmlDocument doc)
        {
            var Fails = 0;
            var Passes = 0;
            var ImgNodes = doc.DocumentNode.Descendants("img");

            Console.WriteLine("Images Found: " + ImgNodes.Count());

            foreach(var ImgNode in ImgNodes)
            {
                if(ImgNode.Attributes["alt"] == null || ImgNode.Attributes["alt"].Equals(""))
                {
                    Console.WriteLine("Missing Alt Tag on Image: " + ImgNode.Attributes["src"].Value);
                    Fails++;
                } else
                {
                    Passes++;
                }
            }
            Console.WriteLine("Fails: " + Fails + ", Passes: " + Passes);
        }
    }
}
