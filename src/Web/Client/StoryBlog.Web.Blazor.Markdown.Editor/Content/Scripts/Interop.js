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
