namespace BettingBotExample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTFA = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.rtbBetLog = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.nudZigLoss = new System.Windows.Forms.NumericUpDown();
            this.nudZigWins = new System.Windows.Forms.NumericUpDown();
            this.nudZigBets = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblProfit = new System.Windows.Forms.Label();
            this.lblBets = new System.Windows.Forms.Label();
            this.lblWins = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkZigLoss = new System.Windows.Forms.CheckBox();
            this.chkZigWins = new System.Windows.Forms.CheckBox();
            this.chkZigBets = new System.Windows.Forms.CheckBox();
            this.nudStopProfit = new System.Windows.Forms.NumericUpDown();
            this.chkStop = new System.Windows.Forms.CheckBox();
            this.btnStopOnWin = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStartLow = new System.Windows.Forms.Button();
            this.btnStartHigh = new System.Windows.Forms.Button();
            this.nudPreroll = new System.Windows.Forms.NumericUpDown();
            this.nudMultiplier = new System.Windows.Forms.NumericUpDown();
            this.nudChance = new System.Windows.Forms.NumericUpDown();
            this.nudStart = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ChrtProfit = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudZigLoss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZigWins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZigBets)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStopProfit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPreroll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMultiplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChrtProfit)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtTFA);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.btnLogIn);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 118);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login";
            // 
            // txtTFA
            // 
            this.txtTFA.Location = new System.Drawing.Point(76, 61);
            this.txtTFA.Name = "txtTFA";
            this.txtTFA.Size = new System.Drawing.Size(100, 20);
            this.txtTFA.TabIndex = 6;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(76, 37);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 5;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(76, 13);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 4;
            // 
            // btnLogIn
            // 
            this.btnLogIn.Location = new System.Drawing.Point(101, 85);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(75, 23);
            this.btnLogIn.TabIndex = 3;
            this.btnLogIn.Text = "Log In";
            this.btnLogIn.UseVisualStyleBackColor = true;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 64);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "2FA Code:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Password:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Username:";
            // 
            // rtbBetLog
            // 
            this.rtbBetLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbBetLog.HideSelection = false;
            this.rtbBetLog.Location = new System.Drawing.Point(0, 393);
            this.rtbBetLog.Name = "rtbBetLog";
            this.rtbBetLog.Size = new System.Drawing.Size(1000, 106);
            this.rtbBetLog.TabIndex = 1;
            this.rtbBetLog.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.nudZigLoss);
            this.panel1.Controls.Add(this.nudZigWins);
            this.panel1.Controls.Add(this.nudZigBets);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.chkZigLoss);
            this.panel1.Controls.Add(this.chkZigWins);
            this.panel1.Controls.Add(this.chkZigBets);
            this.panel1.Controls.Add(this.nudStopProfit);
            this.panel1.Controls.Add(this.chkStop);
            this.panel1.Controls.Add(this.btnStopOnWin);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnStartLow);
            this.panel1.Controls.Add(this.btnStartHigh);
            this.panel1.Controls.Add(this.nudPreroll);
            this.panel1.Controls.Add(this.nudMultiplier);
            this.panel1.Controls.Add(this.nudChance);
            this.panel1.Controls.Add(this.nudStart);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 136);
            this.panel1.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(631, 97);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Losses";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(631, 73);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(31, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "Wins";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(631, 50);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(28, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "Bets";
            // 
            // nudZigLoss
            // 
            this.nudZigLoss.Location = new System.Drawing.Point(556, 94);
            this.nudZigLoss.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudZigLoss.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudZigLoss.Name = "nudZigLoss";
            this.nudZigLoss.Size = new System.Drawing.Size(63, 20);
            this.nudZigLoss.TabIndex = 21;
            this.nudZigLoss.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudZigWins
            // 
            this.nudZigWins.Location = new System.Drawing.Point(556, 71);
            this.nudZigWins.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudZigWins.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudZigWins.Name = "nudZigWins";
            this.nudZigWins.Size = new System.Drawing.Size(63, 20);
            this.nudZigWins.TabIndex = 20;
            this.nudZigWins.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudZigBets
            // 
            this.nudZigBets.Location = new System.Drawing.Point(556, 48);
            this.nudZigBets.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudZigBets.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudZigBets.Name = "nudZigBets";
            this.nudZigBets.Size = new System.Drawing.Size(63, 20);
            this.nudZigBets.TabIndex = 19;
            this.nudZigBets.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblProfit);
            this.groupBox2.Controls.Add(this.lblBets);
            this.groupBox2.Controls.Add(this.lblWins);
            this.groupBox2.Controls.Add(this.lblBalance);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(802, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(190, 108);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stats";
            // 
            // lblProfit
            // 
            this.lblProfit.AutoSize = true;
            this.lblProfit.Location = new System.Drawing.Point(84, 85);
            this.lblProfit.Name = "lblProfit";
            this.lblProfit.Size = new System.Drawing.Size(13, 13);
            this.lblProfit.TabIndex = 7;
            this.lblProfit.Text = "0";
            // 
            // lblBets
            // 
            this.lblBets.AutoSize = true;
            this.lblBets.Location = new System.Drawing.Point(84, 62);
            this.lblBets.Name = "lblBets";
            this.lblBets.Size = new System.Drawing.Size(13, 13);
            this.lblBets.TabIndex = 6;
            this.lblBets.Text = "0";
            // 
            // lblWins
            // 
            this.lblWins.AutoSize = true;
            this.lblWins.Location = new System.Drawing.Point(84, 39);
            this.lblWins.Name = "lblWins";
            this.lblWins.Size = new System.Drawing.Size(24, 13);
            this.lblWins.TabIndex = 5;
            this.lblWins.Text = "0/0";
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(84, 16);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(13, 13);
            this.lblBalance.TabIndex = 4;
            this.lblBalance.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(44, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Profit:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(47, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Bets:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Wins/Losses:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Balance:";
            // 
            // chkZigLoss
            // 
            this.chkZigLoss.AutoSize = true;
            this.chkZigLoss.Location = new System.Drawing.Point(446, 95);
            this.chkZigLoss.Name = "chkZigLoss";
            this.chkZigLoss.Size = new System.Drawing.Size(106, 17);
            this.chkZigLoss.TabIndex = 17;
            this.chkZigLoss.Text = "zig zag on every:";
            this.chkZigLoss.UseVisualStyleBackColor = true;
            // 
            // chkZigWins
            // 
            this.chkZigWins.AutoSize = true;
            this.chkZigWins.Location = new System.Drawing.Point(446, 72);
            this.chkZigWins.Name = "chkZigWins";
            this.chkZigWins.Size = new System.Drawing.Size(106, 17);
            this.chkZigWins.TabIndex = 16;
            this.chkZigWins.Text = "zig zag on every:";
            this.chkZigWins.UseVisualStyleBackColor = true;
            // 
            // chkZigBets
            // 
            this.chkZigBets.AutoSize = true;
            this.chkZigBets.Location = new System.Drawing.Point(446, 49);
            this.chkZigBets.Name = "chkZigBets";
            this.chkZigBets.Size = new System.Drawing.Size(106, 17);
            this.chkZigBets.TabIndex = 15;
            this.chkZigBets.Text = "zig zag on every:";
            this.chkZigBets.UseVisualStyleBackColor = true;
            // 
            // nudStopProfit
            // 
            this.nudStopProfit.DecimalPlaces = 8;
            this.nudStopProfit.Location = new System.Drawing.Point(556, 25);
            this.nudStopProfit.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudStopProfit.Name = "nudStopProfit";
            this.nudStopProfit.Size = new System.Drawing.Size(98, 20);
            this.nudStopProfit.TabIndex = 14;
            // 
            // chkStop
            // 
            this.chkStop.AutoSize = true;
            this.chkStop.Location = new System.Drawing.Point(446, 26);
            this.chkStop.Name = "chkStop";
            this.chkStop.Size = new System.Drawing.Size(92, 17);
            this.chkStop.TabIndex = 13;
            this.chkStop.Text = "Stop on profit:";
            this.chkStop.UseVisualStyleBackColor = true;
            // 
            // btnStopOnWin
            // 
            this.btnStopOnWin.Location = new System.Drawing.Point(710, 99);
            this.btnStopOnWin.Name = "btnStopOnWin";
            this.btnStopOnWin.Size = new System.Drawing.Size(75, 23);
            this.btnStopOnWin.TabIndex = 12;
            this.btnStopOnWin.Text = "Stop on win";
            this.btnStopOnWin.UseVisualStyleBackColor = true;
            this.btnStopOnWin.Click += new System.EventHandler(this.btnStopOnWin_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(710, 70);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 11;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStartLow
            // 
            this.btnStartLow.Location = new System.Drawing.Point(710, 41);
            this.btnStartLow.Name = "btnStartLow";
            this.btnStartLow.Size = new System.Drawing.Size(75, 23);
            this.btnStartLow.TabIndex = 10;
            this.btnStartLow.Text = "Start Low";
            this.btnStartLow.UseVisualStyleBackColor = true;
            this.btnStartLow.Click += new System.EventHandler(this.btnStartLow_Click);
            // 
            // btnStartHigh
            // 
            this.btnStartHigh.Location = new System.Drawing.Point(710, 12);
            this.btnStartHigh.Name = "btnStartHigh";
            this.btnStartHigh.Size = new System.Drawing.Size(75, 23);
            this.btnStartHigh.TabIndex = 9;
            this.btnStartHigh.Text = "Start High";
            this.btnStartHigh.UseVisualStyleBackColor = true;
            this.btnStartHigh.Click += new System.EventHandler(this.btnStartHigh_Click);
            // 
            // nudPreroll
            // 
            this.nudPreroll.Location = new System.Drawing.Point(329, 95);
            this.nudPreroll.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudPreroll.Name = "nudPreroll";
            this.nudPreroll.Size = new System.Drawing.Size(100, 20);
            this.nudPreroll.TabIndex = 8;
            // 
            // nudMultiplier
            // 
            this.nudMultiplier.DecimalPlaces = 8;
            this.nudMultiplier.Location = new System.Drawing.Point(329, 71);
            this.nudMultiplier.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudMultiplier.Name = "nudMultiplier";
            this.nudMultiplier.Size = new System.Drawing.Size(100, 20);
            this.nudMultiplier.TabIndex = 7;
            this.nudMultiplier.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // nudChance
            // 
            this.nudChance.DecimalPlaces = 8;
            this.nudChance.Location = new System.Drawing.Point(329, 47);
            this.nudChance.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudChance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.nudChance.Name = "nudChance";
            this.nudChance.Size = new System.Drawing.Size(100, 20);
            this.nudChance.TabIndex = 6;
            this.nudChance.Value = new decimal(new int[] {
            495,
            0,
            0,
            65536});
            // 
            // nudStart
            // 
            this.nudStart.DecimalPlaces = 8;
            this.nudStart.Location = new System.Drawing.Point(329, 23);
            this.nudStart.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            524288});
            this.nudStart.Name = "nudStart";
            this.nudStart.Size = new System.Drawing.Size(100, 20);
            this.nudStart.TabIndex = 5;
            this.nudStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            524288});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(276, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Pre rolls:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(245, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Chance to win:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(230, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Multiplier On Loss:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(258, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Starting Bet:";
            // 
            // ChrtProfit
            // 
            chartArea2.Name = "ChartArea1";
            this.ChrtProfit.ChartAreas.Add(chartArea2);
            this.ChrtProfit.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.ChrtProfit.Legends.Add(legend2);
            this.ChrtProfit.Location = new System.Drawing.Point(0, 136);
            this.ChrtProfit.Name = "ChrtProfit";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.ChrtProfit.Series.Add(series2);
            this.ChrtProfit.Size = new System.Drawing.Size(1000, 257);
            this.ChrtProfit.TabIndex = 3;
            this.ChrtProfit.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 499);
            this.Controls.Add(this.ChrtProfit);
            this.Controls.Add(this.rtbBetLog);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Text = "JDCAPI Sample Bot";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudZigLoss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZigWins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZigBets)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStopProfit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPreroll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMultiplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChrtProfit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtbBetLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblProfit;
        private System.Windows.Forms.Label lblBets;
        private System.Windows.Forms.Label lblWins;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkZigLoss;
        private System.Windows.Forms.CheckBox chkZigWins;
        private System.Windows.Forms.CheckBox chkZigBets;
        private System.Windows.Forms.NumericUpDown nudStopProfit;
        private System.Windows.Forms.CheckBox chkStop;
        private System.Windows.Forms.Button btnStopOnWin;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStartLow;
        private System.Windows.Forms.Button btnStartHigh;
        private System.Windows.Forms.NumericUpDown nudPreroll;
        private System.Windows.Forms.NumericUpDown nudMultiplier;
        private System.Windows.Forms.NumericUpDown nudChance;
        private System.Windows.Forms.NumericUpDown nudStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChrtProfit;
        private System.Windows.Forms.TextBox txtTFA;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudZigLoss;
        private System.Windows.Forms.NumericUpDown nudZigWins;
        private System.Windows.Forms.NumericUpDown nudZigBets;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
    }
}

