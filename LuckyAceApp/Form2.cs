using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static LuckyAceForm.User;


namespace LuckyAceForm
{
    public partial class MainForm : Form
    {
        private MatchRepository matchRepository;
        private UserRepository userRepository;
        private BetRepository betRepository;
        private User user;
        public MainForm(User logged_user)
        {
            InitializeComponent();
            user = logged_user;

            // Initialize repositories with JSON storage
            var db = new SQLiteDb();
            matchRepository = new MatchRepository(db);
            userRepository = new UserRepository(db);
            betRepository = new BetRepository(db);

            LoadMatches();
            LoadBallance();
            LoadBets();
        }

        private void LoadMatches()
        {
            listBox1.Items.Clear();
            foreach (var match in matchRepository.GetAll())
            {
                listBox1.Items.Add($"{match.Name} {match.Team1} vs {match.Team2} on {match.Date.ToShortDateString()}");
            }
        }
        private void LoadBets()
        {
            listBox2.Items.Clear();
            foreach (var bet in betRepository.GetAll())
            {
                listBox2.Items.Add($"{bet.Amount} on {bet.Team}");
            }
        }

        private void LoadBallance()
        {
            toolStripStatusLabel1.Text = "Ballance: " + user.Balance.ToString();
        }


        // Other event handlers that don't need changes
        private void AdminForm_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label5_Click_1(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label2_Click_1(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedMatch = matchRepository.GetAll()[listBox1.SelectedIndex];
                var match = matchRepository.GetById(selectedMatch.Id);
                var allBets = betRepository.GetAll().ToList();
                int newId = allBets.Count > 0 ? allBets.Max(u => u.Id) + 1 : 1;
                int amount = Convert.ToInt32(numericUpDown1.Value);
                string team = "";
                if (radioButton1.Checked) {
                    team = match.Team1; 
                }
                else if (radioButton2.Checked){
                    team = "Draw";
                } else if (radioButton3.Checked){
                    team = match.Team2;
                }

                Bet bet = new Bet(newId, user.Id, match.Id, amount, team);

                var validationResults = ValidationService.Validate(bet);

                if (validationResults.Any())
                {
                    Console.WriteLine("Validation failed. Errors:");
                    foreach (var error in validationResults)
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }
                }
                else
                {
                    Console.WriteLine("Validation passed. Object saved.");
                    betRepository.Add(bet);
                    user.Balance -= amount;
                    userRepository.Update(user);
                }

                LoadBallance();
                LoadBets();
            }
            catch
            {
                MessageBox.Show("Оберіть матч та команду!");
            }
        }
    }
}
