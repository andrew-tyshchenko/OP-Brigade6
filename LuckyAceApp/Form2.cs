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

        private void AddBet()
        {
            try
            {
                var selectedMatch = matchRepository.GetAll()[listBox1.SelectedIndex];
                var match = matchRepository.GetById(selectedMatch.Id);
                var allBets = betRepository.GetAll().ToList();
                int newId = allBets.Count > 0 ? allBets.Max(u => u.Id) + 1 : 1;
                int amount = Convert.ToInt32(numericUpDown1.Value);
                string team = "";
                if (radioButton1.Checked)
                {
                    team = match.Team1;
                }
                else if (radioButton2.Checked)
                {
                    team = "Draw";
                }
                else if (radioButton3.Checked)
                {
                    team = match.Team2;
                }

                Bet bet = new Bet(newId, user.Id, match.Id, amount, team);

                if (bet.Amount == 0 || bet.Amount.GetType() == typeof(decimal))
                {
                    MessageBox.Show("Amount of bet should be decimal nubmer bigger than zero.");
                    return;
                }

                var validationResults = ValidationService.Validate(bet);

                if (validationResults.Any())
                {
                    foreach (var error in validationResults)
                    {
                        MessageBox.Show($"- {error.ErrorMessage}");
                    }
                }
                else
                {
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
        private void button1_Click(object sender, EventArgs e)
        {
            AddBet();
        }
    }
}
