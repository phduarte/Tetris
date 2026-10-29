// Sons do jogo pela Web Audio API. Os .wav são baixados e decodificados uma vez; cada som toca "do início",
// interrompendo a execução anterior dele, como o "play x from 0" do winmm no Desktop.

const context = new AudioContext();
const buffers = new Map();
const playing = new Map();

/** Baixa e decodifica os sons, sem esperar: um som ainda não carregado simplesmente não toca. */
export function load(names) {
    for (const name of names) {
        fetch(`AudioFiles/${name}.wav`)
            .then(response => response.arrayBuffer())
            .then(data => context.decodeAudioData(data))
            .then(buffer => buffers.set(name, buffer))
            .catch(error => console.warn(`Som ${name} indisponível`, error));
    }
}

/** Toca o som do início. O navegador só libera o áudio após um gesto do usuário; as teclas e cliques contam. */
export function play(name) {
    const buffer = buffers.get(name);
    if (!buffer) {
        return;
    }

    if (context.state === "suspended") {
        context.resume();
    }

    stop(name);
    const source = context.createBufferSource();
    source.buffer = buffer;
    source.connect(context.destination);
    source.onended = () => {
        if (playing.get(name) === source) {
            playing.delete(name);
        }
    };
    playing.set(name, source);
    source.start();
}

/** Interrompe o som, se estiver tocando. */
export function stop(name) {
    playing.get(name)?.stop();
    playing.delete(name);
}

/** Interrompe todos os sons. */
export function stopAll() {
    for (const name of [...playing.keys()]) {
        stop(name);
    }
}
