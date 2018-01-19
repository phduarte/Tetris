using Gadz.Tetris.Core.DomainModel.Pecas;

namespace Gadz.Tetris.Core.DomainModel.Formas {
    internal class TetraminoO : Forma {

        public TetraminoO(Ponto ponto, CorPeca cor) {

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
