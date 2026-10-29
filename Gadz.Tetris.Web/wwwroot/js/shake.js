// Tremor do aparelho, como o ShakeDown do Desktop: a carcaça desce um pouco e volta, duas vezes, cada vez menos.

/** Duração do tremor, em milissegundos. */
const SHAKE_MS = 120;

/** Sacode o elemento para baixo (pela Web Animations API, que pode recomeçar a qualquer momento). */
export function shakeDown(element) {
    element.animate(
        [
            { transform: "translateY(0)" },
            { transform: "translateY(3px)" },
            { transform: "translateY(0)" },
            { transform: "translateY(1.5px)" },
            { transform: "translateY(0)" },
        ],
        { duration: SHAKE_MS, easing: "ease-out" });
}
