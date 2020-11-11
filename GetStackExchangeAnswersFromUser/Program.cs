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
                string answersUrl = "https://api.stackexchange.com/2.2/users/115746/answers?order=desc&sort=activity&site=workplace&filter=withbody";
                string anwersJon = Util.GetJsonFromUrl(answersUrl);
                IJEnumerable<JToken> answersTokens = Util.GetJsonTokensFromJsonString(anwersJon, answersUrl);
                List<Answer> answers = Answer.GetAnswers(answersTokens);


                string questionsUrl = "https://api.stackexchange.com/2.2/questions?fromdate=1604188800&todate=1604620800&order=desc&sort=activity&site=workplace&filter=withbody";
                string questionsJson = Util.GetJsonFromUrl(questionsUrl);
                IJEnumerable<JToken> questionTokens = Util.GetJsonTokensFromJsonString(questionsJson, questionsUrl);
                List<Question> questions = Question.GetQuestions(questionTokens);


                IStackExchangeItem item = Util.GetRandomItem(questions.Cast<IStackExchangeItem>().ToList());

                if (item != null)
                {
                    Console.WriteLine(item.Write());
                }




            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
   
            Console.ReadKey();

        }
    }
}
