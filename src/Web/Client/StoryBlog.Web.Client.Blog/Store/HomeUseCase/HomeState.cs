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

using Fluxor;
using StoryBlog.Web.Client.Blog.Models;

namespace StoryBlog.Web.Client.Blog.Store.HomeUseCase;

[FeatureState]
internal sealed class HomeState : AbstractStore
{
    public int PageNumber
    {
        get;
    }

    public int PageSize
    {
        get;
    }

    public int PagesCount
    {
        get;
    }

    public IReadOnlyCollection<BriefPostModel> Posts
    {
        get;
    }

    public HomeState()
        : this(0, 0, 0, Array.Empty<BriefPostModel>(), storeState: StoreState.Empty)
    {
    }

    public HomeState(
        int pageNumber,
        int pageSize,
        int pagesCount,
        IReadOnlyCollection<BriefPostModel> posts,
        StoreState storeState = StoreState.Empty)
        : base(storeState)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        PagesCount = pagesCount;
        Posts = posts;
    }
}