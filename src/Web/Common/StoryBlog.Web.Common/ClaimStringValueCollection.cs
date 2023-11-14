namespace StoryBlog.Web.Common;

public class ClaimStringValueCollection
{
    public static readonly ClaimStringValueCollection Empty;

    private readonly IEnumerable<string> values;

    private ClaimStringValueCollection(IEnumerable<string> values)
    {
        this.values = values;
    }

    static ClaimStringValueCollection()
    {
        Empty = new ClaimStringValueCollection(Enumerable.Empty<string>());
    }

    public static ClaimStringValueCollection Create(string? values)
    {
        if (String.IsNullOrEmpty(values))
        {
            return Empty;
        }

        const char openBracket = '[';
        const char closeBracket = ']';
        const char quote = '\"';

        if (values[0] != openBracket || values[^1] != closeBracket)
        {
            throw new Exception();
        }

        var temp = values.Substring(1, values.Length - 2);
        var list = new List<string>();

        foreach (var value in EnumerateValues(temp))
        {
            if (value[0] != quote || value[^1] != quote)
            {
                throw new Exception();
            }

            list.Add(value);
        }

        return new ClaimStringValueCollection(list.AsEnumerable());
    }

    public bool Contains(string expected) => values.Contains(expected);

    public bool Contains(IEnumerable<string> expected)
    {
        foreach (var value in values)
        {
            if (expected.Contains(value))
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<string> EnumerateValues(string source)
    {
        const char comma = ',';

        while (0 < source.Length)
        {
            var position = source.IndexOf(comma);
            string segment;

            if (-1 < position)
            {
                segment = source.Substring(0, position - 1);
                source = source.Substring(position);
            }
            else
            {
                segment = source;
                source = String.Empty;
            }

            yield return segment;
        }
    }
}