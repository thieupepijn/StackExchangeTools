using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetStackExchangeAnswersFromUser
{

    public class Answer
    {
        public string Body { get; private set; }

        public Answer(JToken jtoken)
        {
            Body = jtoken["body"].Value<string>();
        }

        public static List<Answer> GetAnswers(IJEnumerable<JToken> jtokens)
        {
            List<Answer> answers = new List<Answer>();
            foreach (JToken jtoken in jtokens)
            {
                Answer answer = new Answer(jtoken);
                answers.Add(answer);
            }
            return answers;
        }


        public static Answer GetRandomAnswer(List<Answer> answers)
        {
            if ((answers != null) && (answers.Count > 0))
            {
                Random random = new Random();
                int randomIndexNumber = random.Next(0, answers.Count);
                return answers[randomIndexNumber];
            }
            else
            {
                return null;
            }
        }

    }
}
