namespace Gadz.Tetris.Model.Tabuleiros {
    public struct Dimensao {

        public int Altura, Largura;

        public Dimensao(int largura, int altura) {
            Altura = altura;
            Largura = largura;
        }

        public override string ToString() {
            return $"{Largura}x{Altura}";
        }
    }
}