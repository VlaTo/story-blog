#region MIT Licence
// Copyright 2023 Vladimir Tolmachev
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies
// or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion

using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Store.HomeUseCase;

/// <summary>
/// 
/// </summary>
public record PostsPageAction(
    int PageNumber,
    int PageSize
);

/// <summary>
/// 
/// </summary>
public sealed record FetchPostsPageAction(
        int PageNumber,
        int PageSize)
    : PostsPageAction(PageNumber, PageSize);

/// <summary>
/// 
/// </summary>
public sealed record FetchPostsPageReadyAction(
        IReadOnlyCollection<BriefModel> Posts,
        int PageNumber,
        int PageSize)
    : PostsPageAction(PageNumber, PageSize);

/// <summary>
/// 
/// </summary>
public sealed record FetchPostsPageFailedAction(
        int PageNumber,
        int PageSize
) : PostsPageAction(PageNumber, PageSize);

/// <summary>
/// 
/// </summary>
/// <param name="PostKey"></param>
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
public sealed record ImmediatePostDeleteAction(
        Guid PostKey,
        Guid LastPostKey,
        int PageNumber,
        int PageSize
) : PostsPageAction(PageNumber, PageSize);

/// <summary>
/// 
/// </summary>
/// <param name="PostKey"></param>
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
public sealed record ImmediatePostDeleteSuccessAction(
        Guid PostKey,
        Guid LastPostKey,
        int PageNumber,
        int PageSize
) : PostsPageAction(PageNumber, PageSize);

/// <summary>
/// 
/// </summary>
/// <param name="PostKey"></param>
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
public sealed record ImmediatePostDeleteFailedAction(
        Guid PostKey,
        int PageNumber,
        int PageSize
) : PostsPageAction(PageNumber, PageSize);

/// <summary>
/// 
/// </summary>
/// <param name="PostKey"></param>
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
public sealed record TailPostsReadyAction(
    Guid LastPostKey,
    IReadOnlyCollection<BriefModel> Posts,
    int PageNumber,
    int PageSize
) : PostsPageAction(PageNumber, PageSize);
