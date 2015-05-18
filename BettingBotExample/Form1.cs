using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JDCAPI;

namespace BettingBotExample
{
    public partial class Form1 : Form
    {
        jdInstance Bot = new jdInstance();

        double lastBet = 0;
        int Wins =0;
        int Losses =0;
        bool StopOnWin = false;
        bool Stop = true;
        bool High = true;
        int PreRollCount = 0;
        double Profit = 0;
        public Form1()
        {
            InitializeComponent();
            
            Bot.OnResult += Bot_OnResult;
        }

        delegate void dUpdateLable(Label _Lable, string Text);
        void UpdateLable(Label _Lable, string Text)
        {
            if (InvokeRequired)
            {
                Invoke(new dUpdateLable(UpdateLable), _Lable, Text);
                return;
            }
            else
            {
                _Lable.Text = Text;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Bot.Disconnect();
            base.OnClosing(e);
            
        }

        void Bot_OnResult(Result result, bool IsMine)
        {
            if (IsMine)
            {
                if (result.win)
                {
                    Wins++;
                    PreRollCount = 0;
                    if (nudPreroll.Value == 0m)
                        lastBet = (double)nudStart.Value;
                    else
                        lastBet = 0;
                    Stop = StopOnWin;
                }
                else
                {
                    Losses++;
                    PreRollCount++;
                    if (PreRollCount == (int)nudPreroll.Value)
                    {
                        lastBet = (double)nudStart.Value;
                    }
                    else if (PreRollCount > (int)nudPreroll.Value)
                    {
                        lastBet *= (double)nudMultiplier.Value;
                    }
                    else
                    {
                        lastBet = 0;
                    }
                }
                
                if (chkZigBets.Checked && (Wins+Losses)% nudZigBets.Value == 0 )
                {
                    High = !High;
                }
                if (chkZigLoss.Checked && Losses % nudZigLoss.Value == 0 && Losses !=0)
                {
                    High = !High;
                
                }
                if (chkZigWins.Checked && Wins % nudZigWins.Value == 0 && Wins != 0)
                {
                    High = !High;
                }
                Profit += double.Parse(result.this_profit);
                UpdateLable(lblWins, Wins + "/" + Losses);
                UpdateLable(lblBalance, Bot.Balance.ToString());
                UpdateLable(lblBets, (Wins + Losses).ToString());
                UpdateLable(lblProfit, Profit.ToString());
                if (Profit > (double)nudStopProfit.Value && chkStop.Checked)
                {
                    Stop = true;
                }
                if (!Stop)
                {
                    Bot.Bet((double)nudChance.Value, lastBet, High);
                }
                updatebets(String.Format("\r\n {4} --> bet {0} at {1} {2} and {3}", result.bet, result.chance, result.high?"High":"Low", (result.win?"won "+result.this_profit:"lost"), result.betid));
            }
        }
        delegate void dupdatebets(string bet);
        void updatebets(string bet)
        {
            if (InvokeRequired)
            {
                Invoke(new dupdatebets(updatebets), bet);
                return;
            }
            ChrtProfit.Series[0].Points.AddY(Profit);
            rtbBetLog.AppendText(bet);
        }

        void Start()
        {
            if (Stop)
            {
                if (nudPreroll.Value > 0)
                {
                    lastBet = 0;
                }
                else
                {
                    lastBet = (double)nudStart.Value;
                }
                Stop = false;
                Bot.Bet((double)nudChance.Value, lastBet, High);
            }
        }
        
        private void btnStartHigh_Click(object sender, EventArgs e)
        {
            High = true;
            Start();
        }

        private void btnStartLow_Click(object sender, EventArgs e)
        {
            High = false;
            Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop = true;
        }

        private void btnStopOnWin_Click(object sender, EventArgs e)
        {
            StopOnWin = true;
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            Bot.Connect(false, txtUsername.Text, txtPassword.Text, txtTFA.Text);
            lblBalance.Text = Bot.Balance.ToString();
            lblBets.Text = Bot.Bets.ToString();
            lblProfit.Text = Bot.Profit.ToString();
            lblWins.Text = Bot.Wins + "/" + Bot.Losses;
        }
    }
}
