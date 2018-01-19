using Gadz.Tetris.Model.Pecas;
using System.Collections.Generic;

namespace Gadz.Tetris.Model.Tabuleiros {

    public delegate void Atualizou();
    public delegate void Terminou();
    public delegate void Limpou();
    public delegate void Moveu();
    public delegate void Correu();

    public interface ITabuleiro {

        #region fields

        Dimensao Dimensao { get; }
        TabuleiroEstado Estado { get; }
        Estatisticas Estatisticas { get; }
        Bloco[,] Matriz { get; }
        IPeca PecaAtual { get; }
        IPeca ProximaPeca { get; }
        int Nivel { get; }
        int Linhas { get; }
        long Duracao { get; }
        IList<Bloco> Blocos { get; }
        bool EstaJogando { get; }

        #endregion

        #region movimentos

        void MoverEsquerda();
        void MoverDireita();
        void MoverBaixo();
        void Rotacionar();
        void CorrerEsquerda();
        void CorrerDireita();
        void Cair();

        #endregion

        #region eventos

        event Atualizou QuandoAtualizar;
        event Terminou QuandoTerminar;
        event Limpou QuandoLimpar;
        event Moveu QuandoMover;
        event Correu QuandoDeslizar;

        void Pausar();
        void Continuar();
        void Terminar();
        void Iniciar();

        #endregion
    }
}
