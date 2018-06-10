namespace Gadz.Tetris.Model {
    internal class FormaTetraminoI : Forma {

        public FormaTetraminoI(Ponto posicaoInicial, int rotacao, CoresDePeca cor) {

            var pontos = new Bloco[4];
            var x = posicaoInicial.X;
            var y = posicaoInicial.Y;

            if (rotacao == 0 || rotacao == 2) {
                pontos[0] = new Bloco(x, y, cor);
                pontos[1] = new Bloco(x, y + 1, cor);
                pontos[2] = new Bloco(x, y + 2, cor);
                pontos[3] = new Bloco(x, y + 3, cor);
            } else {
                pontos[0] = new Bloco(x, y, cor);
                pontos[1] = new Bloco(x + 1, y, cor);
                pontos[2] = new Bloco(x + 2, y, cor);
                pontos[3] = new Bloco(x + 3, y, cor);
            }

            Blocos = pontos;
        }
    }
}
