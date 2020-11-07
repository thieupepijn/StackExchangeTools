using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace GetStackExchangeAnswersFromUser
{
    class Program
    {
       
        static void Main(string[] args)
        {
            try
            {
                string url = "https://api.stackexchange.com/2.2/questions?fromdate=1604188800&todate=1604620800&order=desc&sort=activity&site=workplace&filter=withbody";
                string json = Util.GetJsonFromUrl(url);


                List<Question> questions = Question.GetQuestionsfrom(Util.GetJsonTokensFromJsonString(json, url));
                Question question = Question.GetRandomQuestion(questions);

                if (question != null)
                {
                    Console.WriteLine(question.Title);
                    Console.WriteLine();
                    Console.WriteLine(question.Body);
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
   
            Console.ReadKey();

        }
    }
}
