using StoryBlog.Web.Microservices.Posts.Application.Core;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.GenerateSlug;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //new GenerateSlugHandler()
            var wordReader = new WordReader("The Test Title, for the: Sample Blog #2");
            //var wordReader = new WordReader("he Test");

            foreach (var word in wordReader.EnumerateWords())
            {
                ;
            }
        }
    }
}