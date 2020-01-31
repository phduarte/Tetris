namespace Gadz.Tetris.Model {
    internal class TetraminoO : Tetramino {

        public TetraminoO(Point position, PieceColor color) {

            var blocks = new Block[4];
            int x = position.X, y = position.Y;

            blocks[0] = new Block(x, y, color);
            blocks[1] = new Block(x, y + 1, color);
            blocks[2] = new Block(x + 1, y, color);
            blocks[3] = new Block(x + 1, y + 1, color);

            Blocks = blocks;
        }
    }
}
