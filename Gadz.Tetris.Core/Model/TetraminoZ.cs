namespace Gadz.Tetris.Model
{
    internal class TetraminoZ : Tetramino
    {
        public TetraminoZ(Point position, int rotation, PieceColor color)
        {
            var blocks = new Block[4];
            int x = position.X, y = position.Y;

            switch (rotation)
            {
                case 0:
                case 2:

                    blocks[0] = new Block(x + 1, y, color);
                    blocks[1] = new Block(x + 1, y + 1, color);
                    blocks[2] = new Block(x, y + 1, color);
                    blocks[3] = new Block(x, y + 2, color);

                    break;

                case 1:
                case 3:

                    blocks[0] = new Block(x, y, color);
                    blocks[1] = new Block(x + 1, y, color);
                    blocks[2] = new Block(x + 1, y + 1, color);
                    blocks[3] = new Block(x + 2, y + 1, color);

                    break;

                default:
                    throw new System.Exception("Unexpected Case");
            }

            Blocks = blocks;
        }
    }
}