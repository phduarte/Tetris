namespace Gadz.Tetris.Model
{
    public abstract class Tetramino
    {
        public Block[] Blocks { get; protected set; }

        public static Tetramino Draw(PieceType type, Point point, int rotation)
        {
            return TetraminoFactory.Draw(type, point, rotation);
        }

        public static Tetramino Draw(PieceType type)
        {
            return Draw(type, new Point(0, 0), 0);
        }

        public static Tetramino Draw(PieceType type, Point point)
        {
            return Draw(type, point, 0);
        }

        public static Tetramino Draw(PieceType type, int rotation)
        {
            return Draw(type, new Point(0, 0), rotation);
        }

        #region overrided methods

        public override string ToString()
        {
            return $"{Blocks[0]},{Blocks[1]},{Blocks[2]},{Blocks[3]}";
        }

        #endregion overrided methods
    }
}