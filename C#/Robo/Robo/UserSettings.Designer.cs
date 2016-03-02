namespace Robo
{
    partial class UserSettings
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInMsgFormat = new System.Windows.Forms.TabPage();
            this.txtSettingFormat = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStatusFormat = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabOutMsgFormat = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtstartflusing = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtsofbw = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtsfbw = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtmachinestop = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtcfbw = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtmachinestart = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtwriteseparator = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtreadsetting = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtwritesetting = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtreadstatus = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabInMsgFormat.SuspendLayout();
            this.tabOutMsgFormat.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabInMsgFormat);
            this.tabControl1.Controls.Add(this.tabOutMsgFormat);
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 25);
            this.tabControl1.Location = new System.Drawing.Point(14, 14);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(10, 5);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(353, 401);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 0;
            // 
            // tabInMsgFormat
            // 
            this.tabInMsgFormat.Controls.Add(this.txtSettingFormat);
            this.tabInMsgFormat.Controls.Add(this.label2);
            this.tabInMsgFormat.Controls.Add(this.txtStatusFormat);
            this.tabInMsgFormat.Controls.Add(this.label1);
            this.tabInMsgFormat.Location = new System.Drawing.Point(4, 29);
            this.tabInMsgFormat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabInMsgFormat.Name = "tabInMsgFormat";
            this.tabInMsgFormat.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabInMsgFormat.Size = new System.Drawing.Size(345, 368);
            this.tabInMsgFormat.TabIndex = 0;
            this.tabInMsgFormat.Text = "Incoming message format";
            this.tabInMsgFormat.UseVisualStyleBackColor = true;
            // 
            // txtSettingFormat
            // 
            this.txtSettingFormat.Location = new System.Drawing.Point(20, 206);
            this.txtSettingFormat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSettingFormat.Multiline = true;
            this.txtSettingFormat.Name = "txtSettingFormat";
            this.txtSettingFormat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSettingFormat.Size = new System.Drawing.Size(303, 136);
            this.txtSettingFormat.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Setting message format :";
            // 
            // txtStatusFormat
            // 
            this.txtStatusFormat.Location = new System.Drawing.Point(20, 44);
            this.txtStatusFormat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtStatusFormat.Multiline = true;
            this.txtStatusFormat.Name = "txtStatusFormat";
            this.txtStatusFormat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatusFormat.Size = new System.Drawing.Size(303, 118);
            this.txtStatusFormat.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Status message format :";
            // 
            // tabOutMsgFormat
            // 
            this.tabOutMsgFormat.Controls.Add(this.groupBox1);
            this.tabOutMsgFormat.Controls.Add(this.txtwriteseparator);
            this.tabOutMsgFormat.Controls.Add(this.label4);
            this.tabOutMsgFormat.Controls.Add(this.txtreadsetting);
            this.tabOutMsgFormat.Controls.Add(this.label6);
            this.tabOutMsgFormat.Controls.Add(this.txtwritesetting);
            this.tabOutMsgFormat.Controls.Add(this.label5);
            this.tabOutMsgFormat.Controls.Add(this.txtreadstatus);
            this.tabOutMsgFormat.Controls.Add(this.label3);
            this.tabOutMsgFormat.Location = new System.Drawing.Point(4, 29);
            this.tabOutMsgFormat.Name = "tabOutMsgFormat";
            this.tabOutMsgFormat.Size = new System.Drawing.Size(345, 368);
            this.tabOutMsgFormat.TabIndex = 1;
            this.tabOutMsgFormat.Text = "Outgoing message format";
            this.tabOutMsgFormat.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtstartflusing);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtsofbw);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtsfbw);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtmachinestop);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtcfbw);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtmachinestart);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Location = new System.Drawing.Point(17, 123);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 231);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Machine control messages";
            // 
            // txtstartflusing
            // 
            this.txtstartflusing.Location = new System.Drawing.Point(166, 150);
            this.txtstartflusing.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtstartflusing.Name = "txtstartflusing";
            this.txtstartflusing.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtstartflusing.Size = new System.Drawing.Size(134, 23);
            this.txtstartflusing.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(161, 131);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(125, 15);
            this.label12.TabIndex = 22;
            this.label12.Text = "Start flusing message :";
            // 
            // txtsofbw
            // 
            this.txtsofbw.Location = new System.Drawing.Point(11, 150);
            this.txtsofbw.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtsofbw.Name = "txtsofbw";
            this.txtsofbw.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtsofbw.Size = new System.Drawing.Size(145, 23);
            this.txtsofbw.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 131);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(150, 15);
            this.label11.TabIndex = 20;
            this.label11.Text = "Softner Filter BW message :";
            // 
            // txtsfbw
            // 
            this.txtsfbw.Location = new System.Drawing.Point(166, 99);
            this.txtsfbw.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtsfbw.Name = "txtsfbw";
            this.txtsfbw.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtsfbw.Size = new System.Drawing.Size(134, 23);
            this.txtsfbw.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(163, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(138, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "Sand Filter BW message :";
            // 
            // txtmachinestop
            // 
            this.txtmachinestop.Location = new System.Drawing.Point(166, 44);
            this.txtmachinestop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtmachinestop.Name = "txtmachinestop";
            this.txtmachinestop.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtmachinestop.Size = new System.Drawing.Size(134, 23);
            this.txtmachinestop.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(163, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 15);
            this.label8.TabIndex = 16;
            this.label8.Text = "Stop machine message :";
            // 
            // txtcfbw
            // 
            this.txtcfbw.Location = new System.Drawing.Point(11, 99);
            this.txtcfbw.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtcfbw.Name = "txtcfbw";
            this.txtcfbw.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtcfbw.Size = new System.Drawing.Size(145, 23);
            this.txtcfbw.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(151, 15);
            this.label9.TabIndex = 14;
            this.label9.Text = "Carbon Filter BW message :";
            // 
            // txtmachinestart
            // 
            this.txtmachinestart.Location = new System.Drawing.Point(11, 44);
            this.txtmachinestart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtmachinestart.Name = "txtmachinestart";
            this.txtmachinestart.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtmachinestart.Size = new System.Drawing.Size(145, 23);
            this.txtmachinestart.TabIndex = 13;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(135, 15);
            this.label10.TabIndex = 12;
            this.label10.Text = "Start machine message :";
            // 
            // txtwriteseparator
            // 
            this.txtwriteseparator.Location = new System.Drawing.Point(172, 92);
            this.txtwriteseparator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtwriteseparator.MaxLength = 1;
            this.txtwriteseparator.Name = "txtwriteseparator";
            this.txtwriteseparator.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtwriteseparator.Size = new System.Drawing.Size(155, 23);
            this.txtwriteseparator.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(169, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Write setting separator :";
            // 
            // txtreadsetting
            // 
            this.txtreadsetting.Location = new System.Drawing.Point(172, 37);
            this.txtreadsetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtreadsetting.Name = "txtreadsetting";
            this.txtreadsetting.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtreadsetting.Size = new System.Drawing.Size(155, 23);
            this.txtreadsetting.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(169, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Read setting message :";
            // 
            // txtwritesetting
            // 
            this.txtwritesetting.Location = new System.Drawing.Point(17, 92);
            this.txtwritesetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtwritesetting.Name = "txtwritesetting";
            this.txtwritesetting.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtwritesetting.Size = new System.Drawing.Size(149, 23);
            this.txtwritesetting.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "Write setting prefix :";
            // 
            // txtreadstatus
            // 
            this.txtreadstatus.Location = new System.Drawing.Point(17, 37);
            this.txtreadstatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtreadstatus.Name = "txtreadstatus";
            this.txtreadstatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtreadstatus.Size = new System.Drawing.Size(149, 23);
            this.txtreadstatus.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Read status message :";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(14, 423);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 26);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save && apply";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(239, 423);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(128, 26);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel changes";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // UserSettings
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(381, 456);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "UserSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User settings";
            this.Load += new System.EventHandler(this.UserSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabInMsgFormat.ResumeLayout(false);
            this.tabInMsgFormat.PerformLayout();
            this.tabOutMsgFormat.ResumeLayout(false);
            this.tabOutMsgFormat.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInMsgFormat;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStatusFormat;
        private System.Windows.Forms.TextBox txtSettingFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabOutMsgFormat;
        private System.Windows.Forms.TextBox txtwritesetting;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtreadstatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtreadsetting;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtwriteseparator;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtsfbw;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtmachinestop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtcfbw;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtmachinestart;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtsofbw;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtstartflusing;
        private System.Windows.Forms.Label label12;
    }
}