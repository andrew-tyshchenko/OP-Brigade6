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
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static LuckyAceForm.User;


namespace LuckyAceForm
{
    public partial class AdminForm : Form
    {
        private readonly MatchRepository matchRepository;
        private readonly UserRepository userRepository;
        private readonly BetRepository betRepository;

        public AdminForm()
        {
            InitializeComponent();

            var db = new SQLiteDb();
            // Initialize repositories with JSON storage
            matchRepository = new MatchRepository(db);
            userRepository = new UserRepository(db);
            betRepository = new BetRepository(db);

            LoadMatches();
        }

        private void LoadMatches()
        {
            listBox1.Items.Clear();
            foreach (var match in matchRepository.GetAll())
            {
                listBox1.Items.Add($"{match.Name} {match.Team1} vs {match.Team2} on {match.Date.ToShortDateString()}");
            }
        }

        private void button1_Click(object sender, EventArgs e) // Add Match
        {
            try
            {
                DateTime date = dateTimePicker1.Value;
                string name = textBox4.Text;
                string team1 = textBox5.Text;
                string team2 = textBox6.Text;

                var allMatches = matchRepository.GetAll().ToList();
                int newId = allMatches.Count > 0 ? allMatches.Max(u => u.Id) + 1 : 1;

                Match match = new Match(newId, date, name, team1, team2);

                var validationResults = ValidationService.Validate(match);

                if (validationResults.Any())
                {
                    foreach (var error in validationResults)
                    {
                        MessageBox.Show($"- {error.ErrorMessage}");
                    }
                }
                else
                {
                    matchRepository.Add(match);
                }

                LoadMatches();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding match: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Delete Match
        {
            if (listBox1.SelectedIndex != -1)
            {
                try
                {
                    var selectedMatch = matchRepository.GetAll()[listBox1.SelectedIndex];
                    matchRepository.Delete(selectedMatch.Id);
                    LoadMatches();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting match: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e) // Update Match
        {
            if (listBox1.SelectedIndex != -1)
            {
                try
                {
                    DateTime date = dateTimePicker1.Value;
                    string name = textBox4.Text;
                    string team1 = textBox5.Text;
                    string team2 = textBox6.Text;

                    var matchToUpdate = matchRepository.GetAll()[listBox1.SelectedIndex];
                    matchToUpdate.Date = date;
                    matchToUpdate.Name = name;
                    matchToUpdate.Team1 = team1;
                    matchToUpdate.Team2 = team2;

                    var validationResults = ValidationService.Validate(matchToUpdate);

                    if (validationResults.Any())
                    {
                        foreach (var error in validationResults)
                        {
                            MessageBox.Show($"- {error.ErrorMessage}");
                        }
                    }
                    else
                    {
                        matchRepository.Update(matchToUpdate);
                        LoadMatches();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating match: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Other event handlers that don't need changes
        private void AdminForm_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                try
                {
                    var selectedMatch = matchRepository.GetAll()[listBox1.SelectedIndex];

                    // Populate the form fields with the selected match's data
                    textBox4.Text = selectedMatch.Name;
                    textBox5.Text = selectedMatch.Team1;
                    textBox6.Text = selectedMatch.Team2;
                    dateTimePicker1.Value = selectedMatch.Date;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading match details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label5_Click(object sender, EventArgs e) { }
        private void label5_Click_1(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }

}
