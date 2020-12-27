using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Geom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Questions2Book
{
    public class UtilPDF
    {

        public static void WriteHtmlText2Pdf(string HtmlText, string pdfFileName)
        {
            WriteHtmlText2Pdf(HtmlText, pdfFileName, PageSize.A4);
        }

            public static void WriteHtmlText2Pdf(string HtmlText, string pdfFileName, PageSize pageSize)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(pdfFileName, FileMode.Create, FileAccess.Write)));
            Document document = new Document(pdfDocument, pageSize);
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(HtmlText, pdfDocument, converterProperties);
        }

        

        public static void NumberPdfDocument(string inFileName, string outFileName, bool includeCovers)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inFileName), new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);

            int numberOfPages = pdfDocument.GetNumberOfPages();
            int pagewidth = Convert.ToInt16(pdfDocument.GetPage(numberOfPages).GetPageSize().GetWidth());

            if (includeCovers)
            {
                for (int i = 1; i <= numberOfPages; i++)
                {
                    document.ShowTextAligned(new Paragraph(i.ToString()),
                              pagewidth / 2, 25, i, TextAlignment.RIGHT, VerticalAlignment.TOP, 0);
                }
            }
            else
            {
                int counter = 1;
                for (int i = 3; i <= numberOfPages - 2; i++)
                {
                    document.ShowTextAligned(new Paragraph(counter.ToString()),
                              pagewidth / 2, 25, i, TextAlignment.RIGHT, VerticalAlignment.TOP, 0);
                    counter++;
                }
            }
            document.Close();
        }

      
        public static void MergePdf(List<string> sources, string merged)
        {
            PdfDocument pdfCombined = new PdfDocument(new PdfWriter(merged));
            PdfMerger merger = new PdfMerger(pdfCombined);

           foreach(string source in sources)
            {
                PdfDocument pdf = new PdfDocument(new PdfReader(source));
                merger.Merge(pdf, 1, pdf.GetNumberOfPages());
                pdf.Close();
            }

            merger.Close();
        }



    }
}
