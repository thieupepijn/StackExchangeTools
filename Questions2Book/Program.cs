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
            string bookFileNumberedWithCovers = "BookNumberedWithCovers.pdf";

            string frontCoverFileName = @"..\Assets\ScrummingTheDayAwayVersion1\Cover.pdf";
          

            string coverFileName = System.AppDomain.CurrentDomain.BaseDirectory;

            List<Answer> answers = Answer.GetAnswers(userId, siteName);
            Answer.RemoveBadAnswers(answers);
            List<Question> questions = Question.GetQuestions(answers, siteName);

            string HtmlQuestions = Question.Question2String(questions);
            UtilPDF.WriteHtmlText2Pdf(HtmlQuestions, questionsFileName);

            string HtmlReferences = Question.References2String(questions);
            UtilPDF.WriteHtmlText2Pdf(HtmlReferences, referencesFileName);

            UtilPDF.MergePdf(questionsFileName, referencesFileName, bookFileName);
            UtilPDF.NumberPdfDocument(bookFileName, bookFileNameNumbered);

            UtilPDF.MergePdf(frontCoverFileName, bookFileNameNumbered, bookFileNumberedWithCovers);
        }


      


    }
}
