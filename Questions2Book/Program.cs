using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Geom;
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
            string frontCoverFileName = @"..\Assets\ScrummingTheDayAwayVersion1\Cover.pdf";
            string frontCoverBackFileName = "secondPage";
            string questionsFileName = "Questions.pdf";
            string referencesFileName = "References.pdf";
            string backCover1FileName = "backCover1.pdf";
            string backCover2FileName = "backCover2.pdf";

            string bookFileName = "Book.pdf";
            string bookFileNameNumbered = "BookNumbered.pdf";


            List<Answer> answers = Answer.GetAnswers(userId, siteName);
            Answer.RemoveBadAnswers(answers);
            List<Question> questions = Question.GetQuestions(answers, siteName);

            string HtmlQuestions = Question.Question2String(questions);
            UtilPDF.WriteHtmlText2Pdf(HtmlQuestions, questionsFileName);

            string HtmlReferences = Question.References2String(questions);
            UtilPDF.WriteHtmlText2Pdf(HtmlReferences, referencesFileName);
          
            MakeFrontCoverImageSourcePage(frontCoverBackFileName);
            MakeBackCover(backCover1FileName, backCover2FileName);

            List<string> sources = new List<string>();
            sources.Add(frontCoverFileName);
            sources.Add(frontCoverBackFileName);
            sources.Add(questionsFileName);
            sources.Add(referencesFileName);
            sources.Add(backCover1FileName);
            sources.Add(backCover2FileName);

            UtilPDF.MergePdf(sources, bookFileName);

            UtilPDF.NumberPdfDocument(bookFileName, bookFileNameNumbered, false);
        }


        private static void MakeFrontCoverImageSourcePage(string fileName)
        {
            string source = @"https://www.flickr.com/photos/84263554@N00/50382963456/in/photolist-2jLaRHJ-7ib7V9-nknW6J-5xjyd4-2inRnzM-2j1Gnqo-H6a2Rb-2k57AJP-24eYEMe-2j2nuX5-Jo8Nud-NP26SL-2gfMeVW-6qxvo2-uvtg31-2bsEgTD-TQ6e7h-NexjzR-73gx6e-DcG9wj-5tYc2M-esi8GU-36tUJr-2i6cD-8vGnwi-28tfqdu-2k53Mp6-p6MFqt-5Dw5v-2gKGJdS-CnvUbT-UmmP9X-2bu2kGM-NrW4AD-W61rLT-bDZAFD-2jupJLv-5n9Cyo-RwSnpY-5xVU7L-29JyniY-a1E7P5-QzqYnN-UTtmNy-24Feos-7ZCyAG-22KEjh5-XHif9b-FjiyYV-MxynM000000";
            string licence = @"Attribution-ShareAlike 2.0 Generic (CC BY-SA 2.0)";
            string newLine = "<BR>";
            string text = string.Format("Image taken from {0} {0} {1} {0} {0} under the {2} license", 
                          newLine, source, licence);
            UtilPDF.WriteHtmlText2Pdf(text, fileName);
        }

        private static void MakeBackCover(string backCover1FileName, string backCover2FileName)
        {
            string newLine = "<BR>";
            UtilPDF.WriteHtmlText2Pdf(newLine, backCover1FileName);
            UtilPDF.WriteHtmlText2Pdf(newLine, backCover2FileName);
        }




    }
}
