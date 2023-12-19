using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
[Flags]
public enum AllowedActions : byte
{
    CanEdit = 0x01,
    CanDelete = 0x02,
    CanTogglePublic = 0x04
}