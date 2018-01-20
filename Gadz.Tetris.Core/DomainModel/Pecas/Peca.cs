using Gadz.Tetris.Core.DomainModel.Formas;
using Gadz.Tetris.Core.DomainModel.Tabuleiros;

namespace Gadz.Tetris.Core.DomainModel.Pecas {
    internal class Peca : IPeca {

        #region properties

        public int Rotacao { get; private set; }
        public CorPeca Cor { get; private set; }
        public Forma Forma => FormaFactory.Desenhar(Tipo, Posicao, Rotacao);
        public Ponto Posicao { get; private set; }
        public TipoPeca Tipo { get; private set; }
        public ITabuleiro Tabuleiro { get; private set; }
        public Bloco[] Blocos => Forma.Blocos??Forma.Blocos;

        #endregion

        #region constructors

        internal Peca(TipoPeca tipo, Ponto posicao, int rotacao, ITabuleiro tabuleiro) {
            Tipo = tipo;
            Posicao = posicao;
            Tabuleiro = tabuleiro;
            Rotacao = rotacao;
            Cor = PegarCor();
        }

        internal Peca(IPeca clone) {
            Rotacao = clone.Rotacao;
            Posicao = clone.Posicao;
            Tipo = clone.Tipo;
            Tabuleiro = clone.Tabuleiro;
            Cor = clone.Cor;
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

            if(maxX > Tabuleiro.Largura-1) {

                int excesso = maxX - (Tabuleiro.Largura - 1);
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

        CorPeca PegarCor() {
            return Cores.PegarCorPara(Tipo);
        }

        #endregion

        #region overrided methods

        public override string ToString() {
            return $"{Tipo.ToString()} ({Forma.ToString()})";
        }

        public override bool Equals(object obj) {
            if(obj is IPeca p) {
                return ToString().Equals(p.ToString());
            }
            return false;
        }

        public override int GetHashCode() {
            return ToString().GetHashCode();
        }

        #endregion
    }
}
