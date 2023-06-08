using System.ComponentModel;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Identity.Application.Configuration;

[DataContract, Serializable]
public sealed class StoryBlogIdentityOptions : IIdentityOptions
{
}