using System;
using System.Windows.Forms;

namespace Gadz.Tetris.Desktop
{
    /// <summary>
    /// Defines the <see cref="About" />
    /// </summary>
    public partial class About : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The button2_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
