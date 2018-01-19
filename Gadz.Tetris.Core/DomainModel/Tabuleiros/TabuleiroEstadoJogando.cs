namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {
    public class TabuleiroEstadoJogando : ITabuleiroEstado {

        public bool EstaJogando { get; private set; }

        public TabuleiroEstadoJogando() {
            EstaJogando = true;
        }

        public bool PodeAlterarEstatisticas(ITabuleiro tabuleiro) {
            return true;
        }

        public bool PodeIniciar(ITabuleiro tabuleiro) {
            return false;
        }

        public bool PodeMovimentarBloco(ITabuleiro tabuleiro) {
            return true;
        }

        public bool PodePausar(ITabuleiro tabuleiro) {
            return true;
        }

        public bool PodeReiniciar(ITabuleiro tabuleiro) {
            return true;
        }

        public bool PodeTerminar(ITabuleiro tabuleiro) {
            return true;
        }

        public override string ToString() {
            return "Jogando";
        }
    }
}
