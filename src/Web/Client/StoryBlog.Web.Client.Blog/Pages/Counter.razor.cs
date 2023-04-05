using Fluxor;
using Microsoft.AspNetCore.Components;
using StoryBlog.Web.Client.Blog.Store.CounterUseCase;

namespace StoryBlog.Web.Client.Blog.Pages;

public partial class Counter
{
    [Inject]
    private IState<CounterState> CouterState
    {
        get;
        set;
    }

    [Inject]
    public IDispatcher Dispatcher
    {
        get;
        set;
    }

    private void IncrementCounter()
    {
        Dispatcher.Dispatch(new IncrementCounterAction());
    }
}
