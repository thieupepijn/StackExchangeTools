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
        public Enums.BodyType  BodyType { get; private set; }
        public string Body { get; private set; }
        public Answer Answer { get; private set; }

        public Question(JToken jtoken, Enums.BodyType bodyType)
        {
            QuestionId = jtoken["question_id"].Value<string>();
            Title = jtoken["title"].Value<string>();
            BodyType = bodyType;
            if (bodyType == Enums.BodyType.MARKDOWN)
            {
                Body = jtoken["body_markdown"].Value<string>();
            }
            else
            {
                Body = jtoken["body"].Value<string>();
            }
        }

        public Question(string questionId, Enums.BodyType bodyType)
        {
            string url = string.Empty;

            if (bodyType == Enums.BodyType.MARKDOWN)
            {
                url = string.Format("https://api.stackexchange.com/2.2/questions/{0}?order=desc&sort=activity&site=workplace&filter=!9_bDDx5MI", questionId);
            }
            else if (bodyType == Enums.BodyType.HTML)
            {
                url = string.Format("https://api.stackexchange.com/2.2/questions/{0}?order=desc&sort=activity&site=workplace&filter=withbody", questionId);

            }

            string questionsJson = Util.GetJsonFromUrl(url);
            IJEnumerable<JToken> questionTokens = Util.GetJsonTokensFromJsonString(questionsJson, url);
            new Question(questionTokens.First(), bodyType);

        }


        //public static List<Question> GetQuestions(List<Answer> answers, Enums.BodyType bodyType)
        //{
        //    string questionsUrl = Question.GetQuestionsUrl(answers, bodyType);
        //    string questionsJson = Util.GetJsonFromUrl(questionsUrl);
        //    IJEnumerable<JToken> questionTokens = Util.GetJsonTokensFromJsonString(questionsJson, questionsUrl);
        //    return Question.GetQuestions(questionTokens, bodyType);
        //}


        public static List<Question> GetQuestions(List<Answer> answers, Enums.BodyType bodyType)
        {
            List<Question> questions = new List<Question>();
            foreach(Answer answer in answers)
            {
                Question question = new Question(answer.QuestionId, bodyType);
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

        public void WriteToFile(string directoryName)
        {
            string fileName = string.Format("{0}.txt", QuestionId);
            string subDirectoryName = Enum.GetName(typeof(Enums.BodyType), BodyType);
            FileInfo fileInfo = new FileInfo(Path.Join(directoryName, subDirectoryName, fileName));
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            File.WriteAllText(fileInfo.FullName, Body);
        }

        public static string GetQuestionsUrl(List<Answer> answers, Enums.BodyType bodyType)
        {
            string ids = string.Join(';', answers.Select(a => a.QuestionId));
            if (bodyType == Enums.BodyType.MARKDOWN)
            {
                string url = string.Format("https://api.stackexchange.com/2.2/questions/{0}?order=desc&sort=activity&site=workplace&filter=!9_bDDx5MI", ids);
                return url;
            }
            else
            {
                string url = string.Format("https://api.stackexchange.com/2.2/questions/{0}?order=desc&sort=activity&site=workplace&filter=withbody", ids);
                return url;
            }
        }

        private static List<Question> GetQuestions(IJEnumerable<JToken> jtokens, Enums.BodyType bodyType)
        {
            List<Question> questions = new List<Question>();
            foreach (JToken jtoken in jtokens)
            {
                Question question = new Question(jtoken, bodyType);
                questions.Add(question);
            }
            return questions;
        }

        

        public static bool WriteQuestionsToFile(List<Question> questions)
        {
            string directoryName = Path.Join(Directory.GetCurrentDirectory(), "Questions");
            questions.ForEach(q => q.WriteToFile(directoryName));
            return Directory.Exists(directoryName);
        }
    }
}
