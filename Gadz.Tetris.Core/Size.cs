namespace Gadz.Tetris {
    public struct Size {

        public int Height, Width;

        public Size(int largura, int altura) {
            Height = altura;
            Width = largura;
        }

        public override string ToString() {
            return $"{Width}x{Height}";
        }
    }
}