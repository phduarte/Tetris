namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// Defines the <see cref="TetraminoZ" />
    /// </summary>
    internal class TetraminoZ : Tetramino
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraminoZ"/> class.
        /// </summary>
        /// <param name="config">The position<see cref="TetraminoConfiguration"/></param>
        public TetraminoZ(TetraminoConfiguration config)
        {
            var blocks = new Block[4];
            int x = config.Position.X, y = config.Position.Y;

            switch (config.Rotation)
            {
                case 0:
                case 2:

                    blocks[0] = new Block(x + 1, y, config.Color);
                    blocks[1] = new Block(x + 1, y + 1, config.Color);
                    blocks[2] = new Block(x, y + 1, config.Color);
                    blocks[3] = new Block(x, y + 2, config.Color);

                    break;

                case 1:
                case 3:

                    blocks[0] = new Block(x, y, config.Color);
                    blocks[1] = new Block(x + 1, y, config.Color);
                    blocks[2] = new Block(x + 1, y + 1, config.Color);
                    blocks[3] = new Block(x + 2, y + 1, config.Color);

                    break;

                default:
                    throw new System.NotImplementedException($"Rotation {config.Rotation}");
            }

            Blocks = blocks;
        }
    }
}
