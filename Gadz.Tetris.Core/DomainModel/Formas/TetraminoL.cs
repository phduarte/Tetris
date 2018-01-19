using Gadz.Tetris.Core.DomainModel.Pecas;

namespace Gadz.Tetris.Core.DomainModel.Formas {
    internal class TetraminoL : Forma {
        public TetraminoL(Ponto ponto, int rotacao, CorPeca cor) {

            var pontos = new Bloco[4];
            int x = ponto.X, y = ponto.Y;

            if (rotacao == 0) {
                pontos[0] = new Bloco(x, y, cor);
                pontos[1] = new Bloco(x, y + 1, cor);
                pontos[2] = new Bloco(x, y + 2, cor);
                pontos[3] = new Bloco(x + 1, y + 2, cor);
            } else if (rotacao == 1) {
                pontos[0] = new Bloco(x, y + 1, cor);
                pontos[1] = new Bloco(x + 1, y + 1, cor);
                pontos[2] = new Bloco(x + 2, y + 1, cor);
                pontos[3] = new Bloco(x + 2, y, cor);
            } else if (rotacao == 2) {
                pontos[0] = new Bloco(x, y, cor);
                pontos[1] = new Bloco(x + 1, y, cor);
                pontos[2] = new Bloco(x + 1, y + 1, cor);
                pontos[3] = new Bloco(x + 1, y + 2, cor);
            } else if (rotacao == 3) {
                pontos[0] = new Bloco(x, y, cor);
                pontos[1] = new Bloco(x + 1, y, cor);
                pontos[2] = new Bloco(x + 2, y, cor);
                pontos[3] = new Bloco(x, y + 1, cor);
            }

            Blocos = pontos;
        }
    }
}
