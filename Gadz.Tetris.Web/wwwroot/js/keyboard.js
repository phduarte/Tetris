// Teclado do aparelho: ouve a página inteira (não depende de um elemento com foco) e repassa ao ConsoleFrame
// só as teclas que o jogo trata, cancelando a ação padrão delas (rolar a página, clicar o botão focado).
// Também afunda o botão da carcaça da tecla apertada, como no Desktop.

/** Tempo mínimo que o botão fica afundado, para um toque rápido na tecla aparecer (PRESS_MS do Desktop). */
const PRESS_MS = 120;

/** Começa a ouvir. Devolve um objeto com dispose(), chamado quando o aparelho sai da página. */
export function listen(frame, handledKeys) {
    const keys = new Set(handledKeys);

    // quando cada tecla segurada foi apertada, para soltar o botão só depois do tempo mínimo
    const pressedAt = new Map();

    const buttonsOf = key => [...document.querySelectorAll("[data-key]")].filter(b => b.dataset.key === key);

    const press = key => {
        if (!pressedAt.has(key)) {
            pressedAt.set(key, performance.now());
        }

        buttonsOf(key).forEach(b => b.dataset.pressed = "");
    };

    const release = key => {
        if (!pressedAt.has(key)) {
            return;
        }

        const wait = Math.max(0, pressedAt.get(key) + PRESS_MS - performance.now());
        pressedAt.delete(key);
        setTimeout(() => {
            // apertada de novo durante a espera: continua afundado
            if (!pressedAt.has(key)) {
                buttonsOf(key).forEach(b => delete b.dataset.pressed);
            }
        }, wait);
    };

    const onKeyDown = event => {
        if (!keys.has(event.key)) {
            return;
        }

        event.preventDefault();
        press(event.key);
        frame.invokeMethodAsync("OnKeyDown", event.key, event.ctrlKey, event.repeat);
    };

    const onKeyUp = event => release(event.key);

    // fora de foco o keyup não chega: solta todos os botões
    const onBlur = () => {
        [...pressedAt.keys()].forEach(release);
        frame.invokeMethodAsync("OnBlur");
    };

    document.addEventListener("keydown", onKeyDown);
    document.addEventListener("keyup", onKeyUp);
    window.addEventListener("blur", onBlur);

    return {
        dispose() {
            document.removeEventListener("keydown", onKeyDown);
            document.removeEventListener("keyup", onKeyUp);
            window.removeEventListener("blur", onBlur);
        }
    };
}
