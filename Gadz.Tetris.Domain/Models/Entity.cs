namespace Gadz.Tetris.Domain.Models;

/// <summary>
/// Base das entidades do domínio: objetos com identidade própria, que continuam os mesmos mesmo quando seus dados mudam.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// A identidade da entidade.
    /// </summary>
    public Identity Id { get; set; }

    /// <summary>
    /// Cria a entidade com uma identidade já existente (por exemplo, ao carregar do repositório).
    /// </summary>
    /// <param name="id">A identidade da entidade.</param>
    protected Entity(Identity id)
    {
        Id = id;
    }

    /// <summary>
    /// Cria a entidade com uma identidade nova.
    /// </summary>
    protected Entity()
    {
        Id = Identity.New();
    }

    /// <summary>
    /// A identidade em texto.
    /// </summary>
    /// <returns>O valor de <see cref="Id"/>.</returns>
    public override string ToString()
    {
        return Id.ToString();
    }
}
