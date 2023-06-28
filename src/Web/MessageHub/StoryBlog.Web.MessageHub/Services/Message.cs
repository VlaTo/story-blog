using System.Text;

namespace StoryBlog.Web.MessageHub.Services;

public class Message
{
    private const int Int32BytesCount = 4;

    public string Channel
    {
        get;
    }

    public ArraySegment<byte> Payload
    {
        get;
    }

    public Message(string channel, ArraySegment<byte> payload)
    {
        Channel = channel;
        Payload = payload;
    }

    public ArraySegment<byte> ToBytes()
    {
        var channel = Encoding.UTF8.GetBytes(Channel);
        var count = Int32BytesCount + channel.Length + Payload.Count;
        var bytes = new ArraySegment<byte>(new byte[count + Int32BytesCount]);

        new ArraySegment<byte>(BitConverter.GetBytes(count)).CopyTo(bytes.Slice(0, Int32BytesCount));
        new ArraySegment<byte>(BitConverter.GetBytes(channel.Length)).CopyTo(bytes.Slice(4, Int32BytesCount));
        new ArraySegment<byte>(channel).CopyTo(bytes.Slice(8, channel.Length));
        
        Payload.CopyTo(bytes.Slice(8 + channel.Length));

        return bytes;
    }

    public static Message From(ArraySegment<byte> data)
    {
        var count = BitConverter.ToInt32(data.Slice(0, Int32BytesCount));

        if (count != data.Count - Int32BytesCount)
        {
            throw new Exception();
        }

        var length = BitConverter.ToInt32(data.Slice(4, Int32BytesCount));
        var header = Encoding.UTF8.GetString(data.Slice(8, length));

        return new Message(header, data.Slice(8 + length));
    }
}