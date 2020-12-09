namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// Defines the <see cref="TetraminoO" />
    /// </summary>
    internal class TetraminoO : Tetramino
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraminoO"/> class.
        /// </summary>
        /// <param name="config">The position<see cref="TetraminoConfiguration"/></param>
        public TetraminoO(TetraminoConfiguration config)
        {
            var blocks = new Block[4];
            int x = config.Position.X, y = config.Position.Y;
            
            blocks[0] = new Block(x, y, config.Color);
            blocks[1] = new Block(x, y + 1, config.Color);
            blocks[2] = new Block(x + 1, y, config.Color);
            blocks[3] = new Block(x + 1, y + 1, config.Color);

            Blocks = blocks;
        }
    }
}
