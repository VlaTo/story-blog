namespace StoryBlog.Web.Microservices.Posts.Application.Core;

internal sealed class WordReader
{
    private readonly string text;

    public WordReader(string text)
    {
        this.text = text;
    }

    public IEnumerable<string> EnumerateWords()
    {
        var start = -1;

        for (var position = 0; position <= text.Length; position++)
        {
            if (position == text.Length || Char.IsWhiteSpace(text[position]) || Char.IsPunctuation(text[position]))
            {
                if (0 <= start)
                {
                    var length = position - start;

                    yield return text.Substring(start, length).ToLower();

                    start = -1;
                }

                continue;
            }

            if (0 > start)
            {
                start = position;
            }
        }
    }
}