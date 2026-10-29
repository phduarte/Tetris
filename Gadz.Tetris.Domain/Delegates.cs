namespace Gadz.Tetris.Domain;

/// <summary>
/// Notificação de algo que aconteceu no jogo (peça movida, linha removida, pausa...). Não carrega dados: quem escuta consulta o estado.
/// </summary>
public delegate void GameActionEventHandler();
