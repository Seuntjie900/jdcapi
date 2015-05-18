using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JDCAPIExample
{
    public partial class UserAccount : Form
    {
        public UserAccount(bool SetupAccount)
        {
            InitializeComponent();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            if (SetupAccount)
            {
                label1.Visible = true;
                label4.Visible = false;
                textBox1.PasswordChar = '\0';
            }
            else
            {
                label1.Visible = false;
                label4.Visible = true;
                textBox1.PasswordChar = '*';
            }
        }
        public string user = "";
        public string pass = "";

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == textBox3.Text)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.user = textBox1.Text;
                this.pass = textBox2.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Passwords do not match");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
