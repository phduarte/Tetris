namespace Gadz.Tetris.Model.Boards
{
    /// <summary>
    /// Defines the <see cref="IBoardState" />
    /// </summary>
    public interface IBoardState
    {
        /// <summary>
        /// Gets a value indicating whether IsPlaying
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Gets a value indicating whether CanStart
        /// </summary>
        bool CanStart { get; }

        /// <summary>
        /// Gets a value indicating whether CanPause
        /// </summary>
        bool CanPause { get; }

        /// <summary>
        /// Gets a value indicating whether CanFinish
        /// </summary>
        bool CanFinish { get; }

        /// <summary>
        /// Gets a value indicating whether CanRestart
        /// </summary>
        bool CanRestart { get; }

        /// <summary>
        /// Gets a value indicating whether CanMove
        /// </summary>
        bool CanMove { get; }

        /// <summary>
        /// Gets a value indicating whether CanChangeStats
        /// </summary>
        bool CanChangeStats { get; }
    }
}
