using System;
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
            using (StreamWriter writer = new StreamWriter("Output.txt"))
            {
                writer.WriteLine("Does Heading One Exist:");
                writer.WriteLine(Results.DoesHeadingOneExist);

                writer.WriteLine("\nHeading One Check: ");
                writer.WriteLine(Results.HeadingResult);

                writer.WriteLine("\nAlt Tag Check: ");
                foreach (var AltTagResult in Results.AltTagsResult)
                {
                    writer.WriteLine(AltTagResult);
                }

                writer.WriteLine("\nForm Label Check: ");
                foreach (var FormLabelResult in Results.FormLabelResult)
                {
                    writer.WriteLine(FormLabelResult);
                }

                writer.WriteLine("\nLink Title Check: ");
                foreach (var LinkTitleResult in Results.LinkTitleResult)
                {
                    writer.WriteLine(LinkTitleResult);
                }
            }
        }
    }
}
