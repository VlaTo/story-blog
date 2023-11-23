using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace StoryBlog.Web.Client.Blog.Pages;

public partial class DeleteRequestDialog
{
    [Parameter]
    public string PostTitle
    {
        get;
        set;
    }

    [CascadingParameter]
    private MudDialogInstance MudDialog
    {
        get;
        set;
    }

    private void DoCancelDialog()
        => MudDialog.Cancel();

    private void DoDeleteAction()
        => MudDialog.Close(DialogResult.Ok(true));
}