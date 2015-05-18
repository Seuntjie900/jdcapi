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
    public partial class EditGa : Form
    {
        public EditGa( JDCAPI.GAFlags Flags)
        {
            InitializeComponent();
        }
        public JDCAPI.GAFlags flags = new JDCAPI.GAFlags();
        public string code = "";
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == "")
            {
                MessageBox.Show("Must enter your GA code to change settings");
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                flags.divest = chkDivest.Checked;
                flags.invest = chkInvest.Checked;
                flags.login = chkLogin.Checked;
                flags.manage_api_keys = chkAPI.Checked;
                flags.randomize = chkRandom.Checked;
                flags.withdraw = chkWithdraw.Checked;
                flags.edit = chkAddress.Checked;
                code = txtCode.Text;
                this.Close();
            }
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == "")
            {
                MessageBox.Show("Must enter your GA code to change settings");
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
