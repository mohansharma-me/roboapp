namespace Robo
{
    partial class HealthySettingDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radUNH3 = new System.Windows.Forms.RadioButton();
            this.radUNH2 = new System.Windows.Forms.RadioButton();
            this.radUNH1 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radS3 = new System.Windows.Forms.RadioButton();
            this.radS2 = new System.Windows.Forms.RadioButton();
            this.radS1 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtHF = new System.Windows.Forms.MaskedTextBox();
            this.txtHT = new System.Windows.Forms.MaskedTextBox();
            this.txtU = new System.Windows.Forms.MaskedTextBox();
            this.txtS = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Description :- ";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc.Location = new System.Drawing.Point(106, 23);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(12, 15);
            this.lblDesc.TabIndex = 1;
            this.lblDesc.Text = "-";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtHT);
            this.groupBox1.Controls.Add(this.txtHF);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(7, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 62);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Healthy values";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(207, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "To:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(39, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "From:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.lblDesc);
            this.groupBox2.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(7, -2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(371, 51);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtU);
            this.groupBox3.Controls.Add(this.radUNH3);
            this.groupBox3.Controls.Add(this.radUNH2);
            this.groupBox3.Controls.Add(this.radUNH1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(7, 123);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(371, 110);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Unhealthy values";
            // 
            // radUNH3
            // 
            this.radUNH3.AutoSize = true;
            this.radUNH3.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radUNH3.Location = new System.Drawing.Point(236, 33);
            this.radUNH3.Name = "radUNH3";
            this.radUNH3.Size = new System.Drawing.Size(68, 19);
            this.radUNH3.TabIndex = 6;
            this.radUNH3.TabStop = true;
            this.radUNH3.Text = "Equal to";
            this.radUNH3.UseVisualStyleBackColor = true;
            // 
            // radUNH2
            // 
            this.radUNH2.AutoSize = true;
            this.radUNH2.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radUNH2.Location = new System.Drawing.Point(156, 33);
            this.radUNH2.Name = "radUNH2";
            this.radUNH2.Size = new System.Drawing.Size(74, 19);
            this.radUNH2.TabIndex = 5;
            this.radUNH2.TabStop = true;
            this.radUNH2.Text = "Less than";
            this.radUNH2.UseVisualStyleBackColor = true;
            // 
            // radUNH1
            // 
            this.radUNH1.AutoSize = true;
            this.radUNH1.Checked = true;
            this.radUNH1.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radUNH1.Location = new System.Drawing.Point(60, 33);
            this.radUNH1.Name = "radUNH1";
            this.radUNH1.Size = new System.Drawing.Size(90, 19);
            this.radUNH1.TabIndex = 4;
            this.radUNH1.TabStop = true;
            this.radUNH1.Text = "Greater than";
            this.radUNH1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(102, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Value:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtS);
            this.groupBox4.Controls.Add(this.radS3);
            this.groupBox4.Controls.Add(this.radS2);
            this.groupBox4.Controls.Add(this.radS1);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(7, 239);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(371, 117);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Stoped values";
            // 
            // radS3
            // 
            this.radS3.AutoSize = true;
            this.radS3.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radS3.Location = new System.Drawing.Point(236, 36);
            this.radS3.Name = "radS3";
            this.radS3.Size = new System.Drawing.Size(68, 19);
            this.radS3.TabIndex = 6;
            this.radS3.TabStop = true;
            this.radS3.Text = "Equal to";
            this.radS3.UseVisualStyleBackColor = true;
            // 
            // radS2
            // 
            this.radS2.AutoSize = true;
            this.radS2.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radS2.Location = new System.Drawing.Point(156, 36);
            this.radS2.Name = "radS2";
            this.radS2.Size = new System.Drawing.Size(74, 19);
            this.radS2.TabIndex = 5;
            this.radS2.TabStop = true;
            this.radS2.Text = "Less than";
            this.radS2.UseVisualStyleBackColor = true;
            // 
            // radS1
            // 
            this.radS1.AutoSize = true;
            this.radS1.Checked = true;
            this.radS1.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radS1.Location = new System.Drawing.Point(60, 36);
            this.radS1.Name = "radS1";
            this.radS1.Size = new System.Drawing.Size(90, 19);
            this.radS1.TabIndex = 4;
            this.radS1.TabStop = true;
            this.radS1.Text = "Greater than";
            this.radS1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(102, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "Value:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(217, 365);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(303, 365);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtHF
            // 
            this.txtHF.BeepOnError = true;
            this.txtHF.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHF.Location = new System.Drawing.Point(83, 25);
            this.txtHF.Name = "txtHF";
            this.txtHF.RejectInputOnFirstFailure = true;
            this.txtHF.Size = new System.Drawing.Size(100, 23);
            this.txtHF.SkipLiterals = false;
            this.txtHF.TabIndex = 17;
            this.txtHF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtHT
            // 
            this.txtHT.BeepOnError = true;
            this.txtHT.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHT.Location = new System.Drawing.Point(236, 25);
            this.txtHT.Name = "txtHT";
            this.txtHT.RejectInputOnFirstFailure = true;
            this.txtHT.Size = new System.Drawing.Size(100, 23);
            this.txtHT.SkipLiterals = false;
            this.txtHT.TabIndex = 18;
            this.txtHT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtU
            // 
            this.txtU.BeepOnError = true;
            this.txtU.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtU.Location = new System.Drawing.Point(146, 68);
            this.txtU.Name = "txtU";
            this.txtU.RejectInputOnFirstFailure = true;
            this.txtU.Size = new System.Drawing.Size(100, 23);
            this.txtU.SkipLiterals = false;
            this.txtU.TabIndex = 18;
            this.txtU.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtS
            // 
            this.txtS.BeepOnError = true;
            this.txtS.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtS.Location = new System.Drawing.Point(146, 72);
            this.txtS.Name = "txtS";
            this.txtS.RejectInputOnFirstFailure = true;
            this.txtS.Size = new System.Drawing.Size(100, 23);
            this.txtS.SkipLiterals = false;
            this.txtS.TabIndex = 18;
            this.txtS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // HealthySettingDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(388, 395);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HealthySettingDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Healthy Setting Dialog";
            this.Load += new System.EventHandler(this.HealthySettingDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radUNH3;
        private System.Windows.Forms.RadioButton radUNH2;
        private System.Windows.Forms.RadioButton radUNH1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radS3;
        private System.Windows.Forms.RadioButton radS2;
        private System.Windows.Forms.RadioButton radS1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.MaskedTextBox txtHT;
        private System.Windows.Forms.MaskedTextBox txtHF;
        private System.Windows.Forms.MaskedTextBox txtU;
        private System.Windows.Forms.MaskedTextBox txtS;
    }
}