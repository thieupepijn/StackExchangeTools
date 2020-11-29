using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using QueryStackExchangeApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Questions2Book
{
    class Program
    {
        static void Main(string[] args)
        {
            string userId = "115746";
            string siteName = "workplace";
            string questionsFileName = "Questions.pdf";
            string referencesFileName = "References.pdf";
            string bookFileName = "Book.pdf";
            string bookFileNameNumbered = "BookNumbered.pdf";

            List<Answer> answers = Answer.GetAnswers(userId, siteName);
            Answer.RemoveBadAnswers(answers);
            List<Question> questions = Question.GetQuestions(answers, siteName);

            string HtmlQuestions = Question.Question2String(questions);
            WriteHtmlText2Pdf(HtmlQuestions, questionsFileName);

            string HtmlReferences = Question.References2String(questions);
            WriteHtmlText2Pdf(HtmlReferences, referencesFileName);

            MergePdf(questionsFileName, referencesFileName, bookFileName);
            NumberPdfDocument(bookFileName, bookFileNameNumbered);
        }


        private static void WriteHtmlText2Pdf(string HtmlText, string pdfFileName)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(pdfFileName, FileMode.Create, FileAccess.Write)));
            Document document = new Document(pdfDocument); 
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(HtmlText, pdfDocument, converterProperties);
        }

        private static void NumberPdfDocument(string inFileName, string outFileName)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inFileName), new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);

            int numberOfPages = pdfDocument.GetNumberOfPages();
            int pagewidth = Convert.ToInt16(pdfDocument.GetPage(numberOfPages).GetPageSize().GetWidth());

            for (int i = 1; i <= numberOfPages; i++)
            {
                document.ShowTextAligned(new Paragraph(i.ToString()),
                          pagewidth / 2, 25, i, TextAlignment.RIGHT, VerticalAlignment.TOP, 0);
            }
            document.Close();
        }
            

        private static void MergePdf(string pdfFile1, string pdfFile2, string mergedPdfFile)
        {
           
            PdfDocument pdfCombined = new PdfDocument(new PdfWriter(mergedPdfFile));
            PdfMerger merger = new PdfMerger(pdfCombined);

            //Add pages from the first document
            PdfDocument pdf1 = new PdfDocument(new PdfReader(pdfFile1));
            merger.Merge(pdf1, 1, pdf1.GetNumberOfPages());

            //Add pages from the second pdf document
            PdfDocument pdf2 = new PdfDocument(new PdfReader(pdfFile2));
            merger.Merge(pdf2, 1, pdf2.GetNumberOfPages());

            pdf1.Close();
            pdf2.Close();
            merger.Close();
        }
            


    }
}
