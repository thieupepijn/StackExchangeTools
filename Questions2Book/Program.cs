using QueryStackExchangeApi;
using System.Collections.Generic;
using System.Text;

namespace Questions2Book
{
    class Program
    {
        static void Main(string[] args)
        {
            string userId = "115746";
            string siteName = "workplace";
            
            string colofonFileName = "Colofon.pdf";
            string tableOfContentsFileName = "TableOfContents.pdf";
            string questionsFileName = "Questions.pdf";
            string referencesFileName = "References.pdf";
            string bookFileName = "Book.pdf";
            string bookFileNameNumbered = "BookNumbered.pdf";

            List<Answer> answers = Answer.GetAnswers(userId, siteName);
            Answer.RemoveBadAnswers(answers);
            List<Question> questions = Question.GetQuestions(answers, siteName);

            List<TableOfContentsItem> tableOfContentsList = MakeQuestionPages(questions, 1, questionsFileName);
            MakeTableOfContentsPages(tableOfContentsList, tableOfContentsFileName);

            string HtmlReferences = Question.References2String(questions);
            UtilPDF.WriteHtmlText2Pdf(HtmlReferences, referencesFileName);
          
            MakeColofonPage(colofonFileName);
           
            List<string> sources = new List<string>();
            sources.Add(colofonFileName);
            sources.Add(tableOfContentsFileName);
            sources.Add(questionsFileName);
            sources.Add(referencesFileName);
         

            UtilPDF.MergePdf(sources, bookFileName);

            int colofonPages = UtilPDF.GetNumberOfPages(colofonFileName);
            int tableOfContentPages = UtilPDF.GetNumberOfPages(tableOfContentsFileName);
            int numberPagesStart = colofonPages + tableOfContentPages + 1;

            UtilPDF.NumberPdfDocument(bookFileName, bookFileNameNumbered, numberPagesStart);
        }


        private static void MakeColofonPage(string fileName)
        {
            string source = @"https://www.flickr.com/photos/84263554@N00/50382963456/in/photolist-2jLaRHJ-7ib7V9-nknW6J-5xjyd4-2inRnzM-2j1Gnqo-H6a2Rb-2k57AJP-24eYEMe-2j2nuX5-Jo8Nud-NP26SL-2gfMeVW-6qxvo2-uvtg31-2bsEgTD-TQ6e7h-NexjzR-73gx6e-DcG9wj-5tYc2M-esi8GU-36tUJr-2i6cD-8vGnwi-28tfqdu-2k53Mp6-p6MFqt-5Dw5v-2gKGJdS-CnvUbT-UmmP9X-2bu2kGM-NrW4AD-W61rLT-bDZAFD-2jupJLv-5n9Cyo-RwSnpY-5xVU7L-29JyniY-a1E7P5-QzqYnN-UTtmNy-24Feos-7ZCyAG-22KEjh5-XHif9b-FjiyYV-MxynM000000";
            string licence = @"Attribution-ShareAlike 2.0 Generic (CC BY-SA 2.0)";
            string newLine = "<BR>";
            string text = string.Format("Cover image taken from {0} {0} {1} {0} {0} under the {2} license",
                          newLine, source, licence);
            UtilPDF.WriteHtmlText2Pdf(text, fileName);
        }

        private static List<TableOfContentsItem> MakeQuestionPages(List<Question> questions, int questionsStartNumberPage, string questionsPdFFileName)
        {
            List<TableOfContentsItem> tableOfContents = new List<TableOfContentsItem>();
            int pageCounter = questionsStartNumberPage;
            List<string> fileNames = new List<string>();
            foreach (Question question in questions)
            {
                string questionFileName = string.Format("question{0}.pdf", question.QuestionId);
                string questionHtml = question.Write();
                UtilPDF.WriteHtmlText2Pdf(questionHtml, questionFileName);
                fileNames.Add(questionFileName);
                tableOfContents.Add(new TableOfContentsItem(question.Title, pageCounter));
                int numberOfPages = UtilPDF.GetNumberOfPages(questionFileName);
                pageCounter += numberOfPages;
            }
            UtilPDF.MergePdf(fileNames, questionsPdFFileName);
            return tableOfContents;
        }

        private static void MakeTableOfContentsPages(List<TableOfContentsItem> tableOfContents, string tableOfContentsPdfFileName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<H1>Table of contents</H1>");
            builder.AppendLine("<BR>");

            foreach(TableOfContentsItem contentItem in tableOfContents)
            {
                builder.AppendLine(string.Format("{0} <B>{1}</B>", contentItem.Title, contentItem.PageNumber));
                builder.AppendLine("<BR>");
                builder.AppendLine("<BR>");
            }
            UtilPDF.WriteHtmlText2Pdf(builder.ToString(), tableOfContentsPdfFileName);
        }



#region obsolete

        private static void WriteQuestionsToPDF(List<Question> questions, string questionsPdFFileName)
        {
            string HtmlQuestions = Question.Question2String(questions);
            UtilPDF.WriteHtmlText2Pdf(HtmlQuestions, questionsPdFFileName);
        }


        private static void MakeBlankPage(string blankPageFileName)
        {
            string newLine = "<BR>";
            UtilPDF.WriteHtmlText2Pdf(newLine, blankPageFileName);         
        }

#endregion


    }
}
