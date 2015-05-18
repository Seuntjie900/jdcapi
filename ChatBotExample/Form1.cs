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
namespace ChatBotExample
{
    public partial class Form1 : Form
    {
        jdInstance Bot = new jdInstance();
        Random R = new Random();
        Dictionary<string, DateTime> Activeusers = new Dictionary<string, DateTime>();
        public Form1()
        {
            InitializeComponent();
            //register an event to notify the user that login succeeded
            Bot.LoginEnd += Bot_LoginEnd;
            //register an event to notifu the user that login failed
            Bot.OnLoginError += Bot_OnLoginError;
            //register an event to handle receiving chat messages
            Bot.OnChat += Bot_OnChat;
        }

        void Bot_OnLoginError(string Error)
        {
            MessageBox.Show("Could not log in." + Error);
        }

        void Bot_OnChat(Chat chat)
        {
            if (Activeusers.ContainsKey(chat.UID))
            {
                Activeusers[chat.UID] = chat.Date;
            }
            else
            {
                Activeusers.Add(chat.UID, chat.Date);
            }
            string AppendedMessages = chat.Date.ToShortTimeString() + " ("+chat.UID+") ";
            if (chat.Type=="pm")
            {
                AppendedMessages += "<"+chat.User+"> -> ";
                if (chat.UID == Bot.uid)
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
                AppendedMessages += "<"+chat.User + ">";
            }
            AppendedMessages += " " + chat.Message;
            AppendMessage(AppendedMessages);


            if (chat.Message.ToLower() == "hello bot")
            {
                Bot.Chat("Hello " + chat.User);
            }
            if (chat.Message.ToLower() == "roll dice bot")
            {
                Bot.Chat(chat.User + " rolled a " + R.Next(0, 20) + " on a d20");
            }
            if (chat.Message.ToLower() == "rickroll" || chat.Message.ToLower() == "rick roll")
            {
                Bot.Chat("Never gonna give you up, never gonna let you down, never gonna run round and desert you. Never gonna make you cry, never gonna say goodbuy, never goona tell a lie and hurt you");
            }
            List<string> oldusers = new List<string>();
            foreach (string s in Activeusers.Keys)
            {
                if ((DateTime.Now - Activeusers[s]).TotalMinutes >= 10)
                {
                    oldusers.Add(s);
                }
            }
            foreach(string s in oldusers)
            {
                Activeusers.Remove(s);
            }
            UpdateLable(Activeusers.Count);
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

        delegate void dUpdateLable(int amount);
            void UpdateLable(int amount)
        {
                if (InvokeRequired)
                {
                    Invoke(new dUpdateLable(UpdateLable), amount);
                    return;
                }
                else
                {
                    lblActiveNumber.Text = amount.ToString();
                }
        }

        void Bot_LoginEnd(bool Connected)
        {
            if (Connected)
            MessageBox.Show("Logged in!");
            
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            Bot.BeginConnect(false, txtUsername.Text, txtPassword.Text,"");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Bot.Chat(txtMessage.Text);
            txtMessage.Text = "";
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            Bot.Disconnect();
            base.OnClosing(e);
        }
    }
}
