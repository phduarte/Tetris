using System.Threading;
using System.Windows.Forms;

namespace Gadz.Tetris.Desktop
{
    static class ShakeFormExtensions
    {
        public static void ShakeLeft(this Form form, int times)
        {
            for (var i = times; i > 0; i--)
            {
                form.Left -= i;
                Thread.Sleep(times * i);
                form.Left += i;
            }
        }

        public static void ShakeRight(this Form form, int times)
        {
            for (var i = times; i > 0; i--)
            {
                form.Left += i;
                Thread.Sleep(times * i);
                form.Left -= i;
            }
        }

        public static void ShakeUp(this Form form, int times)
        {
            for (var i = times; i > 0; i--)
            {
                form.Top -= i;
                Thread.Sleep(times * i);
                form.Top += i;
            }
        }

        public static void ShakeDown(this Form form, int times)
        {
            for (var i = times; i > 0; i--)
            {
                form.Top += i;
                Thread.Sleep(times * i);
                form.Top -= i;
            }
        }
    }
}
