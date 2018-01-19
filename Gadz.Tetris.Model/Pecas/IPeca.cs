using Gadz.Tetris.Model.Pecas.Formas;
using Gadz.Tetris.Model.Tabuleiros;

namespace Gadz.Tetris.Model.Pecas {
    public interface IPeca : IMovil {
        int Rotacao { get; }
        IForma Forma { get; }
        Ponto Posicao { get; }
        TipoPeca Tipo { get; }
        CorPeca Cor { get; }
        ITabuleiro Tabuleiro { get; }
        IPeca Clonar();
        Bloco[] Blocos { get; }
    }
}