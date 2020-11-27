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
           
            List<Answer> answers = Answer.GetAnswers(userId, siteName);
            Answer.WriteAnswerstoFile(answers);

            List<Question> questions = Question.GetQuestions(answers, siteName);
            questions.ForEach(q => q.FindAnswer(answers));


            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream("WorkplaceQuestions.pdf", FileMode.Create, FileAccess.Write)));
            Document document = new Document(pdfDocument);

            string allText = Question.Question2String(questions);
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(allText, pdfDocument, converterProperties);


        }





    }
}
