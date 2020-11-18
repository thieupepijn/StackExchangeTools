using System;
using System.Collections.Generic;
using System.Text;

namespace Questions2Book
{
    public class UtilText
    {

        public static string MarkDown2Text(string markdown)
        {
            string text = markdown.Replace("&#39;", "'");
            text = text.Replace("&gt;", string.Empty);
            text = text.Replace("&quot;", "\"");
            text = text.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            return text;

        }

    }
}
