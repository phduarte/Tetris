// Teclado do aparelho: ouve a página inteira (não depende de um elemento com foco) e repassa ao ConsoleFrame
// só as teclas que o jogo trata, cancelando a ação padrão delas (rolar a página, clicar o botão focado).

/** Começa a ouvir. Devolve um objeto com dispose(), chamado quando o aparelho sai da página. */
export function listen(frame, handledKeys) {
    const keys = new Set(handledKeys);

    const onKeyDown = event => {
        if (!keys.has(event.key)) {
            return;
        }

        event.preventDefault();
        frame.invokeMethodAsync("OnKeyDown", event.key, event.ctrlKey, event.repeat);
    };

    const onBlur = () => frame.invokeMethodAsync("OnBlur");

    document.addEventListener("keydown", onKeyDown);
    window.addEventListener("blur", onBlur);

    return {
        dispose() {
            document.removeEventListener("keydown", onKeyDown);
            window.removeEventListener("blur", onBlur);
        }
    };
}
