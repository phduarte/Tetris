namespace Gadz.Tetris.Model {
    public class PecaBuilder {

        #region fields

        TiposDePeca _tipoPeca;
        Ponto _posicao;
        int _rotacao;
        Tabuleiro _tabuleiro;

        #endregion

        #region methods

        public PecaBuilder DoTipo(TiposDePeca tipo) {
            _tipoPeca = tipo;
            return this;
        }
        public PecaBuilder NaPosicao(Ponto posicao) {
            _posicao = posicao;
            return this;
        }

        public PecaBuilder ComRotacao(int rotacao) {
            _rotacao = rotacao;
            return this;
        }

        public PecaBuilder NoTabuleiro(Tabuleiro tabuleiro) {
            _tabuleiro = tabuleiro;
            return this;
        }

        public Peca Construir() {
            return new Peca(_tipoPeca, _posicao, _rotacao, _tabuleiro);
        }

        #endregion
    }
}
