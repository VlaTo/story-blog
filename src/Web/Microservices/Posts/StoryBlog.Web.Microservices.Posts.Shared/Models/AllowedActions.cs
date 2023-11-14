using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
[Flags]
public enum AllowedActions : byte
{
    CanEdit,
    CanDelete
}