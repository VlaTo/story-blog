using System.Windows.Input;
using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using StoryBlog.Web.Client.Blog.Core;
using StoryBlog.Web.Microservices.Comments.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Components;

public partial class Comment : ICommentEditor, ICommentEditorObserver, IDisposable
{
    private IDisposable? editorSubscription;
    private Mode mode;

    protected string Classname => new CssBuilder("storyblog-blog-comment")
        .AddClass(Class)
        .Build();

    [Parameter]
    public string? Class
    {
        get;
        set;
    }

    [Parameter]
    public Guid Key
    {
        get;
        set;
    }

    [Parameter]
    public DateTime CreatedDateTime
    {
        get;
        set;
    }

    [Parameter]
    public RenderFragment ChildContent
    {
        get;
        set;
    }

    [Parameter]
    public IReadOnlyList<CommentModel>? Comments
    {
        get;
        set;
    }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?> UserAttributes
    {
        get;
        set;
    }

    [CascadingParameter(Name = nameof(EditorCoordinator))]
    private ICommentEditorCoordinator EditorCoordinator
    {
        get;
        set;
    }

    private string? CommentReply
    {
        get;
        set;
    }

    private ICommand Reply
    {
        get;
    }

    private ICommand CancelReply
    {
        get;
    }

    public Comment()
    {
        mode = Mode.Reading;
        Reply = new DelegateCommand(DoReply);
        CancelReply = new DelegateCommand(DoCancelReply);
    }

    void IDisposable.Dispose()
    {
        editorSubscription?.Dispose();
        editorSubscription = null;
    }

    void ICommentEditorObserver.OnEditorEventRaised(ICommentEditor editor)
    {
        if (ReferenceEquals(editor, this))
        {
            return ;
        }

        if (Mode.Reply == mode)
        {
            DoCancelReply();
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        editorSubscription = EditorCoordinator.Subscribe(this);
    }

    private void DoReply()
    {
        if (Mode.Reply == mode)
        {
            return ;
        }

        mode = Mode.Reply;

        EditorCoordinator.RaiseEditorEvent(this);

        StateHasChanged();
    }

    private void DoCancelReply()
    {
        if (Mode.Reply != mode)
        {
            return ;
        }

        mode = Mode.Reading;

        var temp = CommentReply;

        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    private enum Mode
    {
        Reading,
        Reply,
        Editing
    }
}