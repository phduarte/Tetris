using Gadz.Tetris.Core.DomainModel.Pecas;

namespace Gadz.Tetris.Core.DomainModel.Formas {
    internal class TetraminoI : Forma {

        public TetraminoI(Ponto posicaoInicial, int rotacao, CorPeca cor) {

            var pontos = new Bloco[4];
            int x = posicaoInicial.X;
            int y = posicaoInicial.Y;

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
