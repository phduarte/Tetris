using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gadz.Tetris.Model.Pecas;

namespace Gadz.Tetris.Model.Tabuleiros {

    public class Tabuleiro : ITabuleiro {

        bool _atingiuTopo => PecaAtual.Posicao.Y == 0;
        IList<Bloco> _blocosJogados = new List<Bloco>();

        public Dimensao Dimensao { get; private set; }
        public TabuleiroEstado Estado { get; private set; }
        public Estatisticas Estatisticas { get; private set; }
        public bool EstaJogando { get { return Estado.EstaJogando; } }
        public Bloco[,] Matriz { get; }
        public IPeca PecaAtual { get; private set; }
        public IPeca ProximaPeca { get; private set; }
        public int Nivel => Estatisticas.Nivel;
        public int Linhas => Estatisticas.Linhas;
        public long Duracao => Estatisticas.Duracao.Ticks;
        public IList<Bloco> Blocos => _blocosJogados;

        public event Atualizou QuandoAtualizar;
        public event Terminou QuandoTerminar;
        public event Limpou QuandoLimpar;
        public event Moveu QuandoMover;
        public event Correu QuandoDeslizar;

        public Tabuleiro(int largura, int altura) {
            Dimensao = new Dimensao(largura, altura);
            Estatisticas = new Estatisticas();
            Estado = new TabuleiroEstadoPausado();
            Matriz = new Bloco[largura, altura];
        }

        public void Cair() {

            while (PodeMoverPraBaixo(PecaAtual)) {
                PecaAtual.MoverBaixo();
            }

            Avaliar();
            QuandoDeslizar?.Invoke();
        }

        public void CorrerDireita() {
            while (PodeMoverParaDireita(PecaAtual)) {
                PecaAtual.MoverDireita();
            }
            Avaliar();
            QuandoDeslizar?.Invoke();
        }

        public void CorrerEsquerda() {
            while (PodeMoverParaEsquerda(PecaAtual)) {
                PecaAtual.MoverEsquerda();
            }
            Avaliar();
            QuandoDeslizar?.Invoke();
        }

        public void Pausar() {
            Estado = new TabuleiroEstadoPausado();
            Estatisticas.Pausar();
        }

        public void Continuar() {
            Estado = new TabuleiroEstadoJogando();
            Estatisticas.Continuar();
            AtualizarAsync();
        }
        
        public void Terminar() {
            QuandoAtualizar?.Invoke();
            Estatisticas.Terminar();
            Estado = new TabuleiroEstadoTerminado();
            QuandoTerminar?.Invoke();
        }

        public void Iniciar() {

            PecaAtual = CriarBlocoAleatorio();
            ProximaPeca = CriarBlocoAleatorio();
            Estatisticas.Iniciar();
            QuandoAtualizar?.Invoke();

            Estado = new TabuleiroEstadoJogando();
            
            AtualizarAsync();
        }

        public void MoverBaixo() {
            if (PodeMoverPraBaixo(PecaAtual)) {
                PecaAtual.MoverBaixo();
                Avaliar();
                QuandoMover?.Invoke();
            }
        }

        public void MoverDireita() {
            if (PodeMoverParaDireita(PecaAtual)) {
                PecaAtual.MoverDireita();
                Avaliar();
                QuandoMover?.Invoke();
            }
        }

        public void MoverEsquerda() {
            if (PodeMoverParaEsquerda(PecaAtual)) {
                PecaAtual.MoverEsquerda();
                Avaliar();
                QuandoMover?.Invoke();
            }
        }

        public void Rotacionar() {
            if (PodeRotacionar(PecaAtual)) {
                PecaAtual.Girar();
                Avaliar();
                QuandoMover?.Invoke();
            }
        }

        bool PodeRotacionar(IPeca peca) {

            if (!Estado.PodeMovimentarBloco(this)) {
                return false;
            }

            return !ExisteColisao(peca);
        }

        bool PodeMoverParaEsquerda(IPeca peca) {

            if (!Estado.PodeMovimentarBloco(this))
                return false;

            var novaPosicao = new Ponto(peca.Posicao.X - 1, peca.Posicao.Y);
            var pecaTeste = new Peca(peca.Tipo, novaPosicao, peca.Rotacao, this);

            foreach (var i in pecaTeste.Blocos) {
                if (i.X < 0) {
                    return false;
                }
            }

            return !ExisteColisao(pecaTeste);
        }

        bool PodeMoverParaDireita(IPeca peca) {

            if (!Estado.PodeMovimentarBloco(this))
                return false;

            var novaPosicao = new Ponto(peca.Posicao.X + 1, peca.Posicao.Y);
            var pecaTeste = new Peca(peca.Tipo, novaPosicao, peca.Rotacao, this);

            foreach (var i in pecaTeste.Blocos) {
                if (i.X > Dimensao.Largura - 1) {
                    return false;
                }
            }

            return !ExisteColisao(pecaTeste);
        }

        bool PodeMoverPraBaixo(IPeca peca) {

            if (!Estado.PodeMovimentarBloco(this))
                return false;

            var novaPosicao = new Ponto(peca.Posicao.X, peca.Posicao.Y + 1);
            var pecaTeste = new Peca(peca.Tipo, novaPosicao, peca.Rotacao, this);

            foreach (var i in pecaTeste.Blocos) {
                if (i.Y > Dimensao.Altura - 1) {
                    return false;
                }
            }

            return !ExisteColisao(pecaTeste);
        }

        void Avaliar() {

            //o movimento não será válido se as casas horizontais do tabuleiro já estiverem sido percorridas
            //neste caso, o hogo é finalizado
            if(!PodeMoverPraBaixo(PecaAtual)) {
                if (_atingiuTopo) {
                    Terminar();
                    return;
                } else {
                    TrocarBlocoAtual();
                }
            }

            //limpar a matriz
            for (int y = 0; y < Dimensao.Altura; y++) {
                for (int x = 0; x < Dimensao.Largura; x++) {
                    Matriz[x, y] = new Bloco(x, y, CorPeca.TRANSPARENTE);
                }
            }

            //pintar os blocos que já saíram
            foreach(var i in _blocosJogados) {
                Matriz[i.X, i.Y] = i;
            }

            //pinta o bloco atual
            foreach (var i in PecaAtual.Blocos) {
                Matriz[i.X, i.Y] = i;
            }

            //verifica se o movimento preencheu uma linha
            int linhas = BreakLines();

            if (linhas > 0) Pontuar(linhas);

            QuandoAtualizar?.Invoke();
        }

        void AtualizarAsync() {
            new Thread(() => {
                while (EstaJogando) {
                    MoverBaixo();
                    Thread.Sleep(Estatisticas.Velocidade);
                }
            }) { IsBackground = true }.Start();
        }

        IPeca CriarBlocoAleatorio() {
            int x = new Random().Next(0, 7);
            TipoPeca tipo = (TipoPeca)x;
            int rotacao = new Random().Next(0, 4);
            return new Peca(tipo, rotacao, this).Clonar();
        }

        bool ExisteColisao(IPeca peca) {
            try {
                foreach (var a in _blocosJogados) {
                    foreach (var b in peca.Blocos) {
                        if (a.ColideCom(b)) {
                            return true;
                        }
                    }
                }
            } catch {

            }
            return false;
        }

        void TrocarBlocoAtual() {
            GuardarBlocos();
            PecaAtual = ProximaPeca.Clonar();
            ProximaPeca = CriarBlocoAleatorio();
            Estatisticas.IncrementarQuantidadeDeBlocos();
            QuandoAtualizar?.Invoke();
        }

        void GuardarBlocos() {
            foreach (var i in PecaAtual.Clonar().Forma.Blocos) {
                _blocosJogados.Add(i);
            }
        }

        int BreakLines() {

            int linhas = 0;

            //note que se no tabuleiro houver menos de 10 blocos, logicamente não haverá linha total preenchida
            //então isso economiza um pouco o processamento
            if (_blocosJogados.Count < 10)
                return linhas;

            //esse laço invertido tem como objetivo começar a percorrer o tabuleiro de baixo pra cima
            int y = Dimensao.Altura - 1;

            do {

                //máximo de linhas possíveis de serem preenchidas de uma vez
                if (linhas == 4)
                    break;

                int blocosVazios = ContarBlocosVaziosNaLinha(y);

                if (blocosVazios > 0) {
                    y--;
                } else {
                    linhas++;
                    RemoverLinha(y);
                    MoverBlocosParaBaixo(y);
                }

            } while (y >= 0);

            return linhas;
        }

        int ContarBlocosVaziosNaLinha(int linha) {
            int blocosVazios = Dimensao.Largura;

            for (int x = 0; x < Dimensao.Largura; x++) {
                for(int b = 0; b < _blocosJogados.Count; b++) {
                    if (_blocosJogados[b].X == x && _blocosJogados[b].Y == linha) {
                        blocosVazios--;
                    }
                }
            }

            return blocosVazios;
        }

        void MoverBlocosParaBaixo(int linha) {
            for (int i = 0; i < _blocosJogados.Count; i++) {
                if (_blocosJogados[i].Y < linha) {
                    _blocosJogados[i] = new Bloco(_blocosJogados[i].X, _blocosJogados[i].Y + 1, _blocosJogados[i].Cor);
                }
            }
        }

        void RemoverLinha(int linha) {
            for(int x = 0; x < Dimensao.Largura; x++) {
                try {
                    var bloco = _blocosJogados.First(_ => _.Y == linha && _.X == x);
                    _blocosJogados.Remove(bloco);
                } catch {
                    break;
                }
            }
        }

        void Pontuar(int linhas) {
            Estatisticas.Pontuar(linhas);
            QuandoLimpar?.Invoke();
        }
    }
}
