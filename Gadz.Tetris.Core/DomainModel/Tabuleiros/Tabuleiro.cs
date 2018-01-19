using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gadz.Tetris.Core.DomainModel.Pecas;

namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {

    internal class Tabuleiro : ITabuleiro {

        #region fields

        IEstatisticasRepository _repository;
        bool _atingiuTopo => ExisteColisao(ProximaPeca);
        const int MAXIMO_LINHAS_POSSIVEIS = 4;
        IList<Bloco> _blocosJogados = new List<Bloco>();

        //o propósito dessa variável é evitar que ocorra 2 avaliações ao mesmo tempo sem precisar de nenhum lock.
        bool _refreshing;

        #endregion

        #region properties

        public Dimensao Dimensao { get; private set; }
        public ITabuleiroEstado Estado { get; private set; }
        public Estatisticas Estatisticas { get; private set; }
        public bool EstaJogando { get { return Estado.EstaJogando; } }
        public Bloco[,] Matriz { get; }
        public IPeca PecaAtual { get; private set; }
        public IPeca ProximaPeca { get; private set; }
        public int Nivel => Estatisticas.Nivel;
        public int Linhas => Estatisticas.Linhas;
        public long Duracao => Estatisticas.Duracao.Ticks;
        public IList<Bloco> Blocos => _blocosJogados;
        public int RecordeMaximo { get; private set; }

        #endregion

        #region events

        public event Atualizou QuandoAtualizar;
        public event Terminou QuandoTerminar;
        public event Limpou QuandoLimpar;
        public event Moveu QuandoMover;
        public event Correu QuandoDeslizar;

        #endregion

        #region constructors

        public Tabuleiro(IEstatisticasRepository repository, int largura, int altura) {
            _repository = repository;
            Dimensao = new Dimensao(largura, altura);
            Estatisticas = new Estatisticas();
            Estado = new TabuleiroEstadoPausado();
            Matriz = new Bloco[largura, altura];
            RecordeMaximo = _repository.MaxScore();
        }

        #endregion

        #region public methods

        public void Cair() {

            while (PodeMoverPraBaixo(PecaAtual)) {
                PecaAtual.MoverBaixo();
            }

            Avaliar();
            QuandoDeslizar?.Invoke();
        }

        public void CorrerDireita() {

            if (!PodeMoverParaDireita(PecaAtual))
                return;

            while (PodeMoverParaDireita(PecaAtual)) {
                PecaAtual.MoverDireita();
            }
            Avaliar();
            QuandoDeslizar?.Invoke();
        }

        public void CorrerEsquerda() {

            if (!PodeMoverParaEsquerda(PecaAtual))
                return;

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

            Estado = new TabuleiroEstadoJogando();
            PecaAtual = CriarBlocoAleatorio();
            ProximaPeca = CriarBlocoAleatorio();
            Estatisticas.Iniciar();
            AtualizarAsync();
            Avaliar();
        }

        public void Reiniciar() {
            Terminar();
            Iniciar();
        }

        public void Salvar() {
            _repository.Save(Estatisticas);
        }

        public void Carregar(Identidade id) {
            Estatisticas = _repository.Load(id);
        }

        public void MoverBaixo() {
            if (PodeMoverPraBaixo(PecaAtual)) {
                PecaAtual.MoverBaixo();
                QuandoMover?.Invoke();
            }

            Avaliar();
        }

        public void MoverDireita() {
            if (PodeMoverParaDireita(PecaAtual)) {
                PecaAtual.MoverDireita();
                QuandoMover?.Invoke();
                RedefinirMatriz();
            }
        }

        public void MoverEsquerda() {
            if (PodeMoverParaEsquerda(PecaAtual)) {
                PecaAtual.MoverEsquerda();
                QuandoMover?.Invoke();
                RedefinirMatriz();
            }
        }

        public void Rotacionar() {
            if (!PodeRotacionar(PecaAtual)) {
                return;
            }

            PecaAtual.Girar();
            QuandoMover?.Invoke();
            RedefinirMatriz();
        }

        #endregion

        #region private methods

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
            var pecaTeste = new PecaBuilder()
                                    .DoTipo(peca.Tipo)
                                    .NaPosicao(novaPosicao)
                                    .ComRotacao(peca.Rotacao)
                                    .NoTabuleiro(this)
                                    .Construir();

            //verifica se ultrapassou o limte de largura do tabuleiro
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
            var pecaTeste = new PecaBuilder()
                .DoTipo(peca.Tipo)
                .NaPosicao(novaPosicao)
                .ComRotacao(peca.Rotacao)
                .NoTabuleiro(this)
                .Construir();

            //verifica se ultrapassou o limte de largura do tabuleiro
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
            var pecaTeste = new PecaBuilder()
                .DoTipo(peca.Tipo)
                .NaPosicao(novaPosicao)
                .ComRotacao(peca.Rotacao)
                .NoTabuleiro(this)
                .Construir();

            //verifica se ultrapassou o limte altura do tabuleiro
            foreach (var i in pecaTeste.Blocos) {
                if (i.Y > Dimensao.Altura - 1) {
                    return false;
                }
            }

            return !ExisteColisao(pecaTeste);
        }

        void Avaliar() {

            if (!EstaJogando) {
                return;
            }

            if (_refreshing) {
                return;
            }

            _refreshing = true;

            //o movimento não será válido se as casas horizontais do tabuleiro já estiverem sido percorridas
            //neste caso, o hogo é finalizado
            if (!PodeMoverPraBaixo(PecaAtual)) {
                if (_atingiuTopo) {
                    Terminar();
                    return;
                } else {

                    lock (PecaAtual) {
                        TrocarPecaAtual();

                        if (_atingiuTopo) {
                            Terminar();
                            return;
                        }
                    }
                }
            }

            PecaAtual.MoverBaixo();

            RedefinirMatriz();
            VerificarSePreencheuLinha();

            _refreshing = false;
        }

        void VerificarSePreencheuLinha() {

            var linhas = LimparLinhas();

            if (linhas > 0) {
                Pontuar(linhas);
            }
        }

        void RedefinirMatriz() {
            //limpar a matriz
            for (int y = 0; y < Dimensao.Altura; y++) {
                for (int x = 0; x < Dimensao.Largura; x++) {
                    Matriz[x, y] = new Bloco(x, y, CorPeca.TRANSPARENTE);
                }
            }

            //pintar os blocos que já saíram
            foreach (var i in _blocosJogados) {
                Matriz[i.X, i.Y] = i;
            }

            //pinta o bloco atual
            foreach (var i in PecaAtual.Blocos) {
                Matriz[i.X, i.Y] = i;
            }

            QuandoAtualizar?.Invoke();
        }

        void AtualizarAsync() {

            new Thread(() => {

                while (EstaJogando) {
                    Avaliar();
                    Thread.Sleep(Estatisticas.Velocidade);
                }
            }){ IsBackground = true }.Start();
        }

        IPeca CriarBlocoAleatorio() {
            var x = new Random().Next(0, 7);
            var tipo = (TipoPeca)x;
            var rotacao = new Random().Next(0, 4);
            var posicao = new Ponto(0, 0);
            var peca =  new PecaBuilder().DoTipo(tipo)
                .NaPosicao(posicao)
                .ComRotacao(rotacao)
                .NoTabuleiro(this)
                .Construir();

            return peca.Clonar();
        }

        bool ExisteColisao(IPeca peca) {

            if (_blocosJogados.Count <= 1)
                return false;

            foreach (var a in _blocosJogados) {
                foreach (var b in peca.Blocos) {
                    if (a.ColideCom(b)) {
                        return true;
                    }
                }
            }

            return false;
        }

        void TrocarPecaAtual() {
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

        int LimparLinhas() {

            int linhas = 0;

            //note que se no tabuleiro houver menos de 10 blocos, logicamente não haverá linha total preenchida
            //então isso economiza um pouco o processamento
            if (_blocosJogados.Count < 10)
                return linhas;

            int linha = Dimensao.Altura - 1;

            //esse laço é invertido (decremental) para que a validação de linhas completadas ocorra debaixo pra cima.
            do {

                if (linhas == MAXIMO_LINHAS_POSSIVEIS)
                    break;

                if (ContarBlocosVaziosNaLinha(linha) > 0) {
                    linha--;
                } else {
                    linhas++;
                    RemoverLinha(linha);
                    MoverBlocosParaBaixo(linha);
                }

            } while (linha >= 0);

            return linhas;
        }

        int ContarBlocosVaziosNaLinha(int linha) {
            return Dimensao.Largura - _blocosJogados.Count(_ => _.Y == linha);
        }

        void MoverBlocosParaBaixo(int linha) {
            for (int i = 0; i < _blocosJogados.Count; i++) {
                if (_blocosJogados[i].Y < linha) {
                    _blocosJogados[i] = new Bloco(_blocosJogados[i].X, _blocosJogados[i].Y + 1, _blocosJogados[i].Cor);
                }
            }
        }

        void RemoverLinha(int linha) {

            var blocosExcluidos = _blocosJogados.Where(x => x.Y == linha).ToList();

            foreach (var bloco in blocosExcluidos) {
                _blocosJogados.Remove(bloco);
            }
        }

        void Pontuar(int linhas) {
            Estatisticas.Pontuar(linhas);
            QuandoLimpar?.Invoke();
        }

        #endregion
    }
}
