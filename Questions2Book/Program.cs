using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using QueryStackExchangeApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Questions2Book
{
    class Program
    {
        static void Main(string[] args)
        {
            string userId = "115746";
            string siteName = "workplace";

            List<Answer> answers = Answer.GetAnswers(userId, siteName);
            Answer.WriteAnswerstoFile(answers);

            List<Question> questions = Question.GetQuestions(answers);
            questions.ForEach(q => q.FindAnswer(answers));


            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream("WorkplaceQuestions.pdf", FileMode.Create, FileAccess.Write)));
            Document document = new Document(pdfDocument);

           
            foreach (Question question in questions)
            {
                
                Paragraph paragraphTitle = new Paragraph(MarkDown2Text(question.Title));
                paragraphTitle.SetBold();
                paragraphTitle.SetFontSize(14);
                document.Add(paragraphTitle);

                Paragraph paragraphBody = new Paragraph(MarkDown2Text(question.Body));
                document.Add(paragraphBody);

                Paragraph paragraphAnswer = new Paragraph(MarkDown2Text(question.Answer.Body));
                paragraphAnswer.SetItalic();
                document.Add(paragraphAnswer);

                int numberOfPages = pdfDocument.GetNumberOfPages();
                int pagewidth = Convert.ToInt16(pdfDocument.GetPage(numberOfPages).GetPageSize().GetWidth());


                document.ShowTextAligned(new Paragraph(numberOfPages.ToString()),
                      pagewidth / 2, 20, numberOfPages, TextAlignment.RIGHT, VerticalAlignment.TOP, 0);
            }        
            document.Close();
        }

        private static string MarkDown2Text(string markdown)
        {
            string text = markdown.Replace("&#39;", "'");
            text = text.Replace("&gt;", string.Empty);
            text = text.Replace("&quot;", "\"");
            text = text.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            return text;

        }
    }
}
