namespace StoryBlog.Web.Microservices.Posts.Application.Services;

public interface IWordTransliterator
{
    ValueTask<string?> TransliterateWordAsync(string word);
}