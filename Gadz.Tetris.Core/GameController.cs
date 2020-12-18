using Gadz.Tetris.Data;
using Gadz.Tetris.Model.Boards;
using Gadz.Tetris.Model.Pieces;
using System;
using System.Collections.Generic;

namespace Gadz.Tetris
{
    /// <summary>
    /// Controlador do Jogo. <para>Fornece uma interface para aplicação cliente.</para>
    /// </summary>
    public class GameController
    {
        /// <summary>
        /// Defines the _board
        /// </summary>
        private readonly Board _board;

        /// <summary>
        /// Ocorre a cada alteração visual, de estado do jogo ou suas estatísticas
        /// </summary>
        public event GameActionEventHandler OnRefresh;

        /// <summary>
        /// Ocorre quando o jogo é finalizado.
        /// </summary>
        public event GameActionEventHandler OnFinish;

        /// <summary>
        /// Ocorre quando uma linha é destruída
        /// </summary>
        public event GameActionEventHandler OnClear;

        /// <summary>
        /// Ocorre quando uma peça é movimentada
        /// </summary>
        public event GameActionEventHandler OnMove;

        /// <summary>
        /// Ocorre quando uma peça desliza na tela
        /// </summary>
        public event GameActionEventHandler OnSlide;

        /// <summary>
        /// Ocorre quando uma peça desliza na tela
        /// </summary>
        public event GameActionEventHandler OnDrop;

        /// <summary>
        /// Ocorre quando o jogador pausa o jogo.
        /// </summary>
        public event GameActionEventHandler OnPause;

        /// <summary>
        /// Ocorre quando o jogo é reiniciado após estar pausado.
        /// </summary>
        public event GameActionEventHandler OnContinue;

        /// <summary>
        /// Ocorre quando o jogador perde.
        /// </summary>
        public event GameActionEventHandler OnLose;

        /// <summary>
        /// Gets a value indicating whether Playing
        /// Informa se o jogo está rodando
        /// </summary>
        public bool Playing => _board.IsPlaying;

        /// <summary>
        /// Gets the BoardWidth
        /// Largura do tabuleiro
        /// </summary>
        public int BoardWidth => _board.Width;

        /// <summary>
        /// Gets the BoardHeight
        /// Altura do tabuleiro
        /// </summary>
        public int BoardHeight => _board.Height;

        /// <summary>
        /// Gets the Speed
        /// Velocidade do jogo
        /// </summary>
        public int Speed => _board.Speed;

        /// <summary>
        /// Gets the Lines
        /// Quantidade de linhas destruídas
        /// </summary>
        public int Lines => _board.Lines;

        /// <summary>
        /// Gets the Matrix
        /// Matriz do tabuleiro
        /// </summary>
        public Block[,] Matrix => _board.Matrix;

        /// <summary>
        /// Gets the Duration
        /// Tempo de duração total do jogo atual
        /// </summary>
        public TimeSpan Duration => _board.Duration;

        /// <summary>
        /// Gets the Score
        /// Pontuação do jogador no jogo atual
        /// </summary>
        public int Score => _board.Score;

        /// <summary>
        /// Gets the Level
        /// Nível de dificuldade
        /// </summary>
        public int Level => _board.Level;

        /// <summary>
        /// Gets the State
        /// Estado do jogo
        /// </summary>
        public IBoardState State => _board.State;

        /// <summary>
        /// Gets the CurrentPiecePosition
        /// Posição da peça atual
        /// </summary>
        public Point CurrentPiecePosition => _board.CurrentPiece.Position;

        /// <summary>
        /// Gets the CurrentBoard
        /// Tabuleiro atual
        /// </summary>
        public Board Board => _board;

        /// <summary>
        /// Gets the Amount Tetris in the game
        /// </summary>
        public int Tetris => _board.Tetris;

        /// <summary>
        /// 
        /// </summary>
        public float TetrisRate => _board.TetrisRate;

        /// <summary>
        /// Prevents a default instance of the <see cref="GameController"/> class from being created.
        /// </summary>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        private GameController(int width, int height)
        {
            if (width < 10)
            {
                throw new ArgumentException(nameof(width));
            }

            if (height < 10)
            {
                throw new ArgumentException(nameof(height));
            }

            _board = new Board(new StatsRepository(), width, height);
            _board.OnRefresh += () => OnRefresh?.Invoke();
            _board.OnFinish += () => OnFinish?.Invoke();
            _board.OnLose += () => OnLose?.Invoke();
            _board.OnClear += () => OnClear?.Invoke();
            _board.OnMove += () => OnMove?.Invoke();
            _board.OnSlide += () => OnSlide?.Invoke();
            _board.OnPause += () => OnPause?.Invoke();
            _board.OnContinue += () => OnContinue?.Invoke();
            _board.OnDrop += () => OnDrop?.Invoke();
        }

        /// <summary>
        /// Cria um novo Controlador do Jogo
        /// </summary>
        /// <param name="width">Largura do tabuleiro</param>
        /// <param name="height">Algura do tabuleiro</param>
        /// <returns>Uma instância do Controlador do Jogo</returns>
        public static GameController Create(int width, int height)
        {
            return new GameController(width, height);
        }

        /// <summary>
        /// Desliza o bloco atual para baixo até encontrar um obstáculo
        /// </summary>
        public void SmashDown() => _board.SmashDown();

        /// <summary>
        /// Desliza o bloco atual para a direita até encontrar um obstáculo
        /// </summary>
        public void RunRight() => _board.SlideRight();

        /// <summary>
        /// Desliza o bloco atual para a esquerda até encontrar um obstáculo
        /// </summary>
        public void RunLeft() => _board.SlideLeft();

        /// <summary>
        /// Inicia o jogo
        /// </summary>
        public void Start() => _board.Start();

        /// <summary>
        /// Movimenta o bloco atual para baixo
        /// </summary>
        public void MoveDown() => _board.MoveDown();

        /// <summary>
        /// Movimenta o bloco atual para a direita
        /// </summary>
        public void MoveRight() => _board.MoveRight();

        /// <summary>
        /// Movimenta o bloco atual para a esquerda
        /// </summary>
        public void MoveLeft() => _board.MoveLeft();

        /// <summary>
        /// Pausa o jogo
        /// </summary>
        public void Pause() => _board.Pause();

        /// <summary>
        /// Salva o progresso do jogo
        /// </summary>
        public void Save() => _board.Save();

        /// <summary>
        /// Sai do j ogo
        /// </summary>
        public void Exit()
        {
            _board.Finish();
            Save();
        }

        /// <summary>
        /// Gira a peça atual
        /// </summary>
        public void Rotate() => _board.Rotate();

        /// <summary>
        /// Continua um jogo pausado
        /// </summary>
        public void Continue() => _board.Continue();

        /// <summary>
        /// Reinicia um jogo
        /// </summary>
        public void Restart() => _board.Restart();

        /// <summary>
        /// Blocos da peça atual
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Block> GetActualBlocks() => _board.CurrentPiece.Blocks;

        /// <summary>
        /// Blocos da próxima peça.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Block> GetNextBlocks() => _board.NextPiece.Blocks;

        /// <summary>
        /// Maior pontuação registrada
        /// </summary>
        /// <returns></returns>
        public int GetMaxScore()
        {
            return _board.Record;
        }
    }
}
