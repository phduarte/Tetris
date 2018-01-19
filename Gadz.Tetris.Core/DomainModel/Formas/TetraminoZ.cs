using Gadz.Tetris.Core.DomainModel.Pecas;

namespace Gadz.Tetris.Core.DomainModel.Formas {
    internal class TetraminoZ : Forma {
        
        public TetraminoZ(Ponto ponto, int rotacao, CorPeca cor) {

            var pontos = new Bloco[4];
            int x = ponto.X, y = ponto.Y;

            switch (rotacao) {
                case 0:

                    pontos[0] = new Bloco(x+1, y,cor);
                    pontos[1] = new Bloco(x+1, y+1, cor);
                    pontos[2] = new Bloco(x, y+1, cor);
                    pontos[3] = new Bloco(x, y+2, cor);

                    break;

                case 1:

                    pontos[0] = new Bloco(x, y, cor);
                    pontos[1] = new Bloco(x + 1, y, cor);
                    pontos[2] = new Bloco(x+1, y + 1, cor);
                    pontos[3] = new Bloco(x+2, y + 1, cor);

                    break;

                case 2:

                    pontos[0] = new Bloco(x + 1, y, cor);
                    pontos[1] = new Bloco(x + 1, y + 1, cor);
                    pontos[2] = new Bloco(x, y + 1, cor);
                    pontos[3] = new Bloco(x, y + 2, cor);

                    break;

                case 3:

                    pontos[0] = new Bloco(x, y, cor);
                    pontos[1] = new Bloco(x + 1, y, cor);
                    pontos[2] = new Bloco(x + 1, y + 1, cor);
                    pontos[3] = new Bloco(x + 2, y + 1, cor);

                    break;
            }

            Blocos = pontos;
        }
    }
}
