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
public class PostsPageAction
{
    public int PageNumber
    {
        get;
    }

    public int PageSize
    {
        get;
    }

    protected PostsPageAction(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class FetchPostsPageAction : PostsPageAction
{
    public FetchPostsPageAction(int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
    }
}

/// <summary>
/// 
/// </summary>
public sealed class FetchPostsPageReadyAction : PostsPageAction
{
    public IReadOnlyCollection<PostModel> Posts
    {
        get; 
        set;
    }

    public FetchPostsPageReadyAction(IReadOnlyCollection<PostModel> posts, int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Posts = posts;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class FetchPostsPageFailedAction : PostsPageAction
{
    public FetchPostsPageFailedAction(int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
    }
}