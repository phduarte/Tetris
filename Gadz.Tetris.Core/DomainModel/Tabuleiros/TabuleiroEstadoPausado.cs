namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {
    public class TabuleiroEstadoPausado : ITabuleiroEstado {

        public bool EstaJogando { get; private set; }

        public TabuleiroEstadoPausado() {
            EstaJogando = false;
        }

        public bool PodeAlterarEstatisticas(ITabuleiro tabuleiro) {
            return false;
        }

        public bool PodeIniciar(ITabuleiro tabuleiro) {
            return false;
        }

        public bool PodeMovimentarBloco(ITabuleiro tabuleiro) {
            return false;
        }

        public bool PodePausar(ITabuleiro tabuleiro) {
            return false;
        }

        public bool PodeReiniciar(ITabuleiro tabuleiro) {
            return true;
        }

        public bool PodeTerminar(ITabuleiro tabuleiro) {
            return true;
        }

        public override string ToString() {
            return "Parado";
        }
    }
}
