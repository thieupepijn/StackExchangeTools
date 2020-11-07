using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GetStackExchangeAnswersFromUser
{
    public class Question
    {

        public string Title { get; private set; }
        public string Body { get; private set; }

        public Question(JToken jsonQuestion)
        {
            Title = jsonQuestion["title"].Value<string>();
            Body = jsonQuestion["body"].Value<string>();
        }

        public static List<Question> GetQuestionsfrom(IJEnumerable<JToken> jsonQuestions)
        {
            List<Question> questions = new List<Question>();
            foreach (JToken jsonQuestion in jsonQuestions)
            {
                Question question = new Question(jsonQuestion);
                questions.Add(question);
            }
            return questions;
        }

        public static Question GetRandomQuestion(List<Question> questions)
        {
            if ((questions != null) && (questions.Count > 0))
            {
                Random random = new Random();
                int randomIndexNumber = random.Next(0, questions.Count);
                return questions[randomIndexNumber];
            }
            else
            {
                return null;
            }
        }


    }
}
