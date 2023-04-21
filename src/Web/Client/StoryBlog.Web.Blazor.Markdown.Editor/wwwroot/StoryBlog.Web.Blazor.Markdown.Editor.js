export var editors = [];
export function createEditor(selector) {
    const element = document.querySelector(selector);
    if (null !== element) {
        const key = "markdown_editor_" + "j56thkj5h646k";
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

