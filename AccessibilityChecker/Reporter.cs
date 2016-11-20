using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AccessibilityChecker
{
    class Reporter
    {
        public void WriteToTextFile(Results Results)
        {
            using (StreamWriter writer = new StreamWriter(DateTime.Now.ToString("d.M.yy H.mm.ss") + " - " + ReplaceUrl(Results.UrlToCheck) + ".txt"))
            {
                writer.WriteLine(Results.UrlToCheck);
                writer.WriteLine();

                writer.Write("Heading One Exists? ");
                writer.Write(Results.DoesHeadingOneExist);
                writer.WriteLine("");

                writer.WriteLine("\nHeading One Check: ");
                writer.WriteLine(Results.HeadingResult);
                writer.WriteLine("");

                writer.WriteLine("\nAlt Tag Check: ");
                foreach (var AltTagResult in Results.AltTagsResult)
                {
                    writer.WriteLine(AltTagResult);
                }
                writer.WriteLine("");

                writer.WriteLine("\nColour ChecK:");
                foreach (var ColourContrastResult in Results.ColourContrastResult)
                {
                    writer.WriteLine(ColourContrastResult);
                }
                writer.WriteLine("");

                writer.WriteLine("\nForm Label Check: ");
                foreach (var FormLabelResult in Results.FormLabelResult)
                {
                    writer.WriteLine(FormLabelResult);
                }
                writer.WriteLine("");

                writer.WriteLine("\nLink Title Check: ");
                foreach (var LinkTitleResult in Results.ContextlessLinkCheckResult)
                {
                    writer.WriteLine(LinkTitleResult);
                }

                writer.WriteLine();
                writer.WriteLine("#######################Successful Stuff#######################");
                foreach (var AltTag in Results.AltTagsFound)
                {
                    writer.WriteLine(AltTag);
                }
            }


        }

        public string ReplaceUrl(string query)
        {
            query = query.Replace("https://", "");
            query = query.Replace("http://", "");
            if (query.Contains("/"))
            {
                query = query.Substring(0, query.IndexOf("/"));
            }

            return query;
        }
    }
}
