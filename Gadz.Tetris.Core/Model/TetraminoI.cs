namespace Gadz.Tetris.Model
{
    internal class TetraminoI : Tetramino
    {
        public TetraminoI(Point position, int rotation, PieceColor color)
        {

            var blocks = new Block[4];
            var x = position.X;
            var y = position.Y;

            if(rotation == 0 || rotation == 2)
            {
                blocks[0] = new Block(x, y, color);
                blocks[1] = new Block(x, y + 1, color);
                blocks[2] = new Block(x, y + 2, color);
                blocks[3] = new Block(x, y + 3, color);
            }
            else
            {
                blocks[0] = new Block(x, y, color);
                blocks[1] = new Block(x + 1, y, color);
                blocks[2] = new Block(x + 2, y, color);
                blocks[3] = new Block(x + 3, y, color);
            }

            Blocks = blocks;
        }
    }
}
