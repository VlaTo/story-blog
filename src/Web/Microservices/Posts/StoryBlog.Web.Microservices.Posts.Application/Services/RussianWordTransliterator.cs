using System.Text;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class RussianWordTransliterator : IWordTransliterator
{
    private static readonly Dictionary<char, string> table;

    static RussianWordTransliterator()
    {
        table = new Dictionary<char, string>
        {
            { 'а', "a" },
            { 'б', "b" },
            { 'в', "v" },
            { 'г', "g" },
            { 'д', "d" },
            { 'е', "e" },
            { 'ё', "e" },
            { 'ж', "dj" },
            { 'з', "z" },
            { 'и', "i" },
            { 'й', "i" },
            { 'к', "k" },
            { 'л', "l" },
            { 'м', "m" },
            { 'н', "n" },
            { 'о', "o" },
            { 'п', "p" },
            { 'р', "r" },
            { 'с', "s" },
            { 'т', "t" },
            { 'у', "u" },
            { 'ф', "f" },
            { 'х', "h" },
            { 'ц', "tc" },
            { 'ч', "ch" },
            { 'ш', "sh" },
            { 'щ', "sch" },
            { 'э', "e" },
            { 'ю', "yu" },
            { 'я', "ya" }
        };
    }

    public ValueTask<string?> TransliterateWordAsync(string word)
    {
        var builder = new StringBuilder();

        for (var index = 0; index < word.Length; index++)
        {
            if (table.TryGetValue(Char.ToLower(word[index]), out var replacement))
            {
                builder.Append(replacement);
            }
        }

        return ValueTask.FromResult(0 == builder.Length ? null : builder.ToString());
    }
}