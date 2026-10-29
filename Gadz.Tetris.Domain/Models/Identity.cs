namespace Gadz.Tetris.Domain.Models;

/// <summary>
/// Identidade de uma <see cref="Entity"/>. Guarda o valor como texto e converte de/para <c>int</c> e <c>string</c>,
/// para aceitar tanto GUIDs quanto os ids numéricos do arquivo de estatísticas.
/// </summary>
public struct Identity
{
    /// <summary>
    /// O valor da identidade.
    /// </summary>
    private string _id;

    /// <summary>
    /// Cria a identidade a partir de um texto (por exemplo, um GUID).
    /// </summary>
    /// <param name="id">O valor da identidade.</param>
    public Identity(string id)
    {
        _id = id;
    }

    /// <summary>
    /// Cria a identidade a partir de um número.
    /// </summary>
    /// <param name="id">O valor da identidade.</param>
    public Identity(int id)
    {
        _id = id.ToString();
    }

    /// <summary>
    /// Gera uma identidade única, baseada num GUID novo.
    /// </summary>
    /// <returns>A nova identidade.</returns>
    public static Identity New()
    {
        return new Identity(Guid.NewGuid().ToString());
    }

    public static implicit operator Identity(int id)
    {
        return new Identity(id);
    }

    public static implicit operator int(Identity id)
    {
        if (int.TryParse(id.ToString(), out int u))
            return u;

        return -1;
    }

    public static implicit operator string(Identity id)
    {
        return id.ToString();
    }

    public static implicit operator Identity(string id)
    {
        return new Identity(id);
    }

    /// <summary>
    /// Converte o texto de uma identidade numa <see cref="Identity"/>.
    /// </summary>
    /// <param name="id">O valor da identidade em texto.</param>
    /// <returns>A identidade correspondente.</returns>
    public static Identity Parse(string id)
    {
        return new Identity(id);
    }

    /// <summary>
    /// O valor da identidade em texto.
    /// </summary>
    /// <returns>O valor guardado.</returns>
    public override string ToString()
    {
        return _id;
    }
}
