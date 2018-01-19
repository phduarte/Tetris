namespace Gadz.Tetris.Model.Tabuleiros {
    public class TabuleiroEstadoJogando : TabuleiroEstado {

        public TabuleiroEstadoJogando() {
            EstaJogando = true;
        }

        public override bool PodeAlterarEstatisticas(ITabuleiro tabuleiro) {
            return true;
        }

        public override bool PodeIniciar(ITabuleiro tabuleiro) {
            return false;
        }

        public override bool PodeMovimentarBloco(ITabuleiro tabuleiro) {
            return true;
        }

        public override bool PodePausar(ITabuleiro tabuleiro) {
            return true;
        }

        public override bool PodeReiniciar(ITabuleiro tabuleiro) {
            return true;
        }

        public override bool PodeTerminar(ITabuleiro tabuleiro) {
            return true;
        }

        public override string ToString() {
            return "Jogando";
        }
    }
}
