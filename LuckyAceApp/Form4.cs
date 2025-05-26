using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static LuckyAceForm.User;

namespace LuckyAceForm
{
    public partial class Form4 : Form // LoginForm
    {
        private UserRepository userRepository;
        public Form4()
        {
            InitializeComponent();
            var db = new SQLiteDb();
            userRepository = new UserRepository(db);
        }

        private void Register()
        {
            string username = textBox1.Text;
            if (string.IsNullOrEmpty(username))
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

            string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);

            var allUsers = userRepository.GetAll().ToList();
            int newId = allUsers.Count > 0 ? allUsers.Max(u => u.Id) + 1 : 1;

            User user = new User(newId, username, hashedPassword, 1000);
            userRepository.Add(user);

            MessageBox.Show("Користувач зареєстрований!");
        }

        private void Login()
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            User user = userRepository.GetAll().FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                MessageBox.Show("Користувача не знайдено!");
                return;
            }

            try
            {
                if (BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
                {
                    MessageBox.Show("Успішний вхід!");
                    if (user.Username == "admin")
                    {
                        new AdminForm().ShowDialog();
                    }
                    else
                    {
                        new MainForm(user).ShowDialog();
                    }

                }
                else
                {
                    MessageBox.Show("Невірний пароль!");
                }
            }
            catch (BCrypt.Net.SaltParseException)
            {
                MessageBox.Show("Помилка перевірки пароля. Можливо, дані пошкоджені.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Register();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login();
        }
    }
}
