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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public string user { get; set; }
        public string pass { get; set; }
        public string code { get; set; }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            user = txtUsername.Text;
            pass = txtPassword.Text;
            code = txtGA.Text;
            this.Close();

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
