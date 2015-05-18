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
using Gma.QrCodeNet.Encoding.Windows.Forms;
using Gma.QrCodeNet.Encoding;
using System.IO;
using System.Text.RegularExpressions;
using WMPLib;

namespace JDCAPIExample
{
    public partial class Form1 : Form
    {
        
        jdInstance JD = new jdInstance();
        QrCodeImgControl qrDeposit = new QrCodeImgControl();
        Random RandObj = new Random();
        public Form1()
        {
            
            InitializeComponent();
            qrDeposit.Size = pictureBox1.Size;
            qrDeposit.Location = pictureBox1.Location;
            grbDeposit.Controls.Add(qrDeposit);
            JD.OnInit += JD_OnInit;
            JD.OnAddress += JD_OnAddress;
            JD.OnBadClientSeed += JD_OnBadClientSeed;
            JD.OnBalance += JD_OnBalance;
            JD.OnBet += JD_OnBet;
            JD.OnChat += JD_OnChat;
            JD.OnChatInfo += JD_OnChatInfo;
            JD.OnClientSeed += JD_OnClientSeed;
            JD.OnDismiss += JD_OnDismiss;
            JD.OnDivestError += JD_OnDivestError;
            JD.OnFormError += JD_OnFormError;
            JD.OnGaCodeOk += JD_OnGaCodeOk;
            JD.onGaDone += JD_onGaDone;
            JD.OnGaInfo += JD_OnGaInfo;
            JD.onInvest += JD_onInvest;
            JD.OnInvestError += JD_OnInvestError;
            JD.OnJDError += JD_OnJDError;
            JD.OnJDMessage += JD_OnJDMessage;
            JD.OnLoginError += JD_OnLoginError;
            JD.OnLossess += JD_OnWins;
            JD.OnMaxProfit += JD_OnMaxProfit;
            JD.OnNewClientSeed += JD_OnNewClientSeed;
            JD.OnNonce += JD_OnNonce;
            JD.OnOldChat += JD_OnOldChat;
            JD.OnPong += JD_OnPong;
            JD.OnResult += JD_OnResult;
            JD.OnRoll += JD_OnRoll;
            JD.OnSecretHash += JD_OnSecretHash;
            JD.OnStake += JD_OnStake;
            JD.OnTimeout += JD_OnTimeout;
            JD.OnTipReceived += JD_OnTipReceived;
            JD.OnWDAddress += JD_OnWDAddress;
            JD.OnWins += JD_OnWins;
            JD.LoginEnd += JD_LoginEnd;
            JD.OnHistory += JD_OnHistory;
            JD.OnOldBets += JD_OnOldBets;
            JD.OnSetupGa += JD_OnSetupGa;
            string hash = "";
            JD.logging = true;
            if (Directory.Exists("JDDesktop"))
            {
                if (File.Exists("JDDesktop\\settings"))
                {
                    using (StreamReader sr = new StreamReader("JDDesktop\\settings"))
                    {
                        hash = sr.ReadLine();
                    }
                }
            }
            else
            {
                Directory.CreateDirectory("JDDesktop");
                File.Create("JDDesktop\\settings");
            }
            if (hash!="")
            {
                JD.BeginConnect(false, hash);
            }
            else
            {
                JD.BeginConnect(false);
            }
        }

        void JD_OnSetupGa(SetupGa GaSetup)
        {
            SetupGA tmp = new SetupGA(GaSetup.secret,JD.uid, "");
            DialogResult tmpRes = tmp.ShowDialog();
            if (tmpRes == System.Windows.Forms.DialogResult.OK)
            {
                JD.SetupGaCode(tmp.Code);
            }
            else
            {
                
            }
        }

        void PlayAlarm()
        {
            if (File.Exists("whale.mp3"))
            {
                WindowsMediaPlayer player = new WindowsMediaPlayer();
                player.URL = "whale.mp3";
                player.controls.play();
            }
        }


        delegate void dOnOldBets(Bet bet, bool Ismine);
        void JD_OnOldBets(Bet bet, bool IsMine)
        {
            if (InvokeRequired)
            {
                Invoke(new dOnOldBets(JD_OnOldBets), bet, IsMine);
                return;
            }
            string target = (bet.high) ? ">" + (100.0 - double.Parse(bet.chance)) : "<" + bet.chance;
            if (IsMine)
            {
                updateMeLine();
                UpdateTextBox(txtNonce, JD.Nonce.ToString());
                dgvMy.Rows.Insert(0, bet.name + "(" + bet.uid + ")", jdInstance.ToDateTime(bet.date.ToString()), bet.betid, bet.lucky / 10000.0, target, bet.bet, bet.payout, bet.this_profit);
                if (dgvMy.Rows.Count > 0)
                {
                    if (bet.this_profit.Contains("-"))
                    {
                        dgvMy.Rows[0].DefaultCellStyle.BackColor = Color.Pink;
                    }
                    else
                    {
                        dgvMy.Rows[0].DefaultCellStyle.BackColor = Color.LightGreen;
                    }

                }
                while (dgvMy.Rows.Count > 100)
                    dgvMy.Rows.RemoveAt(100);
                if (JD.Settings.allbetsme)
                {
                    dgvAll.Rows.Insert(0, bet.name + "(" + bet.uid + ")", jdInstance.ToDateTime(bet.date.ToString()), bet.betid, bet.lucky / 10000.0, target, bet.bet, bet.payout, bet.this_profit);
                    if (bet.this_profit.Contains("-"))
                    {
                        dgvAll.Rows[0].DefaultCellStyle.BackColor = Color.Pink;
                    }
                    else
                    {
                        dgvAll.Rows[0].DefaultCellStyle.BackColor = Color.LightGreen;
                    }

                    while (dgvAll.Rows.Count > 100)
                        dgvAll.Rows.RemoveAt(100);
                }

            }
            else
            {
                string[] stmp = Regex.Split(JD.Settings.watch_player, @"\D+");
                if ((JD.Settings.min_risk <= double.Parse(bet.bet) || JD.Settings.min_change <= (double.Parse(bet.this_profit) < 0 ? -double.Parse(bet.this_profit) : double.Parse(bet.this_profit))) &&
                   (Regex.Split(JD.Settings.watch_player, @"\D+").Contains(bet.uid) || Regex.Split(JD.Settings.watch_player, @"\D+")[0] == "" || Regex.Split(JD.Settings.watch_player, @"\D+")[0] == "0"))
                {
                    dgvAll.Rows.Insert(0, bet.name + "(" + bet.uid + ")", jdInstance.ToDateTime(bet.date.ToString()), bet.betid, bet.lucky / 10000.0, target, bet.bet, bet.payout, bet.this_profit);
                    if (bet.this_profit.Contains("-"))
                    {
                        dgvAll.Rows[0].DefaultCellStyle.BackColor = Color.Pink;
                    }
                    else
                    {
                        dgvAll.Rows[0].DefaultCellStyle.BackColor = Color.LightGreen;
                    }

                    while (dgvAll.Rows.Count > 100)
                        dgvAll.Rows.RemoveAt(100);
                }

                if ((JD.Settings.chat_min_risk <= double.Parse(bet.bet) || JD.Settings.chat_min_change <= (double.Parse(bet.this_profit) < 0 ? -double.Parse(bet.this_profit) : double.Parse(bet.this_profit))) && (Regex.Split(JD.Settings.chat_watch_player, @"\D+").Contains(bet.uid) || Regex.Split(JD.Settings.chat_watch_player, @"\D+")[0] == "" || Regex.Split(JD.Settings.chat_watch_player, @"\D+")[0] == "0"))
                {
                    /*dgvAll.Rows.Insert(0, result.betid, result.date, result.bet, result.high, result.chance, result.lucky / 10000.0, result.this_profit, result.nonce);
                    while (dgvAll.Rows.Count > 100)
                        dgvAll.Rows.RemoveAt(100);*/
                    string win = (((bet.high ? bet.lucky / 10000.0 < 100.0 - double.Parse(bet.chance) : bet.lucky / 10000.0 > 100.0 - double.Parse(bet.chance))) ? "lost" : "won " + bet.this_profit + " CLAM");
                    string msg = string.Format("{0:hh:MM:ss} *** ({1}) <{2}> [#{3}] bet {4} CLAM at {5}% and {6} ***", jdInstance.ToDateTime(bet.date.ToString()), bet.uid, bet.name, bet.betid, bet.bet, bet.chance, win);
                }
            }
        }

        bool Init = false;
        void JD_OnInit(init Init)
        {
            EnableControll(this, true);
            this.Init = true;
            try
            {
                using (StreamWriter sw = new StreamWriter("JDDesktop\\settings"))
                { sw.WriteLine(JD.SecretHash); }
            }
            catch
            {

            }
            JD.Deposit();
            UpdateLable(lblLogin, "Logged Id");
            UpdateTextBox(txtUID, JD.uid);
            UpdateTextBox(txtYourUsername, JD.Username);
            //check em address
            UpdateTextBox(txtEMAddress, JD.WDAddress);
            //email ---
            UpdateTextBox(txtDisplayName, JD.Name);
            //google auth ---
            UpdateTextBox(txtAllBetsID, JD.Settings.watch_player);
            UpdateTextBox(txtChatBetsID, JD.Settings.chat_watch_player);
            UpdateNud(nudDelay, (decimal)JD.Settings.roll_delay);
            UpdateNud(nudMaxX2, (decimal)JD.Settings.max_double);
            UpdateNud(nudAllRisk, (decimal)JD.Settings.min_risk);
            UpdateNud(nudAllWin, (decimal)JD.Settings.min_change);
            UpdateNud(nudChatRisk, (decimal)JD.Settings.chat_min_risk);
            UpdateNud(nudChatWin, (decimal)JD.Settings.chat_min_change);
            updateCheckbox(chkAutoInvest, JD.Settings.autoinvest);
            updateCheckbox(chkShowAll, JD.Settings.allbetsme);
            updateCheckbox(chkMuteChat, JD.Settings.mutechat);
            updateCheckbox(chkShortcuts, JD.Settings.shortcuts);
            updateCheckbox(chkStake, JD.Settings.chatstake);
            updateCheckbox(chkAlarm, JD.Settings.alarm);
            UpdateNud(nudChance, (decimal)JD.Chance);

            UpdateTextBox(txtHash, JD.shash);
            UpdateTextBox(txtClient, JD.seed);
            UpdateTextBox(txtNonce, JD.Nonce.ToString());


            UpdateTextBox(txtGoogleAuth, (JD.ga.active ? "Enabled" : "Disabled") + " Failures:" + JD.ga.failures);

            updateSiteLine();
            updateMeLine();   
            updateStats();
            this.Init = false;
        }

        private void updateMeLine()
        {
            UpdateLable(lblMeInvestProfit, JD.Invest_pft.ToString());
            UpdateLable(lblMeInvested, JD.Investment.ToString());
            UpdateLable(lblMeBets, JD.Bets.ToString());
            UpdateLable(lblMeWagered, JD.Wagered.ToString());
            UpdateLable(lblMeProfit, (JD.Profit).ToString());
            UpdateLable(lblBalance, JD.Balance.ToString());
            UpdateLable(lblWinsLosses, JD.Wins + "/" + JD.Losses);
        }

        void updateSiteLine()
        {
            UpdateLable(lblSiteInvested, JD.Bankroll.ToString());
            UpdateLable(lblSiteProfitPercentage, ((-JD.Stats.profit / JD.Stats.purse) * 100.0).ToString());
            UpdateLable(lblSiteBets, JD.Stats.bets.ToString());
            UpdateLable(lblSiteWagered, JD.Stats.wagered.ToString());
            UpdateLable(lblSiteProfit, (-JD.Stats.profit).ToString());
            UpdateLable(lblMaxProfit, JD.MaxProfit.ToString());
        }

        void updateStats()
        {
            UpdateLable(lblStatsBets, JD.Bets.ToString());
            UpdateLable(lblStatsWins, JD.Wins.ToString());
            UpdateLable(lblStatsLosses, JD.Losses.ToString());
            UpdateLable(lblStatsLuck, JD.Luck);
            UpdateLable(lblStatsWagered, JD.Wagered.ToString());
            UpdateLable(lblStatsBetProfit, (JD.Profit).ToString());
            UpdateLable(lblStatsINvestedOn, JD.Investment.ToString());
            UpdateLable(lblStatsInvestedOff, JD.Offsite.ToString());
            UpdateLable(lblStatsInvestedTotal, (JD.Offsite + JD.Investment).ToString());
            UpdateLable(lblStatsBankrollProfit, (JD.Invest_pft-JD.stake_profit).ToString());
            UpdateLable(lblStatsStakingProfit, JD.stake_profit.ToString());
            UpdateLable(lblStatsInvestProfit, (JD.Invest_pft).ToString());
            if (JD.Offsite+JD.Investment !=0)
            UpdateLable(lblStatsMutliplier, (JD.Investment / (JD.Offsite + JD.Investment)).ToString());


            UpdateLable(lblStatsSiteBets, JD.Stats.bets.ToString());
            UpdateLable(lblStatsSiteWins, JD.Stats.wins.ToString());
            UpdateLable(lblStatsSiteLosses, JD.Stats.losses.ToString());
            UpdateLable(lblStatsSiteLuck, JD.Stats.luck.ToString());
            UpdateLable(lblStatsSiteWagered, JD.Stats.wagered.ToString());
            UpdateLable(lblStatsSiteUp, JD.Stats.profit.ToString());
            UpdateLable(lblStatsSiteProfit, ((-JD.Stats.profit / JD.Bankroll) * 100.0).ToString());
            UpdateLable(lblStatsSiteInvested, JD.Bankroll.ToString());
        }

        delegate void dOnHistory(History History);
        void JD_OnHistory(History History)
        {
            if (InvokeRequired)
            {
                Invoke(new dOnHistory(JD_OnHistory), History);
                return;
            }
            else
            {
                switch (History.Type)
                {
                    case HistoryType.commission: dgvCommission.Rows.Clear(); foreach (CommissionHistory c in History.commission) { dgvCommission.Rows.Insert(0, c.eid, c.date, c.profit, c.commission, c.reason); }; break;
                    case HistoryType.deposit: dgvDeposit.Rows.Clear(); foreach (DepositHistory d in History.deposit) { dgvDeposit.Rows.Insert(0, d.eid, d.date, d.amount, d.note); }; break;
                    case HistoryType.invest: dgvInvest.Rows.Clear(); foreach (InvestHistory i in History.invest) 
                    { 
                        string what = (i.amount >0)? "invest "+i.amount: "divest "+(-i.amount);
                        string perc = string.Format("from {0:0.000000}\nto {1:0.000000}", i.share_old, i.share_new);
                        decimal prft = i.base_new-i.prin_new;
                        string invst = string.Format("from {0:0.00000000}\nto {1:0.00000000}", prft+ i.prin_old, prft+ i.prin_new);
                        
                        dgvInvest.Rows.Insert(0, i.eid, i.date, what, i.commission, perc, invst, prft, i.base_new); 
                    } break;

                    case HistoryType.withdraw: dgvWithdrawals.Rows.Clear(); foreach (WithdrawHistory w in History.withdraw) { dgvWithdrawals.Rows.Insert(0, w.eid, w.date, w.amount, w.address, w.txid); }; break;
                }
            }
        }

        void JD_LoginEnd(bool Connected)
        {
            

            
            
        }

        delegate void dUpdateCheckBox(CheckBox chk, bool Checked);
        delegate void dUpdateNud(NumericUpDown nud, decimal Value);
        delegate void dEnableControll(Control Controll, bool Enabled);
        delegate void dVisibleControll(Control Controll, bool Visble);

        void updateCheckbox(CheckBox chk, bool Checked)
        {
            if (InvokeRequired)
            {
                Invoke( new dUpdateCheckBox(updateCheckbox), chk, Checked);
                return;
            }
            else
            {
                chk.Checked = Checked;
            }
        }
        void UpdateNud(NumericUpDown nud, decimal Value)
        {
            if (InvokeRequired)
            {
                Invoke(new dUpdateNud(UpdateNud), nud, Value);
                return;
            }
            else
            {
                nud.Value = Value;
            }
        }
        void EnableControll(Control Controll, bool Enabled)
        {
            if (InvokeRequired)
            {
                Invoke(new dEnableControll(EnableControll), Controll, Enabled);
                return;
            }
            else
            {
                Controll.Enabled = Enabled;
            }
        }
        void VisibleControll(Control Controll, bool Visible)
        {
            if (InvokeRequired)
            {
                Invoke(new dVisibleControll(VisibleControll), Controll, Visible);
                return;
            }
            else
            {
                Controll.Visible = Enabled;
            }
        }
        delegate void dUpdateLable(Label Lable, string Message);
        void UpdateLable(Label Lable, string Message)
        { 
            if (InvokeRequired)
            {
                Invoke(new dUpdateLable(UpdateLable), Lable, Message);
                return;
            }
            else
            {
                Lable.Text = Message;
            }
        }

        delegate void dUpdateTextBox(TextBox _TextBox, string Message);
        void UpdateTextBox(TextBox _TextBox, string Message)
        {
            if (InvokeRequired)
            {
                Invoke(new dUpdateTextBox(UpdateTextBox), _TextBox, Message);
                return;
            }
            else
            {
                _TextBox.Text = Message;
            }
        }

        void JD_OnWins(long Wins)
        {
            
            UpdateLable(lblWinsLosses, JD.Wins + "/" + JD.Losses);
            UpdateLable(lblMeBets, JD.Bets.ToString());
            UpdateLable(lblStatsBets, JD.Bets.ToString());
            UpdateLable(lblStatsWins, JD.Wins.ToString());
            UpdateLable(lblStatsLosses, JD.Losses.ToString());
            
        }

        void JD_OnWDAddress(string WDaddress)
        {
            
        }

        void JD_OnTipReceived(ReceivedTip Tip)
        {
            
        }

        void JD_OnTimeout()
        {
            JD.Reconnect();
        }

        void JD_OnStake(Stake Staked)
        {
            UpdateLable(lblSiteInvested, Staked.bankroll.ToString());
            
            if (JD.Settings.chatstake)
            {
                AppendMessage(string.Format("INFO: we just staked {0} CLAM (total = {1} CLAM; your total = {2} CLAM)", Staked.stake, Staked.total, Staked.stake_pft, Staked.date));
            }
        }

        void JD_OnSecretHash(string secretHash)
        {
            UpdateTextBox(txtHash, secretHash);
        }

        void JD_OnRoll(Roll roll)
        {
            //Show form with bet result
        }

        delegate void dJDResult(Result result, bool IsMine);
        void JD_OnResult(Result result, bool IsMine)
        {
            
            if (InvokeRequired)
            {
                Invoke(new dJDResult(JD_OnResult), result, IsMine);
                return;
            }
            else
            {
                dgvAll.DataBindings.Clear();
                dgvMy.DataBindings.Clear();
                string target = (result.high)?">" +(100.0-double.Parse(result.chance)):"<"+result.chance;
                if (IsMine)
                {
                    updateMeLine();
                    UpdateTextBox(txtNonce, JD.Nonce.ToString());
                    dgvMy.Rows.Insert(0, result.name + "(" + result.uid + ")", jdInstance.ToDateTime(result.date.ToString()), result.betid, result.lucky / 10000.0, target, result.bet, result.payout, result.this_profit);
                    if (dgvMy.Rows.Count > 0)
                    {
                        if (result.this_profit.Contains("-"))
                        {
                            dgvMy.Rows[0].DefaultCellStyle.BackColor = Color.Pink;
                        }
                        else
                        {
                            dgvMy.Rows[0].DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                        
                    }
                    while (dgvMy.Rows.Count > 100)
                        dgvMy.Rows.RemoveAt(100);
                    if ( JD.Settings.allbetsme )
                    {
                        dgvAll.Rows.Insert(0, result.name + "(" + result.uid + ")", jdInstance.ToDateTime(result.date.ToString()), result.betid, result.lucky / 10000.0, target, result.bet, result.payout, result.this_profit);
                        if (result.this_profit.Contains("-"))
                        {
                            dgvAll.Rows[0].DefaultCellStyle.BackColor = Color.Pink;
                        }
                        else
                        {
                            dgvAll.Rows[0].DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                        
                        while (dgvAll.Rows.Count > 100)
                            dgvAll.Rows.RemoveAt(100);
                    }
                    
                }
                else
                {
                    string[] stmp = Regex.Split(JD.Settings.watch_player, @"\D+");
                    if ((JD.Settings.min_risk <= double.Parse(result.bet) || JD.Settings.min_change <= (double.Parse(result.this_profit) < 0 ? -double.Parse(result.this_profit) : double.Parse(result.this_profit))) &&
                       (Regex.Split(JD.Settings.watch_player, @"\D+").Contains(result.uid) || Regex.Split(JD.Settings.watch_player, @"\D+")[0] == "" || Regex.Split(JD.Settings.watch_player, @"\D+")[0] == "0"))  
                    {
                        dgvAll.Rows.Insert(0, result.name + "(" + result.uid + ")", jdInstance.ToDateTime(result.date.ToString()), result.betid, result.lucky / 10000.0, target, result.bet, result.payout, result.this_profit);
                        if (result.this_profit.Contains("-"))
                        {
                            dgvAll.Rows[0].DefaultCellStyle.BackColor = Color.Pink;
                        }
                        else
                        {
                            dgvAll.Rows[0].DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                        
                        while (dgvAll.Rows.Count > 100)
                            dgvAll.Rows.RemoveAt(100);
                    }

                    if ((JD.Settings.chat_min_risk <= double.Parse(result.bet) || JD.Settings.chat_min_change <= (double.Parse(result.this_profit) < 0 ? -double.Parse(result.this_profit) : double.Parse(result.this_profit))) &&
                       (Regex.Split(JD.Settings.chat_watch_player, @"\D+").Contains(result.uid) || Regex.Split(JD.Settings.chat_watch_player, @"\D+")[0] == "" || Regex.Split(JD.Settings.chat_watch_player, @"\D+")[0] == "0"))
                    {
                        /*dgvAll.Rows.Insert(0, result.betid, result.date, result.bet, result.high, result.chance, result.lucky / 10000.0, result.this_profit, result.nonce);
                        while (dgvAll.Rows.Count > 100)
                            dgvAll.Rows.RemoveAt(100);*/
                        string win = (((result.high? result.lucky / 10000.0 < 100.0-double.Parse(result.chance) : result.lucky / 10000.0 > 100.0-double.Parse(result.chance)))?"lost":"won "+result.profit+" CLAM");
                        string msg = string.Format("{0:hh:MM:ss} *** ({1}) <{2}> [#{3}] bet {4} CLAM at {5}% and {6} ***", jdInstance.ToDateTime(result.date.ToString()), result.uid, result.name, result.betid, result.bet, result.chance, win);
                    }
                    if (JD.Settings.alarm && double.Parse(result.bet)>=100)
                    {
                        PlayAlarm();
                    }
                }
            }
            updateStats();
            
            updateSiteLine();
        }

        void JD_OnPong()
        {
        }

        void JD_OnOldChat(Chat chat)
        {
            string AppendedMessages = chat.Date.ToShortTimeString() + " (" + chat.UID + ") ";
            if (chat.Type == "pm")
            {
                AppendedMessages += "<" + chat.User + "> -> ";
                if (chat.UID == JD.uid)
                {
                    AppendedMessages += "PM";
                }
                else
                {
                    AppendedMessages += "ME";
                }

            }
            else if (chat.Type == "me")
            {
                AppendedMessages += "* " + chat.User;
            }
            else if (chat.Type == "mod")
            {
                AppendedMessages += "<" + chat.User + "> -> <mods>";
            }
            else
            {
                AppendedMessages += "<" + chat.User + ">";
            }
            AppendedMessages += " " + chat.Message;
            AppendMessage(AppendedMessages);
        }

        void JD_OnNonce(int Nonce)
        {
            UpdateTextBox(txtNonce, Nonce.ToString());
        }

        
        void JD_OnNewClientSeed(SeedInfo SeedInfo)
        {
            VisibleControll(pnlRandomize, true);
            UpdateTextBox(txtRandomizeNewHash, SeedInfo.NewServerHash);
            UpdateTextBox(txtHash, SeedInfo.NewServerHash);
            UpdateTextBox(txtRandomizeOld, SeedInfo.OldServerSeed);
            UpdateTextBox(txtRandomizeOldClient, SeedInfo.OldClientSeed);
            UpdateTextBox(txtRandomizeOldHash, SeedInfo.OldServerHash);
            UpdateTextBox(txtRandomizeOldNonce, SeedInfo.TotalRolls);
            string s = "";
            for (int i = 0; i < 24; i++ )
            {
                s += RandObj.Next(0, 9).ToString();
            }
                UpdateTextBox(txtRandomizenewClient, s);
        }

        void JD_OnMaxProfit(decimal MaxProfit)
        {
            UpdateLable(lblMaxProfit, MaxProfit.ToString());
        }

        void JD_OnLoginError(string Error)
        {
            if (Error == "This account requires a username and password" || Error == "Incorrect Password")
            {
                Login LoginForm = new Login();
                DialogResult t = LoginForm.ShowDialog();
                if (t == DialogResult.OK)
                {
                    JD.BeginConnect(false, LoginForm.user, LoginForm.pass, LoginForm.code);
                }
                else
                {
                    JD.BeginConnect(false);
                }

            }

            UpdateLable(lblLogin, Error);
        }

        void JD_OnJDMessage(string Message)
        {
        }

        void JD_OnJDError(string Error)
        {
            MessageBox.Show(Error);
        }

        void JD_OnInvestError(string InvestError)
        {
            MessageBox.Show(InvestError);
        }

        void JD_onInvest(Invest InvestResult)
        {
            UpdateLable(lblInvestInvested, InvestResult.Amount.ToString());
            UpdateLable(lblInvestPercentage, InvestResult.Percentage.ToString());
            UpdateLable(lblMeInvested, InvestResult.Amount.ToString());
            UpdateLable(lblMeInvestProfit, InvestResult.Profit.ToString());
            UpdateLable(lblStatsINvestedOn, InvestResult.Amount.ToString());
            UpdateLable(lblStatsInvestedTotal, (InvestResult.Amount + InvestResult.Offsite).ToString());
            UpdateLable(lblStatsInvestProfit, InvestResult.Profit.ToString());
            UpdateLable(lblStatsInvestedOff, InvestResult.Offsite.ToString());
        }

        void JD_OnGaInfo(GoogleAuthSettings Setup)
        {
            
        }

        void JD_onGaDone()
        {
            //Close Ga Form
        }

        void JD_OnGaCodeOk(GoogleAuthSettings GASettings)
        {
            EditGa GA = new EditGa(GASettings.flags);
            DialogResult tmp = GA.ShowDialog();
            if (tmp == System.Windows.Forms.DialogResult.Yes)
            {
                JD.DoneEditGa(GA.code, GA.flags);
            }
            else if (tmp == System.Windows.Forms.DialogResult.No)
            {
                JD.DisableGa(GA.code);
            }
            else
            {

            }
        }

        void JD_OnFormError(string Error)
        {
            MessageBox.Show(Error);
        }

        void JD_OnDivestError(string DivestError)
        {
            MessageBox.Show(DivestError);
        }

        void JD_OnDismiss()
        {
        }

        void JD_OnClientSeed(string Seed)
        {
            UpdateTextBox(txtRandomizenewClient, Seed);
            UpdateTextBox(txtClient, Seed);
        }

        void JD_OnChatInfo(string Message)
        {
            AppendMessage(Message);
        }

        delegate void dAppendMessage(string Message);
        void AppendMessage(string Message)
        {
            if (InvokeRequired)
            {
                Invoke(new dAppendMessage(AppendMessage), Message);
                return;
            }
            else
            {
                rtbMessages.AppendText(Message + "\r\n");
            }
        }
        void JD_OnChat(Chat chat)
        {
            
            string AppendedMessages = chat.Date.ToShortTimeString() + " (" + chat.UID + ") ";
            if (chat.Type == "pm")
            {
                AppendedMessages += "<" + chat.User + "> -> ";
                if (chat.UID == JD.uid)
                {
                    AppendedMessages += "PM";
                }
                else
                {
                    AppendedMessages += "ME";
                }

            }
            else if (chat.Type == "me")
            {
                AppendedMessages += "* " + chat.User;
            }
            else if (chat.Type == "mod")
            {
                AppendedMessages += "<" + chat.User + "> -> <mods>";
            }
            else
            {
                AppendedMessages += "<" + chat.User + ">";
            }
            AppendedMessages += " " + chat.Message;
            AppendMessage(AppendedMessages);
        }

        void JD_OnBet(Bet bet, bool IsMine)
        {
        }

        void JD_OnBalance(decimal Balance)
        {
            UpdateLable(lblBalance, Balance.ToString());
        }

        void JD_OnBadClientSeed(string Message)
        {
            MessageBox.Show(Message);
        }

        void JD_OnAddress(Address Address)
        {
            
            if (qrDeposit.IsLocked)
                qrDeposit.UnLock();
            
            qrDeposit.Text = "clam:" + Address.DepositAddress;
            qrDeposit.QuietZoneModule = Gma.QrCodeNet.Encoding.Windows.Render.QuietZoneModules.Four;
            
            BitMatrix qrMatrix = qrDeposit.GetQrMatrix();
            if (qrDeposit.IsFreezed)
                qrDeposit.UnFreeze();
            qrDeposit.Freeze();
            UpdateTextBox(txtDeposit, Address.DepositAddress);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            JD.Chat(txtMessage.Text);
            txtMessage.Text = "";
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            JD.Login(txtLoginUsername.Text, txtLoginPassword.Text, txtLoginGa.Text);
            
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            UserAccount tmp = new UserAccount(true);
            if (tmp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                JD.SetupAccount(tmp.user, tmp.pass);
            }
            
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            UserAccount tmp = new UserAccount(false);
            if (tmp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                JD.ChangePassword(tmp.user, tmp.pass);
            }
        }

        private void txtYourUsername_Leave(object sender, EventArgs e)
        {
            
        }

        private void txtEMAddress_Leave(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_String.Emergency_Address, txtEMAddress.Text);
        }

        private void txtEmailAddress_Leave(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_String.Email, txtEmailAddress.Text);
        }

        private void txtDisplayName_Leave(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetName(txtDisplayName.Text);
        }

        private void nudDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Numeric.Roll_Delay, nudDelay.Value);
        }

        private void nudMaxX2_ValueChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Numeric.Max_Double, nudMaxX2.Value);
        }

        private void nudAllRisk_ValueChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Numeric.Minimum_Risk, nudAllRisk.Value);
        }

        private void nudAllWin_ValueChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Numeric.Minimum_Change, nudAllWin.Value);
        }

        private void txtAllBetsID_TextChanged(object sender, EventArgs e)
        {

        }

        private void nudChatRisk_ValueChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Numeric.Chat_Minimum_Risk, nudChatRisk.Value);
        }

        private void nudChatWin_ValueChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Numeric.Chat_Minimum_Change, nudChatWin.Value);
        }

        private void txtChatBetsID_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkAutoInvest_CheckedChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Boolean.AutoInvest, chkAutoInvest.Checked);
        }

        private void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Boolean.AllBetsMe, chkShowAll.Checked);
        }

        private void chkMuteChat_CheckedChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Boolean.MuteChat, chkMuteChat.Checked);
        }

        private void chkShortcuts_CheckedChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Boolean.Shortcuts, chkShortcuts.Checked);
        }

        private void chkStake_CheckedChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Boolean.Chatstake, chkStake.Checked);
        }

        private void chkAlarm_CheckedChanged(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_Boolean.Alarm, chkAlarm.Checked);
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            JD.Disconnect();
            JD = new jdInstance();
            JD.OnInit += JD_OnInit;
            JD.OnAddress += JD_OnAddress;
            JD.OnBadClientSeed += JD_OnBadClientSeed;
            JD.OnBalance += JD_OnBalance;
            JD.OnBet += JD_OnBet;
            JD.OnChat += JD_OnChat;
            JD.OnChatInfo += JD_OnChatInfo;
            JD.OnClientSeed += JD_OnClientSeed;
            JD.OnDismiss += JD_OnDismiss;
            JD.OnDivestError += JD_OnDivestError;
            JD.OnFormError += JD_OnFormError;
            JD.OnGaCodeOk += JD_OnGaCodeOk;
            JD.onGaDone += JD_onGaDone;
            JD.OnGaInfo += JD_OnGaInfo;
            JD.onInvest += JD_onInvest;
            JD.OnInvestError += JD_OnInvestError;
            JD.OnJDError += JD_OnJDError;
            JD.OnJDMessage += JD_OnJDMessage;
            JD.OnLoginError += JD_OnLoginError;
            JD.OnLossess += JD_OnWins;
            JD.OnMaxProfit += JD_OnMaxProfit;
            JD.OnNewClientSeed += JD_OnNewClientSeed;
            JD.OnNonce += JD_OnNonce;
            JD.OnOldChat += JD_OnOldChat;
            JD.OnPong += JD_OnPong;
            JD.OnResult += JD_OnResult;
            JD.OnRoll += JD_OnRoll;
            JD.OnSecretHash += JD_OnSecretHash;
            JD.OnStake += JD_OnStake;
            JD.OnTimeout += JD_OnTimeout;
            JD.OnTipReceived += JD_OnTipReceived;
            JD.OnWDAddress += JD_OnWDAddress;
            JD.OnWins += JD_OnWins;
            JD.LoginEnd += JD_LoginEnd;
            JD.OnHistory += JD_OnHistory;
            JD.OnOldBets += JD_OnOldBets;
            JD.OnSetupGa += JD_OnSetupGa;
            JD.logging = true;
            JD.Connect(false);
        }

        private void btnResetProfit_Click(object sender, EventArgs e)
        {
            JD.ResetProfit();
        }

        private void btnEnabelMaxBet_Click(object sender, EventArgs e)
        {
            JD.SetSettings(SettingsType_Boolean.max_bet, true);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            JD.Disconnect();
            base.OnClosing(e);
        }

        private void btnBetHigh_Click(object sender, EventArgs e)
        {
            JD.Bet((double)nudChance.Value, (double)nudStake.Value, true);
        }

        private void btnBetLow_Click(object sender, EventArgs e)
        {
            JD.Bet((double)nudChance.Value, (double)nudStake.Value, false);
        }

        private void btnInvest_Click(object sender, EventArgs e)
        {
            JD.Invest((double)nudInvestAmount.Value, txtGAInvest.Text);
        }

        private void btnDivest_Click(object sender, EventArgs e)
        {
            JD.Divest((double)nudDivestAmount.Value, txtGADivest.Text);
        }

        private void btnInvestAll_Click(object sender, EventArgs e)
        {
            JD.InvestAll(txtGAInvest.Text);
        }

        private void btnDivestAll_Click(object sender, EventArgs e)
        {
            JD.DivestAll(txtGADivest.Text);
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            JD.Withdraw(txtWithdrawAddress.Text, (double)nudWithdrawAmount.Value, txtWithdrawGA.Text);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            switch (tcHistory.SelectedIndex)
            {
                case 0: JD.History(HistoryType.deposit); break;
                case 1: JD.History(HistoryType.withdraw); break;
                case 2: JD.History(HistoryType.invest); break;
                case 3: JD.History(HistoryType.commission); break;
            }

        }

        private void txtAllBetsID_Leave(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_String.Watch_Player, txtAllBetsID.Text);
        }

        private void txtChatBetsID_Leave(object sender, EventArgs e)
        {
            if (!Init)
            JD.SetSettings(SettingsType_String.Chat_Watch_Player, txtChatBetsID.Text);
        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            JD.Randomize();
        }

        private void btnRandomizeOK_Click(object sender, EventArgs e)
        {
            JD.Seed(txtRandomizenewClient.Text);
            pnlRandomize.Visible= false;
        }

        private void txtRandomizenewClient_TextChanged(object sender, EventArgs e)
        {
            if (txtRandomizenewClient.Text.Length>24)
            {
                txtRandomizenewClient.Text = txtRandomizenewClient.Text.Substring(0, 24);
            }
            btnRandomizeOK.Enabled = (Regex.IsMatch(txtRandomizenewClient.Text, "^[0-9]*$"));
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (JD.ga.active)
            {
                EditGa tmpGa = new EditGa(JD.ga.flags);
                DialogResult tmp = tmpGa.ShowDialog();
                if (tmp == System.Windows.Forms.DialogResult.Yes) //if ok
                {
                    JD.DoneEditGa(tmpGa.code, tmpGa.flags);
                }
                else if (tmp == DialogResult.No) //if disable
                {
                    JD.DisableGa(tmpGa.code);
                }
                else // if cancel
                {

                }
            }
            else
            {
                JD.EditGa();
            }
        }
       
    }
}
