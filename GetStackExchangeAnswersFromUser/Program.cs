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
            try
            {
                string sitesUrl = "https://api.stackexchange.com/2.2/sites";
                string sitesJson = Util.GetJsonFromUrl(sitesUrl);
                IJEnumerable<JToken> sitesTokens = Util.GetJsonTokensFromJsonString(sitesJson, sitesUrl);
                List<Site> sites = Site.GetSites(sitesTokens);

                string answersUrl = "https://api.stackexchange.com/2.2/users/115746/answers?order=desc&sort=activity&site=workplace&filter=withbody";
                string anwersJson = Util.GetJsonFromUrl(answersUrl);
                IJEnumerable<JToken> answersTokens = Util.GetJsonTokensFromJsonString(anwersJson, answersUrl);
                List<Answer> answers = Answer.GetAnswers(answersTokens);

                string questionsUrl = Question.GetQuestionsUrl(answers);
                string questionsJson = Util.GetJsonFromUrl(questionsUrl);
                IJEnumerable<JToken> questionTokens = Util.GetJsonTokensFromJsonString(questionsJson, questionsUrl);
                List<Question> questions = Question.GetQuestions(questionTokens);
                questions.ForEach(q => q.FindAnswer(answers));

                Console.WriteLine(Util.GetRandomItem(sites.Cast<IStackExchangeItem>().ToList()).Write());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }   
            Console.ReadKey();
        }
    }
}
