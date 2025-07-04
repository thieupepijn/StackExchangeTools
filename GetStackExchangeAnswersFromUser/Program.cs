﻿using QueryStackExchangeApi;
using System;
using System.Collections.Generic;

namespace GetStackExchangeAnswersFromUser
{
    class Program
    {
       
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: GetStackExchangeAnswersFromUser <userId> <siteApiName");
                return;
            }

            string userid = args[0];
            string siteApiName = args[1];

            
            try
            {
                /*
                    string sitesUrl = "https://api.stackexchange.com/2.2/sites?pagesize=1000";
                    string sitesJson = Util.GetJsonFromUrl(sitesUrl);
                    IJEnumerable<JToken> sitesTokens = Util.GetJsonTokensFromJsonString(sitesJson, sitesUrl);
                    List<Site> sites = Site.GetSites(sitesTokens);
                */


                List<Answer> answers = Answer.GetAnswers(userid, siteApiName);
                Answer.WriteAnswerstoFile(answers);

                List<Question> questions = Question.GetQuestions(answers, siteApiName);
                questions.ForEach(q => q.FindAnswer(answers));
                Question.WriteQuestionsToFile(questions);

                //Console.WriteLine(Util.GetRandomItem(sites.Cast<IStackExchangeItem>().ToList()).Write());
               

                Console.WriteLine(string.Format("{0} questions and {1} answers retrieved", answers.Count, questions.Count));

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }   
            Console.ReadKey();
        }
    }
}
