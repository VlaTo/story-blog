var MarkdownEditor;
(function (MarkdownEditor) {
    class Editor {
        constructor(editor) {
            this.editor = editor;
            this.content = "";
        }
        setText(value) {
            this.content = value;
            this.editor.innerHTML = value;
        }
        getText() {
            return this.editor.innerHTML;
        }
    }
    MarkdownEditor.Editor = Editor;
})(MarkdownEditor || (MarkdownEditor = {}));
