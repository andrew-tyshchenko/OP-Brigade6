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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static LuckyAceForm.User;

namespace LuckyAceForm
{
    public partial class Form4 : Form
    {
        private string storedHashedPassword;
        private UserRepository userRepository;
        public Form4()
        {
            InitializeComponent();
            userRepository = new UserRepository(new JsonStorage<User>("users.json"));
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
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

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);

            // Get next available ID
            var allUsers = userRepository.GetAll().ToList();
            int newId = allUsers.Count > 0 ? allUsers.Max(u => u.Id) + 1 : 1;

            // Create user with hashed password
            User user = new User(newId, username, hashedPassword);
            userRepository.Add(user);

            MessageBox.Show("Користувач зареєстрований!");
        }

        private void button1_Click(object sender, EventArgs e)
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
                // Use EnhancedVerify for consistency with EnhancedHashPassword
                if (BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
                {
                    MessageBox.Show("Успішний вхід!");
                    new AdminForm().ShowDialog();
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

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
