using Gadz.Tetris.Core.Infrastructure.Data;
using Gadz.Tetris.Core.DomainModel.Pecas;
using Gadz.Tetris.Core.DomainModel.Tabuleiros;
using System;
using System.Collections.Generic;

namespace Gadz.Tetris.Core.Services {

    public class GameController {

        #region fields

        ITabuleiro _tabuleiro;

        #endregion

        #region eventos

        public event Atualizou QuandoAtualizar;
        public event Terminou QuandoTerminar;
        public event Limpou QuandoLimpar;
        public event Moveu QuandoMover;
        public event Correu QuandoDeslizar;

        #endregion

        #region properties

        public bool Playing => _tabuleiro.EstaJogando;
        public int BoardWidth => _tabuleiro.Dimensao.Altura;
        public int BoardHeight => _tabuleiro.Dimensao.Largura;
        public int Speed => _tabuleiro.Estatisticas.Velocidade;
        public int Lines => _tabuleiro.Linhas;
        public Bloco[,] Matrix => _tabuleiro.Matriz;
        public TimeSpan Time => _tabuleiro.Estatisticas.Duracao;
        public int Score => _tabuleiro.Estatisticas.Pontos;
        public int Level => _tabuleiro.Nivel;

        #endregion

        #region constructors

        private GameController(int largura, int altura) {
            _tabuleiro = new Tabuleiro(new EstatisticasRepository(), largura, altura);
            _tabuleiro.QuandoAtualizar += () => { QuandoAtualizar?.Invoke(); };
            _tabuleiro.QuandoTerminar += () => { QuandoTerminar?.Invoke(); };
            _tabuleiro.QuandoLimpar += () => { QuandoLimpar?.Invoke(); };
            _tabuleiro.QuandoMover += () => { QuandoMover?.Invoke(); };
            _tabuleiro.QuandoDeslizar += () => { QuandoDeslizar?.Invoke(); };
        }

        #endregion

        public static GameController Create(int largura, int altura) {
            return new GameController(largura, altura);
        }

        public void SmashDown() {
            _tabuleiro.Cair();
        }

        public static GameController Load(string id) {
            //_tabuleiro = _repository.Load(id);
            return new GameController(20,10);
        }

        public void RunRight()=> _tabuleiro.CorrerDireita();

        public void RunLeft()=> _tabuleiro.CorrerEsquerda();

        public void Start() => _tabuleiro.Iniciar();

        public void MoveDown() => _tabuleiro.MoverBaixo();

        public void MoveRight()=> _tabuleiro.MoverDireita();

        public void MoveLeft()=> _tabuleiro.MoverEsquerda();

        public void Pause()=> _tabuleiro.Pausar();

        public void Save()=> _tabuleiro.Salvar();

        public void Exit() {
            _tabuleiro.Terminar();
            Save();
        }

        public void Rotate()=> _tabuleiro.Rotacionar();

        public void Continue()=> _tabuleiro.Continuar();

        public IEnumerable<Bloco> GetActualBlocks()=> _tabuleiro.PecaAtual.Blocos;

        public IEnumerable<Bloco> GetNextBlocks()=> _tabuleiro.ProximaPeca.Blocos;

        public int GetMaxScore() {
            return _tabuleiro.RecordeMaximo;
        }
    }
}
