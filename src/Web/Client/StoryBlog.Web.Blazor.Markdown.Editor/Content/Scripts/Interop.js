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
