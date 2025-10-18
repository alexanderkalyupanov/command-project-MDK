using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandProject.Forms;

namespace CommandProject
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

            // Запуск формы входа
            LoginForm loginForm = new LoginForm();
            if(loginForm.ShowDialog() == DialogResult.OK)
            {
                // После успешной авторизации запускаем главное меню
                Application.Run(new Forms.MainMenu.MainMenu());
            }

            // Запуск главного меню сразу при старте приложения
            //Application.Run(new Forms.MainMenu.MainMenu());
        }
    }
}
