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
                    if (ImgNode.Attributes["src"] != null)
                    {
                        Console.WriteLine("Missing Alt Tag on Image: " + ImgNode.Attributes["src"].Value);
                    } else
                    {
                        Console.WriteLine("Missing Alt Tag on Image: " + ImgNode.Attributes["data-src"].Value);

                    }
                    Fails++;
                } else
                {
                    results.Insert(1,"Alt Tag Found: " + ImgNode.Attributes["src"].Value + " Alt: " + ImgNode.Attributes["alt"].Value);
                    Passes++;
                }
            }
            Console.WriteLine("Fails: " + Fails + ", Passes: " + Passes);
        }
    }
}
