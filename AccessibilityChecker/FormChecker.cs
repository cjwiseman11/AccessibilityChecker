using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessibilityChecker
{
    class FormChecker
    {
        public List<string> LabelCheck(HtmlDocument doc)
        {
            var FormFieldList = doc.DocumentNode.Descendants("input");
            var Passes = 0;
            var Fails = 0;
            List<string> results = new List<string>();

            Console.WriteLine("Field Inputs Found: " + FormFieldList.Count());

            foreach(var FormField in FormFieldList)
            {
                if (FormField.Attributes["name"] != null || (FormField.Attributes["aria-hidden"] != null && FormField.Attributes["aria-hidden"].Value.Equals("true")))
                {
                    if(FormField.Attributes["aria-hidden"] != null || FormField.Attributes["aria-label"] != null || (FormField.PreviousSibling != null && FormField.PreviousSibling.Name.Equals("label") || FormField.NextSibling != null && FormField.NextSibling.Name.Equals("label")))
                    {
                        Passes++;
                    }
                    else
                    {
                        Fails++;
                        Console.WriteLine("Missing Label for Form Field: " + FormField.Attributes["name"].Value);
                        results.Add("Missing Label for Form Field: " + FormField.Attributes["name"].Value);
                    }
                }
                else
                {
                    Fails++;
                    var FormFieldType = "";
                    if(FormField.Attributes["type"] == null)
                    {
                        FormFieldType = "Error";
                    } else
                    {
                        FormFieldType = FormField.Attributes["type"].Value;
                    }
                    switch (FormFieldType)
                    {
                        case "button":
                            Console.WriteLine("Consider changing button from an 'input' to 'button' element.");
                            results.Add("Consider changing button from an 'input' to 'button' element.");
                            break;
                        case "Error":
                            Console.WriteLine("Missing key attributes on element: " + FormField.OuterHtml);
                            results.Add("Missing key attributes on element: " + FormField.OuterHtml);
                            break;
                        default:
                            Console.WriteLine("Input with type [" + FormFieldType + "] is missing name tag or is being used incorrectly");
                            results.Add("Input with type [" + FormFieldType + "] is missing name tag or is being used incorrectly");
                            break;
                    }
                }
            }
            Console.Write("\nFails: " + Fails + ", Passes: " + Passes);
            return results;
        }
    }
}
