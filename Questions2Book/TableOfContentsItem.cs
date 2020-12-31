using System;
using System.Collections.Generic;
using System.Text;

namespace Questions2Book
{
    public class TableOfContentsItem
    {
        public string Title { get; private set; }
        public int PageNumber { get; private set; }

        public TableOfContentsItem(string title, int pagenumber)
        {
            Title = title;
            PageNumber = pagenumber;
        }

    }
}
