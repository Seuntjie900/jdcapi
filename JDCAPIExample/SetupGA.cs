using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding.Windows.Forms;
using Gma.QrCodeNet.Encoding;

namespace JDCAPIExample
{
    public partial class SetupGA : Form
    {
        QrCodeImgControl qrDeposit = new QrCodeImgControl();
        public SetupGA(string GA, string user, string Error)
        {
            InitializeComponent();
            qrDeposit.Size = panel1.Size;
            qrDeposit.Location = new Point(0,0);
            panel1.Controls.Add(qrDeposit);
            if (qrDeposit.IsLocked)
                qrDeposit.UnLock();

            qrDeposit.Text = "otpauth://totp/justdice:"+user+"?secret=" + GA;
            qrDeposit.QuietZoneModule = Gma.QrCodeNet.Encoding.Windows.Render.QuietZoneModules.Four;

            BitMatrix qrMatrix = qrDeposit.GetQrMatrix();
            if (qrDeposit.IsFreezed)
                qrDeposit.UnFreeze();
            qrDeposit.Freeze();
            lblCode.Text = GA;
            if (Error != "")
            {
                lblError.Text = Error;
            }
        }
        public string Code { get; set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Code = txtCode.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
