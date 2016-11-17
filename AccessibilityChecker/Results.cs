using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessibilityChecker
{
    class Results
    {
        public bool DoesHeadingOneExist { get; set; }

        public string HeadingResult { get; set; }

        public List<string> AltTagsResult { get; set; }

        public List<string> ColourContrastResult { get; set; }

        public List<string> FormLabelResult { get; set; }

        public List<string> ContextlessLinkCheckResult { get; set; }
    }
}
