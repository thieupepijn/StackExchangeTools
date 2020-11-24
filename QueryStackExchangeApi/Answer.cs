using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QueryStackExchangeApi
{

    public class Answer : IStackExchangeItem
    {
        public string AnswerId { get; private set; }
        public string QuestionId { get; private set; }
        public Enums.BodyType BodyType { get; private set; }
        public string Body { get; private set; }

        public Answer(JToken jtoken, Enums.BodyType bodyType)
        {
            AnswerId = jtoken["answer_id"].Value<string>();
            QuestionId = jtoken["question_id"].Value<string>();
            BodyType = bodyType;
            if (BodyType == Enums.BodyType.MARKDOWN)
            {
                Body = jtoken["body_markdown"].Value<string>();
            }
            else if (BodyType == Enums.BodyType.HTML)
            {
                Body = jtoken["body"].Value<string>();
            }
        }

       
        public static List<Answer> GetAnswers(string userId, string siteApiName, Enums.BodyType bodyType)
        {
            string answersUrl = Answer.GetAnswersUrl(userId, siteApiName, bodyType);
            string anwersJson = Util.GetJsonFromUrl(answersUrl);
            IJEnumerable<JToken> answersTokens = Util.GetJsonTokensFromJsonString(anwersJson, answersUrl);
            return Answer.GetAnswers(answersTokens, bodyType);
        }

        private static List<Answer> GetAnswers(IJEnumerable<JToken> jtokens, Enums.BodyType bodyType)
        {
            List<Answer> answers = new List<Answer>();
            foreach (JToken jtoken in jtokens)
            {
                Answer answer = new Answer(jtoken, bodyType);
                answers.Add(answer);
            }
            return answers;
        }




        public string Write()
        {
            return Body;
        }

        public void WriteToFile(string directoryName)
        {
            string fileName = string.Format("{0}.txt", AnswerId);
            string subDirectoryName = Enum.GetName(typeof(Enums.BodyType), BodyType);
            FileInfo fileInfo = new FileInfo(Path.Join(directoryName, subDirectoryName, fileName));
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            File.WriteAllText(fileInfo.FullName, Body);
        }

     
        public static string GetAnswersUrl(string userid, string siteApiName, Enums.BodyType bodyType)
        {
            if (bodyType == Enums.BodyType.MARKDOWN)
            {

                string url = string.Format("https://api.stackexchange.com/2.2/users/{0}/answers?pagesize=100&order=desc&sort=activity&site={1}&filter=!9_bDE(S6I",
                                           userid, siteApiName);
                return url;
            }
            else // if (bodyType == Enums.BodyType.HTML)
            {
                string url = string.Format("https://api.stackexchange.com/2.2/users/{0}/answers?pagesize=100&order=desc&sort=activity&site={1}&filter=withbody",
                                         userid, siteApiName);
                return url;
            }
        }

        public static bool WriteAnswerstoFile(List<Answer> answers)
        {
            string directoryName = Path.Join(Directory.GetCurrentDirectory(), "Answers");
            answers.ForEach(a => a.WriteToFile(directoryName));
            return Directory.Exists(directoryName);
        }
    }
}
