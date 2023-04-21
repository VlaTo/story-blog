using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Comments.Shared.Models;

[DataContract]
public enum CommentModelStatus : byte
{
    Pending,
    Approved,
    Rejected
}