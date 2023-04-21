module MarkdownEditor {
    /**
     * @interface IEditor
     */
    export interface IEditor {
        /**
         * @property content: string
         */
        content: string;

        /**
         * @method setText
         * @param value: string
         */
        setText(value: string): void;

        /**
         * @method getText
         * @return string
         */
        getText(): string;
    }
}