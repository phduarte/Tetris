﻿namespace Gadz.Tetris.Model.Pecas {
    public struct Bloco {

        public Ponto Ponto { get; private set; }
        public CorPeca Cor { get; private set; }
        public int X { get { return Ponto.X; } }
        public int Y { get { return Ponto.Y; } }

        public Bloco(int x, int y, CorPeca cor) {
            Ponto = new Ponto(x, y);
            Cor = cor;
        }

        public bool ColideCom(Bloco bloco) {
            return Ponto.ColideCom(bloco.Ponto);
        }

        public override string ToString() {
            return Ponto.ToString();
        }
    }
}
