namespace Gadz.Tetris.Model.Pecas.Formas {
    internal class TetraminoO {

        public static IForma Desenhar(int x, int y, CorPeca cor) {

            var pontos = new Bloco[4];

            pontos[0] = new Bloco(x, y, cor);
            pontos[1] = new Bloco(x, y + 1, cor);
            pontos[2] = new Bloco(x + 1, y, cor);
            pontos[3] = new Bloco(x + 1, y + 1, cor);

            return new Forma(pontos);
        }
    }
}
