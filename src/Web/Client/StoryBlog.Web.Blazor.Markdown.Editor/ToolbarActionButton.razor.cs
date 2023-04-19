using Microsoft.AspNetCore.Components;
using System.IO;

namespace StoryBlog.Web.Blazor.Markdown.Editor;

public enum EditorAction
{
    Undo,
    Redo,

    Bold,
    Italic,
    Underline
}

public partial class ToolbarActionButton
{
    private EditorAction editorAction;

    public override bool Disabled
    {
        get => base.Disabled;
        set
        {
            base.Disabled = value;

            if (value)
            {
                ;
            }
        }
    }

    [Parameter]
    public EditorAction EditorAction
    {
        get => editorAction;
        set
        {
            if (EditorAction == value)
            {
                return;
            }

            editorAction = value;

            if (HasRendered)
            {
                StateHasChanged();
            }
        }
    }

    public ToolbarActionButton()
    {
        EditorAction = EditorAction.Bold;
    }

    protected override void OnEditorInitialized()
    {
        var enabled = Editor!.IsActionEnabled(EditorAction);
        Disabled = false == enabled;
    }

    private string GetGlyph()
    {
        switch(EditorAction)
        {
            case EditorAction.Undo:
            {
                return "m5.042 9.367 2.189 1.837a.75.75 0 0 1-.965 1.149l-3.788-3.18a.747.747 0 0 1-.21-.284.75.75 0 0 1 .17-.945L6.23 4.762a.75.75 0 1 1 .964 1.15L4.863 7.866h8.917A.75.75 0 0 1 14 7.9a4 4 0 1 1-1.477 7.718l.344-1.489a2.5 2.5 0 1 0 1.094-4.73l.008-.032H5.042z";
            }

            case EditorAction.Redo:
            {
                return "m14.958 9.367-2.189 1.837a.75.75 0 0 0 .965 1.149l3.788-3.18a.747.747 0 0 0 .21-.284.75.75 0 0 0-.17-.945L13.77 4.762a.75.75 0 1 0-.964 1.15l2.331 1.955H6.22A.75.75 0 0 0 6 7.9a4 4 0 1 0 1.477 7.718l-.344-1.489A2.5 2.5 0 1 1 6.039 9.4l-.008-.032h8.927z";
            }

            case EditorAction.Bold:
            {
                return "M10.187 17H5.773c-.637 0-1.092-.138-1.364-.415-.273-.277-.409-.718-.409-1.323V4.738c0-.617.14-1.062.419-1.332.279-.27.73-.406 1.354-.406h4.68c.69 0 1.288.041 1.793.124.506.083.96.242 1.36.478.341.197.644.447.906.75a3.262 3.262 0 0 1 .808 2.162c0 1.401-.722 2.426-2.167 3.075C15.05 10.175 16 11.315 16 13.01a3.756 3.756 0 0 1-2.296 3.504 6.1 6.1 0 0 1-1.517.377c-.571.073-1.238.11-2 .11zm-.217-6.217H7v4.087h3.069c1.977 0 2.965-.69 2.965-2.072 0-.707-.256-1.22-.768-1.537-.512-.319-1.277-.478-2.296-.478zM7 5.13v3.619h2.606c.729 0 1.292-.067 1.69-.2a1.6 1.6 0 0 0 .91-.765c.165-.267.247-.566.247-.897 0-.707-.26-1.176-.778-1.409-.519-.232-1.31-.348-2.375-.348H7z";
            }
        }

        return String.Empty;
    }
}