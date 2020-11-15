using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

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

        public void WriteToFile(DirectoryInfo directory)
        {
            string fileName = string.Format("{0}.txt", AnswerId);
            FileInfo fileInfo = new FileInfo(Path.Join(directory.FullName, fileName));
            File.WriteAllText(fileInfo.FullName, Body);
        }

        public static string GetAnswersUrl(string userid, string siteApiName)
        {
           string url = string.Format("https://api.stackexchange.com/2.2/users/{0}/answers?pagesize=100&order=desc&sort=activity&site={1}&filter=withbody", 
                                      userid, siteApiName);
           return url;
        }

        public static bool WriteAnswerstoFile(List<Answer> answers)
        {
            DirectoryInfo directoyInfo = Directory.CreateDirectory("Answers");
            answers.ForEach(a => a.WriteToFile(directoyInfo));
            return directoyInfo.Exists;
        }
    }
}
