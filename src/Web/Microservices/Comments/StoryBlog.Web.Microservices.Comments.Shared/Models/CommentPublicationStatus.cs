using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Comments.Shared.Models;

[DataContract]
public enum CommentPublicationStatus : byte
{
    Pending,
    Approved,
    Rejected
}