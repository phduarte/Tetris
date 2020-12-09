namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="Entity" />
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Gets the Id
        /// </summary>
        public Identity Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="id">The id<see cref="Identity"/></param>
        protected Entity(Identity id)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        protected Entity()
        {
            Id = Identity.New();
        }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
