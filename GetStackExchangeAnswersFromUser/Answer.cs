using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetStackExchangeAnswersFromUser
{

    public class Answer : IStackExchangeItem
    {
        public string AnswerId { get; private set; }
        public string QuestionId { get; private set; }
        public string Body { get; private set; }
      
        public Answer(JToken jtoken)
        {
            AnswerId = jtoken["answer_id"].Value<string>();
            QuestionId = jtoken["question_id"].Value<string>();
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

        public string Write()
        {
            return Body;
        }
    }
}
