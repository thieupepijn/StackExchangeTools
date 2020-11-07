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
                string questionsUrl = "https://api.stackexchange.com/2.2/questions?fromdate=1604188800&todate=1604620800&order=desc&sort=activity&site=workplace&filter=withbody";
                string questionsJson = Util.GetJsonFromUrl(questionsUrl);

                IJEnumerable<JToken> questionTokens = Util.GetJsonTokensFromJsonString(questionsJson, questionsUrl);
                List<Question> questions = Question.GetQuestions(questionTokens);
                Question question = Question.GetRandomQuestion(questions);



                /*
                if (question != null)
                {
                    Console.WriteLine(question.Title);
                    Console.WriteLine();
                    Console.WriteLine(question.Body);
                }*/


                string answersUrl = "https://api.stackexchange.com/2.2/users/115746/answers?order=desc&sort=activity&site=workplace&filter=withbody";
                string anwersJon = Util.GetJsonFromUrl(answersUrl);

                IJEnumerable<JToken> answersTokens = Util.GetJsonTokensFromJsonString(anwersJon, answersUrl);
                List<Answer> answers = Answer.GetAnswers(answersTokens);
                Answer answer = Answer.GetRandomAnswer(answers);

                if (answer != null)
                {          
                    Console.WriteLine(answer.Body);
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
