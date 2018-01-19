namespace Gadz.Tetris.Model.Tabuleiros {
    public class TabuleiroEstadoTerminado : TabuleiroEstado {

        public TabuleiroEstadoTerminado() {
            EstaJogando = false;
        }

        public override bool PodeAlterarEstatisticas(ITabuleiro tabuleiro) {
            return false;
        }

        public override bool PodeIniciar(ITabuleiro tabuleiro) {
            return true;
        }

        public override bool PodeMovimentarBloco(ITabuleiro tabuleiro) {
            return false;
        }

        public override bool PodePausar(ITabuleiro tabuleiro) {
            return false;
        }

        public override bool PodeReiniciar(ITabuleiro tabuleiro) {
            return true;
        }

        public override bool PodeTerminar(ITabuleiro tabuleiro) {
            return false;
        }
    }
}
