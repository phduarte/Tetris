using Gadz.Tetris.Domain.Models;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model
{
    /// <summary>
    /// Defines the <see cref="DimensaoTests" />
    /// </summary>
    public class DimensaoTests
    {
        /// <summary>
        /// The DeveInstanciar
        /// </summary>
        [Test]
        public void DeveInstanciar()
        {
            var dimensao = new Size(10, 20);
            Assert.AreEqual("10:20", dimensao.ToString());
        }
    }
}
