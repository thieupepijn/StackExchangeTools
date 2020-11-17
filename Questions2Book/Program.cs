using QueryStackExchangeApi;
using System;
using System.Collections.Generic;

namespace Questions2Book
{
    class Program
    {
        static void Main(string[] args)
        {
            string userId = "115746";
            string siteName = "workplace";

            List<Answer> answers = Answer.GetAnswers(userId, siteName);
            Answer.WriteAnswerstoFile(answers);

            List<Question> questions = Question.GetQuestions(answers);
            questions.ForEach(q => q.FindAnswer(answers));
        }
    }
}
