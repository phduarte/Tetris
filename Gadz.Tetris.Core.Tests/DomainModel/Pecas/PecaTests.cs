using Gadz.Tetris.Core.DomainModel.Tabuleiros;
using Gadz.Tetris.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gadz.Tetris.Core.DomainModel.Pecas {

    [TestClass]
    public class PecaTests {

        GameController gameController;

        [TestInitialize]
        public void Setup() {
            gameController = GameController.Create(10, 20);
        }
    }
}
