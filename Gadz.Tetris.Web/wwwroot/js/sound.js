// Sons do jogo pela Web Audio API. Os .wav são baixados e decodificados uma vez; cada som toca "do início",
// interrompendo a execução anterior dele, como o "play x from 0" do winmm no Desktop.

// no iOS, "playback" faz o som tocar mesmo com a chave do modo silencioso ligada (o padrão obedece a chave)
if (navigator.audioSession) {
    navigator.audioSession.type = "playback";
}

const context = new AudioContext();
const buffers = new Map();
const playing = new Map();

/**
 * Libera o áudio. Todo navegador do iOS (o Chrome também usa o motor do Safari) só libera dentro do próprio evento
 * de toque ou tecla — touchend, click ou keydown —, não numa chamada que chega depois pelo .NET. Tocar um som
 * vazio junto garante a liberação nas versões antigas. Continua ouvindo porque o iOS volta a suspender o áudio
 * ("interrupted") quando o app vai para o fundo ou chega uma ligação.
 */
function unlock() {
    if (context.state === "running") {
        return;
    }

    context.resume();
    const silence = context.createBufferSource();
    silence.buffer = context.createBuffer(1, 1, context.sampleRate);
    silence.connect(context.destination);
    silence.start();
}

for (const type of ["touchend", "click", "keydown"]) {
    document.addEventListener(type, unlock, { capture: true });
}

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

/** Toca o som do início. Antes do primeiro toque ou tecla (veja unlock) fica na fila do áudio suspenso. */
export function play(name) {
    const buffer = buffers.get(name);
    if (!buffer) {
        return;
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
