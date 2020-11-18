using Microsoft.VisualStudio.TestTools.UnitTesting;
using Questions2Book;

namespace UnitTests
{
    [TestClass]
    public class Question2BookTests
    {
        [TestMethod]
        public void TestMarkDown2Text()
        {
            string line = "I&#39;ve been thinking about you";
            Assert.AreEqual("I've been thinking about you", UtilText.MarkDown2Text(line));
        }
    }
}
