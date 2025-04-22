using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LuckyAceForm
{
    public partial class Form4 : Form
    {
        private string storedHashedPassword;
        public Form4()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Username = textBox1.Text;
            if (string.IsNullOrEmpty(Username))
            {
                MessageBox.Show("Введіть ім'я користувача");
                return;
            }
            string password = textBox2.Text;
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пароль не може бути порожнім!");
                return;
            }

            storedHashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
            MessageBox.Show("Користувач зареєстрований!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string password = textBox2.Text;
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введіть пароль!");
                return;
            }

            if (BCrypt.Net.BCrypt.EnhancedVerify(password, storedHashedPassword))
            {
                MessageBox.Show("Успішний вхід!");
                new AdminForm().ShowDialog();
            }
            else
            {
                MessageBox.Show("Невірний пароль!");
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
