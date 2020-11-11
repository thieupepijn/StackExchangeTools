using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GetStackExchangeAnswersFromUser
{
    public class Question : IStackExchangeItem
    {

        public string QuestionId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }

        public Question(JToken jtoken)
        {
            QuestionId = jtoken["question_id"].Value<string>();
            Title = jtoken["title"].Value<string>();
            Body = jtoken["body"].Value<string>();
        }

        public static List<Question> GetQuestions(IJEnumerable<JToken> jtokens)
        {
            List<Question> questions = new List<Question>();
            foreach (JToken jtoken in jtokens)
            {
                Question question = new Question(jtoken);
                questions.Add(question);
            }
            return questions;
        }

        public string Write()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(Title);
            builder.AppendLine();
            builder.AppendLine(Body);
            return builder.ToString();
        }
    }
}
