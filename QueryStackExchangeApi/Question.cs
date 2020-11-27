using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;

namespace QueryStackExchangeApi
{
    public class Question : IStackExchangeItem
    {
       
        public string QuestionId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public Answer Answer { get; private set; }


        public Question(Answer answer, string siteName)
        {
            string url = string.Format("https://api.stackexchange.com/2.2/questions/{0}?order=desc&sort=activity&site={1}&filter=withbody", answer.QuestionId, siteName);

            string questionsJson = Util.GetJsonFromUrl(url);
            IJEnumerable<JToken> questionTokens = Util.GetJsonTokensFromJsonString(questionsJson, url);
            JToken jToken = questionTokens.First();

            QuestionId = jToken["question_id"].Value<string>();
            Title = jToken["title"].Value<string>();
            Body = jToken["body"].Value<string>();
            Answer = answer;
        }

        public static List<Question> GetQuestions(List<Answer> answers, string siteName)
        {
            List<Question> questions = new List<Question>();
            foreach(Answer answer in answers)
            {
                Question question = new Question(answer, siteName);
                questions.Add(question);
            }
            return questions;
        }


        public void FindAnswer(List<Answer> answers)
        {
            foreach (Answer answer in answers)
            {
                if (string.Equals(answer.QuestionId, QuestionId, StringComparison.OrdinalIgnoreCase))
                {
                    Answer = answer;
                }
            }
        }

        public string Write()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<H1>");
            builder.AppendLine(Title);
            builder.AppendLine("</H1>");
            builder.AppendLine();
            builder.AppendLine(Body);
            if (Answer != null)
            {
                builder.AppendLine();
                builder.AppendLine("<H3>Answer</H3>");
                builder.AppendLine();
                builder.AppendLine(Answer.Write());
            }
            return builder.ToString();
        }

        public void WriteToFile(string directoryName)
        {
            string fileName = string.Format("{0}.txt", QuestionId);
            FileInfo fileInfo = new FileInfo(Path.Join(directoryName, fileName));
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            File.WriteAllText(fileInfo.FullName, Body);
        }

        public static bool WriteQuestionsToFile(List<Question> questions)
        {
            string directoryName = Path.Join(Directory.GetCurrentDirectory(), "Questions");
            questions.ForEach(q => q.WriteToFile(directoryName));
            return Directory.Exists(directoryName);
        }

        public static string Question2String(List<Question> questions)
        {
            StringBuilder builder = new StringBuilder();
            foreach(Question question in questions)
            {
                string text = question.Write();
                text.Trim(Environment.NewLine.ToCharArray());
                builder.AppendLine(text);
            }
            return builder.ToString();
        }

    }
}
