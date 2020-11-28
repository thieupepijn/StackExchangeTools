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

            List<Answer> answers = Answer.GetAnswers(userId, siteName);
            List<Question> questions = Question.GetQuestions(answers, siteName);

            WriteQuestions2Book(questions, bookFileName);
            NumberPdfDocument(bookFileName, bookFileNameNumbered);
        }


        private static void WriteQuestions2Book(List<Question> questions, string bookFileName)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(bookFileName, FileMode.Create, FileAccess.Write)));
            Document document = new Document(pdfDocument);

            string allText = Question.Question2String(questions);
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(allText, pdfDocument, converterProperties);
        }

        private static void NumberPdfDocument(string inFilePath, string outFilePath)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(inFilePath), new PdfWriter(outFilePath));
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
