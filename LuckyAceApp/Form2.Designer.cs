using System.Windows.Forms;

namespace LuckyAceForm
{
    partial class MainForm
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
            label1 = new Label();
            button1 = new Button();
            listBox1 = new ListBox();
            numericUpDown1 = new NumericUpDown();
            label6 = new Label();
            label5 = new Label();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            label2 = new Label();
            label3 = new Label();
            radioButton3 = new RadioButton();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            listBox2 = new ListBox();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMatches).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBets).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            statusStrip1.SuspendLayout();
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(105, 194);
            label1.Name = "label1";
            label1.Size = new Size(41, 15);
            label1.TabIndex = 2;
            label1.Text = "Events";
            // 
            // button1
            // 
            button1.Location = new Point(263, 151);
            button1.Name = "button1";
            button1.Size = new Size(104, 23);
            button1.TabIndex = 0;
            button1.Text = "Submit Bet";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(0, 226);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(232, 244);
            listBox1.TabIndex = 9;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(105, 151);
            numericUpDown1.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(103, 23);
            numericUpDown1.TabIndex = 17;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(367, 69);
            label6.Name = "label6";
            label6.Size = new Size(44, 15);
            label6.TabIndex = 14;
            label6.Text = "Team 2";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(61, 69);
            label5.Name = "label5";
            label5.Size = new Size(44, 15);
            label5.TabIndex = 13;
            label5.Text = "Team 1";
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(33, 99);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(96, 19);
            radioButton1.TabIndex = 18;
            radioButton1.TabStop = true;
            radioButton1.Text = "Select Team 1";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(184, 99);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(86, 19);
            radioButton2.TabIndex = 19;
            radioButton2.TabStop = true;
            radioButton2.Text = "Select Draw";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(184, 26);
            label2.Name = "label2";
            label2.Size = new Size(84, 21);
            label2.TabIndex = 20;
            label2.Text = "Place a bet";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(211, 69);
            label3.Name = "label3";
            label3.Size = new Size(34, 15);
            label3.TabIndex = 21;
            label3.Text = "Draw";
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(343, 99);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(96, 19);
            radioButton3.TabIndex = 22;
            radioButton3.TabStop = true;
            radioButton3.Text = "Select Team 2";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 482);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(467, 22);
            statusStrip1.TabIndex = 25;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(54, 17);
            toolStripStatusLabel1.Text = "Balance: ";
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new Point(251, 226);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(216, 244);
            listBox2.TabIndex = 26;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(318, 194);
            label4.Name = "label4";
            label4.Size = new Size(49, 15);
            label4.TabIndex = 27;
            label4.Text = "My Bets";
            // 
            // MainForm
            // 
            ClientSize = new Size(467, 504);
            Controls.Add(label4);
            Controls.Add(listBox2);
            Controls.Add(statusStrip1);
            Controls.Add(radioButton3);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(numericUpDown1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(listBox1);
            Controls.Add(label1);
            Controls.Add(button1);
            Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize)dataGridViewMatches).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBets).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }
        private Label label1;
        private Button button1;
        private ListBox listBox1;
        private NumericUpDown numericUpDown1;
        private Label label6;
        private Label label5;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label2;
        private Label label3;
        private RadioButton radioButton3;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ListBox listBox2;
        private Label label4;
    }

}

