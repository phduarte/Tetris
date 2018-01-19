namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {
    public class TabuleiroEstadoTerminado : ITabuleiroEstado {

        public bool EstaJogando { get; private set; }

        public TabuleiroEstadoTerminado() {
            EstaJogando = false;
        }

        public bool PodeAlterarEstatisticas(ITabuleiro tabuleiro) {
            return false;
        }

        public bool PodeIniciar(ITabuleiro tabuleiro) {
            return true;
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
            return false;
        }

        public override string ToString() {
            return "Terminado"; 
        }
    }
}
