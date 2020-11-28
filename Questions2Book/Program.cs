using iText.Html2pdf;
using iText.Kernel.Pdf;
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
            string bookFileName = "WorkplaceQuestions.pdf";
            string bookFileNameNumbered = "WorkplaceQuestionsNumbered.pdf";
            string referencesFileName = "References.pdf";

            List<Answer> answers = Answer.GetAnswers(userId, siteName);
            List<Question> questions = Question.GetQuestions(answers, siteName);
           
            string HtmlQuestions = Question.Question2String(questions);

            WriteHtmlText2Pdf(HtmlQuestions, bookFileName);
            NumberPdfDocument(bookFileName, bookFileNameNumbered);

            string HtmlReferences = Question.References2String(questions);
            WriteHtmlText2Pdf(HtmlReferences, referencesFileName);
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
            




    }
}
