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
        public string Body { get; private set; }
        public int Score { get; private set; }
        public bool IsAccepted { get; private set; }


        public Answer(JToken jtoken)
        {
            AnswerId = jtoken["answer_id"].Value<string>();
            QuestionId = jtoken["question_id"].Value<string>();
            Body = jtoken["body"].Value<string>();
            Score = jtoken["score"].Value<int>();
            IsAccepted = jtoken["is_accepted"].Value<bool>();
        }

       
        public static List<Answer> GetAnswers(string userId, string siteApiName)
        {
            string answersUrl = Answer.GetAnswersUrl(userId, siteApiName);
            string anwersJson = Util.GetJsonFromUrl(answersUrl);
            IJEnumerable<JToken> answersTokens = Util.GetJsonTokensFromJsonString(anwersJson, answersUrl);
            return Answer.GetAnswers(answersTokens);
        }

        private static List<Answer> GetAnswers(IJEnumerable<JToken> jtokens)
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

        public void WriteToFile(string directoryName)
        {
            string fileName = string.Format("{0}.txt", AnswerId);
            FileInfo fileInfo = new FileInfo(Path.Join(directoryName, fileName));
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            File.WriteAllText(fileInfo.FullName, Body);
        }


        public static string GetAnswersUrl(string userid, string siteApiName)
        {

              string url = string.Format("https://api.stackexchange.com/2.2/users/{0}/answers?pagesize=100&order=desc&sort=activity&site={1}&filter=withbody",
                                       userid, siteApiName);
            //string url = string.Format("https://api.stackexchange.com/2.2/users/115746/answers?fromdate=1606348800&todate=1606608000&order=desc&sort=activity&site={1}&filter=withbody",
             //                          userid, siteApiName);
            return url;
        }
        
        public static void RemoveBadAnswers(List<Answer> answers, int minimalScore = 1)
        {
            //if it is an accepted answer by the OP than it ia also a good answer
            answers.RemoveAll(a => a.Score < minimalScore && !a.IsAccepted);
        }
        

        public static bool WriteAnswerstoFile(List<Answer> answers)
        {
            string directoryName = Path.Join(Directory.GetCurrentDirectory(), "Answers");
            answers.ForEach(a => a.WriteToFile(directoryName));
            return Directory.Exists(directoryName);
        }

        public string WriteReference()
        {
            throw new NotImplementedException();
        }
    }
}
