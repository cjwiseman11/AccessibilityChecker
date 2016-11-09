using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AccessibilityChecker
{
    class ColourBlindChecker
    {
        List<string> TextColours = new List<string>();
        List<string> BackgroundColours = new List<string>();

        public void GetColours(ChromeDriver driver)
        {
            var TextElements = driver.FindElementsByTagName("p");
            foreach(var TextElement in TextElements)
            {
                TextColours.Add(TextElement.GetCssValue("color"));
                BackgroundColours.Add(TextElement.GetCssValue("background"));
            }
        }
    }
}
