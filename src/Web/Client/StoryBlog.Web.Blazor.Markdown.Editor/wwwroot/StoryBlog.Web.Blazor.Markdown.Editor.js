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
            this.onInput = (e) => {
                console.log("onInput event: " + e.data);
                console.log("onInput event: " + this.editor.innerHTML);
                const temp = this.editor.isContentEditable;
            };
            this.onBeforeInput = (e) => {
                console.log("onBeforeInput event");
            };
            this.onCompositionStart = (e) => {
                console.log("onCompositionStart event: " + e.data);
            };
            this.onCompositionEnd = (e) => {
                console.log("onCompositionEnd event");
            };
            this.onCompositionUpdate = (e) => {
                console.log("onCompositionUpdate event");
            };
            this.editor = editor;
            this.content = "";
            editor.addEventListener("input", this.onInput);
            editor.addEventListener("beforeinput", this.onBeforeInput);
            editor.addEventListener("compositionstart", this.onCompositionStart);
            editor.addEventListener("compositionend", this.onCompositionEnd);
            editor.addEventListener("compositionupdate", this.onCompositionUpdate);
        }
        setText(value) {
            this.content = value;
            this.editor.innerHTML = value;
        }
    }
    MarkdownEditor.Editor = Editor;
})(MarkdownEditor || (MarkdownEditor = {}));

