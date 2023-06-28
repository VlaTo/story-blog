using System.Text;

namespace StoryBlog.Web.MessageHub;

public static class ConsoleHelper
{
    public static void PrintBuffer(ArraySegment<byte> array)
    {
        var dump = new StringBuilder();
        var offset = 0;

        while (offset < array.Count)
        {
            var count = Math.Min(array.Count - offset, 16);
            var current = array.Slice(offset, count);

            PrintInner(dump, offset, current);

            offset += count;
        }

        Console.WriteLine(dump);
    }

    private static void PrintInner(StringBuilder builder, int offset, ArraySegment<byte> line)
    {
        builder.Append($"{offset:X8} ");

        for (var index = 0; index < line.Count; index++)
        {
            if (8 == index)
            {
                builder.Append(' ');
            }

            builder
                .Append($"{line[index]:X2}")
                .Append(' ');
        }

        builder.AppendLine();
    }
}