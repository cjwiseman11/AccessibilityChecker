﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessibilityChecker
{
    class ImageChecker
    {

        public List<string> AltTagsCheck(HtmlDocument doc)
        {
            var Fails = 0;
            var Passes = 0;
            var ImgNodes = doc.DocumentNode.Descendants("img");
            List<string> results = new List<string>();

            Console.WriteLine("Images Found: " + ImgNodes.Count());
            results.Add("Images Found: " + ImgNodes.Count());

            foreach (var ImgNode in ImgNodes)
            {
                if (ImgNode.Attributes["alt"] == null || ImgNode.Attributes["alt"].Equals(""))
                {
                    if (ImgNode.Attributes["src"] != null)
                    {
                        results.Add("Missing Alt Tag on Image: " + ImgNode.Attributes["src"].Value);
                    } else
                    {
                        results.Add("Missing Alt Tag on Image which has no src value");

                    }
                    Fails++;
                } else
                {
                    AltTagsFoundList.Add("Alt Tag Found: " + ImgNode.Attributes["src"].Value + " Alt: " + ImgNode.Attributes["alt"].Value);
                    Passes++;
                }
            }
            Console.WriteLine("Fails: " + Fails + ", Passes: " + Passes);
            return results;
        }
        public List<string> AltTagsFoundList = new List<string>();

        public static List<string> PageImages(HtmlDocument doc)
        {

            var imageNodes = doc.DocumentNode.Descendants("img");

            List<string> images = new List<string>();

            foreach (var imageNode in imageNodes)
            {
                string source = "";
                string alt = "";

                try
                {

                    alt = imageNode.Attributes["alt"].Value;

                    if (alt == null || alt == "")
                    {
                        alt = "No alt text found.";
                    }
                }
                catch
                {
                    alt = "No alt attribute found.";
                }

                try
                {
                    source = imageNode.Attributes["src"].Value;

                    if (source == null || source == "")
                    {
                        source = "No source found";
                    }

                }
                catch
                {
                    source = "No src attribute found.";
                }

                images.Add(source + " *** " + alt);
            }
            return images;
        }
    }
}
