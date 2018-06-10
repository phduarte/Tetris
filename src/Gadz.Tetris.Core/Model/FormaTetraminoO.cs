namespace Gadz.Tetris.Model {
    internal class FormaTetraminoO : Forma {

        public FormaTetraminoO(Ponto ponto, CoresDePeca cor) {

            var pontos = new Bloco[4];
            int x = ponto.X, y = ponto.Y;

            pontos[0] = new Bloco(x, y, cor);
            pontos[1] = new Bloco(x, y + 1, cor);
            pontos[2] = new Bloco(x + 1, y, cor);
            pontos[3] = new Bloco(x + 1, y + 1, cor);

            Blocos = pontos;
        }
    }
}
