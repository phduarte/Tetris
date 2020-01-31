namespace Gadz.Tetris.Model {
    public interface IBoardState {
        bool IsPlaying { get; }
        bool CanStart { get; }
        bool CanPause { get; }
        bool CanFinish { get; }
        bool CanRestart { get; }
        bool CanMove { get; }
        bool CanChangeStats { get; }
    }
}