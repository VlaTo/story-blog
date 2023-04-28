export var editors = [];
export function createEditor(selector, uniqueKey) {
    const element = document.querySelector(selector);
    if (null !== element) {
        const key = "markdown_editor_" + uniqueKey;
        editors[key] = new MarkdownEditor.Editor(element);
        return key;
    }
    return null;
}

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

