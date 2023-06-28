namespace StoryBlog.Web.MessageHub;

public class Buffer
{
    private readonly List<Memory<byte>> memories;

    public int Count
    {
        get;
        private set;
    }

    public Buffer()
    {
        memories = new List<Memory<byte>>();
        Count = 0;
    }

    public void Clear()
    {
        memories.Clear();
        Count = 0;
    }

    public void Add(Memory<byte> memory)
    {
        var bytes = memory.ToArray();

        Count += bytes.Length;

        memories.Add(new Memory<byte>(bytes));
    }
}