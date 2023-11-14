#region MIT Licence
// Copyright 2023 Vladimir Tolmachev
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System.Text;
using Fluxor;

namespace StoryBlog.Web.Client.Blog.Middlewares;

internal sealed class FluxorLoggingMiddleware : IMiddleware
{
    private IDispatcher? dispatcher;
    private IStore? store;
    private ILogger<FluxorLoggingMiddleware> logger;

    public FluxorLoggingMiddleware(ILogger<FluxorLoggingMiddleware> logger)
    {
        this.logger = logger;
    }

    public Task InitializeAsync(IDispatcher dispatcher, IStore store)
    {
        this.dispatcher = dispatcher;
        this.store = store;

        return Task.CompletedTask;
    }

    public void AfterInitializeAllMiddlewares()
    {
    }

    public bool MayDispatchAction(object action) => true;

    public void BeforeDispatch(object action)
    {
        var text = new StringBuilder()
            .Append("Fluxor: ")
            .Append($"{action.GetType().Name}=")
            .AppendLine(System.Text.Json.JsonSerializer.Serialize(action));
        Console.WriteLine(text);
    }

    public void AfterDispatch(object action)
    {
    }

    public IDisposable BeginInternalMiddlewareChange() => null;
}