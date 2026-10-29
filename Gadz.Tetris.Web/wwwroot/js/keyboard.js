// Teclado do aparelho: ouve a página inteira (não depende de um elemento com foco) e repassa ao ConsoleFrame
// só as teclas que o jogo trata, cancelando a ação padrão delas (rolar a página, clicar o botão focado).
// Também afunda o botão da carcaça da tecla apertada, como no Desktop, e faz as setas do direcional, seguradas
// com o dedo ou o mouse, se repetirem como uma tecla segurada (no celular não há teclado para isso).

/** Tempo mínimo que o botão fica afundado, para um toque rápido na tecla aparecer (PRESS_MS do Desktop). */
const PRESS_MS = 120;

/** Quanto tempo a seta fica segurada até começar a se repetir, como o atraso de repetição do teclado. */
const HOLD_DELAY_MS = 300;

/** Intervalo entre as repetições da seta segurada. */
const HOLD_REPEAT_MS = 50;

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

    // a seta do direcional segurada agora: a tecla e os relógios da repetição
    let held = null;

    const stopHold = () => {
        if (!held) {
            return;
        }

        clearTimeout(held.delay);
        clearInterval(held.repeat);
        release(held.key);
        held = null;
    };

    // o primeiro toque anda como a tecla apertada; segurando, repete como a tecla segurada (repeat = true)
    const onPointerDown = event => {
        const arrow = event.target.closest(".arrow[data-key]");
        if (!arrow || arrow.disabled || event.button !== 0) {
            return;
        }

        event.preventDefault();
        stopHold();
        const key = arrow.dataset.key;
        const send = repeat => frame.invokeMethodAsync("OnKeyDown", key, false, repeat);
        held = {
            key,
            delay: setTimeout(() => held.repeat = setInterval(() => send(true), HOLD_REPEAT_MS), HOLD_DELAY_MS),
        };
        press(key);
        send(false);
    };

    // segurar o dedo não pode abrir o menu de contexto do navegador
    const onContextMenu = event => {
        if (event.target.closest(".arrow")) {
            event.preventDefault();
        }
    };

    // fora de foco o keyup não chega: solta todos os botões
    const onBlur = () => {
        stopHold();
        [...pressedAt.keys()].forEach(release);
        frame.invokeMethodAsync("OnBlur");
    };

    document.addEventListener("keydown", onKeyDown);
    document.addEventListener("keyup", onKeyUp);
    document.addEventListener("pointerdown", onPointerDown);
    document.addEventListener("pointerup", stopHold);
    document.addEventListener("pointercancel", stopHold);
    document.addEventListener("contextmenu", onContextMenu);
    window.addEventListener("blur", onBlur);

    return {
        dispose() {
            stopHold();
            document.removeEventListener("keydown", onKeyDown);
            document.removeEventListener("keyup", onKeyUp);
            document.removeEventListener("pointerdown", onPointerDown);
            document.removeEventListener("pointerup", stopHold);
            document.removeEventListener("pointercancel", stopHold);
            document.removeEventListener("contextmenu", onContextMenu);
            window.removeEventListener("blur", onBlur);
        }
    };
}
