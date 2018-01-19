namespace Gadz.Tetris.Model.Pecas.Formas {
    internal class TetraminoS {
        
        public static IForma Desenhar(int x, int y, int rotacao, CorPeca cor) {

            var pontos = new Bloco[4];

            switch (rotacao) {
                case 0:

                    pontos[0] = new Bloco(x, y, cor);
                    pontos[1] = new Bloco(x, y + 1, cor);
                    pontos[2] = new Bloco(x + 1, y + 1, cor);
                    pontos[3] = new Bloco(x + 1, y + 2, cor);

                    break;

                case 1:

                    pontos[0] = new Bloco(x, y+1, cor);
                    pontos[1] = new Bloco(x+1, y+1, cor);
                    pontos[2] = new Bloco(x+1, y, cor);
                    pontos[3] = new Bloco(x+2, y, cor);

                    break;

                case 2:

                    pontos[0] = new Bloco(x, y, cor);
                    pontos[1] = new Bloco(x, y + 1, cor);
                    pontos[2] = new Bloco(x + 1, y + 1, cor);
                    pontos[3] = new Bloco(x + 1, y + 2, cor);

                    break;

                case 3:

                    pontos[0] = new Bloco(x, y + 1, cor);
                    pontos[1] = new Bloco(x + 1, y + 1, cor);
                    pontos[2] = new Bloco(x + 1, y, cor);
                    pontos[3] = new Bloco(x + 2, y, cor);

                    break;
            }

            return new Forma(pontos);
        }
    }
}
