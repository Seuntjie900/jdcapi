namespace JDCAPIExample
{
    partial class EditGa
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
            this.chkLogin = new System.Windows.Forms.CheckBox();
            this.chkWithdraw = new System.Windows.Forms.CheckBox();
            this.chkInvest = new System.Windows.Forms.CheckBox();
            this.chkDivest = new System.Windows.Forms.CheckBox();
            this.chkAddress = new System.Windows.Forms.CheckBox();
            this.chkAPI = new System.Windows.Forms.CheckBox();
            this.chkPlay = new System.Windows.Forms.CheckBox();
            this.chkRandom = new System.Windows.Forms.CheckBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkLogin
            // 
            this.chkLogin.AutoSize = true;
            this.chkLogin.Location = new System.Drawing.Point(12, 12);
            this.chkLogin.Name = "chkLogin";
            this.chkLogin.Size = new System.Drawing.Size(52, 17);
            this.chkLogin.TabIndex = 0;
            this.chkLogin.Text = "Login";
            this.chkLogin.UseVisualStyleBackColor = true;
            // 
            // chkWithdraw
            // 
            this.chkWithdraw.AutoSize = true;
            this.chkWithdraw.Location = new System.Drawing.Point(12, 39);
            this.chkWithdraw.Name = "chkWithdraw";
            this.chkWithdraw.Size = new System.Drawing.Size(71, 17);
            this.chkWithdraw.TabIndex = 1;
            this.chkWithdraw.Text = "Withdraw";
            this.chkWithdraw.UseVisualStyleBackColor = true;
            // 
            // chkInvest
            // 
            this.chkInvest.AutoSize = true;
            this.chkInvest.Location = new System.Drawing.Point(12, 66);
            this.chkInvest.Name = "chkInvest";
            this.chkInvest.Size = new System.Drawing.Size(55, 17);
            this.chkInvest.TabIndex = 2;
            this.chkInvest.Text = "Invest";
            this.chkInvest.UseVisualStyleBackColor = true;
            // 
            // chkDivest
            // 
            this.chkDivest.AutoSize = true;
            this.chkDivest.Location = new System.Drawing.Point(12, 93);
            this.chkDivest.Name = "chkDivest";
            this.chkDivest.Size = new System.Drawing.Size(56, 17);
            this.chkDivest.TabIndex = 3;
            this.chkDivest.Text = "Divest";
            this.chkDivest.UseVisualStyleBackColor = true;
            // 
            // chkAddress
            // 
            this.chkAddress.AutoSize = true;
            this.chkAddress.Location = new System.Drawing.Point(12, 120);
            this.chkAddress.Name = "chkAddress";
            this.chkAddress.Size = new System.Drawing.Size(260, 17);
            this.chkAddress.TabIndex = 4;
            this.chkAddress.Text = "Emergency withdrawal address and email address";
            this.chkAddress.UseVisualStyleBackColor = true;
            // 
            // chkAPI
            // 
            this.chkAPI.AutoSize = true;
            this.chkAPI.Location = new System.Drawing.Point(12, 147);
            this.chkAPI.Name = "chkAPI";
            this.chkAPI.Size = new System.Drawing.Size(69, 17);
            this.chkAPI.TabIndex = 5;
            this.chkAPI.Text = "API Keys";
            this.chkAPI.UseVisualStyleBackColor = true;
            // 
            // chkPlay
            // 
            this.chkPlay.AutoSize = true;
            this.chkPlay.Location = new System.Drawing.Point(12, 174);
            this.chkPlay.Name = "chkPlay";
            this.chkPlay.Size = new System.Drawing.Size(46, 17);
            this.chkPlay.TabIndex = 6;
            this.chkPlay.Text = "Play";
            this.chkPlay.UseVisualStyleBackColor = true;
            // 
            // chkRandom
            // 
            this.chkRandom.AutoSize = true;
            this.chkRandom.Location = new System.Drawing.Point(12, 201);
            this.chkRandom.Name = "chkRandom";
            this.chkRandom.Size = new System.Drawing.Size(79, 17);
            this.chkRandom.TabIndex = 7;
            this.chkRandom.Text = "Randomize";
            this.chkRandom.UseVisualStyleBackColor = true;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(98, 231);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(174, 20);
            this.txtCode.TabIndex = 8;
            // 
            // btnDisable
            // 
            this.btnDisable.Location = new System.Drawing.Point(116, 257);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(75, 23);
            this.btnDisable.TabIndex = 10;
            this.btnDisable.Text = "Disable GA";
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(197, 257);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "GA Code:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(35, 257);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // EgitGa
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 292);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnDisable);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.chkRandom);
            this.Controls.Add(this.chkPlay);
            this.Controls.Add(this.chkAPI);
            this.Controls.Add(this.chkAddress);
            this.Controls.Add(this.chkDivest);
            this.Controls.Add(this.chkInvest);
            this.Controls.Add(this.chkWithdraw);
            this.Controls.Add(this.chkLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EgitGa";
            this.Text = "EgitGa";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkLogin;
        private System.Windows.Forms.CheckBox chkWithdraw;
        private System.Windows.Forms.CheckBox chkInvest;
        private System.Windows.Forms.CheckBox chkDivest;
        private System.Windows.Forms.CheckBox chkAddress;
        private System.Windows.Forms.CheckBox chkAPI;
        private System.Windows.Forms.CheckBox chkPlay;
        private System.Windows.Forms.CheckBox chkRandom;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
    }
}