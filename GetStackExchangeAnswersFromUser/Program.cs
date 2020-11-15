using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

                string answersUrl = Answer.GetAnswersUrl(userid, siteApiName);
                string anwersJson = Util.GetJsonFromUrl(answersUrl);
                IJEnumerable<JToken> answersTokens = Util.GetJsonTokensFromJsonString(anwersJson, answersUrl);
                List<Answer> answers = Answer.GetAnswers(answersTokens);
                Answer.WriteAnswerstoFile(answers);



                string questionsUrl = Question.GetQuestionsUrl(answers);
                string questionsJson = Util.GetJsonFromUrl(questionsUrl);
                IJEnumerable<JToken> questionTokens = Util.GetJsonTokensFromJsonString(questionsJson, questionsUrl);
                List<Question> questions = Question.GetQuestions(questionTokens);
                questions.ForEach(q => q.FindAnswer(answers));
                Question.WriteQuestionsToFile(questions);

                //Console.WriteLine(Util.GetRandomItem(sites.Cast<IStackExchangeItem>().ToList()).Write());
               

                Console.WriteLine(string.Format("{0} items", answers.Count));

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }   
           // Console.ReadKey();
        }
    }
}
