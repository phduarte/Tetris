using Gadz.Tetris.Data;
using Gadz.Tetris.Model;
using System;
using System.Collections.Generic;

namespace Gadz.Tetris {

    public class GameController {

        #region fields

        readonly Board _board;

        #endregion

        #region eventos

        public event GameActionEventHandler OnRefresh;
        public event GameActionEventHandler OnFinish;
        public event GameActionEventHandler OnClear;
        public event GameActionEventHandler OnMove;
        public event GameActionEventHandler OnSlide;

        #endregion

        #region properties

        public bool Playing => _board.IsPlaying;
        public int BoardWidth => _board.Width;
        public int BoardHeight => _board.Height;
        public int Speed => _board.Speed;
        public int Lines => _board.Lines;
        public Block[,] Matrix => _board.Matrix;
        public TimeSpan Duration => _board.Duration;
        public int Score => _board.Score;
        public int Level => _board.Level;
        public IBoardState State => _board.State;
        public Point CurrentPiecePosition => _board.CurrentPiece.Position;
        public Board CurrentBoard => _board;

        #endregion

        #region constructors

        private GameController(int width, int height) {

            if(width < 10) {
                throw new ArgumentException(nameof(width));
            }

            if(height < 10) {
                throw new ArgumentException(nameof(height));
            }

            _board = new Board(new StatsRepository(), width, height);
            _board.OnRefresh += ()=> { OnRefresh?.Invoke(); };
            _board.OnFinish += ()=> { OnFinish?.Invoke();  };
            _board.OnClear += ()=> { OnClear?.Invoke(); };
            _board.OnMove += ()=> { OnMove?.Invoke(); };
            _board.OnSlide += ()=> { OnSlide?.Invoke(); };
        }

        #endregion

        public static GameController Create(int width, int height) {
            return new GameController(width, height);
        }

        public void SmashDown() {
            _board.SmashDown();
        }

        public void RunRight()=> _board.SlideRight();

        public void RunLeft()=> _board.SlideLeft();

        public void Start() => _board.Start();

        public void MoveDown() => _board.MoveDown();

        public void MoveRight()=> _board.MoveRight();

        public void MoveLeft()=> _board.MoveLeft();

        public void Pause()=> _board.Pause();

        public void Save()=> _board.Save();

        public void Exit() {
            _board.Finish();
            Save();
        }

        public void Rotate()=> _board.Rotate();

        public void Continue()=> _board.Continue();

        public void Restart() => _board.Restart();

        public IEnumerable<Block> GetActualBlocks()=> _board.CurrentPiece.Blocks;

        public IEnumerable<Block> GetNextBlocks()=> _board.NextPiece.Blocks;

        public int GetMaxScore() {
            return _board.Record;
        }
    }
}
