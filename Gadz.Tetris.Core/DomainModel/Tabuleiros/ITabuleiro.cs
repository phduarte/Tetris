using Gadz.Tetris.Core.DomainModel.Pecas;
using System.Collections.Generic;

namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {

    #region delegates

    public delegate void Atualizou();
    public delegate void Terminou();
    public delegate void Limpou();
    public delegate void Moveu();
    public delegate void Correu();

    #endregion

    public interface ITabuleiro {

        #region fields

        Dimensao Dimensao { get; }
        ITabuleiroEstado Estado { get; }
        Estatisticas Estatisticas { get; }
        Bloco[,] Matriz { get; }
        IPeca PecaAtual { get; }
        IPeca ProximaPeca { get; }
        int Nivel { get; }
        int Linhas { get; }
        long Duracao { get; }
        IList<Bloco> Blocos { get; }
        bool EstaJogando { get; }
        int RecordeMaximo { get; }

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

        #endregion

        #region methods

        void Pausar();
        void Continuar();
        void Terminar();
        void Reiniciar();
        void Iniciar();
        void Carregar(Identidade id);
        void Salvar();

        #endregion
    }
}
