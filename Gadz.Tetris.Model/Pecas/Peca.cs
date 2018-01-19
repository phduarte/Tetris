using Gadz.Tetris.Model.Pecas.Formas;
using Gadz.Tetris.Model.Tabuleiros;

namespace Gadz.Tetris.Model.Pecas {
    public class Peca : IPeca {

        #region properties

        public int Rotacao { get; private set; }
        public CorPeca Cor { get; private set; }
        public IForma Forma => new Forma(Posicao.X, Posicao.Y, Tipo, Cor, Rotacao);
        public Ponto Posicao { get; private set; }
        public TipoPeca Tipo { get; private set; }
        public ITabuleiro Tabuleiro { get; private set; }
        public Bloco[] Blocos => Forma.Blocos??Forma.Blocos;

        #endregion

        #region constructors

        public Peca(TipoPeca tipo, Ponto posicao, int rotacao, ITabuleiro tabuleiro) {
            Tipo = tipo;
            Posicao = posicao;
            Tabuleiro = tabuleiro;
            Rotacao = rotacao;
            Cor = colorir();
        }

        public Peca(IPeca clone) {
            Rotacao = clone.Rotacao;
            Posicao = clone.Posicao;
            Tipo = clone.Tipo;
            Tabuleiro = clone.Tabuleiro;
            Cor = colorir();
        }

        public Peca(TipoPeca tipo, int rotacao, ITabuleiro tabuleiro) {

            Tipo = tipo;
            Posicao = new Ponto(0, 0);
            Rotacao = rotacao;
            Tabuleiro = tabuleiro;
            Cor = colorir();
        }

        #endregion

        #region methods

        public void Girar() {

            Rotacao = Rotacao == 3 ? 0 : ++Rotacao;

            int maxX = Posicao.X;
            int minX = Posicao.X;

            foreach(var b in Blocos) {
                if(b.X > maxX) {
                    maxX = b.X;
                }
            }

            foreach (var b in Blocos) {
                if (b.X < minX) {
                    minX = b.X;
                }
            }

            if(maxX > Tabuleiro.Dimensao.Largura-1) {

                int excesso = maxX - (Tabuleiro.Dimensao.Largura - 1);
                for (int i = 0; i < excesso; i++) {
                    MoverEsquerda();
                }
            }

            if(minX < 0) {
                for (int i = minX; i < 0; i++) {
                    MoverDireita();
                }
            }
        }

        public void MoverBaixo() {
            Posicao = new Ponto(Posicao.X, Posicao.Y + 1);
        }

        public void MoverDireita() {
            Posicao = new Ponto(Posicao.X + 1, Posicao.Y);
        }

        public void MoverEsquerda() {
            Posicao = new Ponto(Posicao.X - 1, Posicao.Y);
        }

        public IPeca Clonar() {
            return new Peca(this);
        }

        CorPeca colorir() {
            switch (Tipo) {
                case TipoPeca.T:
                    return CorPeca.ROXO;
                case TipoPeca.I:
                    return CorPeca.CIANO;
                case TipoPeca.L:
                    return CorPeca.LARANJA;
                case TipoPeca.J:
                    return CorPeca.AZUL;
                case TipoPeca.O:
                    return CorPeca.AMARELO;
                case TipoPeca.S:
                    return CorPeca.VERDE;
                case TipoPeca.Z:
                    return CorPeca.VERMELHO;
                default:
                    return CorPeca.TRANSPARENTE;
            }
        }

        public override string ToString() {
            return $"{Tipo.ToString()} ({Forma.ToString()})";
        }

        #endregion
    }
}
