namespace Gadz.Tetris.Model.Tabuleiros {
    public class TabuleiroEstadoPausado : TabuleiroEstado {

        public TabuleiroEstadoPausado() {
            EstaJogando = false;
        }

        public override bool PodeAlterarEstatisticas(ITabuleiro tabuleiro) {
            return false;
        }

        public override bool PodeIniciar(ITabuleiro tabuleiro) {
            return false;
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
            return true;
        }

        public override string ToString() {
            return "Parado";
        }
    }
}
