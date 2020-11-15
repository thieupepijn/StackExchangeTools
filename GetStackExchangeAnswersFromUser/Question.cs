using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;

namespace GetStackExchangeAnswersFromUser
{
    public class Question : IStackExchangeItem
    {

        public string QuestionId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public Answer Answer { get; private set; }

        public Question(JToken jtoken)
        {
            QuestionId = jtoken["question_id"].Value<string>();
            Title = jtoken["title"].Value<string>();
            Body = jtoken["body_markdown"].Value<string>();
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

        public void FindAnswer(List<Answer> answers)
        {
            foreach(Answer answer in answers)
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
            builder.AppendLine(Title);
            builder.AppendLine();
            builder.AppendLine(Body);
            if (Answer != null)
            {
                builder.AppendLine();
                builder.AppendLine(Answer.Write());
            }
            return builder.ToString();
        }

        public void WriteToFile(DirectoryInfo directory)
        {
            string fileName = string.Format("{0}.txt", QuestionId);
            FileInfo fileInfo = new FileInfo(Path.Join(directory.FullName, fileName));
            File.WriteAllText(fileInfo.FullName, Body);
        }

        public static string GetQuestionsUrl(List<Answer> answers)
        {
            string ids = string.Join(';', answers.Select(a => a.QuestionId));
            string url = string.Format("https://api.stackexchange.com/2.2/questions/{0}?order=desc&sort=activity&site=workplace&filter=!9_bDDx5MI", ids);
            return url;
        }

        public static bool WriteQuestionsToFile(List<Question> questions)
        {
            DirectoryInfo directoyInfo = Directory.CreateDirectory("Questions");
            questions.ForEach(q => q.WriteToFile(directoyInfo));
            return directoyInfo.Exists;
        }
    }
}
