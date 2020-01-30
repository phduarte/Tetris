namespace Gadz.Tetris.Model {
    internal class PausedState : IBoardState {

        public bool IsPlaying => false;
        public bool CanChangeStats => false;
        public bool CanStart => false;
        public bool CanMove => false;
        public bool CanPause => false;
        public bool CanRestart => true;
        public bool CanFinish => true;
        public override string ToString() => "Parado";
    }
}
