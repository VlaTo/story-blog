using System.Windows.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
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
    public Guid? ParentKey
    {
        get;
        set;
    }

    [Parameter]
    public Guid PostKey
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

    public string? CommentReply
    {
        get;
        private set;
    }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?> UserAttributes
    {
        get;
        set;
    }

    [Parameter]
    public ICommand<Comment>? SendReplyCommand
    {
        get;
        set;
    }

    [Parameter]
    public EventCallback<Comment> OnSendReplyClick
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

    [Inject]
    internal IStringLocalizer<Comment> Localizer
    {
        get;
        set;
    }

    private ICommand OpenReply
    {
        get;
    }

    private ICommand CancelReply
    {
        get;
    }

    private ICommand SendReply
    {
        get;
    }

    public Comment()
    {
        mode = Mode.Reading;
        OpenReply = new DelegateCommand(DoReply);
        CancelReply = new DelegateCommand(DoCancelReply);
        SendReply = new DelegateCommand(DoSendReply);
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

        StateHasChanged();
    }

    private void DoSendReply()
    {
        if (Mode.Reply != mode)
        {
            return ;
        }

        RaiseSendReplyCommand();
        RaiseSendReplyClick();
    }

    private void RaiseSendReplyCommand()
    {
        var command = SendReplyCommand;

        if (null != command)
        {
            command.Execute(this);
        }
    }

    private void RaiseSendReplyClick()
    {
        var handler = OnSendReplyClick;

        if (handler.HasDelegate)
        {
            var synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
            TaskRunner.Instance.QueueTask(
                handler.InvokeAsync(this),
                synchronizationContext,
                OnSendReplyClickComplete,
                CancellationToken.None
            );
        }
    }

    private void OnSendReplyClickComplete()
    {
        ;
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