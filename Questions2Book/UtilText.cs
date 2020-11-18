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
            text = text.Replace("&amp;", "&");
            text = text.Replace("**", string.Empty);
            text = text.Replace("&lt;br", string.Empty);
            text = text.Replace("&lt;sub", string.Empty);//TODO MATTHIEU FOOTNOTES
            text = text.Replace("&lt;i", string.Empty);
            text = text.Replace("&lt;/i", string.Empty);
            text = text.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            return RemoveBracelets(text);
        }

        public static string RemoveBracelets(string text)
        {
            for (int counter=0;counter<100;counter++)
            {
                string line1 = string.Format("[{0}]", counter);
                string line2 = string.Format("@@@{0}@@@", counter);
                text = text.Replace(line1, line2);
            }

            text = text.Replace("[", string.Empty);
            text = text.Replace("]", string.Empty);


            for (int counter = 0; counter < 100; counter++)
            {
                string line1 = string.Format("@@@{0}@@@", counter);
                string line2 = string.Format("[{0}]", counter);  
                text = text.Replace(line1, line2);
            }

            return text;
        }

    }
}
