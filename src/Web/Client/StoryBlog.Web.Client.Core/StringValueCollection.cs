namespace StoryBlog.Web.Client.Core;

public sealed class StringValueCollection
{
    public static readonly StringValueCollection Empty;

    public string[] Values
    {
        get;
    }

    private StringValueCollection(string[] values)
    {
        Values = values;
    }

    static StringValueCollection()
    {
        Empty = new StringValueCollection(Array.Empty<string>());
    }

    public bool Contains(string value) => Array.Exists(Values, x => String.Equals(x, value));

    public static StringValueCollection From(string? value)
    {
        if (String.IsNullOrEmpty(value))
        {
            return Empty;
        }

        if (value[0] == '[' && value[^1] == ']')
        {
            var values = value
                .Substring(1, value.Length - 2)
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            for (var index = 0; index < values.Length; index++)
            {
                values[index] = values[index].Substring(1, values[index].Length - 2);
            }

            return new StringValueCollection(values);
        }

        return Empty;
    }
}