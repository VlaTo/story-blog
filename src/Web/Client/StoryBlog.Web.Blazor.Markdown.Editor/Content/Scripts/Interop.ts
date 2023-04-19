/**
 * @var editors: MarkdownEditor.IEditor[]
 */
export var editors: MarkdownEditor.IEditor[] = [];

/**
 * @function createEditor
 * @param selector: string
 * @returns
 */
export function createEditor(selector: string): string {
    const element: HTMLDivElement = document.querySelector(selector) as HTMLDivElement;

    if (null !== element) {
        const key: string = "markdown_editor_" + "j56thkj5h646k";

        editors[key] = new MarkdownEditor.Editor(element);

        return key;
    }

    return null;
}