﻿namespace StoryBlog.Web.Common.Events;

public enum BlogCommentAction
{
    Added,
    Updated,
    Deleted

}

public sealed record BlogCommentEvent(
    Guid Key,
    Guid PostKey,
    Guid? ParentKey,
    DateTimeOffset CreateAt,
    BlogCommentAction Action
);