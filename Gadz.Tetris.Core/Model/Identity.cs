using System;

namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="Identity" />
    /// </summary>
    public struct Identity
    {
        /// <summary>
        /// Defines the _id
        /// </summary>
        private string _id;

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        public Identity(string id)
        {
            _id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        public Identity(int id)
        {
            _id = id.ToString();
        }

        /// <summary>
        /// The New
        /// </summary>
        /// <returns>The <see cref="Identity"/></returns>
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
        /// Converts the string representation of a identity in a instance of identity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Identity Parse(string id)
        {
            return new Identity(id);
        }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return _id;
        }
    }
}
