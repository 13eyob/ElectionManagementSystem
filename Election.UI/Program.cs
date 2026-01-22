using System;
using System.Windows.Forms;
using Election.UI.Forms;

namespace Election.UI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmLogin()); // ✅ FrmLogin should be startup form
        }
    }
}