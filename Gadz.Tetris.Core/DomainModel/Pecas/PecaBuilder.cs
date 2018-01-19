using Gadz.Tetris.Core.DomainModel.Tabuleiros;

namespace Gadz.Tetris.Core.DomainModel.Pecas {
    public class PecaBuilder {

        #region fields

        TipoPeca _tipoPeca;
        CorPeca _corPeca;
        Ponto _posicao;
        int _rotacao;
        ITabuleiro _tabuleiro;

        #endregion

        #region methods

        public PecaBuilder DoTipo(TipoPeca tipo) {
            _tipoPeca = tipo;
            return this;
        }
        public PecaBuilder ComCor(CorPeca cor) {
            _corPeca = cor;
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

        public PecaBuilder NoTabuleiro(ITabuleiro tabuleiro) {
            _tabuleiro = tabuleiro;
            return this;
        }

        public IPeca Construir() {
            return new Peca(_tipoPeca, _posicao, _rotacao, _tabuleiro);
        }

        #endregion
    }
}
