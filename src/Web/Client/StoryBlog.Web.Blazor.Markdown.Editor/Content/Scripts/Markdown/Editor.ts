module MarkdownEditor {
    /**
     * @class Editor
     */
    export class Editor implements IEditor {
        private editor: HTMLDivElement;

        content: string;

        constructor(editor: HTMLDivElement) {
            this.editor = editor;
            this.content = "";

            //editor.addEventListener("input", this.onInput);
            //editor.addEventListener("beforeinput", this.onBeforeInput);
            //editor.addEventListener("compositionstart", this.onCompositionStart);
            //editor.addEventListener("compositionend", this.onCompositionEnd);
            //editor.addEventListener("compositionupdate", this.onCompositionUpdate);
        }

        setText(value: string): void {
            this.content = value;
            this.editor.innerHTML = value;
        }

        getText(): string {
            return this.editor.innerHTML;
        }

        /*private onInput: EventListener = (e: InputEvent) => {
            //console.log("onInput event: " + e.data);
            //console.log("onInput event: " + this.editor.innerHTML);

            //const temp = this.editor.isContentEditable;
        }

        private onBeforeInput: EventListener = (e: InputEvent) => {
            //console.log("onBeforeInput event");
        }

        private onCompositionStart: EventListener = (e: CompositionEvent) => {
            //console.log("onCompositionStart event: " + e.data);
        }

        private onCompositionEnd: EventListener = (e: CompositionEvent) => {
            //console.log("onCompositionEnd event");
        }

        private onCompositionUpdate: EventListener = (e: CompositionEvent) => {
            //console.log("onCompositionUpdate event");
        }*/
    }
}