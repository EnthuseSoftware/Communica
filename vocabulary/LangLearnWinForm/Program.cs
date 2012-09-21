using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LangLearnLogic;

namespace LangLearn
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SignIn sgn = new SignIn();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmSignIn(sgn.UsersName()));
        }
    }
}
