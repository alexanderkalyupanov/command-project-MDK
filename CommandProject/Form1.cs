using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Создаём и показываем главное меню при старте приложения
            var mainMenu = new Forms.MainMenu.MainMenu();
            // Когда пользователь закроет MainMenu — закрываем и эту форму (приложение завершится)
            mainMenu.FormClosed += (s, args) => this.Close();
            mainMenu.Show();
            this.Hide();
        }
    }
}
