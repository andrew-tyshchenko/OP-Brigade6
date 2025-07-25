﻿using System.Windows.Forms;

namespace LuckyAceForm
{
    partial class AdminForm
    {
        public TabControl tabControl { get; private set; }

        private TabPage tabMatches;
        private TabPage tabUsers;
        private TabPage tabBets;
        private DataGridView dataGridViewMatches;
        private DataGridView dataGridViewUsers;
        private DataGridView dataGridViewBets;
        private Button btnAddMatch;
        private Button btnAddUser;
        private Button btnAddBet;

        private void InitializeComponent()
        {
            tabMatches = new TabPage();
            tabUsers = new TabPage();
            tabBets = new TabPage();
            dataGridViewMatches = new DataGridView();
            dataGridViewUsers = new DataGridView();
            dataGridViewBets = new DataGridView();
            btnAddMatch = new Button();
            btnAddUser = new Button();
            btnAddBet = new Button();
            button1 = new Button();
            label1 = new Label();
            textBox4 = new TextBox();
            label3 = new Label();
            label4 = new Label();
            listBox1 = new ListBox();
            dateTimePicker1 = new DateTimePicker();
            textBox5 = new TextBox();
            textBox6 = new TextBox();
            label5 = new Label();
            label6 = new Label();
            button2 = new Button();
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMatches).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBets).BeginInit();
            SuspendLayout();
            // 
            // tabMatches
            // 
            tabMatches.Location = new Point(0, 0);
            tabMatches.Name = "tabMatches";
            tabMatches.Size = new Size(200, 100);
            tabMatches.TabIndex = 0;
            // 
            // tabUsers
            // 
            tabUsers.Location = new Point(0, 0);
            tabUsers.Name = "tabUsers";
            tabUsers.Size = new Size(200, 100);
            tabUsers.TabIndex = 0;
            // 
            // tabBets
            // 
            tabBets.Location = new Point(0, 0);
            tabBets.Name = "tabBets";
            tabBets.Size = new Size(200, 100);
            tabBets.TabIndex = 0;
            // 
            // dataGridViewMatches
            // 
            dataGridViewMatches.ColumnHeadersHeight = 29;
            dataGridViewMatches.Location = new Point(0, 0);
            dataGridViewMatches.Name = "dataGridViewMatches";
            dataGridViewMatches.RowHeadersWidth = 51;
            dataGridViewMatches.Size = new Size(240, 150);
            dataGridViewMatches.TabIndex = 0;
            // 
            // dataGridViewUsers
            // 
            dataGridViewUsers.ColumnHeadersHeight = 29;
            dataGridViewUsers.Location = new Point(0, 0);
            dataGridViewUsers.Name = "dataGridViewUsers";
            dataGridViewUsers.RowHeadersWidth = 51;
            dataGridViewUsers.Size = new Size(240, 150);
            dataGridViewUsers.TabIndex = 0;
            // 
            // dataGridViewBets
            // 
            dataGridViewBets.ColumnHeadersHeight = 29;
            dataGridViewBets.Location = new Point(0, 0);
            dataGridViewBets.Name = "dataGridViewBets";
            dataGridViewBets.RowHeadersWidth = 51;
            dataGridViewBets.Size = new Size(240, 150);
            dataGridViewBets.TabIndex = 0;
            // 
            // btnAddMatch
            // 
            btnAddMatch.Location = new Point(0, 0);
            btnAddMatch.Name = "btnAddMatch";
            btnAddMatch.Size = new Size(75, 23);
            btnAddMatch.TabIndex = 0;
            // 
            // btnAddUser
            // 
            btnAddUser.Location = new Point(0, 0);
            btnAddUser.Name = "btnAddUser";
            btnAddUser.Size = new Size(75, 23);
            btnAddUser.TabIndex = 0;
            // 
            // btnAddBet
            // 
            btnAddBet.Location = new Point(0, 0);
            btnAddBet.Name = "btnAddBet";
            btnAddBet.Size = new Size(75, 23);
            btnAddBet.TabIndex = 0;
            // 
            // button1
            // 
            button1.BackgroundImage = LuckyAceApp.Properties.Resources._3;
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.ImageAlign = ContentAlignment.MiddleRight;
            button1.Location = new Point(105, 278);
            button1.Name = "button1";
            button1.Size = new Size(77, 76);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.SeaGreen;
            label1.Location = new Point(303, 50);
            label1.Name = "label1";
            label1.Size = new Size(41, 15);
            label1.TabIndex = 2;
            label1.Text = "Events";
            // 
            // textBox4
            // 
            textBox4.BackColor = Color.SeaGreen;
            textBox4.Location = new Point(65, 69);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(200, 26);
            textBox4.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.SeaGreen;
            label3.Location = new Point(65, 103);
            label3.Name = "label3";
            label3.Size = new Size(31, 15);
            label3.TabIndex = 7;
            label3.Text = "Date";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.SeaGreen;
            label4.Location = new Point(65, 50);
            label4.Name = "label4";
            label4.Size = new Size(39, 15);
            label4.TabIndex = 8;
            label4.Text = "Name";
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.SeaGreen;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(306, 69);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(242, 199);
            listBox1.TabIndex = 9;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CalendarMonthBackground = Color.SeaGreen;
            dateTimePicker1.CalendarTitleBackColor = Color.SeaGreen;
            dateTimePicker1.CalendarTitleForeColor = Color.Black;
            dateTimePicker1.Location = new Point(68, 121);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(197, 23);
            dateTimePicker1.TabIndex = 10;
            // 
            // textBox5
            // 
            textBox5.BackColor = Color.SeaGreen;
            textBox5.Location = new Point(68, 179);
            textBox5.Multiline = true;
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(197, 26);
            textBox5.TabIndex = 11;
            // 
            // textBox6
            // 
            textBox6.BackColor = Color.SeaGreen;
            textBox6.Location = new Point(68, 241);
            textBox6.Multiline = true;
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(197, 26);
            textBox6.TabIndex = 12;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.SeaGreen;
            label5.Location = new Point(68, 161);
            label5.Name = "label5";
            label5.Size = new Size(44, 15);
            label5.TabIndex = 13;
            label5.Text = "Team 1";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.SeaGreen;
            label6.Location = new Point(68, 223);
            label6.Name = "label6";
            label6.Size = new Size(44, 15);
            label6.TabIndex = 14;
            label6.Text = "Team 2";
            // 
            // button2
            // 
            button2.BackgroundImage = LuckyAceApp.Properties.Resources.image_2025_05_26_19_27_30;
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.Location = new Point(323, 278);
            button2.Name = "button2";
            button2.Size = new Size(84, 77);
            button2.TabIndex = 15;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackgroundImage = LuckyAceApp.Properties.Resources._5;
            button3.BackgroundImageLayout = ImageLayout.Stretch;
            button3.Location = new Point(443, 278);
            button3.Name = "button3";
            button3.Size = new Size(81, 76);
            button3.TabIndex = 16;
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click_1;
            // 
            // AdminForm
            // 
            BackgroundImage = LuckyAceApp.Properties.Resources._2;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(601, 374);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(textBox6);
            Controls.Add(textBox5);
            Controls.Add(dateTimePicker1);
            Controls.Add(listBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(textBox4);
            Controls.Add(label1);
            Controls.Add(button1);
            Name = "AdminForm";
            ((System.ComponentModel.ISupportInitialize)dataGridViewMatches).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBets).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        private Button button1;
        private Label label1;
        private TextBox textBox4;
        private Label label3;
        private Label label4;
        private ListBox listBox1;
        private DateTimePicker dateTimePicker1;
        private TextBox textBox5;
        private TextBox textBox6;
        private Label label5;
        private Label label6;
        private Button button2;
        private Button button3;
    }

}

