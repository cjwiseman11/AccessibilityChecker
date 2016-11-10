using ColorMine.ColorSpaces;
using ColorMine.ColorSpaces.Comparisons;
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
        public List<string> TextColours = new List<string>();
        public List<string> BackgroundColours = new List<string>();

        public void GetColours(ChromeDriver driver)
        {
            var TextElements = driver.FindElementsByTagName("p");
            foreach(var TextElement in TextElements)
            {
                TextColours.Add(TextElement.GetCssValue("color"));
                BackgroundColours.Add(TextElement.GetCssValue("background"));
            }
        }

        public void GetColourDifference()
        {
            //"rgba(51, 51, 51, 1)"
            var colorToConvert = TextColours[0].ToString();
            colorToConvert = colorToConvert.Replace("rgba(", "");
            colorToConvert = colorToConvert.Replace(")", "");
            var splitColour = colorToConvert.Split(',');

            var r = Convert.ToDouble(splitColour[0]);
            var g = Convert.ToDouble(splitColour[1]);
            var b = Convert.ToDouble(splitColour[2]);

            var rgb = new Rgb { R = r, G = g, B = b };
            var lab = rgb.To<Lab>();

            double deltaE = rgb.Compare(rgb, new Cie1976Comparison());
        }

        // RGB > LAB Conversion
        // Credit goes to https://github.com/THEjoezack/ColorMine for all the below code.
        // http://colormine.org/
        internal static class RgbConverter
        {
            internal static void ToColorSpace(IRgb color, IRgb item)
            {
                item.R = color.R;
                item.G = color.G;
                item.B = color.B;
            }

            internal static IRgb ToColor(IRgb item)
            {
                return item;
            }
        }
        internal static class XyzConverter
        {
            #region Constants/Helper methods for Xyz related spaces
            internal static IXyz WhiteReference { get; private set; } // TODO: Hard-coded!
            internal const double Epsilon = 0.008856; // Intent is 216/24389
            internal const double Kappa = 903.3; // Intent is 24389/27
            static XyzConverter()
            {
                WhiteReference = new Xyz
                {
                    X = 95.047,
                    Y = 100.000,
                    Z = 108.883
                };
            }

            internal static double CubicRoot(double n)
            {
                return Math.Pow(n, 1.0 / 3.0);
            }
            #endregion

            internal static void ToColorSpace(IRgb color, IXyz item)
            {
                var r = PivotRgb(color.R / 255.0);
                var g = PivotRgb(color.G / 255.0);
                var b = PivotRgb(color.B / 255.0);

                // Observer. = 2°, Illuminant = D65
                item.X = r * 0.4124 + g * 0.3576 + b * 0.1805;
                item.Y = r * 0.2126 + g * 0.7152 + b * 0.0722;
                item.Z = r * 0.0193 + g * 0.1192 + b * 0.9505;
            }

            internal static IRgb ToColor(IXyz item)
            {
                // (Observer = 2°, Illuminant = D65)
                var x = item.X / 100.0;
                var y = item.Y / 100.0;
                var z = item.Z / 100.0;

                var r = x * 3.2406 + y * -1.5372 + z * -0.4986;
                var g = x * -0.9689 + y * 1.8758 + z * 0.0415;
                var b = x * 0.0557 + y * -0.2040 + z * 1.0570;

                r = r > 0.0031308 ? 1.055 * Math.Pow(r, 1 / 2.4) - 0.055 : 12.92 * r;
                g = g > 0.0031308 ? 1.055 * Math.Pow(g, 1 / 2.4) - 0.055 : 12.92 * g;
                b = b > 0.0031308 ? 1.055 * Math.Pow(b, 1 / 2.4) - 0.055 : 12.92 * b;

                return new Rgb
                {
                    R = ToRgb(r),
                    G = ToRgb(g),
                    B = ToRgb(b)
                };
            }

            private static double ToRgb(double n)
            {
                var result = 255.0 * n;
                if (result < 0) return 0;
                if (result > 255) return 255;
                return result;
            }

            private static double PivotRgb(double n)
            {
                return (n > 0.04045 ? Math.Pow((n + 0.055) / 1.055, 2.4) : n / 12.92) * 100.0;
            }
        }

        internal static class LabConverter
        {
            internal static void ToColorSpace(IRgb color, ILab item)
            {
                var xyz = new Xyz();
                xyz.Initialize(color);

                var white = XyzConverter.WhiteReference;
                var x = PivotXyz(xyz.X / white.X);
                var y = PivotXyz(xyz.Y / white.Y);
                var z = PivotXyz(xyz.Z / white.Z);

                item.L = Math.Max(0, 116 * y - 16);
                item.A = 500 * (x - y);
                item.B = 200 * (y - z);
            }

            internal static IRgb ToColor(ILab item)
            {
                var y = (item.L + 16.0) / 116.0;
                var x = item.A / 500.0 + y;
                var z = y - item.B / 200.0;

                var white = XyzConverter.WhiteReference;
                var x3 = x * x * x;
                var z3 = z * z * z;
                var xyz = new Xyz
                {
                    X = white.X * (x3 > XyzConverter.Epsilon ? x3 : (x - 16.0 / 116.0) / 7.787),
                    Y = white.Y * (item.L > (XyzConverter.Kappa * XyzConverter.Epsilon) ? Math.Pow(((item.L + 16.0) / 116.0), 3) : item.L / XyzConverter.Kappa),
                    Z = white.Z * (z3 > XyzConverter.Epsilon ? z3 : (z - 16.0 / 116.0) / 7.787)
                };

                return xyz.ToRgb();
            }

            private static double PivotXyz(double n)
            {
                return n > XyzConverter.Epsilon ? CubicRoot(n) : (XyzConverter.Kappa * n + 16) / 116;
            }

            private static double CubicRoot(double n)
            {
                return Math.Pow(n, 1.0 / 3.0);
            }
        }
    }
}
